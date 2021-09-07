using UnityEngine;

public struct TileEffectOnCharacter
{
    public bool mBlock;
    public bool mSlow;
    public bool mFast;
}

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/Character", order = 1)]
public class CharacterData : ScriptableObject
{
    [Header("Identity")]
    public string Name;
    public GameObject MeshPrefab;
    public bool test;

    // [ContextMenu("SetIdealValues")]
    // public void FileNameToNameField()
    // {
    //     Name = Path.GetFileNameWithoutExtension( AssetDatabase.GetAssetPath(this));
    // }
}