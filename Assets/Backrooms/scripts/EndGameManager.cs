using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

// Ending sequence: the screen fades to black, "YOU ESCAPED" appears,
// then "...for now.", and the game returns to the main menu.
// The UI is built in code, so no panel has to be set up in the scene.
public class EndGameManager : MonoBehaviour
{
    public static EndGameManager Instance;

    [Header("Old UI (hidden, kept so the scene references don't break)")]
    public GameObject endPanel;
    public TextMeshProUGUI endText;

    [Header("Settings")]
    public string menuSceneName = "mainMenu";
    public float fadeDuration = 2f;

    bool ending = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (endPanel != null) endPanel.SetActive(false);
    }

    public void ShowEndGame()
    {
        if (ending) return; // the exit trigger can fire more than once
        ending = true;

        RunTimer.Finish();
        StartCoroutine(EndSequence());
    }

    IEnumerator EndSequence()
    {
        // Freeze the game; the sequence below uses unscaled time
        Time.timeScale = 0f;

        // --- build the overlay ---
        var canvasGO = new GameObject("EndScreen");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        var black = CreateImage(canvasGO.transform, Color.black);
        var blackGroup = black.gameObject.AddComponent<CanvasGroup>();
        blackGroup.alpha = 0f;

        var title = CreateText(canvasGO.transform, "YOU ESCAPED", 110, new Vector2(0, 40));
        title.characterSpacing = 12;
        var subtitle = CreateText(canvasGO.transform, "...for now.", 48, new Vector2(0, -70));
        subtitle.color = new Color(0.75f, 0.1f, 0.1f);
        title.alpha = 0f;
        subtitle.alpha = 0f;

        // --- play the sequence ---
        yield return Fade(a => blackGroup.alpha = a, fadeDuration);
        yield return new WaitForSecondsRealtime(0.5f);
        yield return Fade(a => title.alpha = a, 1.5f);
        yield return new WaitForSecondsRealtime(1.5f);
        yield return Fade(a => subtitle.alpha = a, 1f);
        yield return new WaitForSecondsRealtime(2.5f);

        // --- back to the menu ---
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(menuSceneName);
    }

    static IEnumerator Fade(System.Action<float> setAlpha, float duration)
    {
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            setAlpha(t / duration);
            yield return null;
        }
        setAlpha(1f);
    }

    static Image CreateImage(Transform parent, Color color)
    {
        var go = new GameObject("Background", typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        var img = go.AddComponent<Image>();
        img.color = color;
        return img;
    }

    static TextMeshProUGUI CreateText(Transform parent, string text, float size, Vector2 position)
    {
        var go = new GameObject(text, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var rect = go.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(1600, 200);
        rect.anchoredPosition = position;
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        return tmp;
    }
}
