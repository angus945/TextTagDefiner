using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextTagDefiner
{
    [CreateAssetMenu(fileName = "new TagPatternPack", menuName = "TagableTexts/PatternPack")]
    public class TagPatternPack : ScriptableObject
    {
        [SerializeField] TagPatternToken[] patternTokens = null;
        public TagPatternToken[] tokens { get => patternTokens; }

    }
}
