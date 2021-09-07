using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TileEffectOnCharacter
{
    public ETileType mTileType;
    public ETileEffect mEffect;
}

[System.Serializable]
[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/Character", order = 1)]
public class CharacterData : ScriptableObject
{
    [Header("Identity")] public string mName;
    public GameObject mMeshPrefab;
    
    [SerializeField]
    public List<TileEffectOnCharacter> mTilesEffectOnCharacter = new List<TileEffectOnCharacter>();
}