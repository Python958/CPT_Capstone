using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class NewSaveAndLoad : MonoBehaviour
{
    public string dataPathPos;
    public string dataPathCheckPoints;

    void Start()
    {
        dataPathPos = Path.Combine(Application.persistentDataPath, "PlayerPos.txt");
        dataPathCheckPoints = Path.Combine(Application.persistentDataPath, "Checkpoints.txt");

    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.S)) { SavePlayerPosition(new Vector3(1f, 1f, 1f), dataPath); Debug.Log("Saved"); }

        if (Input.GetKeyDown(KeyCode.L))
        {
            var pos = LoadPlayerPosition(dataPath);
            Debug.Log(pos);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("delete");
            DeleteSave(dataPath);
        }
        */
    }

    public void SavePlayerPosition(Vector3 position, string path)
    {
        string jsonString = JsonUtility.ToJson(position);

        using (StreamWriter streamWriter = File.CreateText(path))
        {
            streamWriter.Write(jsonString);
        }
        Debug.Log("saved pos");
    }

    public void SaveCheckpoints(bool[] values)
    {
        var newVal = new CheckPointHolder(values[0], values[1], values[2]);
        string jsonString = JsonUtility.ToJson(newVal);

        using (StreamWriter streamWriter = File.CreateText(dataPathCheckPoints))
        {
            streamWriter.Write(jsonString);
        }
        Debug.Log("saved checkpoints");
    }

    public bool[] LoadCheckpoints()
    {
        Debug.Log("loaded checkpoints");
        using (StreamReader streamReader = File.OpenText(dataPathCheckPoints))
        {
            string jsonString = streamReader.ReadToEnd();
            var cPH = JsonUtility.FromJson<CheckPointHolder>(jsonString);
            return (new bool[3] { cPH.checkpoint1, cPH.checkpoint2, cPH.checkpoint3 });
        }
    }

    public Vector3 LoadPlayerPosition(string path)
    {
        Debug.Log("loaded pos");
        using (StreamReader streamReader = File.OpenText(path))
        {
            string jsonString = streamReader.ReadToEnd();
            return JsonUtility.FromJson<Vector3>(jsonString);
        }
    }

    public void DeleteSave(string path)
    {
        File.Delete(dataPathPos);
        File.Delete(dataPathCheckPoints);
    }

    public bool SaveExists(string path)
    {
        return(File.Exists(path));
    }
}

[Serializable]
public class CheckPointHolder
{
    public CheckPointHolder(bool val1, bool val2, bool val3)
    {
        checkpoint1 = val1;
        checkpoint2 = val2;
        checkpoint3 = val3;
    }

    public bool checkpoint1;
    public bool checkpoint2;
    public bool checkpoint3;
}