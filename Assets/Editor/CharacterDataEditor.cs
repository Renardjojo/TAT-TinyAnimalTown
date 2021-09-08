using System.IO;
using UnityEngine;
using UnityEditor;

// Declare type of Custom Editor
[CustomEditor(typeof(CharacterData))] //1
public class CharacterDataEditor : Editor 
{
    // Custom form for Player Preferences
    private CharacterData Target;
    private SerializedProperty mListProperty;
    
    private void OnEnable()
    {
        Target = (CharacterData) target;
        mListProperty = serializedObject.FindProperty("mTilesEffectOnCharacter");
    }
    
    Editor gameObjectEditor;
    Texture2D previewBackgroundTexture;
    
    // OnInspector GUI
    public override void OnInspectorGUI()
    {
        Header("Identity");

        EditorGUILayout.BeginHorizontal();
        {
            GUIStyle bgColor = new GUIStyle();
            bgColor.normal.background = previewBackgroundTexture;

            //display the mesh
            if (Target.mMeshPrefab != null)
            {
                if (gameObjectEditor == null)
                    gameObjectEditor = Editor.CreateEditor(Target.mMeshPrefab);

                gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(100, 100), bgColor);
            }
        }
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            Target.mName = EditorGUILayout.TextField(new GUIContent("Name"), Target.mName);
            if (GUILayout.Button("Use file name"))
            {
                Target.mName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(Target));
            }
            EditorGUILayout.EndHorizontal();
        }
        
        {
            EditorGUI.BeginChangeCheck();
            Target.mMeshPrefab = (GameObject)EditorGUILayout.ObjectField("Mesh" ,Target.mMeshPrefab, typeof(GameObject), true);

            if (EditorGUI.EndChangeCheck())
            {
                gameObjectEditor = Editor.CreateEditor(Target.mMeshPrefab);
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        {
            EditorGUILayout.PropertyField(mListProperty, true);
        }

        {
            if (GUILayout.Button("Reset"))
            {
                Target.mTilesEffectOnCharacter = new TileEffectOnCharacter[(int)ETileType.COUNT];
        
                for (int i = 0; i < (int)ETileType.COUNT; i++)
                {
                    Target.mTilesEffectOnCharacter[i].mTileType = (ETileType)i;
                    Target.mTilesEffectOnCharacter[i].mTimeEffect = 120f;
                }
            }
            
            if (GUILayout.Button("Save"))
            {
                EditorUtility.SetDirty(Target);
            }
        }
    }

    Color GetColorStepOfRangedValue(float value, float max)
    {
        float ratio = value / max;
        if (ratio < 0.25)
            return Color.white;
        
        if (ratio < 0.5)
            return Color.green;
        
        if (ratio < 0.75)
            return Color.yellow;
        
        return Color.red;
    }

    void Header(string name)
    {
        EditorGUILayout.Separator();
        GUILayout.Label(name, EditorStyles.boldLabel);
        EditorGUILayout.Separator();
    }
}
