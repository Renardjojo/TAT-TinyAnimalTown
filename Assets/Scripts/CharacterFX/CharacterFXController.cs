using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFXController : MonoBehaviour
{
    public Character mCharacter;
    
    [Header("Jump end")]
    [SerializeField] protected AudioSource mAudioJumpEnd;
    [SerializeField] protected ParticleSystem mParticleJumpEnd;
    
    [Header("Jump Start")]
    public AudioSource mJumpStartSound;
    public float mMinPitchjumpStart = -3f;
    public float mMaxPitchjumpStart = 3f;
    [Range(0f, 1f)]
    public float mAnimalSoundApparitionChanceOnJump = 0.33f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerJumpStart()
    {
        mJumpStartSound.pitch = Random.Range(mMinPitchjumpStart, mMaxPitchjumpStart);
        mJumpStartSound?.Play();

        if (Random.value < mAnimalSoundApparitionChanceOnJump)
        {
            mCharacter?.PlayRandomAnimalSound();
        }
    }
    
    public void PlayerJumpEnd()
    {
        mParticleJumpEnd?.Play();
        mAudioJumpEnd?.Play();
    }
}
