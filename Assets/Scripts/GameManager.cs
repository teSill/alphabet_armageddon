using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    GameModeHandler gameModeHandler;
    SpawnController spawnController;
    WordController wordController;
    CasualModeScript casualScript;
    CompetitiveModeScript competitiveScript;
    HardModeScript hardScript;
    WordHellModeScript wordHellScript;

    SpecialEffects specialEffects;
    TextEffects textEffects;

    public TMP_Text countdownText;
    public TMP_Text gameTimerText;

    // Victory panel objects
    public GameObject victoryPanel;
    public GameObject victoryHeaderPanel;
  
    public TMP_Text correctWordsText;
    public TMP_Text incorrectWordsText;
    public TMP_Text accuracyText;
    public TMP_Text scoreText;
    public TMP_Text highscoreText;
    public TMP_Text bonusPointText; // Comp mode only
    public Button continueButton;

    public bool gameActive;

    public int correctWords;
    public int incorrectWords;
    public int missedWords;
    public int scoreHighscore;

    public string savedCasualHighscore = "SavedCasualHighscore";
    public string savedComplexCasualHighscore = "SavedComplexCasualHighscore";

    // Competitive mode
    public int competitiveScore;
    public int competitiveHighScore;
    public string savedCompHighscore = "SavedCompHighscore";
    public string savedComplexCompHighscore = "SavedComplexCompHighscore";

    // Hard mode
    public int hardScore;
    public int hardHighscore;
    public string savedHardHighscore = "SavedHardHighscore";
    public string savedComplexHardHighscore = "SavedComplexHardHighscore";

    // Bullet hell
    public int wordHellScore;
    public int wordHellHighScore;
    public string savedHellHighscore = "SavedHellHighscore";
    public string savedComplexHellHighscore = "SavedComplexHellHighscore";

    float timeBetweenVictoryScreenLines = 1f;

    static GameManager instance;

    public static GameManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<GameManager>();
                if (instance == null) {
                    instance = new GameObject().AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        casualScript = GetComponent<CasualModeScript>();
        competitiveScript = GetComponent<CompetitiveModeScript>();
        hardScript = GetComponent<HardModeScript>();
        spawnController = GetComponent<SpawnController>();
        wordController = GetComponent<WordController>();
        wordHellScript = GetComponent<WordHellModeScript>();
        textEffects = GameObject.Find("TextEffectManager").GetComponent<TextEffects>();
        specialEffects = GameObject.Find("TextEffectManager").GetComponent<SpecialEffects>();

        if (SceneManager.GetActiveScene().name != "MainMenu") {
            gameModeHandler = GameObject.Find("GameModeHandler").GetComponent<GameModeHandler>();
            StartCoroutine("CountdownGameStart");
        }
    }

    private void Update() {
        if (textEffects.isRunning)
            DisplayVictoryTextEffects();
        
        // Remove the waiting time between text appearing upon pressing enter in victory screen
        if (Input.GetKeyDown(KeyCode.Return) && victoryPanel.activeSelf)
            timeBetweenVictoryScreenLines = 0f;
        if (Input.GetKeyDown(KeyCode.Escape)) 
            ReturnToMainMenu();
    }

    public IEnumerator CountdownGameStart() {
        if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.WordHellMode)
            gameTimerText.text = "Time: " + gameModeHandler.gameTime + " seconds";
        else
            gameTimerText.text = "Time Left: " + gameModeHandler.gameTime;

        countdownText.text = "Game starting in... 3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "Game starting in... 2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "Game starting in... 1";
        yield return new WaitForSeconds(1f);
        countdownText.text = "GOOD LUCK!";
        yield return new WaitForSeconds(1f);
        countdownText.text = "";

        gameActive = true;

        if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.CasualMode)
            casualScript.StartGame();
        else if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.CompetitiveMode)
            competitiveScript.StartGame();
        else if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.HardMode)
            hardScript.StartGame();
        else if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.WordHellMode)
            wordHellScript.StartGame();
    }

    public IEnumerator DisplayWordHellPanelEffects() {
        wordHellScore = correctWords;
        GetHighscoreGameMode();
        
        StartCoroutine(textEffects.DoFancyTextEffect(scoreText, "Score: ", wordHellScore));
        yield return new WaitForSeconds(timeBetweenVictoryScreenLines);

        if (!gameModeHandler.complexWords)
            StartCoroutine(textEffects.DoFancyTextEffect(highscoreText, "Highscore: ", PlayerPrefs.GetInt(savedHellHighscore)));
        else
            StartCoroutine(textEffects.DoFancyTextEffect(highscoreText, "Highscore: ", PlayerPrefs.GetInt(savedComplexHellHighscore)));
        yield return new WaitForSeconds(timeBetweenVictoryScreenLines);

        PlayerInputManager input = GetComponent<PlayerInputManager>();

        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(continueButton.gameObject);
    }


    public IEnumerator DisplayVictoryTextEffects() {
        GetHighscoreGameMode();
        
        // Correct words
        StartCoroutine(textEffects.DoFancyTextEffect(correctWordsText, "Correct words: ", correctWords));
        yield return new WaitForSeconds(timeBetweenVictoryScreenLines);

        // Bonus points (comp mode only)
        if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.CompetitiveMode) {
            StartCoroutine(textEffects.DoFancyTextEffect(bonusPointText, "Bonus points: ", competitiveScript.bonusPoints));
            yield return new WaitForSeconds(timeBetweenVictoryScreenLines);
        }

        // Incorrect & missed words
        StartCoroutine(textEffects.DoFancyTextEffect(incorrectWordsText, "Incorrect & Missed words: ", incorrectWords + missedWords));
        yield return new WaitForSeconds(timeBetweenVictoryScreenLines + 1f);

        // Accuracy
        if (correctWords > 0)
            StartCoroutine(textEffects.DoFancyTextEffect(accuracyText, "Accuracy: ", wordController.CalculateAccuracy()));
        else
            accuracyText.text = "Accuracy: 0%";

        yield return new WaitForSeconds(timeBetweenVictoryScreenLines);

        // Score
        if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.CompetitiveMode)
            StartCoroutine(textEffects.DoFancyTextEffect(scoreText, "Score: ", competitiveScore));
        else
            StartCoroutine(textEffects.DoFancyTextEffect(scoreText, "Score: ", correctWords));
        yield return new WaitForSeconds(timeBetweenVictoryScreenLines);

        if (!gameModeHandler.complexWords) {
            if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.CasualMode) {
                StartCoroutine(textEffects.DoFancyTextEffect(highscoreText, "Highscore: ", PlayerPrefs.GetInt(savedCasualHighscore)));
            } else if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.CompetitiveMode) {
                StartCoroutine(textEffects.DoFancyTextEffect(highscoreText, "Highscore: ", PlayerPrefs.GetInt(savedCompHighscore)));
            } else if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.HardMode) {
                StartCoroutine(textEffects.DoFancyTextEffect(highscoreText, "Highscore: ", PlayerPrefs.GetInt(savedHardHighscore)));
            }
        } else {
            if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.CasualMode) {
                StartCoroutine(textEffects.DoFancyTextEffect(highscoreText, "'Complex' Highscore: ", PlayerPrefs.GetInt(savedComplexCasualHighscore)));
            } else if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.CompetitiveMode) {
                StartCoroutine(textEffects.DoFancyTextEffect(highscoreText, "'Complex' Highscore: ", PlayerPrefs.GetInt(savedComplexCompHighscore)));
            } else if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.HardMode) {
                StartCoroutine(textEffects.DoFancyTextEffect(highscoreText, "'Complex' Highscore: ", PlayerPrefs.GetInt(savedComplexHardHighscore)));
            }
        }

        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(continueButton.gameObject);

        yield return null;
    }

    private void GetHighscoreGameMode() {
        if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.CasualMode) {
            if (!gameModeHandler.complexWords)
                SaveNewHighscore(correctWords, savedCasualHighscore);
            else
                SaveNewHighscore(correctWords, savedComplexCasualHighscore);
        }
        if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.CompetitiveMode) {
            competitiveScore = correctWords + competitiveScript.bonusPoints;
            if (!gameModeHandler.complexWords)
                SaveNewHighscore(competitiveScore, savedCompHighscore);
            else
                SaveNewHighscore(competitiveScore, savedComplexCompHighscore);
        }
        if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.HardMode) {
            if (!gameModeHandler.complexWords)
                SaveNewHighscore(correctWords, savedHardHighscore);
            else
                SaveNewHighscore(correctWords, savedComplexHardHighscore);
        }
        if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.WordHellMode) {
            if (!gameModeHandler.complexWords)
                SaveNewHighscore(correctWords, savedHellHighscore);
            else
                SaveNewHighscore(correctWords, savedComplexHellHighscore);
        }
    }

    private void SaveNewHighscore(int score, string savedString) {
        if (score > PlayerPrefs.GetInt(savedString)) {
            PlayerPrefs.SetInt(savedString, score);
            if (gameModeHandler.selectedGameMode != GameModeHandler.GameMode.CasualMode)
                continueButton.interactable = false;
            if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.WordHellMode)
                Invoke("ActivateNewHighscore", 2f);
            else
                Invoke("ActivateNewHighscore", 8f);
        }
    }

    void ActivateNewHighscore() {
        if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.CasualMode)
            return;

        SoundController sounds = GameObject.Find("SoundManager").GetComponent<SoundController>();
        sounds.HighscoreSound();
        PlayerInputManager input = GetComponent<PlayerInputManager>();
        SpecialEffects specialEffects = GameObject.Find("TextEffectManager").GetComponent<SpecialEffects>();
        StartCoroutine(specialEffects.DisplayHighscorePanel());
        // Use the previously entered name if it exists
        if (PlayerPrefs.GetString(input.savedPlayerName).Length >= 3) {
            input.playerNameInputField.gameObject.SetActive(false);
            input.highscorePanel.transform.GetChild(0).GetComponent<TMP_Text>().text = "Your highscore will be submitted with your previously entered name: '" + 
            PlayerPrefs.GetString(input.savedPlayerName) + "' - press Continue to proceed.";
            continueButton.interactable = true;
        }
    }
    
    public void ReturnToMainMenu() {
        SceneManager.LoadScene("MainMenu");
        MusicPlayer music = GameObject.Find("MusicManager").GetComponent<MusicPlayer>();
        if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.WordHellMode)
            music.mainAudio.UnPause();
    }
}
