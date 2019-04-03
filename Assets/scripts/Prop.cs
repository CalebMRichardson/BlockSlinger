using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public bool isBlank;
    public bool isHole;
    public bool isTrap;
    public int x, y; 
    public float width, height;

    private void Start() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        width = sr.bounds.size.x;
        height = sr.bounds.size.y;
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
}
