using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Prop
{

    public Vector2 direction;
    public Animator animator;
    private TrapAudio trapAudio;


    private void Start() {

        trapAudio = GetComponent<TrapAudio>();

        if (trapAudio == null) {
            Debug.LogError("Trap Auido component is null.");
        }

    }

    public void PlayTriggerAnimation(string _animation) {
        animator.Play(_animation);
    } 

    public void PlayTrapSoundFX() {
        AudioManager.PlaySingle(trapAudio.GetTrapSFX());
    }
}
