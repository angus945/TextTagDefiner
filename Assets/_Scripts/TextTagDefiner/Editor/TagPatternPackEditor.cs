using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using EditorToolket;

namespace TextTagDefiner
{
    [CustomEditor(typeof(TagPatternPack))]
    public class TagPatternPackEditor : Editor
    {

        ReorderableList list;
        TagPatternPack tokenPack;

        void OnEnable()
        {
            tokenPack = (TagPatternPack)target;

            InitialPatternListGUI();
        }
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            this.DrawScriptLine<TagPatternPack>();

            list.DoLayoutList();

            PatternPreviewEditor.DrawPatternPreivew(tokenPack.tokens);
        }

        void InitialPatternListGUI()
        {
            SerializedProperty patternTokens = serializedObject.FindProperty("patternTokens");
            list = new ReorderableList(serializedObject, patternTokens, true, true, true, true);
            list.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Pattern Tokens");
            };
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = patternTokens.GetArrayElementAtIndex(index);
                TagPatternToken token = tokenPack.tokens[index];

                GetTokenContentAndColor(token, out string content, out Color color);
                EditorStyles.label.normal.textColor = color;
                EditorGUI.PropertyField(rect, element, new GUIContent(content));
                EditorStyles.label.normal.textColor = Color.black;
            };
        }
        void GetTokenContentAndColor(TagPatternToken token, out string content, out Color color)
        {
            if (token != null)
            {
                content = token.patternDefine.patternRegex;
                color = token.patternDefine.isValidPattern? Color.black : Color.red;
            }
            else
            {
                content = "------";
                color = Color.gray;
            }
        }
    }
}
