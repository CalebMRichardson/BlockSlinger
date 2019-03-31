using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    //TODO find a single place to put this later
    private const int      LEVEL_DATA_WIDTH  = 14;
    private const int      LEVEL_DATA_HEIGHT = 30;
    private const int      LAYER_HEIGHT = 10;

    private int[] levelEnterance;
    private int[] levelExit;
    private Vector2 deadCenter;

    public GameObject[,] tileLayer;
    public GameObject[,] propLayer;
    public GameObject[,] decalLayer;
    private List<Tile> goalTiles;
    [SerializeField]
    private bool isCurrentLevel; 
    [SerializeField]
    private bool levelComplete;

    private void Awake() {
        tileLayer = new GameObject[ LAYER_HEIGHT, LEVEL_DATA_WIDTH ];
        propLayer = new GameObject[ LAYER_HEIGHT, LEVEL_DATA_WIDTH ];
        levelComplete = false;
        goalTiles = new List<Tile>();
    }

    private void Start() {
        for(int i = 0; i < LAYER_HEIGHT; i++) {
            for(int j = 0; j < LEVEL_DATA_WIDTH; j++) {
                Tile tileScript = tileLayer[i,j].GetComponent<Tile>();
                if(tileScript.IsGoalTile()) {
                    goalTiles.Add(tileScript);
                }
            }
        }

    }

    private void Update() {

        if (!isCurrentLevel) {
            return;
        }

        if(!levelComplete) {
            CheckIfLevelComplete();
        } 
    }

    public void SetIsCurrentLevel(bool _isCurrentLevel) {
        isCurrentLevel = _isCurrentLevel;
    }

    public bool IsLevelComplete() {
        return levelComplete;
    }

    private void CheckIfLevelComplete() {

        bool allGoalTilesFilled = true;

        foreach(Tile tile in goalTiles) {

            if(tile.IsGoalTileComplete() == false) {
                allGoalTilesFilled = false;
            }
        }

        levelComplete = allGoalTilesFilled;
    }

    public void SetEnteranceExit(int[ ] _levelEnterance, int[ ] _levelExit) {
        levelEnterance = _levelEnterance;
        levelExit = _levelExit;
    }

    //TODO look into turning all this into a struct
    public bool IsNorthEnterance() {
        return levelEnterance[ 0 ] == LAYER_HEIGHT - 1;
    }

    public bool IsSouthEnterance() {
        return levelEnterance[ 0 ] == 0;
    }

    public bool IsEastEnterance() {
        return levelEnterance[ 1 ] == LEVEL_DATA_WIDTH - 1;
    }

    public bool IsWestEnterance() {
        return levelEnterance[ 1 ] == 0;
    }


    public Vector2 GetLevelExitPos() {

        Vector2 levelExitPos = new Vector2();

        if (levelExit.Length != 2) {
            Debug.LogError("Level Exit Length is not 2.");
        } else if (levelExit[0] == -1 || levelExit[1] == -1) {
            //TODO this could be a way of checking if its the last level
            Debug.LogError("Level exit at index 0 or 1 is -1");
        } else {
            // This is janky but it returns the position of the proplayer exit doors 
            levelExitPos = propLayer[ levelExit[ 0 ], levelExit[ 1 ] ].transform.position;
        }


        return levelExitPos; 
    }

    public Vector2 GetLevelEnterancePos() {

        Vector2 levelEnterancePos = new Vector2();

        if(levelEnterance.Length != 2) {
            Debug.LogError("Level Exit Length is not 2.");
        } else if(levelEnterance[ 0 ] == -1 || levelEnterance[ 1 ] == -1) {
            //TODO this could be a way of checking if its the first level
            Debug.LogError("Level Enterance at index 0 or 1 is -1");
        } else {
            // This is janky but it returns the position of the proplayer enterance doors 
            levelEnterancePos = propLayer[ levelEnterance[ 0 ], levelEnterance[ 1 ] ].transform.position;
        }


        return levelEnterancePos;
    }

    public void SetCameraFocus() {

        GameObject centerTile =  tileLayer[ LAYER_HEIGHT / 2, LEVEL_DATA_WIDTH / 2 ];
        SpriteRenderer centerTileSR = centerTile.GetComponent<SpriteRenderer>();

        float middleX = centerTile.transform.position.x - (centerTileSR.size.x / 2);
        float middleY = centerTile.transform.position.y - (centerTileSR.size.y / 2);

        deadCenter = new Vector2(middleX, middleY);

        CameraMovement camMovement = Camera.main.GetComponent<CameraMovement>();
        camMovement.SetCamPos(deadCenter);
    }

}