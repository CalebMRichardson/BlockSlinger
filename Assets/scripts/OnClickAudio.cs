using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickAudio : MonoBehaviour
{

    private AudioClip click;

    private void Start() {

        click = Resources.Load(LevelObjectLookup.BUTTON_CLICK_SFX) as AudioClip;

        if (click == null) {
            Debug.LogError("Click is null.");
        }

    } 

    public void PlayClick() {
        AudioManager.PlaySingle(click);
    }

}
