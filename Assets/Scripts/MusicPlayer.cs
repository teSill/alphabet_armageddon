using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour {
    

    public AudioSource[] gameSongs;
    public AudioSource mainAudio;

    public int currentSong;

    static bool hasChangedSong;

    static MusicPlayer instance = null;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() {
        mainAudio = GetComponent<AudioSource>();
        PlaySong();
    }

    public void SetSongInterface() {
        if (SceneManager.GetActiveScene().name != "MainMenu")
            return;

        TMP_Text songText;
        if (GameObject.Find("SettingsPanel") == null)
            songText = null;
        else
            songText = GameObject.Find("CurrentSongText").GetComponent<TMP_Text>();
        TMP_Text songText1 = GameObject.Find("CurrentSongText1").GetComponent<TMP_Text>();
        TMP_Text[] songs = {songText, songText1};

        if (currentSong == 0) {
            foreach(TMP_Text text in songs) {
                if (text != null)
                    text.text = "Dog and Pony Show";
            }
        } else if (currentSong == 1) {
            foreach(TMP_Text text in songs) {
                if (text != null)
                    text.text = "Blues Infusion";
            }
        } else if (currentSong == 2) {
            foreach(TMP_Text text in songs) {
                if (text != null)
                    text.text = "Mirage";
            }
        } else if (currentSong == 3) {
            foreach(TMP_Text text in songs) {
                if (text != null)
                    text.text = "Mirror Mirror";
            }
        } else if (currentSong == 4) {
            foreach(TMP_Text text in songs) {
                if (text != null)
                    text.text = "If I Had A Chicken";
            }
        } else if (currentSong == 5) {
            foreach(TMP_Text text in songs) {
                if (text != null)
                    text.text = "Campfire Song";
            }
        }
    }
    
    public void SetNextSong() {
        Debug.Log("Setting next song");
        if (currentSong < gameSongs.Length -1) {
            currentSong++;
            Debug.Log(currentSong);
        } else {
            currentSong = 0;
        }

        SetSongInterface();
        PlaySong();
    }

    public void PlaySong() {
        if (SceneManager.GetActiveScene().buildIndex == 4)
            return;

        if (currentSong > 0)
            gameSongs[currentSong -1].Stop();
        else
            gameSongs[gameSongs.Length -1].Stop();
        mainAudio.clip = gameSongs[currentSong].clip;
        mainAudio.Play();
        float songDuration = gameSongs[currentSong].clip.length;
        StartCoroutine(PrepareNextSong(songDuration, currentSong));
    }

    IEnumerator PrepareNextSong(float songDuration, int current) {
        float timer = 0;
        while (mainAudio.clip == gameSongs[current].clip && timer < songDuration) {
            timer += Time.deltaTime;
            yield return null;
        }
        // Add a little breathing room to the 'timer' by removing 1 from songDuration
        if (timer >= songDuration - 1f) {
            Debug.Log("Changed song through enumerator");
            SetNextSong();
        } else {
            //Debug.Log("Couldn't change song - Timer was " + timer + " and song duration: " + songDuration);
        }
    }
}
