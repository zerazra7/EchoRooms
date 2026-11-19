using UnityEngine;

public class PlayerNoteScript : MonoBehaviour
{
    private NoteScript activeNote;

    [Tooltip("Optional: a UI object shown while standing at a note (e.g. \"Press E to read\"). Can be left empty.")]
    public GameObject interactMessage;

    void Start()
    {
        // 1) if it was not set in the Inspector, try to find it in the scene
        if (interactMessage == null)
        {
            interactMessage = GameObject.Find("InteractMessage"); // optional, fine if there is none
        }
        // 2) only call SetActive if it exists
        if (interactMessage != null) interactMessage.SetActive(false);
    }

    void Update()
    {
        // only toggle with E when actually standing at a note
        if (activeNote != null && Input.GetKeyDown(KeyCode.E))
            activeNote.ToggleNote();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            // get the NoteScript component
            activeNote = other.GetComponent<NoteScript>();
            if (activeNote == null)
            {
                Debug.LogError($"PlayerNoteScript: {other.name} has no NoteScript!");
                return;
            }
            if (interactMessage != null) interactMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Note") && activeNote != null)
        {
            // if the note is open, close it
            if (activeNote.GetNoteStatus())
                activeNote.ToggleNote();

            activeNote = null;
            if (interactMessage != null) interactMessage.SetActive(false);
        }
    }
}
