using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordHellModeScript : MonoBehaviour {

    GameModeHandler gameModeHandler;
    SpawnController spawnController;
    WordController wordController;

    public TMP_Text typoText;
    float wordSpawnInterval = 0.7f;

    void Start() {
        spawnController = GetComponent<SpawnController>();
        gameModeHandler = GameObject.Find("GameModeHandler").GetComponent<GameModeHandler>();
    }

    public void StartGame() {
        StartCoroutine(spawnController.StartSpawningRocks(wordSpawnInterval));
        StartCoroutine("SetGameTime");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Insert))
            EndGame();

    }

    IEnumerator SetGameTime() {
        while(GameManager.Instance.gameActive) {
            gameModeHandler.gameTime += Time.deltaTime;
            GameManager.Instance.gameTimerText.text = "Time: " + gameModeHandler.gameTime.ToString("F0") + " seconds";
            yield return null;
        }
    }

    public void EndGame() {
        GameManager.Instance.gameActive = false;

        PlayerInputManager input = GameObject.Find("GameManager").GetComponent<PlayerInputManager>();
        GameObject inputField = input.inputField.gameObject;
        inputField.SetActive(false);
        typoText.text = "";
        GameManager.Instance.victoryPanel.SetActive(true);
        GameManager.Instance.victoryHeaderPanel.SetActive(true);
        GameManager.Instance.continueButton.onClick.AddListener(GameManager.Instance.ReturnToMainMenu);

        StartCoroutine(GameManager.Instance.DisplayWordHellPanelEffects());
    }

}
