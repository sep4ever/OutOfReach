using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Window : Interactable
{

    [SerializeField] GameObject windowImage;
    [SerializeField] DialogueBox dialogueBox;
    [SerializeField] List<string> windowThoughts = new List<string>();
    Player player;
    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        windowThoughts = new List<string>{ "I hate that view.", "Always hated it, and always will hate." };
    }

    public override void Interact()
    {
        if (dialogueBox.dialogueLines != windowThoughts) dialogueBox.SetMessages(windowThoughts);
        windowImage.SetActive(true);
        player.canMove = false;
    }

    public void CloseWindow()
    {
        windowImage.SetActive(false);
        dialogueBox.ClearAll();
        player.canMove = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) CloseWindow();
    }
}
