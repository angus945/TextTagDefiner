using EditorToolket;
using System;
using UnityEditor;
using UnityEngine;

namespace TextTagDefiner
{
    public class PatternPreviewEditor
    {
        enum PreviewType
        {
            Auto,
            Custom,
            TextAsset,
        }

        static PreviewType previewType;
        static string previewInputCustom = "typing everything";
        static TextAsset previewInputText;

        public static void DrawPatternPreivew(TagPatternToken patternToken)
        {
            DrawPatternPreivew(new TagPatternToken[] { patternToken });
        }
        public static void DrawPatternPreivew(TagPatternToken[] patternTokens)
        {
            if (patternTokens.Length == 0) return;

            string input = GetPreviewInput(patternTokens, out Action inputFieldHandler);
            string output = ApplyPreviewTexts(input, patternTokens);

            CommonEditor.DrawLayoutGroup("PattenToken Preview", "Helpbox", () =>
            {
                CommonEditor.EnumToolbar<PreviewType>(ref previewType);

                DrawPreview(input, output, inputFieldHandler);
            });
        }
        static string GetPreviewInput(TagPatternToken[] patternTokens, out Action inputFieldHandler)
        {
            inputFieldHandler = null;

            switch (previewType)
            {
                case PreviewType.Auto:
                    return GenerateAutoInput(patternTokens);
                    
                case PreviewType.Custom:
                    inputFieldHandler = () =>
                    {
                        previewInputCustom = EditorGUILayout.TextField(previewInputCustom);
                    };
                    return previewInputCustom;

                case PreviewType.TextAsset:
                    inputFieldHandler = () =>
                    {
                        previewInputText = EditorGUILayout.ObjectField(previewInputText, typeof(TextAsset), false) as TextAsset;
                    };
                    return (previewInputText != null) ? previewInputText.text : "--";
            }

            return "";
        }
        static string GenerateAutoInput(TagPatternToken[] patternTokens)
        {
            string input = "";
            for (int i = 0; i < patternTokens.Length; i++)
            {
                TagPatternToken patternToken = patternTokens[i];
                if (patternToken == null || !patternToken.patternDefine.isValidPattern) continue;

                switch (patternToken.patternDefine.type)
                {
                    case TagPatternDefine.InputRuleType.None:
                        input += $"<{patternToken.patternDefine.keyword} /> ";
                        break;

                    default:
                        input += $"<{patternToken.patternDefine.keyword} preview input babababa/> ";
                        break;

                }
            }
            return input;
        }
        static string ApplyPreviewTexts(string input, TagPatternToken[] patternTokens)
        {
            for (int i = 0; i < patternTokens.Length; i++)
            {
                TagPatternToken patternToken = patternTokens[i];
                if (patternToken != null && patternToken.patternDefine.isValidPattern)
                {
                    input = patternToken.ApplyToText(input);
                }
            }

            return input;
        }

        static void DrawPreview(string input, string output, Action inputFieldHandler)
        {
            GUIStyle wrap =  new GUIStyle() { richText = false, wordWrap = true }; 
            GUIStyle rich = new GUIStyle() { richText = true, wordWrap = true };

            CommonEditor.DrawLayoutGroup("Preview Input", "GroupBox", () =>
            {
                inputFieldHandler?.Invoke();

                GUILayout.Label(input, wrap);
            });
            CommonEditor.DrawLayoutGroup("Source Output", "GroupBox", () =>
            {
                EditorGUILayout.LabelField(output, wrap);
            });
            CommonEditor.DrawLayoutGroup("Rich Output", "GroupBox", () =>
            {
                EditorGUILayout.LabelField(output, rich);
            });

        }
    }
}
