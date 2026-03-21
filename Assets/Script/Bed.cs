using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class Bed : Interactable
{
    [SerializeField] RawImage bedImage;
    [SerializeField] AudioSource music;
    AudioSource breathing;
    float duration = 1.5f;

    void Awake()
    {
        bedImage.color = new Color(0, 0, 0, 0);
        breathing = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = t / duration;
            bedImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        breathing.Play();
        while (music.volume > 0.01)
        {
            music.volume = Mathf.Lerp(music.volume, 0, Time.deltaTime);
            yield return null;
        }

        music.volume = 0;
        music.Pause();
        yield return new WaitForSeconds(breathing.clip.length); // можно поделить на 2, или 2.5, поскольку клип длинный очень.

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = 1 - t / duration;
            bedImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        bedImage.color = new Color(0, 0, 0, 0);

        music.Play();
        while (music.volume < GameManager.Instance.saveData.volume - 0.01f)
        {
            music.volume = Mathf.Lerp(music.volume, GameManager.Instance.saveData.volume, Time.deltaTime);
            yield return null;
        }
        music.volume = GameManager.Instance.saveData.volume;
    }
}
