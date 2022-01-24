using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class SaveSystem
{
    #region Json solution

    public static void SaveFile(string fileName, object data)
    {
        var jsonString = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath, fileName);
        // Cross platform path solution
        // var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            File.WriteAllText(path, jsonString);

            Debug.Log($"Saved data successfully to {path}.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save data to {path}.\n{e}");
        }
    }

    public static T LoadFile<T>(string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            var jsonString = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(jsonString);

            Debug.Log($"Loaded data successfully from {path}.");

            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data from {path}.\n{e}");

            return default;
        }
    }

    public static void DeleteFile(string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            File.Delete(path);

            Debug.Log($"Deleted data in {path}.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to delete data in {path}.\n{e}");
        }
    }

    #endregion
}