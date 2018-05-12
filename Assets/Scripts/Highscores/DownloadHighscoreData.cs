using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DownloadHighscoreData : MonoBehaviour { 

    //normal highscores
    string compPlayerDataURL = "http://www.alphabet-armageddon.com/highscores/CompPlayerData.php";
    string hardPlayerDataURL = "http://www.alphabet-armageddon.com/highscores/HardPlayerData.php";
    string hellPlayerDataURL = "http://www.alphabet-armageddon.com/highscores/HellPlayerData.php";

    //highscores with complex word setting
    string compComplexPlayerDataURL = "http://www.alphabet-armageddon.com/highscores/CompPlayerDataComplex.php";
    string hardComplexPlayerDataURL = "http://www.alphabet-armageddon.com/highscores/HardPlayerDataComplex.php";
    string hellComplexPlayerDataURL = "http://www.alphabet-armageddon.com/highscores/HellPlayerDataComplex.php";

    public string[] compPlayers;
    public string[] hardPlayers;
    public string[] hellPlayers;

    public string[] complexCompPlayers;
    public string[] complexHardPlayers;
    public string[] complexHellPlayers;

    public GameObject[] compModePlayers;
    public GameObject[] hardModePlayers;
    public GameObject[] hellModePlayers;

    public GameObject[] complexCompModePlayers;
    public GameObject[] complexHardModePlayers;
    public GameObject[] complexHellModePlayers;

    private void OnEnable() {
        BeginDownloadingData();
    }

    // Use this for initialization
    private void Update() {
        if (Input.GetKeyDown(KeyCode.F5) && GameObject.Find("UIManager").GetComponent<UIManager>().highscorePanel.activeSelf) {
            BeginDownloadingData();
            Debug.Log("Reloading data.");
        }
    }

    public void BeginDownloadingData() {
        if (SceneManager.GetActiveScene().name != "MainMenu" && GameObject.Find("UIManager").GetComponent<UIManager>().highscorePanel.activeSelf)  {
            Debug.Log("Tried to load and display data but we aren't in the correct scene.");
            return;
        }
        // Regular word mode
        StartCoroutine("DownloadCompData");
        StartCoroutine("DownloadHardData");
        StartCoroutine("DownloadHellData");

        // Complex word mode
        StartCoroutine("DownloadComplexCompData");
        StartCoroutine("DownloadComplexHardData");
        StartCoroutine("DownloadComplexHellData");
    }

    /* *
     *  COMPETITIVE DATA START
     * */ 

    IEnumerator DownloadCompData() {
        WWW playerData = new WWW (compPlayerDataURL);
        yield return playerData;
        string playerDataString = playerData.text;
        compPlayers = playerDataString.Split(';');
        
        InsertLoadedCompData();
    }

    void InsertLoadedCompData() {
        for (int i = 0; i < compPlayers.Length; i++) {
            compModePlayers[i].GetComponent<TMP_Text>().text = i + 1 + ".";
            if (compPlayers.Length >= i) {
                if (compPlayers[i] != "" && compModePlayers[i] != null) {
                    compModePlayers[i].GetComponent<TMP_Text>().text += GetDataValue(compPlayers[i], "Name") + ":" + GetDataValue(compPlayers[i], "Score");
                }
            }
        }
    }

    IEnumerator DownloadComplexCompData() {
        WWW playerData = new WWW (compComplexPlayerDataURL);
        yield return playerData;
        string playerDataString = playerData.text;
        complexCompPlayers = playerDataString.Split(';');
        InsertLoadedComplexCompData();
    }

    void InsertLoadedComplexCompData() {
        for (int i = 0; i < complexCompPlayers.Length; i++) {
            complexCompModePlayers[i].GetComponent<TMP_Text>().text = i + 1 + ".";
            if (complexCompPlayers.Length >= i) {
                if (complexCompPlayers[i] != "" && complexCompModePlayers[i] != null)
                    complexCompModePlayers[i].GetComponent<TMP_Text>().text += GetDataValue(complexCompPlayers[i], "Name") + ":" + GetDataValue(complexCompPlayers[i], "Score");
            }
        }
    }

    /* *
     *  COMPETITIVE DATA END
     *  HARD DATA START
     * */ 

    IEnumerator DownloadHardData() {
        WWW playerData = new WWW (hardPlayerDataURL);
        yield return playerData;
        string playerDataString = playerData.text;
        hardPlayers = playerDataString.Split(';');
        InsertLoadedHardData();
    }

    void InsertLoadedHardData() {
        for (int i = 0; i < hardPlayers.Length; i++) {
            hardModePlayers[i].GetComponent<TMP_Text>().text = i + 1 + ".";
            if (hardPlayers.Length >= i) {
                if (hardPlayers[i] != "" && hardModePlayers[i] != null)
                    hardModePlayers[i].GetComponent<TMP_Text>().text += GetDataValue(hardPlayers[i], "Name") + ":" + GetDataValue(hardPlayers[i], "Score");
            }
        }
    }

    IEnumerator DownloadComplexHardData() {
        WWW playerData = new WWW (hardComplexPlayerDataURL);
        yield return playerData;
        string playerDataString = playerData.text;
        complexHardPlayers = playerDataString.Split(';');
        InsertLoadedComplexHardData();
    }

    void InsertLoadedComplexHardData() {
        for (int i = 0; i < complexHardPlayers.Length; i++) {
            complexHardModePlayers[i].GetComponent<TMP_Text>().text = i + 1 + ".";
            if (complexHardPlayers.Length >= i) {
                if (complexHardPlayers[i] != "" && complexHardModePlayers[i] != null)
                    complexHardModePlayers[i].GetComponent<TMP_Text>().text += GetDataValue(complexHardPlayers[i], "Name") + ":" + GetDataValue(complexHardPlayers[i], "Score");
            }
        }
    }

    /* *
     *  HARD DATA END
     *  HELL DATA START
     * */ 

    IEnumerator DownloadHellData() {
        WWW playerData = new WWW (hellPlayerDataURL);
        yield return playerData;
        string playerDataString = playerData.text;
        hellPlayers = playerDataString.Split(';');
        InsertLoadedHellData();
    }

    void InsertLoadedHellData() {
        for (int i = 0; i < hellPlayers.Length; i++) {
            hellModePlayers[i].GetComponent<TMP_Text>().text = i + 1 + ".";
            if (hellPlayers.Length >= i) {
                if (hellPlayers[i] != "" && hellModePlayers[i] != null)
                    hellModePlayers[i].GetComponent<TMP_Text>().text += GetDataValue(hellPlayers[i], "Name") + ":" + GetDataValue(hellPlayers[i], "Score");
            }
        }
    }

    IEnumerator DownloadComplexHellData() {
        WWW playerData = new WWW (hellComplexPlayerDataURL);
        yield return playerData;
        string playerDataString = playerData.text;
        complexHellPlayers = playerDataString.Split(';');
        InsertLoadedComplexHellData();
    }

    void InsertLoadedComplexHellData() {
        for (int i = 0; i < complexHellPlayers.Length; i++) {
            complexHellModePlayers[i].GetComponent<TMP_Text>().text = i + 1 + ".";
            if (complexHellPlayers.Length >= i) {
                if (complexHellPlayers[i] != "" && complexHellModePlayers[i] != null)
                    complexHellModePlayers[i].GetComponent<TMP_Text>().text += GetDataValue(complexHellPlayers[i], "Name") + ":" + GetDataValue(complexHellPlayers[i], "Score");
            }
        }
    }


    /* *
     *  HELL DATA END
     * */ 

    string GetDataValue(string data, string index) {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }
}
