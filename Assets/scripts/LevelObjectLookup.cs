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
    public const int GATE_CLOSED_LEFT                         = 450;
    public const string GATE_CLOSED_LEFT_PATH                 = "prefabs/walls/gateClosedLeft";
    public const int GATE_CLOSED_RIGHT                        = 451;
    public const string GATE_CLOSED_RIGHT_PATH                = "prefabs/walls/gateClosedRight";


    // AUDIO

    public const string BLOCK_HIT_ONE_SFX                     = "sound/fx/BlockHitOne";
    public const string BLOCK_HIT_TWO_SFX                     = "sound/fx/BlockHitTwo";
    public const string BLOCK_HIT_THREE_SFX                   = "sound/fx/BlockHitThree";     
    public const string BLOCK_SHOOT_SFX                       = "sound/fx/Swoosh";
    public const string BLOCK_IN_GOAL_SFX                     = "sound/fx/Party-blower";
}
