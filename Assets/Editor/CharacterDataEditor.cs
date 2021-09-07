using System.IO;
using UnityEngine;
using UnityEditor;

// Declare type of Custom Editor
[CustomEditor(typeof(CharacterData))] //1
public class CharacterDataEditor : Editor 
{
    // Custom form for Player Preferences
    private CharacterData Target;
    
    private void OnEnable()
    {
        Target = (CharacterData) target;
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
            if (Target.MeshPrefab != null)
            {
                if (gameObjectEditor == null)
                    gameObjectEditor = Editor.CreateEditor(Target.MeshPrefab);

                gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(100, 100), bgColor);
            }
        }
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            Target.Name = EditorGUILayout.TextField(new GUIContent("Name"), Target.Name);
            if (GUILayout.Button("Use file name"))
            {
                Target.Name = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(Target));
            }
            EditorGUILayout.EndHorizontal();
        }
        
        {
            EditorGUI.BeginChangeCheck();
            Target.MeshPrefab = (GameObject)EditorGUILayout.ObjectField("Mesh" ,Target.MeshPrefab, typeof(GameObject), true);

            if (EditorGUI.EndChangeCheck())
            {
                gameObjectEditor = Editor.CreateEditor(Target.MeshPrefab);
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        
        {
            EditorGUILayout.BeginHorizontal();
            Target.test = EditorGUILayout.Toggle(new GUIContent("WaterNeed", "mL"), Target.test);
            Target.test = EditorGUILayout.Toggle(new GUIContent("WaterNeed2", "mL"), Target.test);
            Target.test = EditorGUILayout.Toggle(new GUIContent("WaterNeed3", "mL"), Target.test);
            EditorGUILayout.EndHorizontal();
        }

        {
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
