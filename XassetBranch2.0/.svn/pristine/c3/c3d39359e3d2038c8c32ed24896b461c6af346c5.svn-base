using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EPOOutline
{
    [OutlineEffectPassInfo("Blur")]
    public class BlurPostProcessingPass : OutlinePostProcessingPass
    {
        private static readonly int processDirectionHash = Shader.PropertyToID("_OutlinePostProcessDirection");
        private static readonly int intermediaryTextureHash = Shader.PropertyToID("_Intermediary");

        private static readonly List<string> blurKeywords = new List<string>();

        private Material material;

        private Material Material
        {
            get
            {
                if (material == null)
                    material = new Material(Resources.Load<Shader>("Shaders/OutlinePostProcess"));

                return material;
            }
        }

        [SerializeField]
        private BlurType type;

        [SerializeField]
        private float shift = 1.0f;

        [SerializeField]
        private float angle = 1.0f;

        [SerializeField]
        private bool isBidirectional = true;

        public override void Setup(OutlineParameters parameters, RenderTargetIdentifier current, RenderTargetIdentifier target)
        {
            var scaledWidth = (int)(parameters.Width * parameters.PrimaryRendererScale);
            var scaledHeight = (int)(parameters.Height * parameters.PrimaryRendererScale);

            var direction = new Vector2(parameters.BlurShift / scaledWidth, parameters.BlurShift / scaledHeight);

            KeywordsUtility.GetAllBlurKeywords(blurKeywords);
            foreach (var keyword in blurKeywords)
                parameters.Buffer.DisableShaderKeyword(keyword);

            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            parameters.Buffer.EnableShaderKeyword(KeywordsUtility.GetBlurKeyword(type));
            if (isBidirectional)
                RenderTargetUtility.GetTemporaryRT(parameters, intermediaryTextureHash, scaledWidth, scaledHeight, 0);

            parameters.Buffer.SetGlobalVector(processDirectionHash, rotation * new Vector2(direction.x * shift, 0));
            parameters.ControlProvider.Blit(current, isBidirectional ? intermediaryTextureHash : target, Material, 0);
            if (!isBidirectional)
                return;

            parameters.Buffer.SetGlobalVector(processDirectionHash, rotation * new Vector2(0, direction.y * shift));
            parameters.ControlProvider.Blit(intermediaryTextureHash, target, Material, 0);
            parameters.Buffer.ReleaseTemporaryRT(intermediaryTextureHash);
        }


#if UNITY_EDITOR
        public override void DrawEditor(Rect position, Action serializationCallback)
        {
            base.DrawEditor(position, serializationCallback);

            position.y += base.CalculateEditorHeight() + EditorGUIUtility.standardVerticalSpacing;
            Indent(ref position);
            position.height = EditorGUIUtility.singleLineHeight;

            type = (BlurType)EditorGUI.EnumPopup(position, new GUIContent("Blur type"), type);

            NextLine(ref position);

            shift = EditorGUI.Slider(position, new GUIContent("Shift"), shift, 0.0f, 3.0f);

            NextLine(ref position);

            isBidirectional = EditorGUI.Toggle(position, new GUIContent("Is bidirectional"), isBidirectional);

            NextLine(ref position);

            if (!isBidirectional)
                angle = EditorGUI.Slider(position, new GUIContent("Angle"), angle, 0.0f, 360.0f);
        }

        public override float CalculateEditorHeight()
        {
            var itemsCount = 5;
            if (isBidirectional)
                itemsCount--;

            return base.CalculateEditorHeight() + EditorGUIUtility.singleLineHeight * itemsCount + EditorGUIUtility.standardVerticalSpacing * (itemsCount - 1);
        }
#endif
    }
}