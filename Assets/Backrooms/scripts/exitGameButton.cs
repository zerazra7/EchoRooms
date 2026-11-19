using UnityEngine;

public class exitGameButton : MonoBehaviour
{
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