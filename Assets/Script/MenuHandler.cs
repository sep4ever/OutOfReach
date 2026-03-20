using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] string scene;
    GameObject infoObject;
    GameObject settingsParent;
    public void StartGame()
    {
        SceneManager.LoadScene(scene);
    }

    private void Start()
    {
        infoObject = GameObject.Find("Info");
        settingsParent = GameObject.Find("Settings");
        infoObject.SetActive(false);
        settingsParent.SetActive(false);
    }

    public void ShowInfo()
    {
        infoObject.SetActive(!infoObject.activeSelf);
        settingsParent.SetActive(false);
    }

    public void ShowSettings()
    {
        settingsParent.SetActive(!settingsParent.activeSelf);
        infoObject.SetActive(false);
    }
}
