using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    //TODO find a single place to put this later
    private const int      LEVEL_DATA_WIDTH  = 14;
    private const int      LEVEL_DATA_HEIGHT = 30;
    private const int      LAYER_HEIGHT = 10;

    public GameObject[,] tileLayer;
    public GameObject[,] propLayer;
    public GameObject[,] decalLayer;

    private void Awake() {
        tileLayer = new GameObject[ LAYER_HEIGHT, LEVEL_DATA_WIDTH];
        propLayer = new GameObject[ LAYER_HEIGHT, LEVEL_DATA_WIDTH ]; 
    }
}
