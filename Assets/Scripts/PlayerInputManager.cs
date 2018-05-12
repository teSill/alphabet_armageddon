using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class PlayerInputManager : MonoBehaviour {

    UploadHighscoreData uploadHighscoreData;

    WordHellModeScript wordHell;
    CompetitiveModeScript competitive;
    WordController wordController;
    SpawnController spawnController;
    GameModeHandler gameModeHandler;
    SpecialEffects specialEffects;
    SoundController sounds;

    public InputField inputField;

    TMP_Text correctWordsText;
    TMP_Text incorrectWordsText;
    [HideInInspector]
    public TMP_Text missedWordsText;

    public string wordInput;

    public GameObject highscorePanel;
    public GameObject highscoreHeader;

    public InputField playerNameInputField;

    string playerName;
    public string savedPlayerName = "SavedPlayerName";

	// Use this for initialization
	void Start () {
        if (savedPlayerName != "SavedPlayerName")
            Debug.Log(PlayerPrefs.GetString(savedPlayerName));
        inputField.ActivateInputField();

        sounds = GameObject.Find("SoundManager").GetComponent<SoundController>();
        specialEffects = GameObject.Find("TextEffectManager").GetComponent<SpecialEffects>();
        uploadHighscoreData = GameObject.Find("Highscores").GetComponent<UploadHighscoreData>();
        gameModeHandler = GameObject.Find("GameModeHandler").GetComponent<GameModeHandler>();
		wordController = transform.GetComponent<WordController>();
        spawnController = transform.GetComponent<SpawnController>();
        competitive = GetComponent<CompetitiveModeScript>();
        wordHell = GetComponent<WordHellModeScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return)) {
            wordInput = inputField.text;
            CheckInput();
        }

        // Mainly so that mouse clicks don't defocus the input field
        if (!inputField.isFocused) {
            inputField.ActivateInputField();
        }
	}

    public void SubmitScore() {
        if (PlayerPrefs.GetString(savedPlayerName).Length < 3) {
            playerName = playerNameInputField.text;
            Debug.Log("Saved player name not found. Set to: " + playerNameInputField.text);
        } else {
            playerName = PlayerPrefs.GetString(savedPlayerName);
            Debug.Log("Saved player name " + PlayerPrefs.GetString(savedPlayerName) + " was found.");
        }

        if (playerName != null && playerName.Length >= 3) {
            // Send user name and score to database
            if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.CompetitiveMode) {
                uploadHighscoreData.UploadUserData(playerName, GameManager.Instance.competitiveScore);
            } else if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.HardMode) {
                uploadHighscoreData.UploadUserData(playerName, GameManager.Instance.correctWords);
            } else { 
                uploadHighscoreData.UploadUserData(playerName, GameManager.Instance.correctWords);
            }
            PlayerPrefs.SetString(savedPlayerName, playerName);
            Debug.Log(playerName + " submitted successfully!");
            highscorePanel.SetActive(false);
            GameManager.Instance.continueButton.interactable = true;
        } else {
            highscorePanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "Please enter a name with 3 or more characters.";
        }
    }

    public void CancelScoreSubmission() {
        highscorePanel.SetActive(false);
    }

    public void CheckInput() {
        if (wordController.wordsUsed.Contains(wordInput)) {
            sounds.CorrectWord();
            // Rock prefab destroyed while inside the bonus point panel: apply bonus point and effects
            wordController.wordsUsed.Remove(wordInput);
            StartCoroutine("HandleRockDestroying");
        } else {
            Debug.Log("input was... '" + wordInput + "'");
            Debug.Log(wordController.wordsUsed.Count);
            sounds.IncorrectWord();
            GameManager.Instance.incorrectWords++;
            specialEffects.BreakCombo();
            if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.WordHellMode) {
                GameManager.Instance.gameActive = false;
                wordHell.typoText.text = "I don't think '" + wordInput + "' is the word you were looking for...";
                wordHell.Invoke("EndGame", 4f);
                foreach(GameObject activeRock in spawnController.rocksActiveInScene) {
                    Destroy(activeRock, 0.5f);
                }
            }
        }
        inputField.text = "";
        inputField.ActivateInputField();
    }

    void HandleRockDestroying() {
        if (spawnController.rocksActiveInScene != null) {
            foreach(GameObject matchingRock in spawnController.rocksActiveInScene) {
                if (matchingRock.GetComponentInChildren<Text>().text == wordInput) {
                    if (GameObject.Find("SettingsManager").GetComponent<SettingController>().particleEffects)
                        DisplayRockDestruction(matchingRock.transform.position);
                    matchingRock.SetActive(false);
                    specialEffects.IncrementCombo();
                    if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.CompetitiveMode) {
                        if (competitive.bonusPointPanel.GetComponent<Collider>().bounds.Contains(matchingRock.transform.position)) { 
                            GameManager.Instance.correctWords++;
                            competitive.ApplyBonusPoint();
                        } else {
                            GameManager.Instance.correctWords++;
                        }
                    } else {
                        GameManager.Instance.correctWords++;
                    }
                }
            }
        }
    }

    void DisplayRockDestruction(Vector3 explosionPosition) {
        GameObject destroyFX = Instantiate(specialEffects.rockDestroyEffect);
        destroyFX.transform.position = explosionPosition;

        Destroy(destroyFX, 2f);
    }

    IEnumerator SetRockInactive(GameObject rock) {
        yield return new WaitForSeconds(0.1f);
        if (rock != null)
            rock.SetActive(false);
    }
}
