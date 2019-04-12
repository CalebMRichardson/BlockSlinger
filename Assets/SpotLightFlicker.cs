using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightFlicker : MonoBehaviour
{
    private float minFlickerIntensity = 8.5f;
    private float maxFlickerIntensity = 12.5f;
    private float flickerSpeed = 0.1f;

    private Light spotLight;

    private void Start() {
        spotLight = GetComponent<Light>();

        InvokeRepeating("FlickerController", 0, flickerSpeed);
    }

    void FlickerController() {
        StartCoroutine("Flicker");
    }

    IEnumerator Flicker() {
        spotLight.intensity = Random.Range(minFlickerIntensity, maxFlickerIntensity);
        yield return new WaitForSeconds(flickerSpeed);
    }
}
