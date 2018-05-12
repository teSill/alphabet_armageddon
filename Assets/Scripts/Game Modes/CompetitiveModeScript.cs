using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompetitiveModeScript : MonoBehaviour {

	GameModeHandler gameModeHandler;
    SpawnController spawnController;
    WordController wordController;

    public GameObject bonusPointPanel;
    public TMP_Text bonusPointText;

    public int bonusPoints;

    float wordSpawnInterval = 0.9f;

    public bool applyBonusPoints;

    void Start() {
        spawnController = GetComponent<SpawnController>();
        gameModeHandler = GameObject.Find("GameModeHandler").GetComponent<GameModeHandler>();
        bonusPointPanel = GameObject.Find("BonusPointPanel");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Insert))
            EndGame();
    }

    public void StartGame() {
        StartCoroutine(spawnController.StartSpawningRocks(wordSpawnInterval));
        StartCoroutine("SetGameTime");
    }

    public void ApplyBonusPoint() {
        Vector3 spawnPos = GameObject.Find("TextSpawnPoint").transform.position;
        bonusPoints++;

        // Text animation
        GameObject bonusPointTextObject = Instantiate(bonusPointText.gameObject);
        bonusPointTextObject.transform.SetParent(GameObject.Find("RockCanvas").transform);
        bonusPointTextObject.transform.position = spawnPos;
        StartCoroutine(MoveBonusText(bonusPointTextObject));
    }

    IEnumerator MoveBonusText(GameObject text) {
        Destroy(text, 2f);
        while(text != null) {
            text.transform.Translate(0f, 3f * Time.deltaTime, 0f);
            yield return null;
        }
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
