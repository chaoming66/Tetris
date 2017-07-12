using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class DataEditor : EditorWindow
{
    public PieceDataReader pieceData;
    private string m_filePath = null;

    [MenuItem("Tools/Piece Data Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(DataEditor)).Show(true);
    }

    private void OnEnable()
    {
        m_filePath = Path.Combine(Application.dataPath, Path.Combine("Data", "Pieces.json"));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Load data"))
        {
            LoadGameData();
        }

        if (pieceData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("pieceData");
            EditorGUILayout.PropertyField(serializedProperty, true);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save data"))
            {
                SaveGameData();
            }
        }
    }

    private void SaveGameData()
    {

        string dataAsJson = JsonUtility.ToJson(pieceData);
        File.WriteAllText(m_filePath, dataAsJson);

    }

    private void LoadGameData()
    {
        if (File.Exists(m_filePath))
        {
            string jsonString = File.ReadAllText(m_filePath);
            pieceData = JsonUtility.FromJson<PieceDataReader>(jsonString);
        }
        else
        {
            pieceData = new PieceDataReader();
        }
    }
}
