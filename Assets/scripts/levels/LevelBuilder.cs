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
        builtLevelScript.SetLevelIndex(currentLevel);
       
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

                    case LevelObjectLookup.GATE_CLOSED_RIGHT_UP:

                        if(northWall || southWall) {

                            CreateGameObject(LevelObjectLookup.GATE_CLOSED_RIGHT_PATH, "Gate", j, correctedY, ref _level, false, true, false, false);

                        } else if (eastWall) {

                            CreateGameObject(LevelObjectLookup.GATE_CLOSED_SIDE_PATH, "Gate", j, correctedY, ref _level, false, true, false, false); 

                        } else if (westWall) {
                            GameObject go = CreateGameObject(LevelObjectLookup.GATE_CLOSED_SIDE_PATH, "Gate", j, correctedY, ref _level, false, true, false, false);
                            go.transform.Rotate(Vector2.up * 180);
                        }

                        break;

                    case LevelObjectLookup.GATE_CLOSED_LEFT_DOWN:

                        if(northWall || southWall) {
                            CreateGameObject(LevelObjectLookup.GATE_CLOSED_LEFT_PATH, "Wall", j, correctedY, ref _level, false, true, false, false);
                        } else if(eastWall) {

                            CreateGameObject(LevelObjectLookup.GATE_CLOSED_SIDE_PATH, "Gate", j, correctedY, ref _level, false, true, false, false);

                        } else if(westWall) {
                            GameObject go = CreateGameObject(LevelObjectLookup.GATE_CLOSED_SIDE_PATH, "Gate", j, correctedY, ref _level, false, true, false, false);
                            go.transform.Rotate(Vector2.up * 180);
                        }


                        break;

                    case LevelObjectLookup.HOLE:

                        string resourcePath = "";

                        resourcePath = GetHoleType(ref _propLayerArray, j, i);

                        print(resourcePath);

                        CreateGameObject(resourcePath, "Hole", j, correctedY, ref _level, false, true, false, false);

                        break;

                    case LevelObjectLookup.TRAP_NORTH:

                        GameObject trapNorth = CreateGameObject(LevelObjectLookup.TRAP_PATH, "Trap", j, correctedY, ref _level, false, true, false, false);
                        trapNorth.transform.eulerAngles = new Vector3(0, 0, -90);
                        Trap trapNorthScript = trapNorth.GetComponent<Trap>();

                        trapNorthScript.direction = Vector2.up;

                        break;

                    case LevelObjectLookup.TRAP_EAST:

                        GameObject trapEast = CreateGameObject(LevelObjectLookup.TRAP_PATH, "Trap", j, correctedY, ref _level, false, true, false, false);
                        trapEast.transform.eulerAngles = new Vector3(0, 0, 180);
                        Trap trapEastScript = trapEast.GetComponent<Trap>();

                        trapEastScript.direction = Vector2.right;


                        break;

                    case LevelObjectLookup.TRAP_SOUTH:

                        GameObject trapSouth = CreateGameObject(LevelObjectLookup.TRAP_PATH, "Trap", j, correctedY, ref _level, false, true, false, false);
                        trapSouth.transform.eulerAngles = new Vector3(0, 0, 90);
                        Trap trapSouthScript = trapSouth.GetComponent<Trap>();

                        trapSouthScript.direction = Vector2.down;


                        break;

                    case LevelObjectLookup.TRAP_WEST:

                        GameObject trapWest = CreateGameObject(LevelObjectLookup.TRAP_PATH, "Trap", j, correctedY, ref _level, false, true, false, false);
                        Trap trapWestScript = trapWest.GetComponent<Trap>();

                        trapWestScript.direction = Vector2.left;

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
            tileScript.SetStaringInfo(_x, _y);
        } else if(_isProp) {
            levelScript.propLayer[ _y, _x ] = go;
            Prop prop = go.GetComponent<Prop>();
            prop.SetIsBlank(_isBlank);
            prop.SetXY(_x, _y);
            prop.SetStartingInfo(prop.transform.position, _x, _y);
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

    private string GetHoleType(ref int[,] _propLayer, int _x, int _y) {

        string resourcePath = "";
        int hole = LevelObjectLookup.HOLE;

        int top = _propLayer[_y - 1, _x];
        int bottom = _propLayer[_y + 1, _x];
        int left = _propLayer[_y, _x - 1];
        int right = _propLayer[_y, _x + 1]; 
        int topRight = _propLayer[_y  - 1, _x + 1];
        int topLeft = _propLayer[_y  - 1, _x -1];
        int bottomRight = _propLayer[_y + 1, _x + 1];
        int bottomLeft = _propLayer[_y + 1, _x - 1];

        // Hole Bottom
        if (top == hole && left != hole && bottom != hole && right != hole) {
            resourcePath = LevelObjectLookup.HOLE_BOTTOM_PATH;
            return resourcePath;
        }
        // Hole Center
        else if (top == hole && left == hole && right == hole && bottom == hole && topLeft == hole && topRight == hole && bottomLeft == hole && bottomRight == hole) {
            resourcePath = LevelObjectLookup.HOLE_CENTER_PATH;
            return resourcePath;
        }
        // Hole Center Corners
        else if (top == hole && left == hole && right == hole && bottom == hole && topLeft != hole && topRight != hole && bottomLeft != hole && bottomRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_CENTER_CORNERS_PATH;
            return resourcePath;
        }
        // Hole East
        else if (left == hole && bottom == hole && top == hole && right != hole) {
            resourcePath = LevelObjectLookup.HOLE_EAST_PATH;
            return resourcePath;
        }
        // Hole Left
        else if (right == hole && left != hole && top != hole && bottom != hole) {
            resourcePath = LevelObjectLookup.HOLE_LEFT_PATH;
            return resourcePath;
        }
        // Hole North
        else if (left == hole && right == hole && bottom == hole && top != hole) {
            resourcePath = LevelObjectLookup.HOLE_NORTH_PATH;
            return resourcePath;
        }
        // Hole North East
        else if (left == hole && bottom == hole && bottomLeft == hole && top != hole && right != hole) {
            resourcePath = LevelObjectLookup.HOLE_NORTH_EAST_PATH;
            return resourcePath;
        } 
        // Hole North South
        else if (left == hole && right == hole && top != hole && bottom != hole) {
            resourcePath = LevelObjectLookup.HOLE_NORTH_SOUTH_PATH;
            return resourcePath;
        }
        // Hole North West
        else if (bottom == hole && right == hole && bottomRight == hole && top != hole && left != hole) {
            resourcePath = LevelObjectLookup.HOLE_NORTH_WEST_PATH;
            return resourcePath;
        }
        // Hole right
        else if (left == hole && top != hole && bottom != hole && right != hole) {
            resourcePath = LevelObjectLookup.HOLE_RIGHT_PATH;
            return resourcePath;
        }
        // Hole South
        else if (left == hole && top == hole && right == hole && bottom != hole) {
            resourcePath = LevelObjectLookup.HOLE_SOUTH_PATH;
            return resourcePath;
        }
        // Hole South East
        else if (left == hole && top == hole && topLeft == hole && bottom != hole && right != hole) {
            resourcePath = LevelObjectLookup.HOLE_SOUTH_EAST_PATH;
            return resourcePath;
        }
        // Hole South West
        else if (top == hole && right == hole && topRight == hole && left != hole && bottom != hole) {
            resourcePath = LevelObjectLookup.HOLE_SOUTH_WEST_PATH;
            return resourcePath;
        }
        // Hole Top
        else if (bottom == hole && top != hole && left != hole && right != hole) {
            resourcePath = LevelObjectLookup.HOLE_TOP_PATH;
            return resourcePath;
        }
        // Hole West
        else if (top == hole && right == hole && bottom == hole && left != hole) {
            resourcePath = LevelObjectLookup.HOLE_WEST_PATH;
            return resourcePath;
        }
        // Hole West East
        else if (top == hole && bottom == hole && left != hole && right != hole) {
            resourcePath = LevelObjectLookup.HOLE_WEST_EAST_PATH;
            return resourcePath;
        }
        // Hole Center Corner North East
        else if (top == hole && right == hole && bottom == hole && left == hole && topRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_CENTER_CORNER_NORTH_EAST;
            return resourcePath;
        }
        // Hole Center Corner North West
        else if (top == hole && right == hole && bottom == hole && left == hole && topLeft != hole) {
            resourcePath = LevelObjectLookup.HOLE_CENTER_CORNER_NORTH_WEST;
            return resourcePath;
        }
        // Hole Center Corner South East
        else if (top == hole && right == hole && bottom == hole && left == hole && bottomRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_CENTER_CORNER_SOUTH_EAST;
            return resourcePath;
        }
        // Hole Center Corner South West
        else if (top == hole && right == hole && bottom == hole && left == hole && bottomLeft != hole) {
            resourcePath = LevelObjectLookup.HOLE_CENTER_CORNER_SOUTH_WEST;
            return resourcePath;
        }
        // Hole Corner North West
        else if (bottom == hole && right == hole && top != hole && left != hole && bottomRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_CORNER_NORTH_WEST;
            return resourcePath;
        } 
        // Hole Corner North East
        else if (left == hole && bottom == hole && top != hole && right != hole && bottomLeft != hole) {
            resourcePath = LevelObjectLookup.HOLE_CORNER_NORTH_EAST;
            return resourcePath;
        }
        // Hole Corner South East
        else if (top == hole && left == hole && right != hole && bottom != hole && topLeft != hole) {
            resourcePath = LevelObjectLookup.HOLE_CORNER_SOUTH_EAST;
            return resourcePath;
        }
        // Hole Corner South West
        else if (top == hole && right == hole && left != hole && bottom != hole && topRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_CORNER_SOUTH_WEST;
            return resourcePath;
        }
        // Normal Hole
        else if (left != hole && right != hole && top != hole && bottom != hole) {
            resourcePath = LevelObjectLookup.HOLE_PATH;
            return resourcePath;
        }

        return resourcePath;
    }

}
