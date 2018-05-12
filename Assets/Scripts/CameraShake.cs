using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	
    // Shake duration
    float shakeDuration = 0.2f;
	// Amplitude of the shake. A larger value shakes the camera harder.
	float shakeAmount = 0.1f;
	
	Vector3 originalPos;
	
	void Awake() {
		originalPos = transform.localPosition;
	}

    public void CallCameraShake() {
        if (GameObject.Find("SettingsManager").GetComponent<SettingController>().cameraShake)
            StartCoroutine(ShakeCamera(shakeDuration, shakeAmount));
    }

    IEnumerator ShakeCamera (float duration, float amount) {
        float endTime = Time.time + duration;

        while (duration > 0) {
            transform.localPosition = originalPos + Random.insideUnitSphere * amount;

            duration -= Time.deltaTime;

            yield return null;
        }
        transform.localPosition = originalPos;
    }
}