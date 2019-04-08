using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public bool isBlank;
    public bool isHole;
    public bool isTrap;
    [SerializeField]
    public int x, y;

    protected Vector2 startingPos; 
    protected int startingX, startingY; 

    private void Start() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
    }

    public void SetIsBlank(bool _isBlank) {
        isBlank = _isBlank;
    }

    public bool IsBlank() {
        return isBlank;
    }

    public void SetXY(int _x, int _y) {
        x = _x;
        y = _y;
    }

    public void SetStartingInfo(Vector2 _startingPos, int _startingX, int _startingY) {
        startingPos = _startingPos;
        startingX = _startingX;
        startingY = _startingY; 
    }
}
