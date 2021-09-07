using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EGameState
{
    PATH_SELECTION,
    MOVE,
    SCORE
};

public class LevelManager : MonoBehaviour
{
    protected EGameState mGameState = EGameState.PATH_SELECTION;

    [Header("Game attribut")]
    public float mBestTime = 0f;
    public float mCurrentTime = 0f; // Current time in game

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
    
    [Header("MoveStep")]
    [SerializeField] protected Coroutine mMoveCoroutine;
    
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
                break;
            case EGameState.SCORE:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void PlayAnimation(float deltaY)
    {
        if (deltaY < 0f)
        {
            mCharacter.mAnimator.SetTrigger("JumpLow");
        }
        else if (deltaY > 0f)
        {
            mCharacter.mAnimator.SetTrigger("JumpHigh");
        }
        else
        {
            mCharacter.mAnimator.SetTrigger("JumpSame");
        }
    }

    void ApplyTileEffect()
    {
        mCurrentTime += mCharacter.mUserData.mTilesEffectOnCharacter[(int) mCharacter.mPath.First().tileType]
            .mTimeEffect;
    }

    protected IEnumerator MoveCoroutine()
    {
        float t = 0f;
        Vector3 fromPos = mCharacter.transform.position;

        // Play jump animation
        PlayAnimation(mCharacter.mPath.First().transform.position.y - mCharacter.transform.position.y);
        
        do
        {
            t += Time.deltaTime;
            mCharacter.transform.position = Vector3.Lerp(fromPos, mCharacter.mPath.First().transform.position, t);
            yield return null;
        } while (t < 1f);

        ApplyTileEffect();
        mCharacter.mPath.RemoveAt(0);

        if (mCharacter.mPath.Count != 0)
        {
            StartCoroutine(MoveCoroutine());
        }
        else
        {
            SetGameState(EGameState.SCORE);
        }
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

    public void SetGameState(EGameState newGS)
    {
        switch (newGS)
        {
            case EGameState.PATH_SELECTION:
                mCharacter = Instantiate(mCharacter, mFromTile.transform.position, Quaternion.identity);
                mCharacter.mPath.Clear();
                mCharacter.mPath.Add(mFromTile);
                break;
            case EGameState.MOVE:
                //Remove the first path that is the start position
                mCharacter.mPath.RemoveAt(0);
                mMoveCoroutine = StartCoroutine(MoveCoroutine());
                break;
            case EGameState.SCORE:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newGS), newGS, null);
        }

        mGameState = newGS;
    }
    
    bool IsTileAdjacentToLastOnPath(Tile tileToAdd)
    {
        Vector2 direction = new Vector2(tileToAdd.transform.position.x - mCharacter.mPath.Last().transform.position.x,
            tileToAdd.transform.position.z - mCharacter.mPath.Last().transform.position.z);
        return direction.sqrMagnitude <= Tile.TILE_SIZE;
    }
    
    public void AddPath(Tile tileToAdd)
    {
        Debug.Log("AddPath call");
        //check if previous tile is same (remove of list)
        for (int i = 0; i < mCharacter.mPath.Count; i++)
        {
            if (tileToAdd == mCharacter.mPath[i])
            {
                if (tileToAdd == mFromTile)
                {
                    mCharacter.mPath.RemoveRange(1, mCharacter.mPath.Count - i);
                }
                else
                {
                    mCharacter.mPath.RemoveRange(i + 1, mCharacter.mPath.Count - i);
                }
                Debug.Log("Path exist and exite");
                return;
            }
        }
        
        //check if tile is nearst than another
        if (IsTileAdjacentToLastOnPath(tileToAdd))
        {
            //Is path done ?
            if (tileToAdd == mToTile)
            {
                SetGameState(EGameState.MOVE);
                Debug.Log("Done");
            }

            mCharacter.mPath.Add(tileToAdd);
            Debug.Log("Tile added");
        }
    }
}
