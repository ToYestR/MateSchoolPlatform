using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EPOOutline
{
    public static class EPOEditorUtility
    {
        public static ReorderableList CreateReorderableList(SerializedProperty items)
        {
            var postProcessingList = new ReorderableList(items.serializedObject, items, true, true, true, true);

            postProcessingList.drawHeaderCallback = position =>
                {
                    EditorGUI.LabelField(position, new GUIContent("After carve post processing passes"));
                };

            postProcessingList.onAddCallback = list =>
                {
                    var menu = new GenericMenu();

                    var types = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                from assemblyType in domainAssembly.GetTypes()
                                where typeof(OutlinePostProcessingPass).IsAssignableFrom(assemblyType) && !assemblyType.IsAbstract
                                select assemblyType).ToArray();

                    foreach (var type in types)
                    {
                        var attr = type.GetCustomAttribute<OutlineEffectPassInfoAttribute>();

                        menu.AddItem(new GUIContent(attr == null ? type.Name : attr.Name), false, () =>
                           {
                               var effect = (OutlinePostProcessingPass)Activator.CreateInstance(type);

                               var last = items.arraySize;
                               items.InsertArrayElementAtIndex(last);
                               var element = items.GetArrayElementAtIndex(last);
                               element.stringValue = SerializationUtility.Serialize(effect);

                               items.serializedObject.ApplyModifiedProperties();
                           });
                    }

                    menu.ShowAsContext();
                };

            postProcessingList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var item = items.GetArrayElementAtIndex(index);
                    var deserialized = SerializationUtility.Deserialize<OutlinePostProcessingPass>(item.stringValue);

                    Action serializationAction = () =>
                        {
                            var serialized = SerializationUtility.Serialize(deserialized);
                            if (serialized != item.stringValue)
                            {
                                item.stringValue = serialized;
                                items.serializedObject.ApplyModifiedProperties();
                            }
                        };

                    deserialized.DrawEditor(rect, serializationAction);
                    serializationAction();
                };

            postProcessingList.elementHeightCallback = index =>
                {
                    var item = items.GetArrayElementAtIndex(index);
                    var deserialized = SerializationUtility.Deserialize<OutlinePostProcessingPass>(item.stringValue);

                    return deserialized.CalculateEditorHeight();
                };

            postProcessingList.onRemoveCallback = (list) =>
                {
                    items.DeleteArrayElementAtIndex(list.index);

                    items.serializedObject.ApplyModifiedProperties();
                };

            return postProcessingList;
        }
    }
}