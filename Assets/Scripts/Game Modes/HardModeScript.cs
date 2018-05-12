using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HardModeScript : MonoBehaviour {

    GameModeHandler gameModeHandler;
    SpawnController spawnController;
    WordController wordController;

    float wordSpawnInterval = 0.8f;

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
        while(gameModeHandler.gameTime > 0) {
            gameModeHandler.gameTime -= Time.deltaTime;
            GameManager.Instance.gameTimerText.text = "Time left: " + gameModeHandler.gameTime.ToString("F0");
            yield return null;
        }
        GameManager.Instance.gameTimerText.text = "Time left: 0";
        EndGame();
    }

    void EndGame() {
        GameManager.Instance.gameActive = false;

        PlayerInputManager input = GameObject.Find("GameManager").GetComponent<PlayerInputManager>();
        GameObject inputField = input.inputField.gameObject;
        inputField.SetActive(false);

        GameManager.Instance.victoryPanel.SetActive(true);
        GameManager.Instance.victoryHeaderPanel.SetActive(true);
        GameManager.Instance.continueButton.onClick.AddListener(GameManager.Instance.ReturnToMainMenu);

        StartCoroutine(GameManager.Instance.DisplayVictoryTextEffects());
    }
}
