using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Tile : MonoBehaviour
{
    public static float TILE_SIZE = 1f;
    [SerializeField] private ETileType tileType;
}

public enum ETileType
{
    GOOD_GUY,
    OBJECT,
    PEDESTRIAN_LINE,
    PEDESTRIAN_TURN,
    BUILDING,
    SEWER,
    SLOPE_DOWN,
    SLOPE_UP,
    STAIR,
    COUNT
}

public enum ETileEffect
{
    FAST, 
    SLOW,
    BLOCK,
    COUNT
}
