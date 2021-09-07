using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TileEffectOnCharacter
{
    public ETileType mTileType;
    public float mTimeEffect;
}

[System.Serializable]
[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/Character", order = 1)]
public class CharacterData : ScriptableObject
{
    [Header("Identity")] public string mName;
    public GameObject mMeshPrefab;

    [SerializeField] public TileEffectOnCharacter[] mTilesEffectOnCharacter;

    void Start()
    {
        mTilesEffectOnCharacter = new TileEffectOnCharacter[(int)ETileType.COUNT];
        
        for (int i = 0; i < (int)ETileType.COUNT; i++)
        {
            mTilesEffectOnCharacter[i].mTileType = (ETileType)i;
            mTilesEffectOnCharacter[i].mTileType = 0f;
        }
    }
}