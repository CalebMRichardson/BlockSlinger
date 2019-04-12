using System.Collections;
using System.Collections.Generic;


public static class LevelObjectLookup 
{
    // TILES

    public const int BLANK_TILE                               = 000;
    public const string BLANK_TILE_PATH                       = "prefabs/tiles/blank";
    public const int GOAL_TILE                                = 150;
    public const string GOAL_TILE_PATH                        = "prefabs/tiles/goalTile";
    public const int BASIC_TILE_DARK                          = 100;
    public const string BASIC_TILE_DARK_PATH                  = "prefabs/tiles/tileDarkGreen";
    public const int BASIC_TILE_LIGHT                         = 200;
    public const string BASIC_TILE_LIGHT_PATH                 = "prefabs/tiles/tileLightGreen";


    // BLOCKS

    public const int DEFAULT_BLOCK                            = 300;
    public const string DEFAULT_BLOCK_PATH                    = "prefabs/blocks/defaultBlock";
    public const string BLANK_BLOCK_PATH                      = "prefabs/blocks/blank";


    // WALLS

    public const int WALL                                     = 400;
    public const string WALL_NS_PATH                          = "prefabs/walls/wallNS";
    public const string WALL_EW_PATH                          = "prefabs/walls/wallEW";
    public const string WALL_SOUTH_CORNER_PATH                = "prefabs/walls/wallSouthCorner";
    public const int GATE_CLOSED_LEFT_DOWN                    = 450;
    public const string GATE_CLOSED_LEFT_PATH                 = "prefabs/walls/gateClosedLeft";
    public const int GATE_CLOSED_RIGHT_UP                     = 451;
    public const string GATE_CLOSED_RIGHT_PATH                = "prefabs/walls/gateClosedRight";
    public const string GATE_CLOSED_SIDE_PATH                 = "prefabs/walls/gateClosedSide";
    public const string HALLWAY_NS_PATH                       = "prefabs/walls/hallwayNS";
    public const string HALLWAY_EW_PATH                       = "prefabs/walls/hallwayEW";

    // OBSTACLE

    public const int HOLE                                     = 500;
    public const string HOLE_PATH                             = "prefabs/obstacles/hole/hole";
    public const string HOLE_BOTTOM_PATH                      = "prefabs/obstacles/hole/holeBottom";
    public const string HOLE_CENTER_PATH                      = "prefabs/obstacles/hole/holeCenter";
    public const string HOLE_CENTER_CORNERS_PATH              = "prefabs/obstacles/hole/holeCenterCorners";
    public const string HOLE_EAST_PATH                        = "prefabs/obstacles/hole/holeE";
    public const string HOLE_LEFT_PATH                        = "prefabs/obstacles/hole/holeLeft";
    public const string HOLE_NORTH_PATH                       = "prefabs/obstacles/hole/holeN";
    public const string HOLE_NORTH_EAST_PATH                  = "prefabs/obstacles/hole/holeNE";
    public const string HOLE_NORTH_SOUTH_PATH                 = "prefabs/obstacles/hole/holeNorthSouth";
    public const string HOLE_NORTH_WEST_PATH                  = "prefabs/obstacles/hole/holeNW";
    public const string HOLE_RIGHT_PATH                       = "prefabs/obstacles/hole/holeRight";
    public const string HOLE_SOUTH_PATH                       = "prefabs/obstacles/hole/holeS";
    public const string HOLE_SOUTH_EAST_PATH                  = "prefabs/obstacles/hole/holeSE";
    public const string HOLE_SOUTH_WEST_PATH                  = "prefabs/obstacles/hole/holeSW";
    public const string HOLE_TOP_PATH                         = "prefabs/obstacles/hole/holeTop";
    public const string HOLE_WEST_PATH                        = "prefabs/obstacles/hole/holeW";
    public const string HOLE_WEST_EAST_PATH                   = "prefabs/obstacles/hole/holeWestEast";
    public const string HOLE_CENTER_CORNER_NORTH_EAST         = "prefabs/obstacles/hole/holeCenterCornerNE";
    public const string HOLE_CENTER_CORNER_NORTH_WEST         = "prefabs/obstacles/hole/holeCenterCornerNW";
    public const string HOLE_CENTER_CORNER_SOUTH_EAST         = "prefabs/obstacles/hole/holeCenterCornerSE";
    public const string HOLE_CENTER_CORNER_SOUTH_WEST         = "prefabs/obstacles/hole/holeCenterCornerSW";
    public const string HOLE_CORNER_NORTH_WEST                = "prefabs/obstacles/hole/holeCornerNW";
    public const string HOLE_CORNER_NORTH_EAST                = "prefabs/obstacles/hole/holeCornerNE";
    public const string HOLE_CORNER_SOUTH_EAST                = "prefabs/obstacles/hole/holeCornerSE";
    public const string HOLE_CORNER_SOUTH_WEST                = "prefabs/obstacles/hole/holeCornerSW";
    public const int TRAP_NORTH                               = 600;
    public const int TRAP_EAST                                = 601;
    public const int TRAP_SOUTH                               = 602;
    public const int TRAP_WEST                                = 603;
    public const string TRAP_PATH                             = "prefabs/obstacles/trapObstacle";

    // AUDIO

    public const string BLOCK_HIT_ONE_SFX                     = "sound/fx/BlockHitOne";
    public const string BLOCK_HIT_TWO_SFX                     = "sound/fx/BlockHitTwo";
    public const string BLOCK_HIT_THREE_SFX                   = "sound/fx/BlockHitThree";     
    public const string BLOCK_SHOOT_SFX                       = "sound/fx/Swoosh";
    public const string BLOCK_IN_GOAL_SFX                     = "sound/fx/Party-blower";
    public const string CAMERA_SWOOSH                         = "sound/fx/cameraSwoosh";
}
