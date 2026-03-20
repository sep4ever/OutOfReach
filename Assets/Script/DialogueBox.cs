using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] List<string> dialogueLines;
    [SerializeField] TMP_Text text;
    [SerializeField] float typingSpeed = 0.05f;

    public int iterator = 0;
    public bool finished = false;

    void Start()
    {
        Type();
    }

    void Update()
    {
        finished = dialogueLines[iterator].Length == text.maxVisibleCharacters;
        if (!finished || Input.GetKey(KeyCode.Z)) Type();
        if (iterator < dialogueLines.Count - 1 && Input.GetKeyDown(KeyCode.E) && finished) iterator++;
    }

    Coroutine typingCoroutine;
    void Type()
    {
        if (typingCoroutine == null)
        {
            Clear();
            typingCoroutine = StartCoroutine("TypingCoroutine");
        }
        else if (Input.GetKeyDown(KeyCode.E) && !finished) text.maxVisibleCharacters = dialogueLines[iterator].Length;
    }

    IEnumerator TypingCoroutine()
    {
        //text.maxVisibleCharacters = 0;
        //text.text = dialogueLines[iterator];
        while (text.maxVisibleCharacters < dialogueLines[iterator].Length)
        {
            finished = false;
            text.maxVisibleCharacters++;
            yield return new WaitForSeconds(typingSpeed);
        }
        finished = true;
        typingCoroutine = null;
    }

    void Clear()
    {
        text.maxVisibleCharacters = 0;
        text.text = dialogueLines[iterator];
        finished = false;
    }
}
