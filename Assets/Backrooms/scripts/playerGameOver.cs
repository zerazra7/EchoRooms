using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

// on the player: dies when the monster touches them
public class PlayerGameOver : MonoBehaviour
{
    [Header("Game Over UI Panel")]
    [SerializeField] private GameObject gameOverPanel;

    private bool isGameOver = false;

    // touched by the monster: show the death screen (only once)
    private void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;
        if (other.CompareTag("Enemy"))
        {
            // monster is getting up after a jump, the player can run past it
            var enemy = other.GetComponentInParent<enemyController>();
            if (enemy != null && enemy.IsHarmless) return;

            isGameOver = true;
            GameOver();
        }
    }

    private void GameOver()
    {
        DeathScreen.Show(gameOverPanel, "It caught you...");
    }

    // buttons of the old game over panel
    // restart: time back to normal and open the same level again
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // quit: close the game (stops play mode in the editor)
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
