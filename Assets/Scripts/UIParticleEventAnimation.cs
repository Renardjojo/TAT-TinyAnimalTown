using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParticleEventAnimation : MonoBehaviour
{

    [SerializeField] private List<ParticleSystem> particles;

    public void PlayParticle(int indexList)
    {
        particles[indexList].Play();
    }

    public void StopParticle(int indexList)
    {
        particles[indexList].Stop();
    }
}
