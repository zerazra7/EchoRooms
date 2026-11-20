using UnityEngine;

// exit game button in the main menu
public class exitGameButton : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    // a web game can't close the browser tab, so hide the button on web
    void Start()
    {
        gameObject.SetActive(false);
    }
#endif

    // closes the game (stops play mode in the editor, Quit doesn't work there)
    public void exitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
