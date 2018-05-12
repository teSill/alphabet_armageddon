using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour {

    SettingController settings;
    MusicPlayer musicPlayer;

    public AudioClip menuForward;
    public AudioClip menuBack;
    public AudioClip startGame;

    public AudioClip wordSuccessSound;
    public AudioClip wordErrorSound;

    AudioSource audioSource;

    // Separate audio source for these as their volume was much higher than the rest
    AudioSource explosionSource;
    AudioSource comboSource;
    AudioSource highscoreSource;
    
    AudioSource hellModeSource;

	// Use this for initialization
	void Start () {
        settings = GameObject.Find("SettingsManager").GetComponent<SettingController>();
        musicPlayer = GameObject.Find("MusicManager").GetComponent<MusicPlayer>();

        audioSource = GetComponent<AudioSource>();
        explosionSource = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();
        comboSource = GameObject.Find("ComboSound").GetComponent<AudioSource>();
        highscoreSource = GameObject.Find("HighscoreSound").GetComponent<AudioSource>();
        
        hellModeSource = GameObject.Find("HellMusic").GetComponent<AudioSource>();

        if (hellModeSource.isPlaying)
            hellModeSource.Stop();

        for (int i = 0; i < musicPlayer.gameSongs.Length; i++) {
            musicPlayer.gameSongs[i].Stop();
        }

        
        DontDestroyOnLoad(gameObject);
        if (gameObject == null)
            Destroy(gameObject);
        
	}

    // MENU SOUNDS
	
    public void MoveForwardMenu() {
        audioSource.PlayOneShot(menuForward);
    }

    public void MoveBackMenu() {
        audioSource.PlayOneShot(menuBack);
    }
	
    public void StartGame() {
        audioSource.PlayOneShot(startGame);
    }

    // IN GAME SOUNDS

    public void CorrectWord() {
        if (settings.gameSounds)
            audioSource.PlayOneShot(wordSuccessSound);
    }

    public void IncorrectWord() {
        if (settings.gameSounds)
            audioSource.PlayOneShot(wordErrorSound);
    }

    public void ExplosionSound() {
        if (settings.gameSounds)
            explosionSource.Play();
    }

    public void ComboSound() {
        if (settings.gameSounds)
            comboSource.Play();
    }

    public void HighscoreSound() {
        if (settings.gameSounds)
            highscoreSource.Play();
    }
}
