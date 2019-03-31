using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioSource fxSource;

    public static AudioManager instance = null;

    private static float lowPitchRange = .95f;
    private static float highPitchRange = 1.05f;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }

        fxSource = GetComponent<AudioSource>();
    }


    public static void PlaySingle(AudioClip _clip) {

        float volume = Random.Range(.7f, 1f);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        fxSource.pitch = randomPitch;
        fxSource.clip = _clip;
        fxSource.Play();
    }

    public static void PlaySingleAtVolume(AudioClip _clip, float _volume) {

        fxSource.PlayOneShot(_clip, _volume);

    }

    public static void PlayRandomSfx(AudioClip[] _clips) {

        int randomIndex = Random.Range(0, _clips.Length);

        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        fxSource.pitch = randomPitch;
        fxSource.clip = _clips[ randomIndex ];
        fxSource.Play();
    }
}
