﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Prop
{
    private BlockAudio blockAudio;
    private GameObject currentLevel;
    private Level currentLevelScript;
    private bool blockInGoal; 

    private void Start() {
        blockAudio = GetComponent<BlockAudio>();
        currentLevel = transform.parent.gameObject;
        currentLevelScript = currentLevel.GetComponent<Level>();
        blockInGoal = false;
    }

    public bool CheckIfInGoal() {

        bool inGoal = false;

        Tile tile = currentLevelScript.tileLayer[ y, x ].GetComponent<Tile>();

        if (tile.IsGoalTile()) {
            inGoal = true;
            tile.SetIsGoalTileComplete(inGoal);
        }

        return inGoal;
    }

    public void PlayRandomHitSFX() {
        AudioManager.PlayRandomSfx(blockAudio.GetHitCollectionSFX());
    }

    public void PlayShootSFX() {
        AudioManager.PlaySingle(blockAudio.GetShootSFX());
    }

    public void PlayBlockInGoalSFX() {
        AudioManager.PlaySingleAtVolume(blockAudio.GetBlockInGoalSFX(), 0.3f);
    }

    public GameObject GetCurrentLevel() {
        return currentLevel;
    }

    public Level GetCurrentLevelScript() {
        return currentLevelScript;
    }

    public void SetBlockInGoal(bool _blockInGoal) {
        blockInGoal = _blockInGoal;
    }

    public bool GetBlockInGoal() {
        return blockInGoal; 
    }
}
