using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public bool isBlank;
    public int x, y; 

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
