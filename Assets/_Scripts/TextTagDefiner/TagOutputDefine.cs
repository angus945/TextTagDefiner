using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TextTagDefiner
{
    [System.Serializable]
    public class TagOutputDefine
    {
        public enum ReplaceType
        { 
            Custom,        
            TagInput,
        }

        [System.Serializable]
        public struct OutputReplace
        {
            public ReplaceType type;
            public string word;
        }

        public const string inputIdentifier = "<input/>";

        [SerializeField] OutputReplace[] replaces;
        public string output
        {
            get
            {
                string output = "";
                for (int i = 0; i < replaces.Length; i++)
                {
                    OutputReplace replace = replaces[i];
                    switch (replace.type)
                    {
                        case ReplaceType.Custom:
                            output += replace.word;
                            break;

                        case ReplaceType.TagInput:
                            output += inputIdentifier;
                            break;
                    }
                }
                return output;
            }
        }

        public TagOutputDefine()
        {
            this.replaces = new OutputReplace[3];
        }

        public string ApplyOutput(string inputText, MatchCollection matches, Func<string, string> pickInputHandler, Func<string, string, string> replaceHandler, Action<string> inputListener)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                string match = matches[i].ToString();
                string tagInput = pickInputHandler.Invoke(match);

                string replace = replaceHandler.Invoke(match, tagInput);
                inputText = inputText.Replace(match, replace);

                inputListener?.Invoke(tagInput);
            }

            return inputText;
        }
        public string DefaultReplaceHandler(string match, string tagInput)
        {
            return match.Replace(match, output).Replace(inputIdentifier, tagInput);
        }
    }
}
