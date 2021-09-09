using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelData
{
    public Tile from;
    public Tile to;
    public Character character;
    public float bestTime;
}
public class GameManager : MonoBehaviour
{
    [Header("Objects")]
    public List<LevelData> mLevelList;
    protected int mCurrentLevel = -1;
    
    [SerializeField]
    public LevelManager mLevelManager;

    // Start is called before the first frame update
    void Start()
    {
        mLevelManager.SetGameState(EGameState.MENU);
    }

    public void NextLevel()
    {
        mCurrentLevel = ++mCurrentLevel % mLevelList.Count;
        
        if (mLevelManager.mCharacter)
            mLevelManager.mCharacter.gameObject.SetActive(false);
        
        mLevelManager.mCharacter = mLevelList[mCurrentLevel].character;
        mLevelManager.mCharacter.gameObject.SetActive(true);
        
        mLevelManager.mFromTile = mLevelList[mCurrentLevel].from;
        mLevelManager.mToTile = mLevelList[mCurrentLevel].to;
        mLevelManager.mBestTime = mLevelList[mCurrentLevel].bestTime;
        
        mLevelManager.SetGameState(EGameState.PATH_SELECTION);
    }

    public void ResetGame()
    {
        mCurrentLevel = -1;
        mLevelManager.ResetLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
