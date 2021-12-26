using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace EditorToolket
{
    public static class CommonEditor
    {
        public static void DrawScriptLine<T>(this Editor editor) where T : UnityEngine.Object
        {
            EditorGUI.BeginDisabledGroup(true);
            switch (editor.target)
            {
                case MonoBehaviour monoBehaviour:
                    EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(monoBehaviour), typeof(T), true);
                    break;

                case ScriptableObject scriptableObject:
                    EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(scriptableObject), typeof(T), true);
                    break;
            }
            EditorGUI.EndDisabledGroup();
        }

        public static void DrawLayoutGroup(string header, string layout , Action drawElementHandler)
        {
            //HelpBox, GroupBox, window

            GUILayout.BeginVertical(header, layout);

            GUILayout.Space(20);
            drawElementHandler.Invoke();

            GUILayout.EndVertical();

            GUILayout.Space(15);
        }

        public static void EnumToolbar<T>(ref T type) where T : Enum
        {
            int index = (int)(object)type;
            type = (T)(object)GUILayout.Toolbar(index, System.Enum.GetNames(typeof(T)));
        }

        public static int GUIEnumPopup<T>(Rect rect, int index) where T : Enum
        {
            return (int)(object)EditorGUI.EnumPopup(rect, (T)(object)index);
        }
    }

}
