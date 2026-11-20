using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityStandardAssets.Characters.FirstPerson;

// pause menu, opens and closes with tab
public class PauseGameManager : MonoBehaviour
{
    public TextMeshProUGUI LevelText; // name of the current level

    public GameObject PauseCanvas;

    public KeyCode PauseKey = KeyCode.Tab;

    bool isPaused;

    private void Update()
    {
        LevelText.text = SceneManager.GetActiveScene().name;

        // tab switches between paused and not paused
        if (Input.GetKeyUp(PauseKey))
        {
            isPaused = !isPaused;

            if (!isPaused) ResumeGame();
        }

        // while paused: show the menu, stop time and sound, free the mouse, stop the player
        if (isPaused)
        {
            PauseCanvas.SetActive(true);

            Time.timeScale = 0;
            AudioListener.pause = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Object.FindAnyObjectByType<FirstPersonController>().enabled = false;
        }
        else
        {
            PauseCanvas.SetActive(false);
        }
    }

    // resume button: everything back to normal, mouse locked to the center again
    public void ResumeGame()
    {
        isPaused = false;

        Time.timeScale = 1;
        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Object.FindAnyObjectByType<FirstPersonController>().enabled = true;
    }

    // quit button: back to the main menu
    public void QuitGame()
    {
        isPaused = false;

        Time.timeScale = 1;
        AudioListener.pause = false;

        SceneManager.LoadScene("mainMenu");
    }

    // restart button: opens the same level again
    public void RestartLevel()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
