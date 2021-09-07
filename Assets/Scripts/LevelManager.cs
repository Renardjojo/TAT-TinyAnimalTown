using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EGameState
{
    PATH_SELECTION,
    MOVE
};

public class LevelManager : MonoBehaviour
{
    protected EGameState mGameState = EGameState.PATH_SELECTION;
    
    [Header("Objects")]
    public Camera mCam;
    public Character mCharacter;
    
    public Tile mFromTile;
    
    [Tooltip("Destination")]
    public Tile mToTile;
    
#if UNITY_EDITOR
    [Header("PathStep")]
    [SerializeField] protected bool mUseMobileInput = false;
#endif
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (mGameState)
        {
            case EGameState.PATH_SELECTION:
                UpdatePathSelectionLogic();
                break;
            case EGameState.MOVE:
                UpdateMoveLogic();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void UpdateMoveLogic()
    {
        
    }
    
    void UpdatePathSelectionLogic()
    {
#if UNITY_EDITOR
        bool isClic = (!mUseMobileInput && Input.GetMouseButtonDown(0)) || Input.touchCount == 1;     
#elif UNITY_STANDALONE
        bool isClic = Input.GetMouseButton(0);     
#else
        bool isClic = Input.touchCount == 1;
#endif        

        if (isClic)
        {
            Ray ray = mCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                AddPath(hit.transform.GetComponent<Tile>());
                Debug.DrawLine(mCam.transform.position, hit.point, Color.red, 1f);
            }
            
        }
    }

    void SetGameState(EGameState newGS)
    {
        switch (newGS)
        {
            case EGameState.PATH_SELECTION:
                mCharacter.mPath.Clear();
                mCharacter.mPath.Add(mFromTile);
                break;
            case EGameState.MOVE:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newGS), newGS, null);
        }

        mGameState = newGS;
    }
    
    bool IsTileAdjacentToLastOnPath(Tile tileToAdd)
    {
        return (tileToAdd.transform.position - mCharacter.mPath.Last().transform.position).sqrMagnitude <= Tile.TILE_SIZE;
    }
    
    public void AddPath(Tile tileToAdd)
    {
        //check if previous tile is same (remove of list)
        for (int i = 0; i < mCharacter.mPath.Count; i++)
        {
            if (tileToAdd == mCharacter.mPath[i])
            {
                mCharacter.mPath.RemoveRange(i, mCharacter.mPath.Count - i);
                return; // don't add the tile
            }
        }
        
        //check if tile is nearst than another
        if (IsTileAdjacentToLastOnPath(tileToAdd))
        {
            //Is path done ?
            if (tileToAdd == mToTile)
                SetGameState(EGameState.MOVE);
                
            mCharacter.mPath.Add(tileToAdd);
        }
    }
}
