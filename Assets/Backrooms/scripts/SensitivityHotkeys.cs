using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// - and + change the mouse sensitivity in game (same setting as the menu slider)
// new value shows up at the top for a moment
public class SensitivityHotkeys : MonoBehaviour
{
    const string MenuSceneName = "mainMenu";
    const float Step = 0.1f;
    const float ShowTime = 1.5f;

    // runs once when the game starts, then checks every scene that loads
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        SceneManager.sceneLoaded += (scene, mode) => TryCreate(scene);
        TryCreate(SceneManager.GetActiveScene());
    }

    static void TryCreate(Scene scene)
    {
        // every level except the menu (the menu has the slider), only one
        if (scene.name == MenuSceneName) return;
        if (FindAnyObjectByType<SensitivityHotkeys>() != null) return;
        new GameObject("SensitivityHotkeys").AddComponent<SensitivityHotkeys>();
    }

    TextMeshProUGUI text;
    float hideAt;

    // hidden text at the top of the screen, shows up when the value changes
    void Start()
    {
        var canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 102;
        var scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        var textGO = new GameObject("SensitivityText", typeof(RectTransform));
        textGO.transform.SetParent(transform, false);
        var rect = textGO.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0, -40);
        rect.sizeDelta = new Vector2(1000, 60);

        text = textGO.AddComponent<TextMeshProUGUI>();
        text.fontSize = 34;
        text.alignment = TextAlignmentOptions.Center;
        text.color = new Color(0.95f, 0.92f, 0.78f, 0.9f);
        text.raycastTarget = false;
        text.gameObject.SetActive(false);
    }

    void Update()
    {
        // -1, 0 or +1 depending on the key
        int change = 0;

        if (Input.GetKeyDown(KeyCode.KeypadMinus)) change--;
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) change++;

        // checking the typed character so it works on turkish keyboards too
        if (change == 0)
        {
            foreach (char c in Input.inputString)
            {
                if (c == '-') change--;
                else if (c == '+' || c == '=') change++;
            }
        }

        // key pressed: change the value by 0.1, save it and show the text
        if (change != 0)
        {
            float value = MouseSettings.Sensitivity + change * Step;
            MouseSettings.Sensitivity = Mathf.Round(value * 10f) / 10f;

            text.text = MouseSettings.SensitivityText();
            text.gameObject.SetActive(true);
            hideAt = Time.unscaledTime + ShowTime;
        }

        // hide the text again after 1.5 seconds
        if (text.gameObject.activeSelf && Time.unscaledTime > hideAt)
            text.gameObject.SetActive(false);
    }
}
