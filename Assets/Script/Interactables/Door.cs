using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Door : Interactable
{
    [SerializeField] RawImage bedImage;
    Player player;
    QuestHandling questHandling;
    AudioSource wieldSound;
    DialogueBox dialogueBox;

    [SerializeField][TextArea(10,10)] List<string> completedThoughts;
    [SerializeField][TextArea(10,10)] List<string> uncompletedThoughts;

    public override void Interact()
    {
        if (fadingCoroutine != null) return;
        if (questHandling.questId == 6) fadingCoroutine = StartCoroutine(FadeToBlack());
        List<string> thoughts = questHandling.quests[6].isCompleted ? completedThoughts : uncompletedThoughts;
        if (!dialogueBox.isActive && fadingCoroutine == null) dialogueBox.SetMessages(thoughts);
    }

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
        questHandling = FindAnyObjectByType<QuestHandling>();
        wieldSound = GetComponent<AudioSource>();
        dialogueBox = FindAnyObjectByType<DialogueBox>();
    }

    float duration = 1f;
    Coroutine fadingCoroutine = null;
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
        wieldSound.volume = 0f;
        wieldSound.Play();
        while (wieldSound.volume < .99f)
        {
            wieldSound.volume = Mathf.MoveTowards(wieldSound.volume, 1, Time.deltaTime * 0.5f);
            yield return null;
        }
        yield return new WaitForSeconds(duration * 2); 

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = 1 - t / duration;
            bedImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        bedImage.color = new Color(0, 0, 0, 0);
        while (wieldSound.volume > 0.01f)
        {
            wieldSound.volume = Mathf.MoveTowards(wieldSound.volume, 0, Time.deltaTime);
            yield return null;
        }
        wieldSound.Stop();
        player.canMove = true;
        fadingCoroutine = null;
    }
}
