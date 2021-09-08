using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public AudioSource mSoundStarWin;
    public AudioSource mJingle1Star;
    public AudioSource mJingle2Star;
    public AudioSource mJingle3Star;
    public AudioSource mSetReadyGoSound;

    public void PlayerWinStarEvent()
    {
        mSoundStarWin?.Play();
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
