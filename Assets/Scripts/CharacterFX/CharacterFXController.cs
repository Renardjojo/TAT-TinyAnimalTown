using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFXController : MonoBehaviour
{
    [Header("Jump end")]
    [SerializeField] protected AudioSource mAudioJumpEnd;
    [SerializeField] protected ParticleSystem mParticleJumpEnd;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerJumpEnd()
    {
        mParticleJumpEnd?.Play();
        mAudioJumpEnd?.Play();
    }
}
