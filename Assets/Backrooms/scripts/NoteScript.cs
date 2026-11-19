using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteScript : MonoBehaviour
{
    private bool noteStatus;
    public GameObject note;

    [Tooltip("How much of the screen height the opened note takes up (0-1)")]
    [Range(0.3f, 1f)]
    public float noteScreenHeight = 0.8f;

    public void ToggleNote()
    {
        noteStatus = !noteStatus;
        note.SetActive(noteStatus);

        // pause / resume the game
        if (noteStatus)
        {
            ResizeNoteImage();
            Time.timeScale = 0f; // game paused
        }
        else
        {
            Time.timeScale = 1f; // game resumed
        }
    }

    public bool GetNoteStatus()
    {
        return noteStatus;
    }

    // Makes the note picture fill a part of the screen, whatever the resolution,
    // and keeps the picture's original proportions.
    void ResizeNoteImage()
    {
        Image noteImage = null;
        foreach (Image img in note.GetComponentsInChildren<Image>(true))
        {
            // skip the dark background panel itself, we want the paper inside it
            if (img.gameObject != note && img.sprite != null)
            {
                noteImage = img;
                break;
            }
        }
        if (noteImage == null) return;

        Canvas canvas = noteImage.canvas;
        float scaleFactor = canvas != null ? canvas.scaleFactor : 1f;

        Rect spriteRect = noteImage.sprite.rect;
        float aspect = spriteRect.width / spriteRect.height;

        // A full page (about 700 px tall) fills noteScreenHeight of the screen.
        // Shorter notes, like a small strip of paper, stay smaller by the same amount,
        // so the handwriting is the same size on every note.
        const float fullPageHeight = 700f;
        float pageFraction = Mathf.Min(1f, spriteRect.height / fullPageHeight);

        float height = Screen.height * noteScreenHeight * pageFraction / scaleFactor;
        float width = height * aspect;

        // don't let wide notes go off the sides of the screen
        float maxWidth = Screen.width * 0.9f / scaleFactor;
        if (width > maxWidth)
        {
            width = maxWidth;
            height = width / aspect;
        }

        RectTransform rect = noteImage.rectTransform;
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f); // size is measured from the center
        rect.anchoredPosition = Vector2.zero; // middle of the screen
        rect.localScale = Vector3.one;
        rect.sizeDelta = new Vector2(width, height);

        // move the "Press E to close." hint just under the paper so it doesn't cover the text
        foreach (TMP_Text hint in note.GetComponentsInChildren<TMP_Text>(true))
        {
            if (!hint.text.StartsWith("Press E")) continue;

            RectTransform hintRect = hint.rectTransform;
            hintRect.SetParent(note.transform, false);
            hintRect.anchorMin = hintRect.anchorMax = new Vector2(0.5f, 0.5f);
            hintRect.pivot = new Vector2(0.5f, 1f);
            hintRect.sizeDelta = new Vector2(width, hintRect.sizeDelta.y);
            hintRect.anchoredPosition = new Vector2(0f, -height / 2f - 10f);
            hint.alignment = TextAlignmentOptions.Center;
        }
    }
}
