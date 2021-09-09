using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class Character : MonoBehaviour
{
    public List<Tile> mPath;
    public CharacterData mUserData;
    public Animator mAnimator;
    public AudioSource mAnimalSound;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddTile(Tile tileToAdd)
    {
        mPath.Add(tileToAdd);
        tileToAdd.Select();
    }

    public void RemoveTile(int from, int to)
    {
        for (int i = from; i < to; i++)
        {
            RemoveTile(from);
        }
    }
    
    public void RemoveTile(int index)
    {
        mPath[index].UnSelect();
        mPath.RemoveAt(index);
    }

    public void ClearPath()
    {
        RemoveTile(0, mPath.Count);
    }

    public void PlayRandomAnimalSound()
    {
        mAnimalSound.clip = mUserData.mSounds[UnityEngine.Random.Range(0, mUserData.mSounds.Length)];
        mAnimalSound.Play();
    }
}
