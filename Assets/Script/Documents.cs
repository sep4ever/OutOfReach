using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class Documents : Interactable
{
    [SerializeField][TextArea(30, 50)] List<string> documentLines;
    [SerializeField] TMP_Text documentText;
    [SerializeField] GameObject document;

    int documentPage;
    [SerializeField] bool interacting = false;

    [SerializeField] Transform playerTransform;
    [SerializeField] float interactionRange;
    [SerializeField] float range;

    public override void Interact()
    {
        interacting = true;
        documentText.text = documentLines[documentPage];
        if (documentPage < documentLines.Count - 1) documentPage++;
        else documentPage = 0;
    }

    private void Update()
    {
        range = Vector3.Distance(transform.position, playerTransform.position);
        document.SetActive(interacting);
        if (Input.GetKeyDown(KeyCode.Return) || range > interactionRange) interacting = false;
    }
}
