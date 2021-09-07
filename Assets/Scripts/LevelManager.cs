using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameState
{
    PATH_SELECTION,
    MOVE
};

public class LevelManager : MonoBehaviour
{
    protected EGameState mGameState;
    
    [Header("Objects")]
    public Camera mCam;
    public Character mCharacter;
    
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
        bool isClic = (!mUseMobileInput && Input.GetMouseButton(0)) || Input.touchCount == 1;        
        bool isClicDown = (!mUseMobileInput && Input.GetMouseButtonDown(0)) || Input.touchCount == 1;        
#elif UNITY_STANDALONE
        bool isClic = Input.GetMouseButton(0);
        bool isClicDown = Input.GetMouseButtonDown(0));        
#else
        bool isClic = Input.touchCount == 1;
#endif        

        if (isClic)
        {
            Ray ray = mCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "aisle")
                {

                }
                else if (hit.transform.tag == "escape")
                {

                }
                
                Debug.DrawLine(mCam.transform.position, hit.point, Color.green, 1f);
            }
        }
    }

    void SetGameState(EGameState newGS)
    {
        switch (newGS)
        {
            case EGameState.PATH_SELECTION:
                break;
            case EGameState.MOVE:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newGS), newGS, null);
        }

        mGameState = newGS;
    }
}
