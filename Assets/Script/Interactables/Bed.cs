using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class Bed : Interactable
{
    [SerializeField] RawImage bedImage;
    [SerializeField] AudioSource music;
    AudioSource breathing;
    float duration = 1.5f;

    Player player;
    QuestHandling questHandling;
    DialogueBox dialogueBox;
    void Awake()
    {
        questHandling = FindAnyObjectByType<QuestHandling>();
        dialogueBox = FindAnyObjectByType<DialogueBox>();
        player = FindAnyObjectByType<Player>();
        bedImage.color = new Color(0, 0, 0, 0);
        breathing = GetComponent<AudioSource>();
    }

    Coroutine fadingCoroutine = null;
    bool sleep;
    public override void Interact()
    {
        if (sleep)
        {
            if (fadingCoroutine == null) fadingCoroutine = StartCoroutine(FadeToBlack());
        }
        else
        {
            if (!dialogueBox.isActive) dialogueBox.SetMessages(new List<string> { "Я пока не хочу спать." });
        }
    }

    void Update()
    {
        sleep = questHandling.questId == 3 || questHandling.questId == 7 || questHandling.questId == 11;
    }

    IEnumerator FadeToBlack()
    {
        float t = 0f;
        player.canMove = false;

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
        yield return new WaitForSeconds(breathing.clip.length / 2.5f); // можно поделить на 2, или 2.5, поскольку клип длинный очень.

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
        player.canMove = true;
        fadingCoroutine = null;
        if (!dialogueBox.isActive)
        {
            if (questHandling.quests[3].isCompleted) dialogueBox.SetMessages(new List<string> { "О-опять... Она опять мне снилась...", "Я переживаю... Вдруг что-то случилось?", "Господи, это единственное, что сдерживает меня от петли..." });
            if (questHandling.quests[7].isCompleted) dialogueBox.SetMessages(new List<string> { "Я... Я не буду больше спать... Мне болезненно видеть её лицо во снах...", "Вернее... Обрывки лица, владельца которого я забываю...", "Скоро я забуду и самого себя..." });
        }
    }
}
