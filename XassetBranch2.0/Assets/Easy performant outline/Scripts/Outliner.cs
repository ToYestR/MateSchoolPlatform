using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Rendering;

namespace EPOOutline
{
    [RequireComponent(typeof(Camera))]
    [ExecuteAlways]
    public class Outliner : MonoBehaviour, IGraphicsControlProvider, ISerializationCallbackReceiver
    {
        [SerializeField]
        private int blurIterations = 3;

        [SerializeField]
        private int dilateIterations = 2;

        [SerializeField]
        [Range(0.05f, 1.0f)]
        private float primaryRendererScale = 0.75f;

        [SerializeField]
        [Range(0.05f, 1.0f)]
        private float infoBufferScale = 0.25f;

        [SerializeField]
        [Range(0.0f, 2.0f)]
        private float blurShift = 1.0f;

        [SerializeField]
        [Range(0.0f, 2.0f)]
        private float dilateShift = 1.0f;

        [SerializeField]
        private BlurType blurType = BlurType.Gaussian5x5;

        [SerializeField]
        private List<string> serializedAfterCarvePostProcessingPasses = new List<string>();

        private List<OutlinePostProcessingPass> afterCarvePostProcessingPasses = new List<OutlinePostProcessingPass>();

        [SerializeField]
        private bool usingInfoBuffer = false;
        
        [SerializeField]
        private bool sustainedPerformanceMode = true;

        private List<Outlinable> outlinables = new List<Outlinable>();

        private CommandBuffer basicBuffer;

#if UNITY_EDITOR
        private CommandBuffer editorBuffer;
#endif

        private OutlineParameters parameters = new OutlineParameters();

        private Camera targetCamera;

        public void SetScale(float value)
        {
            primaryRendererScale = Mathf.Clamp01(value);
        }

        private void OnEnable()
        {
            basicBuffer = new CommandBuffer();
            basicBuffer.name = "Outline";
            targetCamera = GetComponent<Camera>();
            targetCamera.AddCommandBuffer(CameraEvent.AfterForwardAlpha, basicBuffer);

#if UNITY_EDITOR
            editorBuffer = new CommandBuffer();
            if (SceneView.lastActiveSceneView != null && SceneView.lastActiveSceneView.camera != null)
                SceneView.lastActiveSceneView.camera.AddCommandBuffer(CameraEvent.AfterForwardAlpha, editorBuffer);
#endif
        }

        private void OnDisable()
        {
            targetCamera?.RemoveCommandBuffer(CameraEvent.AfterForwardAlpha, basicBuffer);

#if UNITY_EDITOR
            if (SceneView.lastActiveSceneView != null && SceneView.lastActiveSceneView.camera != null)
                SceneView.lastActiveSceneView.camera.RemoveCommandBuffer(CameraEvent.AfterForwardAlpha, editorBuffer);
#endif

            basicBuffer.Dispose();
            basicBuffer = null;

#if UNITY_EDITOR
            editorBuffer.Dispose();
            editorBuffer = null;
#endif
        }

#if UNITY_EDITOR
        private void ReAddBufferToEditorCamera()
        {
            if (SceneView.lastActiveSceneView == null)
                return;

            var editorCamera = SceneView.lastActiveSceneView.camera;
            if (!editorCamera)
                return;

            editorCamera.RemoveCommandBuffer(CameraEvent.AfterForwardAlpha, editorBuffer);
            editorCamera.AddCommandBuffer(CameraEvent.AfterForwardAlpha, editorBuffer);
        }
#endif

        private void LateUpdate()
        {
            parameters.Layers.Clear();
            Outlinable.GetAllActiveOutlinables(outlinables);
            foreach (var outlinable in outlinables)
                parameters.Layers.Add(outlinable.Layer);

            parameters.SustainedPerformanceMode = sustainedPerformanceMode;

            parameters.AfterCarvePostProcessingPasses.Clear();
            foreach (var afterCarvePostProcess in afterCarvePostProcessingPasses)
                parameters.AfterCarvePostProcessingPasses.Add(afterCarvePostProcess);

            parameters.BlurType = blurType;

#if UNITY_EDITOR
            ReAddBufferToEditorCamera();

            if (SceneView.lastActiveSceneView != null && SceneView.lastActiveSceneView.camera != null)
            {
                parameters.IsEditorCamera = true;

                UpdateParameters(SceneView.lastActiveSceneView.camera, true);
                var targetTexture = SceneView.lastActiveSceneView.camera.targetTexture;
                var width = targetTexture == null ? Screen.width : targetTexture.width;
                var height = targetTexture == null ? Screen.height : targetTexture.height;

                SceneView.lastActiveSceneView.camera.forceIntoRenderTexture = true;
                OutlineEffect.SetupBuffer(parameters);
            }
#endif

            {
                parameters.IsEditorCamera = false;
                UpdateParameters(targetCamera, false);
                targetCamera.forceIntoRenderTexture = true;
                OutlineEffect.SetupBuffer(parameters);
            }
        }

