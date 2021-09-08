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

    [Header("FX")]
        [SerializeField] protected AnimationCurve mOutlineAnimCurve;
        private float mTimeAcculatedOulineEffect;
        public float mOutlineWidth;
        [Range(0f, 1f)]
        public float mOutlineFXOffset = 0.1f;
        [Range(0f, 10f)]
        public float mOutlineFXDuration = 1f;
    
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
        Vector3 toPos = new Vector3(mCharacter.mPath.First().transform.position.x,
            mCharacter.mPath.First().transform.position.y + Tile.TILE_SIZE,
            mCharacter.mPath.First().transform.position.z);
        
        // Play jump animation
        PlayAnimation(mCharacter.mPath.First().transform.position.y - mCharacter.transform.position.y);
        
        do
        {
            t += Time.deltaTime;
            mCharacter.transform.position = Vector3.Lerp(fromPos, toPos, t);
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

    void UpdateSelectionPathControl()
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
    
    void UpdatePathSelectionLogic()
    {
        UpdateSelectionPathControl();
        AnimatePathOulineFX();
    }

    public void SetGameState(EGameState newGS)
    {
        switch (newGS)
        {
            case EGameState.PATH_SELECTION:
                Vector3 newPos = new Vector3(mFromTile.transform.position.x, mFromTile.transform.position.y + Tile.TILE_SIZE, mFromTile.transform.position.z);
                mCharacter = Instantiate(mCharacter, newPos, Quaternion.identity);
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
        //check if previous tile is same (remove of list)
        for (int i = 0; i < mCharacter.mPath.Count; i++)
        {
            if (tileToAdd == mCharacter.mPath[i])
            {
                if (tileToAdd == mFromTile)
                {
                    mCharacter.RemoveTile(1, mCharacter.mPath.Count - i);
                }
                else
                {
                    mCharacter.RemoveTile(i + 1, mCharacter.mPath.Count);
                }
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
            }

            mCharacter.AddTile(tileToAdd);
        }
    }

    public void ResetLevel()
    {
        
    }

    void AnimatePathOulineFX()
    {
        mTimeAcculatedOulineEffect += Time.deltaTime / mOutlineFXDuration;
        if (mTimeAcculatedOulineEffect > 1f)
        {
            mTimeAcculatedOulineEffect = 1f - mTimeAcculatedOulineEffect;
        }

        float add = 0f;
        for (int i = mCharacter.mPath.Count - 1; i >= 0; i--)
        {
            add += mOutlineFXOffset;
            float t = mTimeAcculatedOulineEffect + add;
            while (t > 1f)
            {
                t = t - 1f;
            }
            
            mCharacter.mPath[i].mOutlineScipt.OutlineWidth =
                mOutlineAnimCurve.Evaluate(t) * mOutlineWidth;
        }
    }
}
