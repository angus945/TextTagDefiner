using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TextTagDefiner
{
    [CreateAssetMenu(fileName = "new TagPattern (Token)", menuName = "TagableTexts/PatternToken")]
    public class TagPatternToken : ScriptableObject
    {
        [SerializeField] TagPatternDefine tagPatternDefine = new TagPatternDefine();
        [SerializeField] TagOutputDefine tagOutputDefine = new TagOutputDefine();
        public TagPatternDefine patternDefine { get => tagPatternDefine; }
        public TagOutputDefine outputDefine { get => tagOutputDefine; }

        public string ApplyToText(string input)
        {
            return ApplyToText(input, tagOutputDefine.DefaultReplaceHandler, null);
        }
        public string ApplyToText(string input, Action<string> inputListener)
        {
            return ApplyToText(input, tagOutputDefine.DefaultReplaceHandler, inputListener);
        }
        public string ApplyToText(string input, Func<string, string, string> replaceHandler)
        {
            return ApplyToText(input, replaceHandler, null);
        }
        public string ApplyToText(string input, Func<string, string, string> replaceHandler, Action<string> inputListener)
        {
            if (string.IsNullOrEmpty(input)) return input;

            if (!tagPatternDefine.PatternMatches(input, out MatchCollection matchCollection)) return input;

            return tagOutputDefine.ApplyOutput(input, matchCollection, tagPatternDefine.PickInput, replaceHandler, inputListener);
        }
    }
}
