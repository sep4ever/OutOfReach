using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] public List<string> dialogueLines = new List<string>();
    [SerializeField] TMP_Text text;
    [SerializeField] float typingSpeed = 0.05f;

    public int iterator = 0;
    public bool finished = false;

    [SerializeField] bool firstStart = false;

    void Update()
    {
        if (dialogueLines == null || iterator >= dialogueLines.Count)
            return;

        if (firstStart)
        {
            firstStart = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!finished)
            {
                text.maxVisibleCharacters = dialogueLines[iterator].Length;
                return;
            }
            if (iterator < dialogueLines.Count - 1)
            {
                iterator++;
                Type();
            }
        }
    }

    Coroutine typingCoroutine;
    public void Type()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        Clear();
        typingCoroutine = StartCoroutine(TypingCoroutine());
    }

    IEnumerator TypingCoroutine()
    {
        string currentLine = dialogueLines[iterator];
        while (text.maxVisibleCharacters < currentLine.Length)
        {
            finished = false;
            text.maxVisibleCharacters++;
            yield return new WaitForSeconds(typingSpeed);
        }
        finished = true;
        typingCoroutine = null;
    }

    public void Clear()
    {
        text.maxVisibleCharacters = 0;
        text.text = dialogueLines[iterator];
        finished = false;
    }

    public void ClearAll()
    {
        text.maxVisibleCharacters = 0;
        text.text = "";
        finished = false;
        dialogueLines = new List<string>();
    }

    public void SetMessages(List<string> newMessages)
    {
        firstStart = true;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        iterator = 0;
        dialogueLines = newMessages;
        Clear();
        Type();
    }
}
