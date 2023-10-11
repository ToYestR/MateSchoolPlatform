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
    [OutlineEffectPassInfo("Change alpha")]
    [System.Serializable]
    public class ChangeAlphaPostProcessingPass : OutlinePostProcessingPass
    {
        private static readonly int amountHash = Shader.PropertyToID("_Amount");

        private Material material;

        public enum ChangeAlphaType
        {
            Multiply = 0,
            Power = 1
        }

        private Material Material
        {
            get
            {
                if (material == null)
                    material = new Material(Resources.Load<Shader>("Shaders/Post processing/ChangeAlpha"));

                return material;
            }
        }

        [SerializeField]
        private ChangeAlphaType changeAlphaType;

        [SerializeField]
        private float amount = 2.0f;

        public override void Setup(OutlineParameters parameters, RenderTargetIdentifier current, RenderTargetIdentifier target)
        {
            parameters.Buffer.SetGlobalFloat(amountHash, amount);
            parameters.ControlProvider.Blit(current, target, Material, (int)changeAlphaType);
        }

#if UNITY_EDITOR
        public override void DrawEditor(Rect position, Action serializationCallback)
        {
            base.DrawEditor(position, serializationCallback);

            position.y += base.CalculateEditorHeight() + EditorGUIUtility.standardVerticalSpacing;
            Indent(ref position);
            position.height = EditorGUIUtility.singleLineHeight;


            changeAlphaType = (ChangeAlphaType)EditorGUI.EnumPopup(position, new GUIContent("Change alpha type"), changeAlphaType);

            NextLine(ref position);

            amount = EditorGUI.FloatField(position, new GUIContent("Amount"), amount);
            if (amount < 0.0f)
                amount = 0.0f;
        }

        public override float CalculateEditorHeight()
        {
            return base.CalculateEditorHeight() + EditorGUIUtility.singleLineHeight * 3.0f + EditorGUIUtility.standardVerticalSpacing * 2.0f;
        }
#endif
    }
}