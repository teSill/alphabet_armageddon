using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadHighscoreData : MonoBehaviour {

    DownloadHighscoreData downloadData;
    GameModeHandler gameModeHandler;

    string uploadURL = "";

    void Awake() {
        gameModeHandler = GameObject.Find("GameModeHandler").GetComponent<GameModeHandler>();
        downloadData = GetComponent<DownloadHighscoreData>();
        DontDestroyOnLoad(gameObject);
    }

    public void UploadUserData(string username, int score) {
        WWWForm form = new WWWForm();
        form.AddField("playernamePost", username);
        form.AddField("scorePost", score);

        if (gameModeHandler.complexWords)
            form.AddField("complexPost", "on");

        // Send data to the correct database
        if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.CompetitiveMode) {
            form.AddField("gamemodePost", "comp");
        } else if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.HardMode) {
            form.AddField("gamemodePost", "hard");
        } else if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.WordHellMode) {
            form.AddField("gamemodePost", "hell");
        } else {
            Debug.Log("Error selecting game mode database.");
            return;
        }

        WWW www = new WWW(uploadURL, form);

        Debug.Log("Uploaded " + username + " with score of " + score);
    }

}
