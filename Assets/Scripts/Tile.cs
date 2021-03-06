using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Tile : MonoBehaviour
{
    public static float TILE_SIZE = 1f;
    public ETileType tileType;
    
    //--------- Ouline FX------------//
    public Outline mOutlineScipt;
    public AudioSource mSound;

    void Awake()
    {
        mOutlineScipt = GetComponent<Outline>();
    }

    void Start()
    {
        UnSelect();
    }

    public void Select()
    {
        mOutlineScipt.enabled = true;
        enabled = true;
        mOutlineScipt.OutlineWidth = 0f;
    }

    public void UnSelect()
    {
        mOutlineScipt.enabled = false;
        enabled = false;
        mOutlineScipt.OutlineWidth = 0f;
    }
}

public enum ETileType
{
    GOOD_GUY,
    OBJECT,
    ROAD,
    BUILDING,
    SEWER,
    STAIR,
    PEDESTRIAN_CROSS,
    DOWN,
    UP,
    LINE,
    TURN,
    STEP_ROAD,
    COUNT
}