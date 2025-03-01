﻿using System.Collections;
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
    public const int BUILDING                                 = 410;
    public const string CEILING_LEFT_CORNER_PATH              = "prefabs/walls/ceilingLeftCorner";
    public const string CEILING_TOP_PATH                      = "prefabs/walls/ceilingTop";
    public const string CEILING_RIGHT_CORNER_PATH             = "prefabs/walls/ceilingRightCorner";
    public const string CEILING_LEFT_PATH                     = "prefabs/walls/ceilingLeft";
    public const string CEILING_RIGHT_PATH                    = "prefabs/walls/ceilingRight";
    public const string CEILING_BOTTOM_PATH                   = "prefabs/walls/ceilingBottom";
    public const string CEILING_CENTER_PATH                   = "prefabs/walls/ceilingCenter";
    public const int PRISON_GATE_LEFT                         = 415;
    public const string PRISON_GATE_LEFT_PATH                 = "prefabs/walls/prisonGateLeft";
    public const int PRISON_GATE_RIGHT                        = 416; 
    public const string PRISON_GATE_RIGHT_PATH                = "prefabs/walls/prisonGateRight";
    public const int PRISON_CORNER_WALL                       = 417;
    public const string PRISON_CORNER_WALL_PATH               = "prefabs/walls/prisonWallCorner"; 
    public const int PRISON_WALL                              = 418;
    public const string PRISON_WALL_PATH                      = "prefabs/walls/prisonWall";
    public const int PRISON_WALL_T                            = 419;
    public const string PRISON_WALL_T_PATH                    = "prefabs/walls/prisonWallT";
    public const int PRISON_WALL_NS                           = 420;
    public const string PRISON_WALL_NS_PATH                   = "prefabs/walls/prisonWallNS";
    public const int GATE_CLOSED_LEFT_DOWN                    = 450;
    public const string GATE_CLOSED_LEFT_PATH                 = "prefabs/walls/gateClosedLeft";
    public const int GATE_CLOSED_RIGHT_UP                     = 451;
    public const string GATE_CLOSED_RIGHT_PATH                = "prefabs/walls/gateClosedRight";
    public const string GATE_CLOSED_SIDE_PATH                 = "prefabs/walls/gateClosedSide";
    public const int GATE_OPEN_RIGHT_UP                       = 452;
    public const string GATE_OPEN_RIGHT_UP_PATH               = "prefabs/walls/gateOpenRight";
    public const string GATE_OPEN_SIDE                        = "prefabs/walls/gateSideOpen";
    public const int GATE_OPEN_LEFT_UP                        = 453;
    public const string GATE_OPEN_LEFT_UP_PATH                = "prefabs/walls/gateOpenLeft";
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
    public const string HOLE_EAST_CORNER_NORTH                = "prefabs/obstacles/hole/holeECornerN";
    public const string HOLE_EAST_CORNER_SOUTH                = "prefabs/obstacles/hole/holeECornerS";
    public const string HOLE_NORTH_CORNER_EAST                = "prefabs/obstacles/hole/holeNCornerE";
    public const string HOLE_NORTH_CORNER_WEST                = "prefabs/obstacles/hole/holeNCornerW";
    public const string HOLE_NORTH_SOUTH_CORNER_EAST          = "prefabs/obstacles/hole/holeNSCornerE";
    public const string HOLE_NORTH_SOUTH_CORNER_WEST          = "prefabs/obstacles/hole/holeNSCornerW";
    public const string HOLE_SOUTH_CORNER_WEST                = "prefabs/obstacles/hole/holeSCornerW";
    public const string HOLE_WEST_CORNER_NORTH                = "prefabs/obstacles/hole/holeWCornerN";
    public const string HOLE_WEST_CORNER_SOUTH                = "prefabs/obstacles/hole/holeWCornerS";
    public const string HOLE_WEST_EAST_CORNER_NORTH           = "prefabs/obstacles/hole/holeWECornerN";
    public const string HOLE_WEST_EAST_CORNER_SOUTH           = "prefabs/obstacles/hole/holeWECornerS";
    public const string HOLE_SOUTH_CORNER_EAST                = "prefabs/obstacles/hole/holeSCornerE";
    public const int TRAP_NORTH                               = 600;
    public const int TRAP_EAST                                = 601;
    public const int TRAP_SOUTH                               = 602;
    public const int TRAP_WEST                                = 603;
    public const string TRAP_PATH                             = "prefabs/obstacles/trapObstacle";

    // DECALS

    public const int SKELETON                                 = 800;
    public const string SKELETON_PATH                         = "prefabs/decals/skeleton";
    public const int SKELETON_PRISONER                        = 801;
    public const string SKELETON_PRISONER_PATH                = "prefabs/decals/skeletonPrisoner";
    public const int SKELETON_SKULL                           = 803;
    public const string SKELETON_SKULL_PATH                   = "prefabs/decals/skull";
    public const int WINDOW                                   = 810;
    public const string WINDOW_PATH                           = "prefabs/decals/window";
    public const int BLOODY_HAND                              = 820;
    public const string BLOODY_HAND_PATH                      = "prefabs/decals/bloodyHand";
    public const int BLOODY_FOOT                              = 821;
    public const string BLOODY_FOOT_PATH                      = "prefabs/decals/bloodyFoot";
    public const int TORCH_EAST_WEST                          = 830;
    public const string TORCH_EAST_WEST_PATH                  = "prefabs/decals/torchEastWest";
    public const string BLANK_DECAL_PATH                      = "prefabs/decals/blank";

    // AUDIO

    public const string BLOCK_HIT_ONE_SFX                     = "sound/fx/BlockHitOne";
    public const string BLOCK_HIT_TWO_SFX                     = "sound/fx/BlockHitTwo";
    public const string BLOCK_HIT_THREE_SFX                   = "sound/fx/BlockHitThree";     
    public const string BLOCK_SHOOT_SFX                       = "sound/fx/Swoosh";
    public const string BLOCK_IN_GOAL_SFX                     = "sound/fx/Goal";
    public const string CAMERA_SWOOSH                         = "sound/fx/cameraSwoosh";
    public const string BUTTON_CLICK_SFX                      = "sound/fx/click";
    public const string TRAP_SFX                              = "sound/fx/Trap";
    public const string FALL_SFX                              = "sound/fx/fallSound";
    public const string GATE_DOWN_SFX                         = "sound/fx/Gate";

}
