using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    WordController WordController;
    PlayerInputManager playerInput;
    CameraShake cameraShake;
    WordHellModeScript wordHell;
    GameModeHandler gameModeHandler;

    public GameObject rockPrefab;

    [HideInInspector]
    public List<GameObject> rocksActiveInScene = new List<GameObject>();

    public GameObject[] startPositions;
    public GameObject[] endPositions;

    GameObject rockCanvas;

    void Start() {
        WordController = GetComponent<WordController>();
        playerInput = GetComponent<PlayerInputManager>();
        wordHell = GetComponent<WordHellModeScript>();
        gameModeHandler = GameObject.Find("GameModeHandler").GetComponent<GameModeHandler>();

        cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();

        rockCanvas = GameObject.Find("RockCanvas");
    }

    public IEnumerator StartSpawningRocks(float spawnInterval) {
        WordController.GenerateRandomWord();
        yield return new WaitForSeconds(spawnInterval);
        if (GameManager.Instance.gameActive)
            StartCoroutine(StartSpawningRocks(spawnInterval));
        else {
            Debug.Log("Game has ended.");
            // If game has stopped running, destroy every object in scene
            if (!GameManager.Instance.gameActive) {
                foreach(GameObject activeRock in rocksActiveInScene) {
                    // Set explosion gameobject to true to play the animation
                    GameObject explosion = activeRock.transform.Find("Explosion").gameObject;
                    explosion.SetActive(true);
                    // Delete the rock prefab after explosion has played
                    Destroy(activeRock, 0.5f);
                }
            }
        }
    }

    public void SpawnRock() {
        Transform rockStartPos = startPositions[Random.Range(0, startPositions.Length)].transform;
        Transform rockEndPos;

        if (rockStartPos.gameObject.name == "StartPosition1")
            rockEndPos = GameObject.Find("EndPosition1").transform;
        else if (rockStartPos.gameObject.name == "StartPosition2")
            rockEndPos = GameObject.Find("EndPosition2").transform; 
        else if(rockStartPos.gameObject.name == "StartPosition3")
            rockEndPos = GameObject.Find("EndPosition3").transform;
        else if (rockStartPos.gameObject.name == "StartPosition4")
            rockEndPos = GameObject.Find("EndPosition4").transform;
        else 
            rockEndPos = GameObject.Find("EndPosition5").transform;

        GameObject newRock = Instantiate(rockPrefab);

        rocksActiveInScene.Add(newRock);

        newRock.transform.position = rockStartPos.position;

        newRock.transform.SetParent(rockCanvas.transform);

        float randomizedWordSpeed = Random.Range(gameModeHandler.wordSpeed - 0.15f, gameModeHandler.wordSpeed + 0.15f);

        StartCoroutine(MoveRock(rockEndPos, newRock, randomizedWordSpeed));
    }

    IEnumerator MoveRock(Transform target, GameObject newRock, float speed) {
        // Move rocks down to their destinations
        while (newRock != null && Vector3.Distance(newRock.transform.position, target.position) > 0.05f) {
            newRock.transform.position = Vector3.MoveTowards(newRock.transform.position, target.position, speed * Time.deltaTime);
            yield return null;
        }

        // Remvove the current word from the list so that the player can't type it to get a correct answer more than once
        if (newRock != null)
            WordController.wordsUsed.Remove(newRock.GetComponentInChildren<Text>().text);
        else
            Debug.Log("Tried to remove object from list but game has probably ended already.");

        if (newRock != null) {
            WordController.wordsUsed.Remove(newRock.GetComponentInChildren<Text>().text);
            // This will only be called for objects that the player didn't catch
            if (newRock.activeSelf) {
                cameraShake.CallCameraShake();
                if (GameManager.Instance.gameActive) {
                    GameObject explosion = newRock.transform.Find("Explosion").gameObject;
                    if (explosion.activeSelf) {
                        Debug.Log("Word has been typed correctly.");
                    } else {
                        SpecialEffects specialEffects = GameObject.Find("TextEffectManager").GetComponent<SpecialEffects>();
                        SoundController sounds = GameObject.Find("SoundManager").GetComponent<SoundController>();
                        specialEffects.BreakCombo();
                        sounds.ExplosionSound();
                        GameManager.Instance.missedWords++;
                        if (gameModeHandler.selectedGameMode == GameModeHandler.GameMode.WordHellMode) {
                            DisplayRockDestruction(bigExplosionPrefab, newRock.transform.position);
                            wordHell.EndGame();
                        } else {
                            SettingController settings = GameObject.Find("SettingsManager").GetComponent<SettingController>();
                            if (settings.particleEffects)
                                DisplayRockDestruction(explosionPrefab, newRock.transform.position);
                        }
                    }
                }
            }
            rocksActiveInScene.Remove(newRock);
            Destroy(newRock);
        }
    }
    
    public GameObject explosionPrefab;
    public GameObject bigExplosionPrefab;

    void DisplayRockDestruction(GameObject explosion, Vector3 explosionPosition) {
        GameObject destroyFX = Instantiate(explosion);
        destroyFX.transform.position = explosionPosition;

        Destroy(destroyFX, 2f);
    }
}
