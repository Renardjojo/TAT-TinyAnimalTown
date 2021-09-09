using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public AudioSource mSoundStarWin1;
    public AudioSource mSoundStarWin2;
    public AudioSource mSoundStarWin3;
    public AudioSource mJingle1Star;
    public AudioSource mJingle2Star;
    public AudioSource mJingle3Star;
    public AudioSource mSetReadyGoSound;

    public void PlayerWinStar1Event()
    {
        mSoundStarWin1?.Play();
    }
    public void PlayerWinStar2Event()
    {
        mSoundStarWin2?.Play();
    }
    public void PlayerWinStar3Event()
    {
        mSoundStarWin3?.Play();
    }
    
    public void Player1StartEvent()
    {
        mJingle1Star?.Play();
    }
    
    public void Player2StarEvent()
    {
        mJingle2Star?.Play();
    }
    
    public void Player3StarEvent()
    {
        mJingle3Star?.Play();
    }
    
    public void PlaySetReadyGoEvent()
    {
        mSetReadyGoSound?.Play();
    }
}
