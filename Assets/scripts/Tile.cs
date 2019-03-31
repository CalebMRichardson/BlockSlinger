using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private bool isGoalTile;
    private bool isGoalTileComplete; 
    public int x,y;

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

    public void SetIsGoalTileComplete(bool _isGoalTileComplete) {
        isGoalTileComplete = _isGoalTileComplete; 
    }

    public bool IsGoalTileComplete() {
        return isGoalTileComplete;
    }
}
