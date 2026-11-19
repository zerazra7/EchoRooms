using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelChanger : MonoBehaviour
{

    // used by the New Game button
    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1f; // set time back to normal
        SceneManager.LoadScene(sceneName);
    }
}
