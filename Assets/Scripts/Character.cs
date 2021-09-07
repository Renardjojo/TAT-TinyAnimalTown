using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected List<Tile> mPath;
    public CharacterData mUserData;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void AddPath(Tile tileToAdd)
    {
        //TODO check if previous tile is same (remove of list)
        //TODO check if tile is nearst than another
        mPath.Add(tileToAdd);
    }
}
