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
    [OutlineEffectPassInfo("Noise")]
    [System.Serializable]
    public class NoisePostProcessingPass : OutlinePostProcessingPass
    {
        private static readonly int amountHash = Shader.PropertyToID("_Amount");
        private static readonly int redHash = Shader.PropertyToID("_RedFactor");
        private static readonly int greenHash = Shader.PropertyToID("_GreenFactor");
        private static readonly int blueHash = Shader.PropertyToID("_BlueFactor");
        private static readonly int alphaHash = Shader.PropertyToID("_AlphaFactor");
        private static readonly int sizeHash = Shader.PropertyToID("_Size");

        public enum NoiseType
        {
            BasicAlphaOnly = 0,
            BasicFull = 1,
            BasicGradientAlphaOnly = 2
        }

        private Material material;

        private Material Material
        {
            get
            {
                if (material == null)
                    material = new Material(Resources.Load<Shader>("Shaders/Post processing/Noise"));

                return material;
            }
        }

        [SerializeField]
        private NoiseType type;

        [SerializeField]
        private float totalAmount = 0.5f;

        [SerializeField]
        private float redAmount = 0.5f;

        [SerializeField]
        private float greenAmount = 0.5f;

        [SerializeField]
        private float blueAmount = 0.5f;

        [SerializeField]
        private float alphaAmount = 0.5f;

        [SerializeField]
        private float size = 1.0f;

        public override void Setup(OutlineParameters parameters, RenderTargetIdentifier current, RenderTargetIdentifier target)
        {
            parameters.Buffer.SetGlobalFloat(amountHash, totalAmount);
            parameters.Buffer.SetGlobalFloat(redHash, redAmount);
            parameters.Buffer.SetGlobalFloat(greenHash, greenAmount);
            parameters.Buffer.SetGlobalFloat(blueHash, blueAmount);
            parameters.Buffer.SetGlobalFloat(alphaHash, alphaAmount);
            parameters.Buffer.SetGlobalFloat(sizeHash, size);

            parameters.ControlProvider.Blit(current, target, Material, (int)type);
        }

#if UNITY_EDITOR
        public override void DrawEditor(Rect position, Action serializationCallback)
        {
            base.DrawEditor(position, serializationCallback);
            position.y += base.CalculateEditorHeight() + EditorGUIUtility.standardVerticalSpacing;

            position.height = EditorGUIUtility.singleLineHeight;
            
            Indent(ref position);

            type = (NoiseType)EditorGUI.EnumPopup(position, new GUIContent("Noise type"), type);

            NextLine(ref position);

            totalAmount = EditorGUI.Slider(position, new GUIContent("Amount"), totalAmount, 0.0f, 1.0f);

            switch (type)
            {
                case NoiseType.BasicFull:

                    NextLine(ref position);

                    redAmount = EditorGUI.Slider(position, new GUIContent("Red amount"), redAmount, 0.0f, 1.0f);

                    NextLine(ref position);

                    greenAmount = EditorGUI.Slider(position, new GUIContent("Green amount"), greenAmount, 0.0f, 1.0f);

                    NextLine(ref position);

                    blueAmount = EditorGUI.Slider(position, new GUIContent("Blue amount"), blueAmount, 0.0f, 1.0f);

                    NextLine(ref position);

                    alphaAmount = EditorGUI.Slider(position, new GUIContent("Alpha amount"), alphaAmount, 0.0f, 1.0f);

                    break;

                case NoiseType.BasicGradientAlphaOnly:

                    NextLine(ref position);

                    size = EditorGUI.Slider(position, new GUIContent("Size"), size, 0.0f, 10.0f);

                    break;
            }
        }

        public override float CalculateEditorHeight()
        {
            var additional = 0.0f;
            switch (type)
            {
                case NoiseType.BasicAlphaOnly:
                    additional = EditorGUIUtility.singleLineHeight;
                    break;

                case NoiseType.BasicFull:
                    additional += EditorGUIUtility.singleLineHeight * 5 + EditorGUIUtility.standardVerticalSpacing * 3;
                    break;

                case NoiseType.BasicGradientAlphaOnly:
                    additional += EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing;
                    break;
            }

            return base.CalculateEditorHeight() + EditorGUIUtility.singleLineHeight * 2.0f + additional;
        }
#endif
    }
}
