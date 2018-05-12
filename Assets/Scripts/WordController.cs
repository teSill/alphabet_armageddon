using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;





public class WordController : MonoBehaviour {

    GameModeHandler gameModeHandler;
    SpawnController spawnController;
    DataController dataController;

    public List<string> wordsFull = new List<string>();

    public List<string> wordsUsed = new List<string>();

    public Text randomWordText;

    void Start() {
        gameModeHandler = GameObject.Find("GameModeHandler").GetComponent<GameModeHandler>();
        spawnController = GetComponent<SpawnController>();
        dataController = GetComponent<DataController>();

        if (!gameModeHandler.complexWords) {
            StartCoroutine(dataController.LoadCommon());
            Debug.Log("Loaded the common words data file.");
        } else {
            StartCoroutine(dataController.LoadComplex());
            Debug.Log("Loaded the complex words data file.");
        }
    }
	
	public void GenerateRandomWord() {
        if (wordsFull.Count > 0) {
            string randomWord = wordsFull[Random.Range(0, wordsFull.Count)];
            wordsUsed.Add(randomWord);
            randomWordText.text = randomWord;
            Debug.Log("'" + randomWord + "' was added");
        } else {
            Debug.Log("Word data has not yet been loaded.");
        }
        spawnController.SpawnRock();
    }

    public int CalculateAccuracy() {
        // Somewhere along the way 2 gets added to the incorrect/missed words... I have not yet found out why or where. temporary solution to just count -2
        float badWords = GameManager.Instance.incorrectWords + GameManager.Instance.missedWords;
        float allWords = GameManager.Instance.correctWords + badWords;

        float tempResult = (GameManager.Instance.correctWords / allWords) * 100;

        int result = Mathf.RoundToInt(tempResult);

        return result;
    }
}
