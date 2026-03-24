using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class Documents : Interactable
{
    [SerializeField][TextArea(30, 50)] List<string> documentLines;
    [SerializeField] TMP_Text documentText;
    [SerializeField] GameObject document;

    [SerializeField] RawImage overlayImage;

    int documentPage;
    [SerializeField] bool interacting = false;

    [SerializeField] Transform playerTransform;
    [SerializeField] float interactionRange;
    [SerializeField] float range;

    QuestHandling questHandling;
    AudioSource tearingPaperSound;

    public Player player;

    private void Awake()
    {
        questHandling = FindAnyObjectByType<QuestHandling>();
        player = playerTransform.GetComponent<Player>();
        tearingPaperSound = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        if (questHandling.questId != 11) OpenDocuments();
        else if (fadingCoroutine == null) fadingCoroutine = StartCoroutine(FadeToBlack());
    }

    private void Update()
    {
        range = Vector3.Distance(transform.position, playerTransform.position);
        document.SetActive(interacting);
        if (Input.GetKeyDown(KeyCode.Return) || range > interactionRange) interacting = false;
    }

    void OpenDocuments()
    {
        interacting = true;
        documentText.text = documentLines[documentPage];
        if (documentPage < documentLines.Count - 1) documentPage++;
        else documentPage = 0;
    }

    float duration = 1f;
    Coroutine fadingCoroutine;
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
        tearingPaperSound.Play();
        yield return new WaitForSeconds(tearingPaperSound.clip.length);

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = 1 - t / duration;
            overlayImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        overlayImage.color = new Color(0, 0, 0, 0);

        player.canMove = true;
        fadingCoroutine = null;
    }
}
