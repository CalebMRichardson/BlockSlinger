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

        if (currentLevel > 25) {
            Debug.LogError("Current Level is more then max levels: " + currentLevel);
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
        BuildDecalLayer(decalLayerArray, ref builtLevel);

        Level builtLevelScript = builtLevel.GetComponent<Level>();
        builtLevelScript.SetEnteranceExit(levelEnterance, levelExit);
        builtLevelScript.SetLevelIndex(currentLevel);
       
        return builtLevel;
    }

    private void BuildTileLayer(int[,] _tileLayerArray, ref GameObject _level) {

        int correctedY = LAYER_HEIGHT - 1;

        //TODO remove this :D
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

    private void BuildDecalLayer(int[ , ] _decalLayerArray, ref GameObject _level) {

        int correctedY = LAYER_HEIGHT - 1;



        for(int i = 0; i < LAYER_HEIGHT; i++) {

            for(int j = 0; j < LEVEL_DATA_WIDTH; j++) {

                switch(_decalLayerArray[ i, j ]) {

                    case LevelObjectLookup.BLANK_TILE:

                        CreateGameObject(LevelObjectLookup.BLANK_DECAL_PATH, "Blank", j, correctedY, ref _level, false, false, true, false);

                        break;

                    case LevelObjectLookup.SKELETON:

                        CreateGameObject(LevelObjectLookup.SKELETON_PATH, "Skeleton", j, correctedY, ref _level, false, false, true, false);

                        break;

                    case LevelObjectLookup.SKELETON_PRISONER:

                        CreateGameObject(LevelObjectLookup.SKELETON_PRISONER_PATH, "Skeleton_Prisoner", j, correctedY, ref _level, false, false, true, false);

                        break;

                    case LevelObjectLookup.SKELETON_SKULL:

                        CreateGameObject(LevelObjectLookup.SKELETON_SKULL_PATH, "Skeleton_Skull", j, correctedY, ref _level, false, false, true, false);

                        break;

                    case LevelObjectLookup.WINDOW:

                        CreateGameObject(LevelObjectLookup.WINDOW_PATH, "Window", j, correctedY, ref _level, false, false, true, false);

                        break;

                    case LevelObjectLookup.BLOODY_HAND:

                        CreateGameObject(LevelObjectLookup.BLOODY_HAND_PATH, "Bloody_Hand", j, correctedY, ref _level, false, false, true, false);

                        break;

                    case LevelObjectLookup.BLOODY_FOOT:

                        CreateGameObject(LevelObjectLookup.BLOODY_FOOT_PATH, "Bloody_Foot", j, correctedY, ref _level, false, false, true, false);

                        break;

                    case LevelObjectLookup.TORCH_EAST_WEST:

                        GameObject go = CreateGameObject(LevelObjectLookup.TORCH_EAST_WEST_PATH, "Torch", j, correctedY, ref _level, false, false, true, false);

                        // Left most position
                        if (j -1 == 0) {
                            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();

                            sr.flipX = true;
                        }

                        break;

                    default:

                        CreateGameObject(LevelObjectLookup.BLANK_DECAL_PATH, "Blank", j, correctedY, ref _level, false, false, true, false);
                        Debug.LogError("LevelBuilder Decale hit default case.");

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

        Level currentLevelScript = _level.GetComponent<Level>();

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

                    case LevelObjectLookup.BUILDING:

                        string resourcePathBuilding = "";

                        resourcePathBuilding = GetBuildingType(ref _propLayerArray, j, i);

                        CreateGameObject(resourcePathBuilding, "Building", j, correctedY, ref _level, false, true, false, false);

                        break;

                    case LevelObjectLookup.PRISON_GATE_LEFT:

                        CreateGameObject(LevelObjectLookup.PRISON_GATE_LEFT_PATH, "Prison", j, correctedY, ref _level, false, true, false, false);

                        break;

                    case LevelObjectLookup.PRISON_GATE_RIGHT:

                        CreateGameObject(LevelObjectLookup.PRISON_GATE_RIGHT_PATH, "Prison", j, correctedY, ref _level, false, true, false, false);

                        break;

                    case LevelObjectLookup.PRISON_CORNER_WALL:

                        CreateGameObject(LevelObjectLookup.PRISON_CORNER_WALL_PATH, "Prison", j, correctedY, ref _level, false, true, false, false);

                        break;

                    case LevelObjectLookup.PRISON_WALL:

                        CreateGameObject(LevelObjectLookup.PRISON_WALL_PATH, "Prison", j, correctedY, ref _level, false, true, false, false);

                        break;

                    case LevelObjectLookup.PRISON_WALL_T:

                        CreateGameObject(LevelObjectLookup.PRISON_WALL_T_PATH, "Prison", j, correctedY, ref _level, false, true, false, false);

                        break;

                    case LevelObjectLookup.PRISON_WALL_NS:

                        CreateGameObject(LevelObjectLookup.PRISON_WALL_NS_PATH, "Prison", j, correctedY, ref _level, false, true, false, false);

                        break;

                    case LevelObjectLookup.GATE_CLOSED_RIGHT_UP:

                        if(northWall || southWall) {

                            GameObject northSouthGate = CreateGameObject(LevelObjectLookup.GATE_CLOSED_RIGHT_PATH, "Gate", j, correctedY, ref _level, false, true, false, false);
                        } else if (eastWall) {

                            GameObject eastGate = CreateGameObject(LevelObjectLookup.GATE_CLOSED_SIDE_PATH, "Gate", j, correctedY, ref _level, false, true, false, false); 

                        } else if (westWall) {
                            GameObject westGate = CreateGameObject(LevelObjectLookup.GATE_CLOSED_SIDE_PATH, "Gate", j, correctedY, ref _level, false, true, false, false);
                            westGate.transform.Rotate(Vector2.up * 180);
                        }

                        break;

                    case LevelObjectLookup.GATE_CLOSED_LEFT_DOWN:

                        if(northWall || southWall) {

                            CreateGameObject(LevelObjectLookup.GATE_CLOSED_LEFT_PATH, "Gate", j, correctedY, ref _level, false, true, false, false);

                        } else if(eastWall) {

                            CreateGameObject(LevelObjectLookup.GATE_CLOSED_SIDE_PATH, "Gate", j, correctedY, ref _level, false, true, false, false);

                        } else if(westWall) {

                            GameObject go = CreateGameObject(LevelObjectLookup.GATE_CLOSED_SIDE_PATH, "Gate", j, correctedY, ref _level, false, true, false, false);
                            go.transform.Rotate(Vector2.up * 180);

                        }


                        break;

                    case LevelObjectLookup.GATE_OPEN_LEFT_UP:

                        if(northWall || southWall) {

                            CreateGameObject(LevelObjectLookup.GATE_OPEN_LEFT_UP_PATH, "Gate", j, correctedY, ref _level, false, true, false, false);

                        } else if(eastWall) {

                            CreateGameObject(LevelObjectLookup.GATE_OPEN_SIDE, "Gate", j, correctedY, ref _level, false, true, false, false);

                        } else if(westWall) {

                            CreateGameObject(LevelObjectLookup.GATE_OPEN_SIDE, "Gate", j, correctedY, ref _level, false, true, false, false);

                        }

                        break;

                    case LevelObjectLookup.GATE_OPEN_RIGHT_UP:

                        if (northWall || southWall) {

                            CreateGameObject(LevelObjectLookup.GATE_OPEN_RIGHT_UP_PATH, "Gate", j, correctedY, ref _level, false, true, false, false);

                        } else if (eastWall) {

                            CreateGameObject(LevelObjectLookup.GATE_OPEN_SIDE, "Gate", j, correctedY, ref _level, false, true, false, false);

                        } else if (westWall) {

                            CreateGameObject(LevelObjectLookup.GATE_OPEN_SIDE, "Gate", j, correctedY, ref _level, false, true, false, false);
                        }

                        break;

                    case LevelObjectLookup.HOLE:

                        string resourcePath = "";

                        resourcePath = GetHoleType(ref _propLayerArray, j, i);

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

        if (go == null) {
            Debug.LogError(_resourcePath); 
        }

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
        } else if(_isDecal) {
            levelScript.decalLayer[ _y, _x ] = go;
            Decal decal = go.GetComponent<Decal>();
            decal.SetXY(_x, _y); 
        }

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
        if(top == hole && left != hole && bottom != hole && right != hole) {
            resourcePath = LevelObjectLookup.HOLE_BOTTOM_PATH;
            return resourcePath;
        }
        // Hole Center
        else if(top == hole && left == hole && right == hole && bottom == hole && topLeft == hole && topRight == hole && bottomLeft == hole && bottomRight == hole) {
            resourcePath = LevelObjectLookup.HOLE_CENTER_PATH;
            return resourcePath;
        }
        // Hole Center Corners
        else if(top == hole && left == hole && right == hole && bottom == hole && topLeft != hole && topRight != hole && bottomLeft != hole && bottomRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_CENTER_CORNERS_PATH;
            return resourcePath;
        }
        // Hole East
        else if(left == hole && bottom == hole && top == hole && topLeft == hole && bottomLeft == hole && right != hole) {
            resourcePath = LevelObjectLookup.HOLE_EAST_PATH;
            return resourcePath;
        }
        // Hole Left
        else if(right == hole && left != hole && top != hole && bottom != hole) {
            resourcePath = LevelObjectLookup.HOLE_LEFT_PATH;
            return resourcePath;
        }
        // Hole North
        else if(left == hole && right == hole && bottom == hole && bottomLeft == hole && bottomRight == hole && top != hole) {
            resourcePath = LevelObjectLookup.HOLE_NORTH_PATH;
            return resourcePath;
        }
        // Hole North East
        else if(left == hole && bottom == hole && bottomLeft == hole && top != hole && right != hole) {
            resourcePath = LevelObjectLookup.HOLE_NORTH_EAST_PATH;
            return resourcePath;
        }
        // Hole North South
        else if(left == hole && right == hole && top != hole && bottom != hole) {
            resourcePath = LevelObjectLookup.HOLE_NORTH_SOUTH_PATH;
            return resourcePath;
        }
        // Hole North West
        else if(bottom == hole && right == hole && bottomRight == hole && top != hole && left != hole) {
            resourcePath = LevelObjectLookup.HOLE_NORTH_WEST_PATH;
            return resourcePath;
        }
        // Hole right
        else if(left == hole && top != hole && bottom != hole && right != hole) {
            resourcePath = LevelObjectLookup.HOLE_RIGHT_PATH;
            return resourcePath;
        }
        // Hole South
        else if(left == hole && top == hole && right == hole && bottom != hole) {
            resourcePath = LevelObjectLookup.HOLE_SOUTH_PATH;
            return resourcePath;
        }
        // Hole South East
        else if(left == hole && top == hole && topLeft == hole && bottom != hole && right != hole) {
            resourcePath = LevelObjectLookup.HOLE_SOUTH_EAST_PATH;
            return resourcePath;
        }
        // Hole South West
        else if(top == hole && right == hole && topRight == hole && left != hole && bottom != hole) {
            resourcePath = LevelObjectLookup.HOLE_SOUTH_WEST_PATH;
            return resourcePath;
        }
        // Hole Top
        else if(bottom == hole && top != hole && left != hole && right != hole) {
            resourcePath = LevelObjectLookup.HOLE_TOP_PATH;
            return resourcePath;
        }
        // Hole West
        else if(top == hole && right == hole && bottom == hole && topRight == hole && bottomRight == hole && left != hole) {
            resourcePath = LevelObjectLookup.HOLE_WEST_PATH;
            return resourcePath;
        }
        // Hole West East
        else if(top == hole && bottom == hole && left != hole && right != hole) {
            resourcePath = LevelObjectLookup.HOLE_WEST_EAST_PATH;
            return resourcePath;
        }
        // Hole Center Corner North East
        else if(top == hole && right == hole && bottom == hole && left == hole && topRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_CENTER_CORNER_NORTH_EAST;
            return resourcePath;
        }
        // Hole Center Corner North West
        else if(top == hole && right == hole && bottom == hole && left == hole && topLeft != hole) {
            resourcePath = LevelObjectLookup.HOLE_CENTER_CORNER_NORTH_WEST;
            return resourcePath;
        }
        // Hole Center Corner South East
        else if(top == hole && right == hole && bottom == hole && left == hole && bottomRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_CENTER_CORNER_SOUTH_EAST;
            return resourcePath;
        }
        // Hole Center Corner South West
        else if(top == hole && right == hole && bottom == hole && left == hole && bottomLeft != hole) {
            resourcePath = LevelObjectLookup.HOLE_CENTER_CORNER_SOUTH_WEST;
            return resourcePath;
        }
        // Hole Corner North West
        else if(bottom == hole && right == hole && top != hole && left != hole && bottomRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_CORNER_NORTH_WEST;
            return resourcePath;
        }
        // Hole Corner North East
        else if(left == hole && bottom == hole && top != hole && right != hole && bottomLeft != hole) {
            resourcePath = LevelObjectLookup.HOLE_CORNER_NORTH_EAST;
            return resourcePath;
        }
        // Hole Corner South East
        else if(top == hole && left == hole && right != hole && bottom != hole && topLeft != hole) {
            resourcePath = LevelObjectLookup.HOLE_CORNER_SOUTH_EAST;
            return resourcePath;
        }
        // Hole Corner South West
        else if(top == hole && right == hole && left != hole && bottom != hole && topRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_CORNER_SOUTH_WEST;
            return resourcePath;
        }
        // Hole East Corner North
        else if(right == hole && bottom == hole && top != hole && bottomRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_EAST_CORNER_NORTH;
            return resourcePath;
        }
        // Hole East Corner South
        else if(top == hole && right == hole && bottom != hole && topRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_EAST_CORNER_SOUTH;
            return resourcePath;
        }
        // Hole North Corner East
        else if(top == hole && left == hole && bottomLeft == hole && right != hole && topLeft != hole) {
            resourcePath = LevelObjectLookup.HOLE_NORTH_CORNER_EAST;
            return resourcePath;
        }
        // Hole North Corner West
        else if(top == hole && right == hole && bottomRight == hole && left != hole && topRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_NORTH_CORNER_WEST;
            return resourcePath;
        }
        // Hole North South Corner East
        else if(top == hole && left == hole && bottom == hole && right != hole && bottomLeft != hole && topLeft != hole) {
            resourcePath = LevelObjectLookup.HOLE_NORTH_SOUTH_CORNER_EAST;
            return resourcePath;
        }
        // Hole North South Corner West
        else if(top == hole && right == hole && bottom == hole && left != hole && topRight != hole && bottomRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_NORTH_SOUTH_CORNER_WEST;
            return resourcePath;
        }
        // Hole South Corner West
        else if(top == hole && bottom == hole && right == hole && topRight == hole && left != hole && bottomRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_SOUTH_CORNER_WEST;
            return resourcePath;
        }
        // Hole West Corner North
        else if(left == hole && bottom == hole && bottomRight == hole && top != hole && bottomLeft != hole) {
            resourcePath = LevelObjectLookup.HOLE_WEST_CORNER_NORTH;
            return resourcePath;
        }
        // Hole West Corner South 
        else if(top == hole && left == hole && topLeft != hole && bottom != hole) {
            resourcePath = LevelObjectLookup.HOLE_WEST_CORNER_SOUTH;
            return resourcePath;
        }
        // Hole West East Corner North
        else if(left == hole && bottom == hole && right == hole && top != hole && bottomLeft != hole && bottomRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_WEST_EAST_CORNER_NORTH;
            return resourcePath;
        }
        // Hole West East Corner South
        else if(top == hole && left == hole && right == hole && bottom != hole && topLeft != hole && topRight != hole) {
            resourcePath = LevelObjectLookup.HOLE_WEST_EAST_CORNER_SOUTH;
            return resourcePath;
        }
        // Hole South Corner East
        else if(top == hole && left == hole && bottom == hole && topLeft == hole && right != hole && bottomLeft != hole) {
            resourcePath = LevelObjectLookup.HOLE_SOUTH_CORNER_EAST;
            return resourcePath; 
        }
        // Normal Hole
        else {
            resourcePath = LevelObjectLookup.HOLE_PATH;
            return resourcePath;
        }

        return resourcePath;
    }

    private string GetBuildingType (ref int[,] _propLayer, int _x, int _y) {

        int building = LevelObjectLookup.BUILDING;

        int top = _propLayer[_y - 1, _x];
        int bottom = _propLayer[_y + 1, _x];
        int left = _propLayer[_y, _x - 1];
        int right = _propLayer[_y, _x + 1];
        int topRight = _propLayer[_y  - 1, _x + 1];
        int topLeft = _propLayer[_y  - 1, _x -1];
        int bottomRight = _propLayer[_y + 1, _x + 1];
        int bottomLeft = _propLayer[_y + 1, _x - 1];

        string resourcePath = "";

        // Ceiling Left Corner
        if (right == building && bottom == building && top != building && left != building) {
            resourcePath = LevelObjectLookup.CEILING_LEFT_CORNER_PATH;
            return resourcePath;
        } 
        // Ceiling Top
        else if (left == building && right == building && top != building) {
            resourcePath = LevelObjectLookup.CEILING_TOP_PATH;
            return resourcePath;
        } 
        // Ceiling Right Corner
        else if (left == building && bottom == building && top != building && right != building) {
            resourcePath = LevelObjectLookup.CEILING_RIGHT_CORNER_PATH;
            return resourcePath;
        }
        // Ceiling Left
        else if (top == building && bottom == building && left != building) {
            resourcePath = LevelObjectLookup.CEILING_LEFT_PATH;
            return resourcePath;
        }
        // Ceiling Right
        else if (top == building && bottom == building && right != building) {
            resourcePath = LevelObjectLookup.CEILING_RIGHT_PATH;
            return resourcePath;
        }
        // Ceiling Bottom Left Corner
        else if (top == building && right == building && left != building && bottom != building) {
            resourcePath = LevelObjectLookup.CEILING_BOTTOM_PATH;
            return resourcePath;
        }
        // Ceiling Bottom Right Corner
        else if (top == building && left == building && right != building && bottom != building) {
            resourcePath = LevelObjectLookup.CEILING_BOTTOM_PATH;
            return resourcePath;
        }
        // Ceiling Bottom 
        else if (left == building && right == building && bottom != building) {
            resourcePath = LevelObjectLookup.CEILING_BOTTOM_PATH;
            return resourcePath;
        }
        // Ceiling Center
        else if (left == building && right == building && top == building && bottom == building) {
            resourcePath = LevelObjectLookup.CEILING_CENTER_PATH;
            return resourcePath; 
        }

        return resourcePath;
    }

}
