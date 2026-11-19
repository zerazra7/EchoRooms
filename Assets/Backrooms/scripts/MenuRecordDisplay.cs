using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// Shows the best escape time at the bottom of the main menu,
// and the time of the run that just finished (once).
// Nothing has to be set up in the scene: the text is created automatically
// whenever the "mainMenu" scene is loaded.
public class MenuRecordDisplay : MonoBehaviour
{
    const string MenuSceneName = "mainMenu";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        SceneManager.sceneLoaded += (scene, mode) => TryCreate(scene);
        TryCreate(SceneManager.GetActiveScene()); // in case the game starts on the menu
    }

    static void TryCreate(Scene scene)
    {
        if (scene.name != MenuSceneName) return;
        if (FindAnyObjectByType<MenuRecordDisplay>() != null) return;
        new GameObject("RecordDisplay").AddComponent<MenuRecordDisplay>();
    }

    void Start()
    {
        // --- build a small overlay canvas with one line of text at the bottom ---
        var canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        var scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        var textGO = new GameObject("RecordText", typeof(RectTransform));
        textGO.transform.SetParent(transform, false);
        var rect = textGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);
        rect.anchoredPosition = new Vector2(0, 70);
        rect.sizeDelta = new Vector2(1400, 80);

        var text = textGO.AddComponent<TextMeshProUGUI>();
        text.fontSize = 34;
        text.alignment = TextAlignmentOptions.Center;
        text.color = new Color(0.95f, 0.92f, 0.78f, 0.9f);
        text.raycastTarget = false; // don't block clicks on the menu buttons

        // --- fill in the text ---
        float best = RunTimer.Best;
        float last = RunTimer.Last;

        if (best <= 0f)
        {
            text.text = "Best escape: --:--";
            return;
        }

        string line = "Best escape: " + RunTimer.Format(best);
        if (last > 0f)
        {
            line = (RunTimer.LastWasRecord ? "NEW RECORD!     " : "Your time: " + RunTimer.Format(last) + "     ") + line;
            RunTimer.ClearLast();
        }
        text.text = line;
    }
}