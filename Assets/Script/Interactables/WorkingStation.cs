using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class WorkingStation : Interactable
{
    [SerializeField] RawImage workingStationImage;
    [SerializeField] DialogueBox dialogueBox;
    [SerializeField] List<string> workingDialogues;
    QuestHandling questHandling;
    AudioSource crushSound;
    Player player;
    float duration = 2f;

    private void Awake()
    {
        questHandling = FindAnyObjectByType<QuestHandling>();
        player = FindAnyObjectByType<Player>();
        crushSound = GetComponent<AudioSource>();
        //crushSound.Play();
    }

    Coroutine fadingCoroutine = null;
    public override void Interact()
    {
        if (!canInteract) return;
        if (player.hasAxe)
        {
            if (fadingCoroutine == null) fadingCoroutine = StartCoroutine(FadeToBlack());
            return;
            //gameObject.SetActive(false);
        }
        //workingDialogues.Clear();
        switch (questHandling.questId)
        {
            case 1:
                workingDialogues = new List<string> { "Чёртовы отчёты... Зачем я устроился на \"прибыльную\" работу?" };
                break;
            case 5:
                workingDialogues = new List<string> { "Работа, работа и опять работа... Господи. Неужели я всё ещё должен заполнять документы?", "Неужто ещё не знают, что мы все умрём...", "Это всё бессмысленно." };
                break;

            case 9:
                workingDialogues = new List<string> { "Работа, сон, работа, сон." };
                break;

            default:
                if (!dialogueBox.finishedDialogue && fadingCoroutine != null || dialogueBox.isActive) { return; }
                workingDialogues = new List<string> { "Не время для работы." };
                dialogueBox.SetMessages(workingDialogues);
                fadingCoroutine = null;
                return;
        }
        if (fadingCoroutine == null)
        {
            fadingCoroutine = StartCoroutine(FadeToBlack());
        }
    }

    IEnumerator FadeToBlack()
    {

        float t = 0f;
        player.canMove = false;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = t / duration;
            workingStationImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        if (player.hasAxe)
        {
            for (int i = 0; i < 4; i++)
            {
                crushSound.pitch = Random.Range(0.5f, 1.2f);
                crushSound.Play();
                yield return new WaitForSeconds(crushSound.clip.length);
            }
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            canInteract = false;
        }

        yield return new WaitForSeconds(duration * 2);

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = 1 - t / duration;
            workingStationImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        workingStationImage.color = new Color(0, 0, 0, 0);
        player.canMove = true;

        fadingCoroutine = null;
        if (!player.hasAxe && !dialogueBox.isActive) dialogueBox.SetMessages(workingDialogues);
    }
}
