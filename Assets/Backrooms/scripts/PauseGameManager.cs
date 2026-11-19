using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityStandardAssets.Characters.FirstPerson;
public class PauseGameManager : MonoBehaviour
{
    public TextMeshProUGUI LevelText; // name of the current level

    public GameObject PauseCanvas;

    public KeyCode PauseKey = KeyCode.Tab; // key used to pause

    bool isPaused;

    private void Update()
    {
        LevelText.text = SceneManager.GetActiveScene().name;

        if (Input.GetKeyUp(PauseKey)) 
        { 
            isPaused = !isPaused;

            if (!isPaused ) ResumeGame();
        }

        if (isPaused)
        {
            PauseCanvas.SetActive(true);

            Time.timeScale = 0;
            AudioListener.pause = true;

            Cursor.lockState = CursorLockMode.None; // unlock the cursor so it can move again
            Cursor.visible = true;

            Object.FindFirstObjectByType<FirstPersonController>().enabled = false; // disable the character

        }else
        {
            PauseCanvas.SetActive(false);
        }

    }

    public void ResumeGame()
    {
        isPaused = false; // unpause the game

        Time.timeScale = 1;
        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.Locked; // lock the cursor back to the center
        Cursor.visible = false;

        Object.FindFirstObjectByType<FirstPersonController>().enabled = true; // enable the character again
    }

    public void QuitGame()
    {
        isPaused = false; // unpause the game

        Time.timeScale = 1;
        AudioListener.pause = false;

        SceneManager.LoadScene("mainMenu");
    }

    public void RestartLevel()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

        
}
