using EditorToolket;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TextTagDefiner
{
    [CustomEditor(typeof(TagPatternToken))]
    public class TagPatternTokenEditor : Editor
    {

        TagPatternToken patternToken;
        ReorderableList replaceList;

        void OnEnable()
        {
            patternToken = (TagPatternToken)target;

            InitialReplaceList();
        }

        public override void OnInspectorGUI()
        {
            this.DrawScriptLine<TagPatternToken>();

            serializedObject.Update();

            DrawTagDetectGUI();
            DrawOutputSetting();

            serializedObject.ApplyModifiedProperties();

            PatternPreviewEditor.DrawPatternPreivew(patternToken);
        }

        void DrawTagDetectGUI()
        {
            CommonEditor.DrawLayoutGroup("Tag Define", "Helpbox", () =>
            {
                SerializedProperty tagPatternDefine = serializedObject.FindProperty("tagPatternDefine");

                SerializedProperty tagKeyword = tagPatternDefine.FindPropertyRelative("tagKeyword");
                SerializedProperty ruleType = tagPatternDefine.FindPropertyRelative("ruleType");
                SerializedProperty customPattern = tagPatternDefine.FindPropertyRelative("customPattern");

                EditorGUILayout.PropertyField(tagKeyword);
                EditorGUILayout.PropertyField(ruleType);
                if(ruleType.intValue == 1)
                {
                    GUI.color = patternToken.patternDefine.isValidPattern ? Color.white : Color.red;
                    EditorGUILayout.PropertyField(customPattern);
                    GUI.color = Color.white;
                }

                EditorGUILayout.LabelField("Regex Pattern", patternToken.patternDefine.patternRegex);
            });
        }
        void DrawOutputSetting()
        {
            CommonEditor.DrawLayoutGroup("Pattern Output", "Helpbox", () =>
            {
                SerializedProperty tagOutputDefine = serializedObject.FindProperty("tagOutputDefine");

                replaceList.DoLayoutList();

                EditorGUILayout.LabelField("full output : " + patternToken.outputDefine.output);
            });
        }
        void InitialReplaceList()
        {
            SerializedProperty replaces = serializedObject.FindProperty("tagOutputDefine").FindPropertyRelative("replaces");

            replaceList = new ReorderableList(serializedObject, replaces);
            replaceList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Output");
            };
            replaceList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFoucsed) =>
            {
                rect.y += 2;

                SerializedProperty element = replaces.GetArrayElementAtIndex(index);

                SerializedProperty type = element.FindPropertyRelative("type");
                SerializedProperty word = element.FindPropertyRelative("word");

                Rect enumPopupRect = new Rect(rect);
                enumPopupRect.width *= 0.48f;
                type.intValue = CommonEditor.GUIEnumPopup<TagOutputDefine.ReplaceType>(enumPopupRect, type.intValue);

                Rect outputFieldRect = new Rect(rect);
                outputFieldRect.width *= 0.48f;
                outputFieldRect.x += rect.width * 0.5f;
                outputFieldRect.height = 20;
                switch ((TagOutputDefine.ReplaceType)type.intValue)
                {
                    case TagOutputDefine.ReplaceType.Custom:
                        word.stringValue = EditorGUI.TextField(outputFieldRect, word.stringValue);
                        break;

                    case TagOutputDefine.ReplaceType.TagInput:
                        if (patternToken.patternDefine.type == TagPatternDefine.InputRuleType.None)
                        {
                            EditorGUI.LabelField(outputFieldRect, "<color=\"red\"> tag pattern does not support input </color>", new GUIStyle() { richText = true });
                        }
                        else EditorGUI.LabelField(outputFieldRect, TagOutputDefine.inputIdentifier);
                        //GUI.contentColor = Color.black;
                        break;
                }
            };

        }
    }
}
