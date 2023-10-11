using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    public static void SaveByJson(string saveFileName, object data)
    {
        var json = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            File.WriteAllText(path, json);
#if UNITY_EDITOR
            Debug.Log("Save successful, path: " + path);
#endif
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError("Save failed, path: " + path + ", exception: " + exception);
#endif
        }
    }

    public static T LoadFromJson<T>(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);
            return data;
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError("Load failed, path: " + path + ", exception: " + exception);
#endif
            return default;
        }
    }

    public static void DeleteSaveFile(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        try
        {
            File.Delete(path);
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError("Delete failed, path: " + path + ", exception: " + exception);
#endif
        }
    }
}
