using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    //TODO find a single place to put this later
    private const int      LEVEL_DATA_WIDTH  = 14;
    private const int      LEVEL_DATA_HEIGHT = 30;
    private const int      LAYER_HEIGHT = 10;
    private int levelIndex;

    private int[] levelEnterance;
    private int[] levelExit;
    private Vector2 deadCenter;

    public GameObject[,] tileLayer;
    public GameObject[,] propLayer;
    public GameObject[,] decalLayer;

    private List<Tile> goalTiles;
    [SerializeField]
    private List<BlockMovement> blocks;
    [SerializeField]
    private bool isCurrentLevel;
    [SerializeField]
    private bool levelComplete;
    private Vector2 startingPos;
    [SerializeField]
    private bool blockMoving;

    private void Awake() {
        tileLayer = new GameObject[ LAYER_HEIGHT, LEVEL_DATA_WIDTH ];
        propLayer = new GameObject[ LAYER_HEIGHT, LEVEL_DATA_WIDTH ];

        levelComplete = false;
        goalTiles = new List<Tile>();
    }

    private void Start() {
        AddGoalTiles();
        AddBlocks();
        ConnectHallway();
        blockMoving = false;
    }

    private void Update() {

        if(!isCurrentLevel) {
            return;
        }

        if (levelComplete) {
            return;
        }

        if(!levelComplete) {
            CheckIfLevelComplete();
        }

        bool _blockMoving = false;

        foreach(BlockMovement block in blocks) {
            
            if (block.moveState == BlockMovement.MoveState.MOVING) {
                _blockMoving = true;
            }
        }

        blockMoving = _blockMoving;
    }

    public bool IsCurrentLevel() {
        return isCurrentLevel;
    }

    private void AddGoalTiles() {

        for(int i = 0; i < LAYER_HEIGHT; i++) {

            for(int j = 0; j < LEVEL_DATA_WIDTH; j++) {

                Tile tileScript = tileLayer[i,j].GetComponent<Tile>();

                if(tileScript.IsGoalTile()) {
                    goalTiles.Add(tileScript);
                }
            }
        }
    }

    private void AddBlocks() {

        for (int i = 0; i < LAYER_HEIGHT; i++) {

            for (int j = 0; j < LEVEL_DATA_WIDTH; j++) {

                BlockMovement blockMovement = propLayer[i,j].GetComponent<BlockMovement>();

                if (blockMovement != null) {
                    blocks.Add(blockMovement);
                }
            }
        }
    }

    private void ConnectHallway() {

        bool northExit = levelExit[ 0 ] == LAYER_HEIGHT - 1;
        bool southExit = levelExit[ 0 ] == 0;
        bool eastExit  = levelExit[ 1 ] == LEVEL_DATA_WIDTH - 1;
        bool westExit  = levelExit[ 1 ] == 0;

        if (northExit || southExit) {

            GameObject nHallway = CreateHallway(LevelObjectLookup.HALLWAY_NS_PATH);
            float xPos = GetLevelExitPos(true).x;
            float yPos = GetLevelExitPos(true).y;

            nHallway.transform.position = new Vector2(xPos, yPos);
        }

        if (eastExit || westExit) {
            GameObject nHallway = CreateHallway(LevelObjectLookup.HALLWAY_EW_PATH);
            float xPos = GetLevelExitPos(false).x;
            float yPos = GetLevelExitPos(false).y;

            nHallway.transform.position = new Vector2(xPos, yPos);
        } 
    }

    private GameObject CreateHallway(string _hallwayPath) {

        string hallwayName = "_" + name + "Hallway Exit";

        GameObject hallway = Instantiate(Resources.Load(_hallwayPath)) as GameObject;
        hallway.name = hallwayName;
        hallway.transform.parent = transform;

        return hallway;
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

    public Vector2 GetLevelExitPos(bool _northSouthEnterance) {

        Vector2 levelExitPos = new Vector2();

        if (levelExit.Length != 2) {
            Debug.LogError("Level Exit Length is not 2.");
        } else if (levelExit[0] == -1 || levelExit[1] == -1) {
            //TODO this could be a way of checking if its the last level
            Debug.LogError("Level exit at index 0 or 1 is -1");
        } else {
            
            // This is janky but it returns the position of the proplayer exit doors 
            GameObject exit = propLayer[levelExit[0], levelExit[1]];

            SpriteRenderer exitSR = exit.GetComponent<SpriteRenderer>();

            if(_northSouthEnterance) {
                levelExitPos = new Vector2(exit.transform.position.x + (exitSR.size.x / 2), exit.transform.position.y);
            } else {
                levelExitPos = new Vector2(exit.transform.position.x, exit.transform.position.y - (exitSR.size.y / 2));
            }
        }

        return levelExitPos; 
    }

    public Vector2 GetLevelEnterancePos(bool _northSouthEnterance) {

        Vector2 levelEnterancePos = new Vector2();

        if(levelEnterance.Length != 2) {
            Debug.LogError("Level Exit Length is not 2.");
        } else if(levelEnterance[ 0 ] == -1 || levelEnterance[ 1 ] == -1) {
            //TODO this could be a way of checking if its the first level
            Debug.LogError("Level Enterance at index 0 or 1 is -1");
        } else {
            // This is janky but it returns the position of the proplayer enterance doors 
            //levelEnterancePos = propLayer[ levelEnterance[ 0 ], levelEnterance[ 1 ] ].transform.position;
            GameObject enterance = propLayer[ levelEnterance[ 0 ], levelEnterance[ 1 ] ];
            SpriteRenderer enteranceSR = enterance.GetComponent<SpriteRenderer>(); 

            if (_northSouthEnterance) {
                levelEnterancePos = new Vector2(enterance.transform.position.x + (enteranceSR.size.x / 2), enterance.transform.position.y);
            } else {
                levelEnterancePos = new Vector2(enterance.transform.position.x, enterance.transform.position.y - (enteranceSR.size.y / 2));
            }

        }


        return levelEnterancePos;
    }

    public Vector2 GetCameraFocus() {

        GameObject centerTile =  tileLayer[ LAYER_HEIGHT / 2, LEVEL_DATA_WIDTH / 2 ];
        SpriteRenderer centerTileSR = centerTile.GetComponent<SpriteRenderer>();

        float middleX = centerTile.transform.position.x - (centerTileSR.size.x / 2);
        float middleY = centerTile.transform.position.y - (centerTileSR.size.y / 2);

        deadCenter = new Vector2(middleX, middleY);

        return deadCenter;
    }

    public void SetLevelIndex(int _levelIndex) {
        levelIndex = _levelIndex;
    }

    public int GetLevelIndex() {
        return levelIndex; 
    }

    public void SetPosition(Vector2 _pos) {
        startingPos = _pos;
    }

    public void SetBlockMoving(bool _blockMoving) {
        blockMoving = _blockMoving;
    }

    public bool IsBlockMoving() {
        return blockMoving;
    }
}