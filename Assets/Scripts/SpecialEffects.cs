using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpecialEffects : MonoBehaviour {

    SettingController settings;

    public TMP_Text comboText;
    public TMP_Text comboPointText;

    public GameObject sparkEffect;
    public GameObject fireWorkEffect;
    public GameObject comboBreakEffect;
    public GameObject starEffect;
    public GameObject rockDestroyEffect;

    int currentCombo;
    int division = 5;

    public int comboBonusMultiplier;


    private void Start() {
        settings = GameObject.Find("SettingsManager").GetComponent<SettingController>();

    }

    /* *
     * COMBO EFFECTS 
     * */

    public void IncrementCombo() {
        comboText.gameObject.SetActive(true);
        GameObject spark = Instantiate(sparkEffect);
        Destroy(spark, 2f);
        currentCombo++;
        comboText.text = "Combo = " + currentCombo;
        StartCoroutine("IncreaseFontSize");
        if (currentCombo % division == 0) {
            // Special effects and bonuses
            SoundController sounds = GameObject.Find("SoundManager").GetComponent<SoundController>();
            if (settings.gameSounds)
                sounds.ComboSound();

            HandleBonusPoints();

            if (settings.particleEffects) {
                GameObject fireWork = Instantiate(fireWorkEffect);
                Destroy(fireWork, 2f);
            }
        }
    }

    void HandleBonusPoints() {
        if (comboBonusMultiplier == 0) {
            comboBonusMultiplier = 1;
        } else {
            comboBonusMultiplier = comboBonusMultiplier + comboBonusMultiplier;
        }
        GameManager.Instance.correctWords += 1 * comboBonusMultiplier;

        comboPointText.text = "COMBO BONUS +" + comboBonusMultiplier;

        comboPointText.gameObject.SetActive(true);
        StartCoroutine(EndEffect(1.5f, comboPointText.gameObject));

        if (!settings.particleEffects)
            return;
        starEffect.SetActive(true);
        StartCoroutine(EndEffect(1f, starEffect));
    }

    public void BreakCombo() {
        if (currentCombo == 0)
            return;

        comboBonusMultiplier = 0;

        if (!comboText.gameObject.activeSelf)
            comboText.gameObject.SetActive(true);

        if (settings.particleEffects) {
            GameObject comboBreak = Instantiate(comboBreakEffect);
            Destroy(comboBreak, 2f);
        }

        currentCombo = 0;
        comboText.text = "";
    }

    IEnumerator IncreaseFontSize() {
        comboText.fontSize = 20f;

        float speed = 60f;
        while (comboText.fontSize < 40) {
            comboText.fontSize += 1f * Time.deltaTime * speed;
            yield return null;
        }
    }

    IEnumerator EndEffect(float time, GameObject effect) {
        yield return new WaitForSeconds(time);
        if (effect.activeSelf)
            effect.SetActive(false);
    }  

    /**
     * END GAME UI
     **/ 

    public IEnumerator DisplayHighscorePanel() {
        PlayerInputManager input = GameObject.Find("GameManager").GetComponent<PlayerInputManager>();
        float maxSize = 1.4f;
        float scaleTime = 1f;

        input.highscorePanel.SetActive(true);

        while(maxSize > input.highscorePanel.transform.localScale.x) {
            input.highscorePanel.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * scaleTime;
            yield return null;
        }
        while(1 < input.highscorePanel.transform.localScale.x) {
            input.highscorePanel.transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * scaleTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        input.highscoreHeader.SetActive(true);
    }

    List<TMP_Text> textsToDisplay = new List<TMP_Text>();
    int currentText = 0;

}
