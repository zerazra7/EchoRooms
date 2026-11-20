using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

// ending: fade to black -> "YOU ESCAPED" -> "...for now." -> main menu
// all the ui is made here in code, no panel needed in the scene
public class EndGameManager : MonoBehaviour
{
    public static EndGameManager Instance;

    [Header("Old UI (not used anymore, the scene still references it)")]
    public GameObject endPanel;
    public TextMeshProUGUI endText;

    [Header("Settings")]
    public string menuSceneName = "mainMenu";
    public float fadeDuration = 2f;

    bool ending = false;

    // only one EndGameManager, the exit door finds it through Instance
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (endPanel != null) endPanel.SetActive(false);
    }

    // the exit door calls this when the player has the keycard
    public void ShowEndGame()
    {
        // the door trigger can fire more than once, only end once
        if (ending) return;
        ending = true;

        RunTimer.Finish();
        StartCoroutine(EndSequence());
    }

    IEnumerator EndSequence()
    {
        // stop the game, the fades below still run because they use real time
        Time.timeScale = 0f;

        // canvas that covers the whole screen, on top of everything
        var canvasGO = new GameObject("EndScreen");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // black background and the two texts, all invisible at first
        var black = CreateImage(canvasGO.transform, Color.black);
        var blackGroup = black.gameObject.AddComponent<CanvasGroup>();
        blackGroup.alpha = 0f;

        var title = CreateText(canvasGO.transform, "YOU ESCAPED", 110, new Vector2(0, 40));
        title.characterSpacing = 12;
        var subtitle = CreateText(canvasGO.transform, "...for now.", 48, new Vector2(0, -70));
        subtitle.color = new Color(0.75f, 0.1f, 0.1f);
        title.alpha = 0f;
        subtitle.alpha = 0f;

        // show them one after another
        yield return Fade(a => blackGroup.alpha = a, fadeDuration);
        yield return new WaitForSecondsRealtime(0.5f);
        yield return Fade(a => title.alpha = a, 1.5f);
        yield return new WaitForSecondsRealtime(1.5f);
        yield return Fade(a => subtitle.alpha = a, 1f);
        yield return new WaitForSecondsRealtime(2.5f);

        // back to normal and load the main menu
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(menuSceneName);
    }

    // takes alpha from 0 to 1 over the duration
    static IEnumerator Fade(System.Action<float> setAlpha, float duration)
    {
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            setAlpha(t / duration);
            yield return null;
        }
        setAlpha(1f);
    }

    // makes an image that fills the whole screen
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

    // makes a centered white text at the given position
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
