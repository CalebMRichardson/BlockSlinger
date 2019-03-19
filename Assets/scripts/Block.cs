using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Prop
{
    private BlockAudio blockAudio;

    private void Start() {
        blockAudio = GetComponent<BlockAudio>();
    }

    public void PlayRandomHitFX() {
        AudioManager.PlayRandomSfx(blockAudio.GetHitCollectionFX());
    }

    public void PlayShootFX() {
        AudioManager.PlaySingle(blockAudio.GetShootFX());
    }
}
