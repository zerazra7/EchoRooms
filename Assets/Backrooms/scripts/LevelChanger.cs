using UnityEngine;
using UnityEngine.SceneManagement;

// menu buttons use this to open a scene (new game etc.)
public class LevelChanger : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        // time could still be stopped from a pause or death
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
