using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Reflection;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EPOOutline
{
    public abstract class OutlinePostProcessingPass
    {
        [SerializeField]
        private bool enabled = true;

        public bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                enabled = value;
            }
        }

        [SerializeField]
        private long layerMask = -1;

        public long LayerMask
        {
            get
            {
                return layerMask;
            }
        }

        public string PassName
        {
            get
            {
                var attribute = GetType().GetCustomAttribute<OutlineEffectPassInfoAttribute>();
                if (attribute == null)
                    return GetType().Name;

                return attribute.Name;
            }
        }

        public abstract void Setup(OutlineParameters parameters, RenderTargetIdentifier current, RenderTargetIdentifier target);

#if UNITY_EDITOR
        protected void Indent(ref Rect rect)
        {
            rect.x += EditorGUIUtility.singleLineHeight;
            rect.width -= EditorGUIUtility.singleLineHeight;
        }

        protected void NextLine(ref Rect rect)
        {
            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        public virtual void DrawEditor(Rect position, Action serializationCallback)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            var namePosition = position;
            namePosition.width -= EditorGUIUtility.singleLineHeight;
            namePosition.x += EditorGUIUtility.singleLineHeight;

            var enableTogglePosition = position;
            enableTogglePosition.width = EditorGUIUtility.singleLineHeight;

            enabled = EditorGUI.Toggle(enableTogglePosition, enabled);
            EditorGUI.HelpBox(namePosition, PassName, MessageType.None);
            Indent(ref position);
            NextLine(ref position);

            if (EditorGUI.DropdownButton(position, new GUIContent("Layer"), FocusType.Passive))
            {
                var menu = new GenericMenu();

                menu.AddItem(new GUIContent("All"), layerMask == -1, () =>
                    {
                        layerMask = layerMask == -1 ? 0 : -1;
                        serializationCallback();
                    });

                for (var index = 0; index <= 63; index++)
                {
                    var capturedIndex = index;
                    var capturedMask = 1 << index;
                    menu.AddItem(new GUIContent(((index / 10) * 10).ToString() + "/" + index.ToString()), (layerMask & capturedMask) != 0, () =>
                        {
                            layerMask ^= capturedMask;
                            serializationCallback();
                        });
                }

                menu.ShowAsContext();
            }
        }

        public virtual float CalculateEditorHeight()
        {
            return EditorGUIUtility.singleLineHeight * 2.0f + EditorGUIUtility.standardVerticalSpacing; 
        }
#endif
    }
}