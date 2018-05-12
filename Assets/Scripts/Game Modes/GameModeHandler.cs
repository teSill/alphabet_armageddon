using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeHandler : MonoBehaviour {

    public enum GameMode {
        None,
        CasualMode,
        CompetitiveMode,
        HardMode,
        WordHellMode
    };

    public GameMode selectedGameMode;

    public bool complexWords;

    public float gameTime = 30f;

    public float wordSpeed;

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

}
