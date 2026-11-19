using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerGameOver : MonoBehaviour
{
    [Header("Game Over UI Panel")]
    [SerializeField] private GameObject gameOverPanel;  // drag it in from the Inspector

    private bool isGameOver = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;
        if (other.CompareTag("Enemy"))
        {
            isGameOver = true;
            GameOver();
        }
    }

    private void GameOver()
    {
        // freeze the game and show "YOU DIED", then the Game Over panel
        DeathScreen.Show(gameOverPanel, "It caught you...");
    }

    // --- hook these up to the UI buttons ---

    // for the New Game button
    public void RestartGame()
    {
        // set time scale back to normal
        Time.timeScale = 1f;
        // reload the same scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // for the Exit Game button
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        // stops Play mode in the editor
        EditorApplication.isPlaying = false;
#endif
    }
}
