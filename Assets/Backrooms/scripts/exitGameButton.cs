using UnityEngine;

public class exitGameButton : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    void Start()
    {
        // a browser game can't close itself, so hide the Exit button there
        gameObject.SetActive(false);
    }
#endif

    public void exitGame()
    {
#if UNITY_EDITOR
        // Application.Quit does nothing inside the editor, so stop Play mode instead
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}