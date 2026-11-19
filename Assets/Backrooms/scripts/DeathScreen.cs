using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

// Death sequence: the screen turns dark red, "YOU DIED" slams in with a line
// about what happened, then the screen fades to black and the main menu loads.
// The old Game Over panel stays hidden.
// The UI is built in code, so nothing has to be set up in the scene.
public class DeathScreen : MonoBehaviour
{
    const string MenuSceneName = "mainMenu";

    GameObject gameOverPanel;
    string reason;

    static bool showing;

    // Call this instead of gameOverPanel.SetActive(true)
    public static void Show(GameObject gameOverPanel, string reason)
    {
        if (showing) return; // e.g. the monster and a death zone at the same time
        showing = true;

        Time.timeScale = 0f; // freeze the game, the animation uses unscaled time
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        var screen = new GameObject("DeathScreen").AddComponent<DeathScreen>();
        screen.gameOverPanel = gameOverPanel;
        screen.reason = reason;
    }

    void Start()
    {
        StartCoroutine(Sequence());
    }

    void OnDestroy()
    {
        showing = false; // scene reloaded, ready for the next death
    }

    IEnumerator Sequence()
    {
        // --- build the overlay ---
        var canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 998;
        var scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        var group = gameObject.AddComponent<CanvasGroup>();
        group.blocksRaycasts = false; // never block the Game Over buttons

        var bgGO = new GameObject("Background", typeof(RectTransform));
        bgGO.transform.SetParent(transform, false);
        var bgRect = bgGO.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        var bg = bgGO.AddComponent<Image>();
        bg.color = new Color(0.25f, 0f, 0f, 0f);

        var title = CreateText("YOU DIED", 150, new Vector2(0, 30));
        title.color = new Color(0.8f, 0.05f, 0.05f);
        title.fontStyle = FontStyles.Bold;
        title.characterSpacing = 15;
        var subtitle = CreateText(reason, 44, new Vector2(0, -90));
        subtitle.color = new Color(0.9f, 0.85f, 0.75f);
        title.alpha = 0f;
        subtitle.alpha = 0f;

        // --- play it ---
        // red flash, while all game sounds (chase music, monster) fade out
        yield return Animate(0.4f, t =>
        {
            bg.color = new Color(0.25f, 0f, 0f, t * 0.85f);
            AudioListener.volume = 1f - t;
        });
        foreach (AudioSource source in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
            source.Stop();
        AudioListener.volume = 1f; // nothing is playing now, so the menu can have sound again

        // title drops in from big to normal size
        yield return Animate(0.35f, t =>
        {
            title.alpha = t;
            title.rectTransform.localScale = Vector3.one * Mathf.Lerp(1.6f, 1f, t);
        });
        yield return new WaitForSecondsRealtime(0.6f);
        yield return Animate(0.8f, t => subtitle.alpha = t);
        yield return new WaitForSecondsRealtime(1.8f);

        // fade to black and go back to the main menu
        var black = new GameObject("Black", typeof(RectTransform));
        black.transform.SetParent(transform, false);
        var blackRect = black.GetComponent<RectTransform>();
        blackRect.anchorMin = Vector2.zero;
        blackRect.anchorMax = Vector2.one;
        blackRect.sizeDelta = Vector2.zero;
        var blackImage = black.AddComponent<Image>();
        blackImage.color = new Color(0f, 0f, 0f, 0f);
        yield return Animate(1f, t => blackImage.color = new Color(0f, 0f, 0f, t));

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(MenuSceneName);
    }

    TextMeshProUGUI CreateText(string text, float size, Vector2 position)
    {
        var go = new GameObject("Text", typeof(RectTransform));
        go.transform.SetParent(transform, false);
        var rect = go.GetComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(1800, 200);
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.raycastTarget = false;
        return tmp;
    }

    IEnumerator Animate(float duration, System.Action<float> step)
    {
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            step(t / duration);
            yield return null;
        }
        step(1f);
    }
}
