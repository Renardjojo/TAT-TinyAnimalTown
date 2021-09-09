using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EGameState
{
    MENU,
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
        public CameraController mCamController;
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

        public GameObject mPrefabScorePopupText;
        public GameObject mFlag;
        
    [Header("UI")]
        public Animator mUIAnimator;
        [SerializeField] protected TextController mUIChono;
        [SerializeField] protected TextController mUIChonoBestTime;
    
    [Header("Sound/Music")]
        public AudioSource mLevelATM;
        public AudioSource mGameMusique;
        public AudioSource mTileRefusedSound;
        public AudioSource mBonusSound;
        public AudioSource mMalusSound;
        public AudioSource mLastTileSound;
        public AudioSource[] mTouchTileDefaultSound;

    // Start is called before the first frame update
    void Start()
    {
        mGameMusique?.Play();
    }

    // Update is called once per frame
    void Update()
    {
        switch (mGameState)
        {
            case EGameState.MENU:
                break;
            case EGameState.PATH_SELECTION:
                UpdatePathSelectionLogic();
                AnimatePathOulineFX();
                break;
            case EGameState.MOVE:
                AnimatePathOulineFX();
                break;
            case EGameState.SCORE:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    Vector2 Convert3DPosTo2DPosXZ(Vector3 pos)
    {
        return new Vector2(pos.x, pos.z);
    }
    
    //Return true if rotation append
    bool CheckAndTurnCharacter(Tile TileToGo)
    {
        Vector2 toPos = Convert3DPosTo2DPosXZ(TileToGo.transform.position);
        Vector2 fromPos = Convert3DPosTo2DPosXZ(mCharacter.transform.position);
        Vector2 dirCharToGoal = (toPos - fromPos).normalized;
        float dot = Vector2.Dot(Convert3DPosTo2DPosXZ(mCharacter.transform.forward), dirCharToGoal);

        //Need to turn
        if (Mathf.Abs(dot) < 0.5)
        {
            //Turn left
            if (dot > 0f)
            {
                mCharacter.transform.forward = new Vector3(dirCharToGoal.x, mCharacter.transform.forward.y, dirCharToGoal.y);
            }
            else //Turn right
            {
                mCharacter.transform.forward = new Vector3(-dirCharToGoal.x, mCharacter.transform.forward.y, -dirCharToGoal.y);
            }
            return true;
        }
        return false;
    }
    
    void CheckAndPlayEffectSound(float value)
    {
        if (value < 120f)
        {
            mBonusSound?.Play();
        }
        else if (value > 120f)
        {
            mMalusSound?.Play();
        }
    }
    
    void AddTimeToCurrent(float timeToAdd)
    {
        if (timeToAdd == 0f)
            return;
        
        TextMeshPro text = Instantiate(mPrefabScorePopupText, mCharacter.transform.position + Vector3.up * Tile.TILE_SIZE, Camera.main.transform.rotation).GetComponentInChildren<TextMeshPro>();
        
        var ts = TimeSpan.FromSeconds(timeToAdd);
        text.text = string.Format("+{0:00}m{1:00}", ts.TotalMinutes, ts.Seconds);
        const float timeMax = 480f;
        const float timeMin = 30f;
        text.color = Color.Lerp(Color.green, Color.red, timeToAdd / (timeMax - timeMin));
        
        CheckAndPlayEffectSound(timeToAdd);
        mCurrentTime += timeToAdd;
        mUIChono.SetValueAsTime(mCurrentTime);
    }
    
    void ResetCurrentTime()
    {
        mCurrentTime = 0f;
        mUIChono.SetValueAsTime(mCurrentTime);
    }

    void ApplyTileEffect(bool isTurning, bool isGoDown, bool isGoUp)
    {
        //Apply altitude effect
        float timeToAdd = 0f;
        if (isGoUp)
        {
            timeToAdd += mCharacter.mUserData.mTilesEffectOnCharacter[(int)ETileType.UP].mTimeEffect;
        }
        else if (isGoDown)
        {
            timeToAdd += mCharacter.mUserData.mTilesEffectOnCharacter[(int)ETileType.DOWN].mTimeEffect;
        }
        
        //Apply turn effect
        if (isTurning)
        {
            timeToAdd += mCharacter.mUserData.mTilesEffectOnCharacter[(int)ETileType.TURN].mTimeEffect;
        }
        else
        {
            timeToAdd += mCharacter.mUserData.mTilesEffectOnCharacter[(int)ETileType.LINE].mTimeEffect;
        }
        
        // Apply tile effect
        timeToAdd += mCharacter.mUserData.mTilesEffectOnCharacter[(int) mCharacter.mPath.First().tileType].mTimeEffect;
        AddTimeToCurrent(timeToAdd);
    }

    protected IEnumerator MoveCoroutine()
    {
        bool isTurning = CheckAndTurnCharacter(mCharacter.mPath.First());
        bool isGoDown = false;
        bool isGoUp = false;
        
        float t = 0f;
        Vector3 fromPos = mCharacter.transform.position;
        Vector3 toPos = new Vector3(mCharacter.mPath.First().transform.position.x,
            mCharacter.mPath.First().transform.position.y + Tile.TILE_SIZE,
            mCharacter.mPath.First().transform.position.z);
        
        // Play jump animation
        float deltaY = mCharacter.mPath.First().transform.position.y - mCharacter.transform.position.y + Tile.TILE_SIZE;
        if (deltaY < 0f)
        {
            isGoDown = true;
            mCharacter.mAnimator.SetTrigger("JumpLow");
        }
        else if (deltaY > 0f)
        {
            isGoUp = true;
            mCharacter.mAnimator.SetTrigger("JumpHigh");
        }
        else
        {
            mCharacter.mAnimator.SetTrigger("JumpSame");
        }
        
        do
        {
            t += Time.deltaTime;
            mCharacter.transform.position = Vector3.Lerp(fromPos, toPos, t);
            yield return null;
        } while (t < 1f);
        
        // Apply time effect
        ApplyTileEffect(isTurning, isGoDown, isGoUp);
        mCharacter.RemoveTile(0);

        if (mCharacter.mPath.Count != 0)
        {
            StartCoroutine(MoveCoroutine());
        }
        else
        {
            if (mLastTileSound)
                mLastTileSound.Play();
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
                if (!AddPath(hit.transform.GetComponent<Tile>()))
                    mTileRefusedSound?.Play();
                
                Debug.DrawLine(mCam.transform.position, hit.point, Color.red, 1f);
            }
        }
    }
    
    void UpdatePathSelectionLogic()
    {
        UpdateSelectionPathControl();
    }

    public void SetGameState(EGameState newGS)
    {
        switch (newGS)
        {
            case EGameState.MENU:
                break;
            case EGameState.PATH_SELECTION:
                InitLevel();
                mLevelATM?.Play();
                break;
            case EGameState.MOVE:
                //Remove the first path that is the start position
                mCharacter.mPath.RemoveAt(0);
                mMoveCoroutine = StartCoroutine(MoveCoroutine());
                
                mCamController.mTarget = mCharacter.transform;
                mCamController.SetInTargetMode();
                break;
            case EGameState.SCORE:
                float scoreRatio = mCurrentTime / mBestTime;
                if (scoreRatio < 1.2)
                {
                    mUIAnimator.SetTrigger("showWin3Star");
                }
                else if (scoreRatio < 1.5)
                {
                    mUIAnimator.SetTrigger("showWin2Star");
                }
                else
                {
                    mUIAnimator.SetTrigger("showWin1Star");
                }
                
                mLevelATM.Stop();
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
        return direction.sqrMagnitude <= Tile.TILE_SIZE * 1.5f;
    }
    
    public bool AddPath(Tile tileToAdd)
    {        
        // Check if tile can be added (is Block ?)
        if (mCharacter.mUserData.mTilesEffectOnCharacter[(int) tileToAdd.tileType].mTimeEffect == 0f && mCharacter.mUserData.mTilesEffectOnCharacter[(int) tileToAdd.tileType].mTileType != ETileType.STEP_ROAD)
            return false;

        // Check if previous tile is same (remove of list)
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
                return false;
            }
        }

        // Check if tile is nearest than another
        if (!IsTileAdjacentToLastOnPath(tileToAdd))
            return false;
        
        //Check if tile is up or down and if previous is step
        {
            // Play jump animation
            float deltaY = mCharacter.mPath.Last().transform.position.y - tileToAdd.transform.position.y;
            if (deltaY > Tile.TILE_SIZE / 2f)
            {
                if (!(tileToAdd.tileType == ETileType.STAIR || tileToAdd.tileType == ETileType.STEP_ROAD))
                    return false;
            }
            else if (deltaY < -Tile.TILE_SIZE / 2f)
            {
                if (!(mCharacter.mPath.Last().tileType == ETileType.STAIR || mCharacter.mPath.Last().tileType == ETileType.STEP_ROAD))
                    return false; 
            }
        }
        
        //Is path done ?
        if (tileToAdd == mToTile)
        {
            mLastTileSound?.Play();
            SetGameState(EGameState.MOVE);
        }

        if (tileToAdd.mSound)
        {
            tileToAdd.mSound.Play();
        }
        else
        {
            mTouchTileDefaultSound[Random.Range(0, mTouchTileDefaultSound.Length)]?.Play();
        }
            
        mCharacter.AddTile(tileToAdd);
        
        return true;
    }

    void InitLevel()
    {
        Vector3 newPos = new Vector3(mFromTile.transform.position.x, mFromTile.transform.position.y + Tile.TILE_SIZE, mFromTile.transform.position.z);
        mCharacter.transform.position = newPos;
        mCharacter.ClearPath();
        mCharacter.mPath.Add(mFromTile);
        ResetCurrentTime();
        mCamController.SetInFreeMode();

        mFlag.transform.position = mToTile.transform.position + Vector3.up * Tile.TILE_SIZE * 0.5f;
        mUIChonoBestTime.SetValueAsTime(mBestTime);
    }

    public void ResetLevel()
    {
        SetGameState(EGameState.PATH_SELECTION);
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
