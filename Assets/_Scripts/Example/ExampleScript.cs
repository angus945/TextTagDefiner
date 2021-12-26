using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TextTagDefiner;

[ExecuteAlways]
[RequireComponent(typeof(Text))]
public class ExampleScript : MonoBehaviour
{

    Text text;

    [SerializeField] TextAsset exampleText = null;
    [SerializeField] TagPatternToken token = null;

    void OnEnable()
    {
        text = GetComponent<Text>();
    }
    void OnValidate()
    {
        if (exampleText == null || token == null) return;

        string content = token.ApplyToText(exampleText.text);

        text.text = content; 
    }

}