        private void UpdateParameters(Camera camera, bool editorCamera)
        {
            parameters.UseHDR = camera.allowHDR;
            parameters.DilateShift = dilateShift;
            parameters.BlurShift = blurShift;

            parameters.Antialiasing = camera.allowMSAA ? QualitySettings.antiAliasing : 0;

            parameters.BlurIterations = blurIterations;
            parameters.DilateIterations = dilateIterations;

            parameters.RenderMask = camera.cullingMask;

            Predicate<Outlinable> editorPredicate = obj =>
                {
#if UNITY_EDITOR
                    var stage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

                    return stage == null || stage.IsPartOfPrefabContents(obj.gameObject);
#else
                    return true;
#endif
                };

            Predicate<Outlinable> gamePredicate = obj =>
                {
#if UNITY_EDITOR
                    var stage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

                    return stage == null || !stage.IsPartOfPrefabContents(obj.gameObject);
#else
                    return true;
#endif
                };

            parameters.DrawPredicate = editorCamera ? editorPredicate : gamePredicate;
            parameters.PrimaryRendererScale = primaryRendererScale;
            parameters.InfoBufferScale = infoBufferScale;

            var targetTexture = camera.targetTexture;

            if (UnityEngine.XR.XRSettings.enabled && !editorCamera)
            {
                parameters.Width = UnityEngine.XR.XRSettings.eyeTextureWidth;
                parameters.Height = UnityEngine.XR.XRSettings.eyeTextureHeight;
            }
            else
            {
                parameters.Width = targetTexture == null ? camera.scaledPixelWidth : targetTexture.width;
                parameters.Height = targetTexture == null ? camera.scaledPixelHeight : targetTexture.height;
            }

            parameters.UsingInfoBuffer = usingInfoBuffer;

            parameters.Target = new RenderTargetIdentifier(BuiltinRenderTextureType.CameraTarget, 0, CubemapFace.Unknown, -1);
            parameters.Depth = camera.depthTextureMode == DepthTextureMode.Depth || camera.depthTextureMode == DepthTextureMode.DepthNormals ? 
                BuiltinRenderTextureType.Depth :
                BuiltinRenderTextureType.None;

            parameters.ControlProvider = this;

#if UNITY_EDITOR
            parameters.Buffer = editorCamera ? editorBuffer : basicBuffer;
#else
            parameters.Buffer = basicBuffer;
#endif
        }

        private void OnValidate()
        {
            if (primaryRendererScale < 0)
                primaryRendererScale = 0;

            if (blurIterations < 0)
                blurIterations = 0;

            if (dilateIterations < 0)
                dilateIterations = 0;
        }

        public void SetTarget(RenderTargetIdentifier target)
        {
            parameters.Buffer.SetRenderTarget(target);
        }

        public void DrawRenderer(Renderer renderer, Material material, int submesh, bool useCutoutVersion)
        {
            parameters.Buffer.DrawRenderer(renderer, material, submesh, useCutoutVersion ? 1 : 0);
        }

        public void Blit(RenderTargetIdentifier first, RenderTargetIdentifier second)
        {
            parameters.Buffer.Blit(first, second);
        }

        public void Blit(RenderTargetIdentifier first, RenderTargetIdentifier second, Material material, int passIndex)
        {
            parameters.Buffer.Blit(first, second, material, passIndex);
        }

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination);
        }

        public void OnBeforeSerialize()
        {
            serializedAfterCarvePostProcessingPasses = SerializationUtility.Serialize(afterCarvePostProcessingPasses);
        }

        public void OnAfterDeserialize()
        {
            afterCarvePostProcessingPasses = SerializationUtility.Deserialize<OutlinePostProcessingPass>(serializedAfterCarvePostProcessingPasses);
        }
    }
}