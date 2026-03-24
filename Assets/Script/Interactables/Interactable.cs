using UnityEngine;

public enum InteractionType
{
    Window,
    WorkStation,
    Bed,
    Axe,
    Documents,
    Shotgun,
    Door
}

public class Interactable : MonoBehaviour
{
    public bool canInteract = true;
    public InteractionType id;
    public virtual void Interact()
    {
        Debug.Log("Interacted with " + transform.name);
    }
}
