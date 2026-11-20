using UnityEngine;

// on the player: reading notes with E when standing next to one
public class PlayerNoteScript : MonoBehaviour
{
    private NoteScript activeNote; // the note the player is standing at

    [Tooltip("\"Press E to read\" text, can be empty")]
    public GameObject interactMessage;

    // find the "press E" text if it's not set, and hide it at the start
    void Start()
    {
        if (interactMessage == null)
            interactMessage = GameObject.Find("InteractMessage");
        if (interactMessage != null) interactMessage.SetActive(false);
    }

    // E opens/closes the note, only when standing at one
    void Update()
    {
        if (activeNote != null && Input.GetKeyDown(KeyCode.E))
            activeNote.ToggleNote();
    }

    // walked up to a note: remember it and show "press E"
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            activeNote = other.GetComponent<NoteScript>();
            if (activeNote == null)
            {
                Debug.LogError($"PlayerNoteScript: {other.name} has no NoteScript!");
                return;
            }
            if (interactMessage != null) interactMessage.SetActive(true);
        }
    }

    // walked away from the note: close it if it's open and hide "press E"
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Note") && activeNote != null)
        {
            if (activeNote.GetNoteStatus())
                activeNote.ToggleNote();

            activeNote = null;
            if (interactMessage != null) interactMessage.SetActive(false);
        }
    }
}
