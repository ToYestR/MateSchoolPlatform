using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EPOOutline
{
    [System.Serializable]
    [OutlineEffectPassInfo("Scanline")]
    public class ScanlinePostProcessingPass : OutlinePostProcessingPass
    {
        private static readonly int scalerHash = Shader.PropertyToID("_Scaler");
        private static readonly int speedHash = Shader.PropertyToID("_Speed");
        private static readonly int directionHash = Shader.PropertyToID("_Direction");

        private Material material;

        private Material Material
        {
            get
            {
                if (material == null)
                    material = new Material(Resources.Load<Shader>("Shaders/Post processing/Scanline"));

                return material;
            }
        }

        [SerializeField]
        private float scale = 2.0f;

        [SerializeField]
        private float speed = 2.0f;

        [SerializeField]
        private float angle = 0.0f;

        [Preserve]
        public ScanlinePostProcessingPass()
        {

        }

        public override void Setup(OutlineParameters parameters, RenderTargetIdentifier current, RenderTargetIdentifier target)
        {
            parameters.Buffer.SetGlobalVector(directionHash, Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up);
            parameters.Buffer.SetGlobalFloat(scalerHash, scale);
            parameters.Buffer.SetGlobalFloat(speedHash, speed);
            parameters.ControlProvider.Blit(current, target, Material, -1);
        }

#if UNITY_EDITOR
        public override void DrawEditor(Rect position, Action serializationCallback)
        {
            base.DrawEditor(position, serializationCallback);
            position.y += base.CalculateEditorHeight() + EditorGUIUtility.standardVerticalSpacing;
            position.height = EditorGUIUtility.singleLineHeight;

            Indent(ref position);

            scale = EditorGUI.Slider(position, new GUIContent("Scale"), scale, 0.05f, 10.0f);

            NextLine(ref position);

            speed = EditorGUI.Slider(position, new GUIContent("Speed"), speed, 0.0f, 10.0f);

            NextLine(ref position);

            angle = EditorGUI.Slider(position, new GUIContent("Angle"), angle, 0.0f, 360.0f);
        }

        public override float CalculateEditorHeight()
        {
            return base.CalculateEditorHeight() + EditorGUIUtility.singleLineHeight * 4.0f + EditorGUIUtility.standardVerticalSpacing * 3.0f;
        }
#endif
    }
}