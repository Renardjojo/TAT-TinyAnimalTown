using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Objects")]
    public List<Character> mListCharacters;
    protected int mCurrentCharacter = 0;
    
    [SerializeField]
    public LevelManager mLevelManager;

    // Start is called before the first frame update
    void Start()
    {
        InitLevelManager();
    }

    void InitLevelManager()
    {
        mLevelManager.mCharacter = mListCharacters[mCurrentCharacter];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
