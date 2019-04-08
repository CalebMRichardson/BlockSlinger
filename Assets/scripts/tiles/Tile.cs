using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private bool isGoalTile;
    private bool isGoalTileComplete; 
    public int x,y;

    protected int startingX, startingY; 

    private void Start() {
        isGoalTileComplete = false;
    }

    public void SetIsGoalTile(bool _isGoalTile) {
        isGoalTile = _isGoalTile;
    }

    public bool IsGoalTile() {
        return isGoalTile;
    }

    public void SetXY(int _x, int _y) {
        x = _x;
        y = _y;
    }

    // Used to collect info for when the level reloads
    public void SetStaringInfo(int _startingX, int _startingY) {
        startingX = _startingX;
        startingY = _startingY; 
    }

    public void SetIsGoalTileComplete(bool _isGoalTileComplete) {
        isGoalTileComplete = _isGoalTileComplete; 
    }

    public bool IsGoalTileComplete() {
        return isGoalTileComplete;
    }

}
