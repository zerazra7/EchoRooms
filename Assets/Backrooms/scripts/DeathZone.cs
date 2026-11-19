using UnityEngine;
using UnityEngine.UI;

public class DeathZone : MonoBehaviour
{
    [Header("Game Over UI")]
    
    public GameObject gameOverPanel;

    [Header("Optional: Player Controller")]
    public MonoBehaviour playerController;

    [Tooltip("Shown under YOU DIED")]
    public string deathMessage = "You didn't make it...";

    private bool isDead = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
        if (other.CompareTag("Player"))
        {
            isDead = true;
            ShowGameOver();
        }
    }

    private void ShowGameOver()
    {
        DeathScreen.Show(gameOverPanel, deathMessage);

      
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        
        if (playerController != null)
            playerController.enabled = false;
    }
}
