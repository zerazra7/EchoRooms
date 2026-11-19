using UnityEngine;
using UnityEngine.SceneManagement;

public class RingTeleporter : MonoBehaviour
{
    [Tooltip("Name of the scene to load")]
    public string sceneName = "Level 0";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Tell Level 0 that the player is coming back through the ring
            ReturnSpawn.cameBackThroughRing = true;
            SceneManager.LoadScene(sceneName);
        }
    }
}