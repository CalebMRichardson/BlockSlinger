using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAudio : MonoBehaviour
{

    private AudioClip trapSFX;


    private void Start() {

        trapSFX = Resources.Load(LevelObjectLookup.TRAP_SFX) as AudioClip;
        if (trapSFX == null) {
            Debug.LogError("TrapSFX is null.");
        } 

    }

    public AudioClip GetTrapSFX() {
        return trapSFX;
    }

}
