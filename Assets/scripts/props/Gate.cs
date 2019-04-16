using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : Prop
{

    private Animator animator;
    public bool exitGate = false;
    private AudioClip gateDownSFX; 

    private void Start() {
        animator = GetComponent<Animator>();

        gateDownSFX = Resources.Load(LevelObjectLookup.GATE_DOWN_SFX) as AudioClip;
        
        if (gateDownSFX == null) {
            Debug.Log("Gate Down SFX is null.");
        }
    }

    public void SetExitGate(bool _exitGate) {
        exitGate = _exitGate;
    }

    public void PlayOpenAnimation() {
        print("CALLED");
        animator.Play("gate_down");
    }

    public void PlayGateSFX() {
        // AudioManager.PlaySingleAtVolume(gateDownSFX, .3f);
    }

}
