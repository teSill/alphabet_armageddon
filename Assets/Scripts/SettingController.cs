using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingController : MonoBehaviour {

    SoundController sounds;
    MusicPlayer musicPlayer;
    UIManager ui;

    public bool particleEffects;
    public bool gameSounds;
    public bool cameraShake;
    
    public float volume = 15f;
    
    static SettingController instance = null;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
  
    void Start() {
        ui = GameObject.Find("UIManager").GetComponent<UIManager>();
        musicPlayer = GameObject.Find("MusicManager").GetComponent<MusicPlayer>();
        particleEffects = true;
        gameSounds = true;
        cameraShake = true;

        if (SceneManager.GetActiveScene().name != "MainMenu")
            return;

        musicPlayer.SetSongInterface();
    }

    public void HandleButtonToggle(bool boolean, GameObject panel) {
        GameObject on = panel.transform.GetChild(0).gameObject;
        GameObject off = panel.transform.GetChild(1).gameObject;
        GameObject gamePanel = panel.transform.GetChild(2).gameObject;

        if (boolean) {
            gamePanel.transform.position = off.transform.position;
            gamePanel.GetComponent<Image>().color = Color.green;
        } else {
            gamePanel.transform.position = on.transform.position;
            gamePanel.GetComponent<Image>().color = Color.yellow;
        }
    }

    public void SetSettingInterface() {
        GameObject soundPanel = GameObject.Find("GameSoundsOnOffPanel");
        GameObject particlePanel = GameObject.Find("ParticleOnOffPanel");
        GameObject shakePanel = GameObject.Find("ShakeOnOffPanel");

        HandleButtonToggle(gameSounds, soundPanel);
        HandleButtonToggle(particleEffects, particlePanel);
        HandleButtonToggle(cameraShake, shakePanel);
    }
}
