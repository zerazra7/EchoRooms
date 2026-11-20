using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

// death screen: red screen + "YOU DIED", then it goes black and back to the main menu
// all the ui is made here in code, the old game over panel just stays hidden
public class DeathScreen : MonoBehaviour
{
    const string MenuSceneName = "mainMenu";

    string reason;

    static bool showing;

    // the monster and the death zones call this when the player dies
    public static void Show(GameObject gameOverPanel, string reason)
    {
        // only one death screen at a time (monster and water can hit at the same moment)
        if (showing) return;
        showing = true;

        // stop the game, the animation below still runs because it uses real time
        Time.timeScale = 0f;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        var screen = new GameObject("DeathScreen").AddComponent<DeathScreen>();
        screen.reason = reason;
    }

    void Start()
    {
        StartCoroutine(Sequence());
    }

    // scene changed, so the next death can show the screen again
    void OnDestroy()
    {
        showing = false;
    }

    IEnumerator Sequence()
    {
        // canvas that covers the whole screen, on top of everything
        var canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 998;
        var scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // red background, title and the reason text, all invisible at first
        var bg = CreateFullScreenImage("Background", new Color(0.25f, 0f, 0f, 0f));

        var title = CreateText("YOU DIED", 150, new Vector2(0, 30));
        title.color = new Color(0.8f, 0.05f, 0.05f);
        title.fontStyle = FontStyles.Bold;
        title.characterSpacing = 15;
        var subtitle = CreateText(reason, 44, new Vector2(0, -90));
        subtitle.color = new Color(0.9f, 0.85f, 0.75f);
        title.alpha = 0f;
        subtitle.alpha = 0f;

        // screen turns red and all sounds fade out
        yield return Animate(0.4f, t =>
        {
            bg.color = new Color(0.25f, 0f, 0f, t * 0.85f);
            AudioListener.volume = 1f - t;
        });

        // stop every sound, then turn the volume back up for the menu
        foreach (AudioSource source in FindObjectsByType<AudioSource>())
            source.Stop();
        AudioListener.volume = 1f;

        // "YOU DIED" comes in big and shrinks to normal size, then the reason shows up
        yield return Animate(0.35f, t =>
        {
            title.alpha = t;
            title.rectTransform.localScale = Vector3.one * Mathf.Lerp(1.6f, 1f, t);
        });
        yield return new WaitForSecondsRealtime(0.6f);
        yield return Animate(0.8f, t => subtitle.alpha = t);
        yield return new WaitForSecondsRealtime(1.8f);

        // fade to black
        var black = CreateFullScreenImage("Black", new Color(0f, 0f, 0f, 0f));
        yield return Animate(1f, t => black.color = new Color(0f, 0f, 0f, t));

        // back to normal and load the main menu
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(MenuSceneName);
    }

    // makes an image that fills the whole screen
    Image CreateFullScreenImage(string name, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(transform, false);
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        var img = go.AddComponent<Image>();
        img.color = color;
        img.raycastTarget = false;
        return img;
    }

    // makes a centered text at the given position
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

    // runs step() every frame with a value going from 0 to 1 over the duration
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
