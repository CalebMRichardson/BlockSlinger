using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LevelBuilder : MonoBehaviour
{
    private const string LEVEL = "LEVEL_";
    private int currentLevel = -1;
    private const int LEVEL_DATA_WIDTH  = 14;
    private const int LEVEL_DATA_HEIGHT = 30;
    private const int LAYER_HEIGHT = 10;
    private JObject levelData;

    public void Awake() {
        LoadLevelData();
    }

    private void LoadLevelData() {

        string levelDataPath = "level_data/levels";

        // Text Asset of levels.json
        TextAsset levelFile = Resources.Load<TextAsset>(levelDataPath);

        string levelDataString = levelFile.ToString();

        // TODO handl levelData being empty

        levelData = JObject.Parse(levelDataString);

    }

    public GameObject Build(int _currentLevel) {

        currentLevel = _currentLevel;

        GameObject builtLevel = Instantiate(Resources.Load("prefabs/Level")) as GameObject;

        if (currentLevel == -1) {
            Debug.LogError("Current Level is -1");
            return null; 
        }

        // used to find the level name by combining LEVEL with currentLevel
        StringBuilder levelKey = new StringBuilder(LEVEL);
        levelKey.Append(currentLevel);

        builtLevel.name = levelKey.ToString();

        string levelExitKey = levelKey + "_EXIT";
        string levelEnteranceKey = levelKey + "_ENTERANCE";

        JArray levelJArray = (JArray)JsonConvert.DeserializeObject(levelData[levelKey.ToString()].ToString());

        JArray levelEnteranceArray = (JArray)JsonConvert.DeserializeObject(levelData[levelEnteranceKey].ToString());
        JArray levelExitArray = (JArray)JsonConvert.DeserializeObject(levelData[levelExitKey].ToString());

        if (levelJArray == null) {
            Debug.LogError("Level Array is Null.");
        }

        int[] levelEnterance = levelEnteranceArray.ToObject<int[ ]>();
        int[] levelExit = levelExitArray.ToObject<int[ ]>();

        int[,] levelArray = levelJArray.ToObject<int[,]>();
        int[,] tileLayerArray = SetArrayLayerData(levelArray, 0);
        int[,] decalLayerArray = SetArrayLayerData(levelArray, 1);
        int[,] propLayerArray = SetArrayLayerData(levelArray, 2);

        Block[,] blocks = new Block[LAYER_HEIGHT, LEVEL_DATA_WIDTH];

        BuildTileLayer(tileLayerArray, ref builtLevel);
        BuildPropLayer(propLayerArray, ref builtLevel);

        Level builtLevelScript = builtLevel.GetComponent<Level>();
        builtLevelScript.SetEnteranceExit(levelEnterance, levelExit);
       
        return builtLevel;
    }

    private void BuildTileLayer(int[,] _tileLayerArray, ref GameObject _level) {

        int correctedY = LAYER_HEIGHT - 1;

        Level levelScript = _level.GetComponent<Level>();
        
        for (int i = 0; i < LAYER_HEIGHT; i++) {
            for (int j = 0; j < LEVEL_DATA_WIDTH; j++) {

                string nameObjectPosition = "" + correctedY + ":" + j;
                
                switch(_tileLayerArray[i,j]) {

                    case LevelObjectLookup.BLANK_TILE:

                        CreateGameObject(LevelObjectLookup.BLANK_TILE_PATH, "BlankTile", j, correctedY, ref _level, true, false, false, false);

                        break;

                    case LevelObjectLookup.GOAL_TILE:

                        GameObject goalTile = CreateGameObject(LevelObjectLookup.GOAL_TILE_PATH, "GoalTile", j, correctedY, ref _level, true, false, false, false);
                        Tile tileScript = goalTile.GetComponent<Tile>();

                        if (tileScript != null) {
                            tileScript.SetIsGoalTile(true);
                        } else {
                            Debug.LogError(goalTile.name + " doesn't have Component: Tile.cs");
                        }

                        break;

                    case LevelObjectLookup.BASIC_TILE_DARK:

                        CreateGameObject(LevelObjectLookup.BASIC_TILE_DARK_PATH, "BasicTileDark", j, correctedY, ref _level, true, false, false, false);

                        break;

                    case LevelObjectLookup.BASIC_TILE_LIGHT:

                        CreateGameObject(LevelObjectLookup.BASIC_TILE_LIGHT_PATH, "BasicTileLight", j, correctedY, ref _level, true, false, false, false);

                        break;
                }
            }
            correctedY--;
        }
    }

    private void BuildPropLayer(int[ , ] _propLayerArray, ref GameObject _level) {

        int leftMostPosition = 0;
        int rightMostPosition = LEVEL_DATA_WIDTH -1;
        int bottomMostPosition = 0;
        int topMostPosition = LAYER_HEIGHT -1;
        int correctedY = LAYER_HEIGHT -1;

        for(int i = 0; i < LAYER_HEIGHT; i++) {

            for(int j = 0; j < LEVEL_DATA_WIDTH; j++) {

                bool eastWall = j == rightMostPosition;
                bool westWall = j == leftMostPosition;
                bool southWall = correctedY == bottomMostPosition;
                bool northWall = correctedY == topMostPosition;
                bool southWestCorner = westWall && southWall;
                bool northWestCorner = westWall && northWall;
                bool northEastCorner = eastWall && northWall;
                bool southEastCorner = eastWall && southWall;

                switch (_propLayerArray[i,j]) {

                    case LevelObjectLookup.BLANK_TILE:

                        CreateGameObject(LevelObjectLookup.BLANK_BLOCK_PATH, "Blank", j, correctedY, ref _level, false, true, false, true);

                        break;

                    case LevelObjectLookup.DEFAULT_BLOCK:

                        CreateGameObject(LevelObjectLookup.DEFAULT_BLOCK_PATH, "Block", j, correctedY, ref _level, false, true, false, false);

                        break;

                    case LevelObjectLookup.WALL:

                        if (northWall && !westWall && !eastWall) {
                           CreateGameObject(LevelObjectLookup.WALL_NS_PATH, "Wall", j, correctedY, ref _level, false, true, false, false); 
                        } else if (southWall && !westWall && !eastWall) {
                            GameObject go = CreateGameObject(LevelObjectLookup.WALL_NS_PATH, "Wall", j, correctedY, ref _level, false, true, false, false);
                            go.transform.Rotate(Vector2.left * 0);                           
                        }

                        if (eastWall && !southWall) {
                            GameObject go = CreateGameObject(LevelObjectLookup.WALL_EW_PATH, "Wall", j, correctedY, ref _level, false, true, false, false);
                            //go.transform.Rotate(Vector2.left * 180);
                        } else if (westWall && !southWall) {
                            GameObject go = CreateGameObject(LevelObjectLookup.WALL_EW_PATH, "Wall", j, correctedY, ref _level, false, true, false, false);
                            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                            sr.flipX = true;

                        }

                        if (southWall && eastWall) {
                            CreateGameObject(LevelObjectLookup.WALL_SOUTH_CORNER_PATH, "Wall", j, correctedY, ref _level, false, true, false, false);
                        } else if (southWall && westWall) {
                            GameObject go = CreateGameObject(LevelObjectLookup.WALL_SOUTH_CORNER_PATH, "Wall", j, correctedY, ref _level, false, true, false, false);
                            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                            sr.flipX = true;
                        }

                        break;

                    case LevelObjectLookup.GATE_CLOSED_RIGHT:

                        CreateGameObject(LevelObjectLookup.GATE_CLOSED_RIGHT_PATH, "Gate", j, correctedY, ref _level, false, true, false, false);

                        if (southWall) {
                            //TODO roate
                        }

                        if (eastWall) {
                            //TODO Rotate -90
                            //when art comes in
                        } else if (westWall) {
                            //TODO rotate 90
                            //when art comes in
                        }

                        break;

                    case LevelObjectLookup.GATE_CLOSED_LEFT:

                        CreateGameObject(LevelObjectLookup.GATE_CLOSED_LEFT_PATH, "Wall", j, correctedY, ref _level, false, true, false, false);

                        if (southWall) {
                            // rotate 180
                        }

                        break;
                }

            }
            correctedY--;
        }
    }

    private GameObject CreateGameObject(string _resourcePath, string _name, int _x, int _y, ref GameObject _level, bool _isTile, bool _isProp, bool _isDecal, bool _isBlank) {

        Level levelScript = _level.GetComponent<Level>(); 

        GameObject go = Instantiate(Resources.Load(_resourcePath)) as GameObject; 
        go.name = "_" + _name + _y + ":" + _x;
        go.transform.parent = _level.transform;
        SpriteRenderer goSpriteRenderer = go.GetComponent<SpriteRenderer>();
        go.transform.position = new Vector2(goSpriteRenderer.size.x * _x, goSpriteRenderer.size.y * _y);

        if(_isTile) {
            levelScript.tileLayer[ _y, _x ] = go;
            Tile tileScript = go.GetComponent<Tile>();
            tileScript.SetXY(_x, _y);
        } else if(_isProp) {
            levelScript.propLayer[ _y, _x ] = go;
            Prop prop = go.GetComponent<Prop>();
            prop.SetIsBlank(_isBlank);
            prop.SetXY(_x, _y);
        } else if(_isDecal)
            levelScript.decalLayer[ _y, _x ] = go;

        return go;
    }

    private int[,] SetArrayLayerData(int[,] _layerArray, int _layerLevel) {

        int[,] layerArray = new int[LAYER_HEIGHT,LEVEL_DATA_WIDTH];

        int scaledYStartingPos = _layerLevel * LAYER_HEIGHT;

        for (int i = scaledYStartingPos; i < LAYER_HEIGHT + scaledYStartingPos; i++) {
            for (int j = 0; j < LEVEL_DATA_WIDTH; j++) {
                layerArray[ i - scaledYStartingPos, j ] = _layerArray[ i, j ];
            }
        }

        return layerArray;
    }

   
}
