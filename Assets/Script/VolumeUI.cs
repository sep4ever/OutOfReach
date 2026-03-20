using UnityEngine;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour
{
    [SerializeField] Slider slider;

    private void Start()
    {
        slider.value = GameManager.Instance.saveData.volume;
        slider.onValueChanged.AddListener(OnChanged);
    }

    void OnChanged(float value)
    {
        GameManager.Instance.saveData.volume = value;
        GameManager.Instance.SaveGame();
        GameManager.Instance.ApplyVolume();
    }
}