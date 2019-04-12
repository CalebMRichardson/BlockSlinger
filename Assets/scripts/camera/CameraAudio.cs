using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAudio : MonoBehaviour
{

    private AudioClip swooshSFX;

    private void Start() {

        swooshSFX = Resources.Load(LevelObjectLookup.CAMERA_SWOOSH) as AudioClip; 
        if (swooshSFX == null) {
            Debug.LogError("Camera SwooshSFX is null."); 
        }
    }

    public AudioClip GetSwooshSFX() {
        return swooshSFX;
    }

}
