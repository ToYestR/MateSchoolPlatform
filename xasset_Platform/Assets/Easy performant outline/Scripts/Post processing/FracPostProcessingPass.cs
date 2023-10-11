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
    [OutlineEffectPassInfo("Frac")]
    [System.Serializable]
    public class FracPostProcessingPass : OutlinePostProcessingPass
    {
        private static readonly int scalerHash = Shader.PropertyToID("_Scaler");

        private Material material;

        private Material Material
        {
            get
            {
                if (material == null)
                    material = new Material(Resources.Load<Shader>("Shaders/Post processing/Frac"));

                return material;
            }
        }

        [SerializeField]
        private float scale = 2.0f;

        public override void Setup(OutlineParameters parameters, RenderTargetIdentifier current, RenderTargetIdentifier target)
        {
            parameters.Buffer.SetGlobalFloat(scalerHash, scale);
            parameters.ControlProvider.Blit(current, target, Material, -1);
        }

#if UNITY_EDITOR
        public override void DrawEditor(Rect position, Action serializationCallback)
        {
            base.DrawEditor(position, serializationCallback);

            position.y += base.CalculateEditorHeight() + EditorGUIUtility.standardVerticalSpacing;
            position.height = EditorGUIUtility.singleLineHeight;

            Indent(ref position);

            var newScale = EditorGUI.FloatField(position, new GUIContent("Scale"), scale);
            scale = Mathf.Clamp(newScale, -1.0f, 15.0f);
        }

        public override float CalculateEditorHeight()
        {
            return base.CalculateEditorHeight() + EditorGUIUtility.singleLineHeight * 2.0f;
        }
#endif
    }
}