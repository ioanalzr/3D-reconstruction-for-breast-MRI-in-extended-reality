using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SpeechToText : MonoBehaviour
{
    public Text speechText;
    private KeywordRecognizer keywordRecognizer;

    private void Start()
    {
        string[] keywords = new string[] { "start"};
        keywordRecognizer = new KeywordRecognizer(keywords);
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        speechText.text = args.text;
    }
}
