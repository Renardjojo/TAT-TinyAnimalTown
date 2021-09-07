using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Character : MonoBehaviour
{
    public List<Tile> mPath;
    public CharacterData mUserData;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var path in mPath)
        {
            EditorGUIUtility.PingObject(path);
            Debug.Log("Pinged " + path.name);
        }
    }
}
