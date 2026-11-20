using UnityEngine;
using UnityEngine.UI;
using TMPro;

// a note on the wall/floor: opens and closes its picture on the screen
public class NoteScript : MonoBehaviour
{
    private bool noteStatus;
    public GameObject note;

    [Tooltip("How much of the screen height the opened note takes up (0-1)")]
    [Range(0.3f, 1f)]
    public float noteScreenHeight = 0.8f;

    // opens the note if it's closed, closes it if it's open
    // the game is paused while reading
    public void ToggleNote()
    {
        noteStatus = !noteStatus;
        note.SetActive(noteStatus);

        if (noteStatus)
        {
            ResizeNoteImage();
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public bool GetNoteStatus()
    {
        return noteStatus;
    }

    // makes the note the same size on every screen and keeps its shape
    void ResizeNoteImage()
    {
        // find the paper picture (not the dark background behind it)
        Image noteImage = null;
        foreach (Image img in note.GetComponentsInChildren<Image>(true))
        {
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

        // a full page (~700px) gets noteScreenHeight of the screen,
        // short notes stay smaller so the writing looks the same size on all notes
        const float fullPageHeight = 700f;
        float pageFraction = Mathf.Min(1f, spriteRect.height / fullPageHeight);

        float height = Screen.height * noteScreenHeight * pageFraction / scaleFactor;
        float width = height * aspect;

        // wide notes shouldn't go out of the screen
        float maxWidth = Screen.width * 0.9f / scaleFactor;
        if (width > maxWidth)
        {
            width = maxWidth;
            height = width / aspect;
        }

        // put the paper in the middle with the new size
        RectTransform rect = noteImage.rectTransform;
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.localScale = Vector3.one;
        rect.sizeDelta = new Vector2(width, height);

        // move the "Press E to close." text right under the paper
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
