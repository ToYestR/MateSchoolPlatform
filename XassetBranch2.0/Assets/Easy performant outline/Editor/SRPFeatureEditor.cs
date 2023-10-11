using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

#if SRP_OUTLINE
namespace EPOOutline
{
	[CustomPropertyDrawer(typeof(SRPOutlineFeature.Settings), true)]
    public class SRPFeatureEditor : PropertyDrawer
    {
        private ReorderableList afterCarveList;

        private void NewLine(ref Rect postion)
        {
            postion.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var renderScale = property.FindPropertyRelative("PrimaryRendererScale");
            var infoBufferScale = property.FindPropertyRelative("InfoBufferScale");
            var blurType = property.FindPropertyRelative("BlurType");
            var blurIterations = property.FindPropertyRelative("BlurIterations");
            var dilateIterations = property.FindPropertyRelative("DilateIterations");
            var blurShift = property.FindPropertyRelative("BlurShift");
            var dilateShift = property.FindPropertyRelative("DilateShift");
            var layerMask = property.FindPropertyRelative("LayerMask");

            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(position, renderScale);
            NewLine(ref position);
            EditorGUI.PropertyField(position, infoBufferScale);
            NewLine(ref position);
            EditorGUI.PropertyField(position, blurType);
            NewLine(ref position);
            EditorGUI.PropertyField(position, blurIterations);
            NewLine(ref position);
            EditorGUI.PropertyField(position, blurShift);
            NewLine(ref position);
            EditorGUI.PropertyField(position, dilateIterations);
            NewLine(ref position);
            EditorGUI.PropertyField(position, dilateShift);
            NewLine(ref position);
            EditorGUI.PropertyField(position, layerMask);

            if (afterCarveList == null)
                afterCarveList = EPOEditorUtility.CreateReorderableList(property.FindPropertyRelative("serializedAfterCarvePostProcessingPasses"));

            afterCarveList.DoList(position);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var listHeight = 0.0f;
            if (afterCarveList != null)
                listHeight += afterCarveList.GetHeight();

            return EditorGUIUtility.singleLineHeight * 8.0f + EditorGUIUtility.standardVerticalSpacing * 7.0f + listHeight;
        }
    }
}
#endif