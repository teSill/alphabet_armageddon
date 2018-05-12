using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEffects : MonoBehaviour {

     // Sets the reveal speed in seconds.
   public float revealSpeed = 0.05f;

    // The current page of the text, needs to be changed when you display the next page.
    public int currentPage = 0;

    // Lets other scripts know when to allow the next page to load.
    public bool isRunning;
    
    public IEnumerator DoFancyTextEffect(TMP_Text text, string message, int intType) {
        isRunning = true;

        int totalCharacters = message.Length + intType;
        int counter = 0;

        if (text == GameManager.Instance.accuracyText) {
            text.text = message + intType + "%";
        } else
            text.text = message + intType;

        while (counter < totalCharacters) {
            int visibleCount = counter % (totalCharacters + 1);
            
            text.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalCharacters)
                yield return new WaitForSeconds(0.05f);

            counter += 1;

            yield return new WaitForSeconds(0.05f);
        }
        isRunning = false;
	}
}
