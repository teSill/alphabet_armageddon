using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataController : MonoBehaviour {

    WordController wordController;

	// Have to load data correctly before calling Start in other classes, so we use Awake here.
	void Awake () {
		wordController = GetComponent<WordController>();

	}
	
	FileInfo dataFile;

    StreamReader reader;

    string commonPath = "";
    string complexPath = "";
    public string commonResult = "";
    string complexResult = "";

    public IEnumerator LoadCommon() {
        commonPath = Application.streamingAssetsPath + "/english_words.txt";

        if (commonPath.Contains("://")) {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(commonPath);
            yield return www.SendWebRequest();
            commonResult = www.downloadHandler.text;
        } else {
            commonResult = File.ReadAllText(commonPath);
        }
        LoadCommonWordData();
    }

    public IEnumerator LoadComplex() {
        complexPath = Application.streamingAssetsPath + "/hard_words.txt";

        if (complexPath.Contains("://")) {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(complexPath);
            yield return www.SendWebRequest();
            complexResult = www.downloadHandler.text;
        } else {
            complexResult = File.ReadAllText(complexPath);
        }
        LoadComplexWordData();
    }

    public void LoadCommonWordData() {
        string[] allLines = commonResult.Split(new string[] { "\r","\n" },
             StringSplitOptions.RemoveEmptyEntries);
        
        // Only add words with more than 2 letters to our list
        for(int i = 0; i < allLines.Length; i++) {
            if (allLines[i].Length > 2) {
                wordController.wordsFull.Add(allLines[i]);
            }
        }
    }

    public void LoadComplexWordData() {
        string[] allLines = complexResult.Split(new string[] { "\r","\n" },
             StringSplitOptions.RemoveEmptyEntries);
        
        // Only add words with more than 2 letters to our list
        for(int i = 0; i < allLines.Length; i++) {
            if (allLines[i].Length > 2) {
                wordController.wordsFull.Add(allLines[i]);
            }
        }
    }
}
