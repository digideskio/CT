using System.Collections;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

//this utility class was downloaded from http://www.jacobpennock.com/Blog/?page_id=715
//it allows user to create custom assets that can be created from the Unity editor
//any changes you make to them will remain persistent throughout edit time and runtime
//making them perfect for storing dialogue data for an entire game.

public static class CustomAssetUtility
{
    public static void CreateAsset<T> () where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T> ();
        
        string path = AssetDatabase.GetAssetPath (Selection.activeObject);
        if (path == "") 
        {
            path = "Assets";
        } 
        else if (Path.GetExtension (path) != "") 
        {
            path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
        }
        
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + typeof(T).ToString() + ".asset");
        
        AssetDatabase.CreateAsset (asset, assetPathAndName);
        
        AssetDatabase.SaveAssets ();
        EditorUtility.FocusProjectWindow ();
        Selection.activeObject = asset;
    }
}
#endif