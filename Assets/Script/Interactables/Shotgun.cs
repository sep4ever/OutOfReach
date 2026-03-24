using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Shotgun : Interactable
{
    [SerializeField] List<string> thoughts;
    [SerializeField] List<string> nearDeathThoughts;
    QuestHandling questHandling;
    DialogueBox dialogue;

    [SerializeField] RawImage overlayImage;
    AudioSource shootingSound;
    Player player;

    private void Awake()
    {
        questHandling = FindAnyObjectByType<QuestHandling>();
        dialogue = FindAnyObjectByType<DialogueBox>();
        player = FindAnyObjectByType<Player>();
        shootingSound = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        if (questHandling.questId == 16)
        {
            if (!dialogue.isActive) dialogue.SetMessages(nearDeathThoughts);
            if (dialogue.finishedDialogue && fadingCoroutine == null) fadingCoroutine = StartCoroutine(FadeToBlack());
        }
        else if (!dialogue.isActive) dialogue.SetMessages(thoughts);
    }


    float duration = 1.5f;
    float pause = 3f;
    Coroutine fadingCoroutine = null;
    IEnumerator FadeToBlack()
    {
        float t = 0f;
        player.canMove = false;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = t / duration;
            overlayImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        yield return new WaitForSeconds(shootingSound.clip.length);
        shootingSound.Play();
        yield return new WaitForSeconds(pause);

        player.canMove = true;
        fadingCoroutine = null;
        SceneManager.LoadScene("End");
    }
}
