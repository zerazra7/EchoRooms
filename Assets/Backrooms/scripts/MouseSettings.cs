using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

// mouse sensitivity slider, bottom right of the main menu
// saved in PlayerPrefs, MouseLook multiplies its sensitivity with it
public class MouseSettings : MonoBehaviour
{
    const string MenuSceneName = "mainMenu";
    const string PrefKey = "MouseSensitivity";
    const float MinValue = 0.2f;
    const float MaxValue = 3f;

    static float? cached;

    // the sensitivity value, 1 = original speed
    // read from PlayerPrefs the first time, saved again every time it changes
    public static float Sensitivity
    {
        get
        {
            if (cached == null) cached = Mathf.Clamp(PlayerPrefs.GetFloat(PrefKey, 1f), MinValue, MaxValue);
            return cached.Value;
        }
        set
        {
            cached = Mathf.Clamp(value, MinValue, MaxValue);
            PlayerPrefs.SetFloat(PrefKey, cached.Value);
            PlayerPrefs.Save();
        }
    }

    // "Mouse sensitivity: 1.0" text, also used by SensitivityHotkeys
    public static string SensitivityText()
    {
        // InvariantCulture so it's always 1.0 (turkish windows would show 1,0)
        return "Mouse sensitivity: " + Sensitivity.ToString("0.0", CultureInfo.InvariantCulture);
    }

    // runs once when the game starts, then checks every scene that loads
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        SceneManager.sceneLoaded += (scene, mode) => TryCreate(scene);
        TryCreate(SceneManager.GetActiveScene());
    }

    // only in the menu, and only one
    static void TryCreate(Scene scene)
    {
        if (scene.name != MenuSceneName) return;
        if (FindAnyObjectByType<MouseSettings>() != null) return;
        new GameObject("MouseSettings").AddComponent<MouseSettings>();
    }

    TextMeshProUGUI label;

    void Start()
    {
        // canvas on top of the menu
        var canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 101;
        var scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        gameObject.AddComponent<GraphicRaycaster>(); // needed to drag the slider

        // box in the bottom right corner, holds the text and the slider
        var box = new GameObject("SensitivityBox", typeof(RectTransform));
        box.transform.SetParent(transform, false);
        var boxRect = box.GetComponent<RectTransform>();
        boxRect.anchorMin = boxRect.anchorMax = new Vector2(1f, 0f);
        boxRect.pivot = new Vector2(1f, 0f);
        boxRect.anchoredPosition = new Vector2(-60, 60);
        boxRect.sizeDelta = new Vector2(420, 90);

        // text at the top of the box
        var labelGO = new GameObject("Label", typeof(RectTransform));
        labelGO.transform.SetParent(box.transform, false);
        var labelRect = labelGO.GetComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0f, 1f);
        labelRect.anchorMax = new Vector2(1f, 1f);
        labelRect.pivot = new Vector2(0.5f, 1f);
        labelRect.anchoredPosition = Vector2.zero;
        labelRect.sizeDelta = new Vector2(0, 45);
        label = labelGO.AddComponent<TextMeshProUGUI>();
        label.fontSize = 30;
        label.alignment = TextAlignmentOptions.Center;
        label.color = new Color(0.95f, 0.92f, 0.78f, 0.9f);
        label.raycastTarget = false;

        // unity's normal slider under the text, starts at the saved value
        var sliderGO = DefaultControls.CreateSlider(new DefaultControls.Resources());
        sliderGO.transform.SetParent(box.transform, false);
        var sliderRect = sliderGO.GetComponent<RectTransform>();
        sliderRect.anchorMin = new Vector2(0f, 0f);
        sliderRect.anchorMax = new Vector2(1f, 0f);
        sliderRect.pivot = new Vector2(0.5f, 0f);
        sliderRect.anchoredPosition = new Vector2(0, 10);
        sliderRect.sizeDelta = new Vector2(-20, 22);

        var slider = sliderGO.GetComponent<Slider>();
        slider.minValue = MinValue;
        slider.maxValue = MaxValue;
        slider.value = Sensitivity;
        slider.onValueChanged.AddListener(OnSliderChanged);

        // same colors as the menu text
        foreach (Image img in sliderGO.GetComponentsInChildren<Image>(true))
        {
            if (img.name == "Background") img.color = new Color(0f, 0f, 0f, 0.6f);
            else if (img.name == "Fill") img.color = new Color(0.95f, 0.92f, 0.78f, 0.8f);
            else if (img.name == "Handle") img.color = new Color(0.95f, 0.92f, 0.78f, 1f);
        }
        slider.handleRect.sizeDelta = new Vector2(24, 12);

        label.text = SensitivityText();
    }

    // slider moved: round to 0.1 steps, save and update the text
    void OnSliderChanged(float value)
    {
        Sensitivity = Mathf.Round(value * 10f) / 10f;
        label.text = SensitivityText();
    }
}
