using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class HighscoreUIManager : MonoBehaviour {

    DownloadHighscoreData downloadHighscores;

    // Normal word setting
	public GameObject compModePanel;
    public GameObject hardModePanel;
    public GameObject hellModePanel;

    // Complex word setting
    public GameObject complexCompModePanel;
    public GameObject complexHardModePanel;
    public GameObject complexHellModePanel;

    public GameObject[] allPanels;

    public TMP_Text complexButtonWordText;

    public bool complexWords;

    private void Start() {
        downloadHighscores = GameObject.Find("Highscores").GetComponent<DownloadHighscoreData>();
    }

    public void ToggleComplexWords() {
        if (!complexWords) {
            complexButtonWordText.text = "V";
            complexButtonWordText.color = Color.green;
            complexWords = true;
        } else {
            complexButtonWordText.text = "X";
            complexButtonWordText.color = Color.red;
            complexWords = false;
        }
        foreach(GameObject panel in allPanels) {
            panel.SetActive(false);
        }
    }

    public void OnOpenCompPanel() {
        if (complexWords) {
            complexCompModePanel.SetActive(true);
            complexHardModePanel.SetActive(false);
            complexHellModePanel.SetActive(false);
        } else {
            compModePanel.SetActive(true);
            hardModePanel.SetActive(false);
            hellModePanel.SetActive(false);
        }
    }

    public void OnOpenHardPanel() {
        if (complexWords) {
            complexCompModePanel.SetActive(false);
            complexHardModePanel.SetActive(true);
            complexHellModePanel.SetActive(false);
        } else {
            compModePanel.SetActive(false);
            hardModePanel.SetActive(true);
            hellModePanel.SetActive(false);
        }
    }

    public void OnOpenHellPanel() {
        if (complexWords) {
            complexCompModePanel.SetActive(false);
            complexHardModePanel.SetActive(false);
            complexHellModePanel.SetActive(true);
        } else {
            compModePanel.SetActive(false);
            hardModePanel.SetActive(false);
            hellModePanel.SetActive(true);
        }
    }
}
