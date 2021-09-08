using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFXController : MonoBehaviour
{
    [Header("Jump end")]
    [SerializeField] protected AudioSource mAudioJumpEnd;
    [SerializeField] protected ParticleSystem mParticleJumpEnd;
    
    [Header("Jump Start")]
    public AudioSource mJumpStartSound;
    public AudioSource mAnimalSound;
    [Range(0f, 1f)]
    public float mAnimalSoundApparitionChanceOnJump;
    
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
        mJumpStartSound.pitch = Random.Range(-3f, 3f);
        mJumpStartSound?.Play();

        if (Random.value < mAnimalSoundApparitionChanceOnJump)
        {
            mAnimalSound?.Play();
        }
    }
    
    public void PlayerJumpEnd()
    {
        mParticleJumpEnd?.Play();
        mAudioJumpEnd?.Play();
    }
}
