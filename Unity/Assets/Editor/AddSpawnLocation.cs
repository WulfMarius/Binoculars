using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;

using ModComponentAPI;

public class AddLootTableEntry : MonoBehaviour
{
    private const string TARGET_DIRECTORY = "AssetBundles";

    [MenuItem("Component/Add SpawnLocation")]
    public static void Execute()
    {
        GameObject gameObject = Selection.activeObject as GameObject;
        if (gameObject == null)
        {
            return;
        }

        TextEditor editor = new TextEditor();
        editor.Paste();
        string json = editor.text;

        ModSpawnLocationComponent spawnLocation = gameObject.AddComponent<ModSpawnLocationComponent>();
        JsonUtility.FromJsonOverwrite(json, spawnLocation);
    }
}
