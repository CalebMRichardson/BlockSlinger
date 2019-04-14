﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAudio : MonoBehaviour
{

    private AudioClip hitOneSFX;
    private AudioClip hitTwoSFX;
    private AudioClip hitThreeSFX;
    private AudioClip[] hitSFXCollection; 
    private AudioClip shootSFX;
    private AudioClip blockInGoalSFX;
    private AudioClip fallSFX;

    private void Start() {

        hitSFXCollection = new AudioClip[ 3 ];

        hitOneSFX = Resources.Load(LevelObjectLookup.BLOCK_HIT_ONE_SFX) as AudioClip;
        if(hitOneSFX == null) {
            Debug.LogError("HitOne is null. " + LevelObjectLookup.BLOCK_HIT_ONE_SFX);
        } else {
            hitSFXCollection[ 0 ] = hitOneSFX;
        }

        hitTwoSFX = Resources.Load(LevelObjectLookup.BLOCK_HIT_TWO_SFX) as AudioClip;
        if(hitTwoSFX == null) {
            Debug.LogError("HitTwo is null. " + LevelObjectLookup.BLOCK_HIT_TWO_SFX);
        } else {
            hitSFXCollection[ 1 ] = hitTwoSFX;
        }

        hitThreeSFX = Resources.Load(LevelObjectLookup.BLOCK_HIT_THREE_SFX) as AudioClip;
        if(hitThreeSFX == null) {
            Debug.LogError("HitThree is null. " + LevelObjectLookup.BLOCK_HIT_THREE_SFX);
        } else {
            hitSFXCollection[ 2 ] = hitThreeSFX;
        }

        shootSFX = Resources.Load(LevelObjectLookup.BLOCK_SHOOT_SFX) as AudioClip;
        if(shootSFX == null) {
            Debug.LogError("ShootSound is null. " + LevelObjectLookup.BLOCK_SHOOT_SFX);
        }

        blockInGoalSFX = Resources.Load(LevelObjectLookup.BLOCK_IN_GOAL_SFX) as AudioClip;
        if (blockInGoalSFX == null) {
            Debug.LogError("BlockInGoalSFX is null. " + LevelObjectLookup.BLOCK_IN_GOAL_SFX);
        }

        fallSFX = Resources.Load(LevelObjectLookup.FALL_SFX) as AudioClip;
        if (fallSFX == null) {
            Debug.LogError("FallSFX is null."); 
        }
    }

    public AudioClip[] GetHitCollectionSFX() {
        return hitSFXCollection;
    }

    public AudioClip GetShootSFX() {
        return shootSFX; 
    }

    public AudioClip GetBlockInGoalSFX() {
        return blockInGoalSFX;
    }

    public AudioClip GetFallSFX() {
        return fallSFX; 
    }
}
