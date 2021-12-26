using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TextTagDefiner
{
    [System.Serializable]
    public class TagPatternDefine
    {
        public enum InputRuleType
        {
            None,
            Custom,
            AnyString,
            //Unsigned_Integer,
            //Unsigned_Float,
            //Signed_Integer,
            //Signed_Float,
        }
        public static readonly Dictionary<InputRuleType, string> InputRuleTables = new Dictionary<InputRuleType, string>()
        {
            [InputRuleType.None]                 = "",
            [InputRuleType.AnyString]            = "(.+?)",
            //[InputRuleType.Unsigned_Integer]     = "/[0-9]+/",
            //[InputRuleType.Unsigned_Float]       = "/[.0-9]+/",
            //[InputRuleType.Signed_Integer]       = "/[.0-9]+/",
            //[InputRuleType.Signed_Float]         = "/[-.0-9]+/",
        };

        [SerializeField] string tagKeyword;
        [SerializeField] InputRuleType ruleType;
        [SerializeField] string customPattern;

        public string keyword { get => tagKeyword; }
        public InputRuleType type { get => ruleType; }
        public string inputRule
        {
            get
            {
                switch (ruleType)
                {
                    case InputRuleType.Custom:
                        return customPattern;

                    default:
                        return InputRuleTables[ruleType];
                }
            }
        }
        public string patternRegex { get => $"<{tagKeyword} {inputRule}/>"; }

        public bool isValidPattern
        {
            get
            {
                if (string.IsNullOrEmpty(patternRegex)) return false;
                if (string.IsNullOrWhiteSpace(patternRegex)) return false;

                try
                {
                    Regex.Match("", patternRegex);
                }
                catch (ArgumentException)
                {
                    return false;
                }

                return true;
            }
        }

        public TagPatternDefine()
        {
            tagKeyword = "typing keyword";
            ruleType = InputRuleType.AnyString;
            customPattern = "";
        }

        public bool PatternMatches(string input, out MatchCollection matchCollection)
        {
            matchCollection = isValidPattern ? Regex.Matches(input, patternRegex) : null;
            return isValidPattern;
        }
        public string PickInput(string match)
        {
            return match.Replace($"<{keyword} ", "").Replace("/>", "");
        }

    }

}
