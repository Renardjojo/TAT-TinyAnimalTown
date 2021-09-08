using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Objects")]
    public List<Character> mListCharacters;
    protected int mCurrentCharacter = -1;
    
    [SerializeField]
    public LevelManager mLevelManager;

    // Start is called before the first frame update
    void Start()
    {
        NextLevel();
    }

    void NextLevel()
    {
        mCurrentCharacter = ++mCurrentCharacter % mListCharacters.Count;

        mLevelManager.mCharacter = mListCharacters[mCurrentCharacter];
        mLevelManager.SetGameState(EGameState.PATH_SELECTION);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
