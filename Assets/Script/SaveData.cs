using UnityEngine;
using System.IO;

[System.Serializable]
public class SavingData
{
    public float volume = 1f;
}
public class SaveData : MonoBehaviour
{
    public static void Save(SavingData data)
    {
        string json = JsonUtility.ToJson(data, true);
        string path = Application.persistentDataPath + "/save.json";
        File.WriteAllText(path, json);
    }

    public static SavingData Load()
    {
        string path = Application.persistentDataPath + "/save.json";

        if (!File.Exists(path))
            return new SavingData();

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SavingData>(json);
    }
}
