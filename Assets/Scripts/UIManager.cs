using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {

    GameModeHandler gameModeHandler;
    MusicPlayer musicPlayer;
    SettingController settings;

    public GameObject settingsPanel;
    public Slider volumeSlider;
    
    public TMP_Text normalWordText;
    public TMP_Text complexWordText;

    public TMP_Text casualNormalWordText;
    public TMP_Text casualComplexWordText;

    public TMP_Text tipText;
    public TMP_Text selectedModeText;

    public GameObject mainPanel;
    public GameObject confirmationPanel;
    public GameObject casualConfirmationPanel;
    public GameObject gameModePanel;
    public GameObject highscorePanel;

    public Slider gameLengthSlider;
    public TMP_Text gameLengthText;
    public TMP_Text casualGameLengthText;

    public Slider gameSpeedSlider;
    public TMP_Text gameSpeedText;

    public Button startButton;
    
    EventSystem eventSystem;

    void Start() {
        gameModeHandler = GameObject.Find("GameModeHandler").GetComponent<GameModeHandler>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        musicPlayer = GameObject.Find("MusicManager").GetComponent<MusicPlayer>();
        settings = GameObject.Find("SettingsManager").GetComponent<SettingController>();
        musicPlayer.SetSongInterface();
        DontDestroyOnLoad(gameModeHandler);
    }

    void Update() {
        // Some additions to keyboard navigation
        if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.CasualMode && confirmationPanel.activeSelf) {
            if (eventSystem.currentSelectedGameObject.name == "Slider") {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                    gameLengthSlider.value -= 9f;
                    gameLengthSlider.onValueChanged.AddListener(GetLengthSliderValue);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                    gameLengthSlider.value += 9f;
                    gameLengthSlider.onValueChanged.AddListener(GetLengthSliderValue);
                }
            } else {
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (confirmationPanel.activeSelf && !gameModePanel.activeSelf || casualConfirmationPanel.activeSelf && !gameModePanel.activeSelf) {
                CloseConfirmationPanel();
                CloseCasualConfirmationPanel();
                eventSystem.SetSelectedGameObject(GameObject.Find("CasualButton"));
            } else if (gameModePanel.activeSelf && !confirmationPanel.activeSelf) {
                CloseGameModePanel();
                eventSystem.SetSelectedGameObject(GameObject.Find("PlayButton"));
            } else if (highscorePanel.activeSelf) {
                CloseHighscores();
                eventSystem.SetSelectedGameObject(GameObject.Find("PlayButton"));
            } else if (settingsPanel.activeSelf) {
                CloseSettingsPanel();
                eventSystem.SetSelectedGameObject(GameObject.Find("PlayButton"));
            }
        }
    }

    public void OpenSettingsPanel() {
        settingsPanel.SetActive(true);
        settings.SetSettingInterface();
        volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
        volumeSlider.value = settings.volume;
        Button button = GameObject.Find("NextSongButton").GetComponent<Button>();
        button.onClick.RemoveListener(musicPlayer.SetNextSong);
        button.onClick.AddListener(musicPlayer.SetNextSong);
        volumeSlider.onValueChanged.AddListener(HandleVolume);
        musicPlayer.SetSongInterface();
    }

    public void HandleVolume(float value) {
        settings.volume = volumeSlider.value;
        Debug.Log("Volume: " + settings.volume + " - slider value: " + volumeSlider.value);
        musicPlayer.mainAudio.volume = settings.volume / 100;
    }

    public void CloseSettingsPanel() {
        settingsPanel.SetActive(false);
    }

    public void ToggleGameSounds() {
        GameObject panel = GameObject.Find("GameSoundsOnOffPanel");

        settings.gameSounds = !settings.gameSounds;
        Debug.Log("Game sounds: " + settings.gameSounds);

        settings.HandleButtonToggle(settings.gameSounds, panel);
    }

    public void ToggleParticleEffects() {
        GameObject panel = GameObject.Find("ParticleOnOffPanel");

        settings.particleEffects = !settings.particleEffects;
        Debug.Log("Particle effects: " + settings.particleEffects);

        settings.HandleButtonToggle(settings.particleEffects, panel);
    }

    public void ToggleCameraShake() {
        GameObject panel = GameObject.Find("ShakeOnOffPanel");

        settings.cameraShake = !settings.cameraShake;
        Debug.Log("Camera shake: " + settings.cameraShake);

        settings.HandleButtonToggle(settings.cameraShake, panel);
    }

    public void OpenHighscores() {
        highscorePanel.SetActive(true);
        TMP_Text titleText = GameObject.Find("TitleText").GetComponent<TMP_Text>();
        titleText.text = "Highscores";
    }

    public void CloseHighscores() {
        highscorePanel.SetActive(false);
        TMP_Text titleText = GameObject.Find("TitleText").GetComponent<TMP_Text>();
        titleText.text = "Alphabet Armageddon";
    }

    /**
     *  Start of handling of the game mode  panel
     * */

    public void OpenGameModePanel() {
        mainPanel.SetActive(false);
        confirmationPanel.SetActive(false);
        gameModePanel.SetActive(true);
    }

    public void CloseGameModePanel() {
        gameModePanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void ChooseCasualMode() {
        gameModeHandler.wordSpeed = 2f;
        gameModeHandler.gameTime = 30f;
        gameModeHandler.selectedGameMode = GameModeHandler.GameMode.CasualMode;
        gameSpeedSlider.value = gameModeHandler.wordSpeed;
        gameSpeedSlider.onValueChanged.AddListener(GetSpeedSliderValue);
        gameLengthSlider.value = gameModeHandler.gameTime;
        gameLengthSlider.onValueChanged.AddListener(GetLengthSliderValue);
        startButton.onClick.AddListener(StartCasualGame);
        OpenCasualConfirmationPanel();
    }

    public void ChooseCompetitiveMode() {
        gameModeHandler.gameTime = 60f;
        gameModeHandler.selectedGameMode = GameModeHandler.GameMode.CompetitiveMode;
        tipText.text = "TIP: Detonating rocks at the right time will grant you bonus points.";
        gameLengthText.text = "Game Length: 1 minute";
        OpenConfirmationPanel();
        startButton.onClick.AddListener(StartCompetetitiveGame);
    }

    public void ChooseHardMode() {
        gameModeHandler.gameTime = 120f;
        gameModeHandler.selectedGameMode = GameModeHandler.GameMode.HardMode;
        tipText.text = "TIP: There will be words.";
        gameLengthText.text = "Game Length: 2 minutes";
        OpenConfirmationPanel();
        startButton.onClick.AddListener(StartHardGame);
    }

    public void ChooseWordHellMode() {
        gameModeHandler.gameTime = 0f;
        gameModeHandler.selectedGameMode = GameModeHandler.GameMode.WordHellMode;
        OpenConfirmationPanel();
        tipText.text = "TIP: Take a deep breath.";
        gameLengthText.text = "Game Length: Probably not long.";
        OpenConfirmationPanel();
        startButton.onClick.AddListener(StartWordHellGame);
    }

     /**
     *  End of handling of the game mode  panel
     * */


    /**
     *  Start of handling of the confirmation panel.
     * */

    void GetLengthSliderValue(float value) {
        casualGameLengthText.text = "Game length: " + gameLengthSlider.value  + " seconds";
        gameModeHandler.gameTime = gameLengthSlider.value;
    }

    void GetSpeedSliderValue(float value) {
        gameSpeedText.text = "Word descending speed: " + gameSpeedSlider.value;
        gameModeHandler.wordSpeed = gameSpeedSlider.value;
    }


    public void StartCasualGame() {
        DontDestroyOnLoad(GameObject.Find("Wandering_Spirits_FX"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        
        
    }

    public void StartCompetetitiveGame() {
        DontDestroyOnLoad(GameObject.Find("Wandering_Spirits_FX"));
        gameModeHandler.wordSpeed = 3f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +2);
    }

    public void StartHardGame() {
        DontDestroyOnLoad(GameObject.Find("Wandering_Spirits_FX"));
        gameModeHandler.wordSpeed = 4f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +3);
    }

    public void StartWordHellGame() {
        gameModeHandler.wordSpeed = 4f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +4);

        // Stop playing other songs and let hell mode do its thing
        SettingController settings = GameObject.Find("SettingsManager").GetComponent<SettingController>();
        musicPlayer.mainAudio.Pause();
        GameObject.Find("HellMusic").GetComponent<AudioSource>().Play();
    }

    public void OpenConfirmationPanel() {
        // Set the text inside the confirmation panel accordingly to the chosen game mode
        if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.CompetitiveMode) {
            selectedModeText.text = "Competitive";
        } else if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.HardMode) {
            selectedModeText.text = "Hard";
        } else if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.WordHellMode) {
            selectedModeText.text = "Word Hell";
        }
        gameModePanel.SetActive(false);
        confirmationPanel.SetActive(true);
    }

    public void CloseConfirmationPanel() {
        confirmationPanel.SetActive(false);
        gameModePanel.SetActive(true);
    }

    public void OpenCasualConfirmationPanel() {
        gameModePanel.SetActive(false);
        casualConfirmationPanel.SetActive(true);
    }

    public void CloseCasualConfirmationPanel() {
        casualConfirmationPanel.SetActive(false);
        gameModePanel.SetActive(true);
    }

	public void ToggleNormalWord() {
        if (gameModeHandler.complexWords) {
            complexWordText.text = "X";
            complexWordText.color = Color.red;
            normalWordText.text = "V";
            normalWordText.color = Color.green;
            gameModeHandler.complexWords = false;
            Debug.Log("Normal mode");
        } else {
            complexWordText.text = "V";
            complexWordText.color = Color.green;
            normalWordText.text = "X";
            normalWordText.color = Color.red;
            gameModeHandler.complexWords = true;
            Debug.Log("Complex mode");
        }
    }

    public void ToggleComplexWord() {
        if (!gameModeHandler.complexWords) {
            complexWordText.text = "V";
            complexWordText.color = Color.green;
            normalWordText.text = "X";
            normalWordText.color = Color.red;
            gameModeHandler.complexWords = true;
            Debug.Log("Complex mode");
        } else {
            complexWordText.text = "X";
            complexWordText.color = Color.red;
            normalWordText.text = "V";
            normalWordText.color = Color.green;
            gameModeHandler.complexWords = false;
            Debug.Log("Normal mode");
        }
    }

    public void ToggleCasualNormalWord() {
        if (gameModeHandler.complexWords) {
            casualComplexWordText.text = "X";
            casualComplexWordText.color = Color.red;
            casualNormalWordText.text = "V";
            casualNormalWordText.color = Color.green;
            gameModeHandler.complexWords = false;
            Debug.Log("Normal mode");
        } else {
            casualComplexWordText.text = "V";
            casualComplexWordText.color = Color.green;
            casualNormalWordText.text = "X";
            casualNormalWordText.color = Color.red;
            gameModeHandler.complexWords = true;
            Debug.Log("Complex mode");
        }
    }

    public void ToggleCasualComplexWord() {
        if (!gameModeHandler.complexWords) {
            casualComplexWordText.text = "V";
            casualComplexWordText.color = Color.green;
            casualNormalWordText.text = "X";
            casualNormalWordText.color = Color.red;
            gameModeHandler.complexWords = true;
            Debug.Log("Complex mode");
        } else {
            casualComplexWordText.text = "X";
            casualComplexWordText.color = Color.red;
            casualNormalWordText.text = "V";
            casualNormalWordText.color = Color.green;
            gameModeHandler.complexWords = false;
            Debug.Log("Normal mode");
        }
    }

    /**
     *  End of handling of the confirmation panel.
     * */
}
