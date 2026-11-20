using UnityEngine;

// deep water in level 1, the player dies when they fall in
public class DeathZone : MonoBehaviour
{
    [Header("Game Over UI")]
    public GameObject gameOverPanel;

    [Header("Optional: Player Controller")]
    public MonoBehaviour playerController;

    [Tooltip("Shown under YOU DIED")]
    public string deathMessage = "You didn't make it...";

    private bool isDead = false;

    // player touched the water: show the death screen (only once)
    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
        if (other.CompareTag("Player"))
        {
            isDead = true;
            ShowGameOver();
        }
    }

    // death screen, free the mouse and stop the player from moving
    private void ShowGameOver()
    {
        DeathScreen.Show(gameOverPanel, deathMessage);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerController != null)
            playerController.enabled = false;
    }
}
