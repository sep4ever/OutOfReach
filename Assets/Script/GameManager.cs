using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public SavingData saveData = new SavingData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        SaveData.Save(saveData);
    }

    public void LoadGame()
    {
        saveData = SaveData.Load();

        if (saveData == null) saveData = new SavingData();
        ApplyVolume();
    }

    public void ApplyVolume()
    {
        AudioListener.volume = saveData.volume;
    }
}