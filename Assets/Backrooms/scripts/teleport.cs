using UnityEngine;
using UnityEngine.SceneManagement;

// the ring in level 1: walking through it takes the player back to level 0
public class RingTeleporter : MonoBehaviour
{
    [Tooltip("Name of the scene to load")]
    public string sceneName = "Level 0";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // tells level 0 to use a random spawn point instead of the corner
            ReturnSpawn.cameBackThroughRing = true;
            SceneManager.LoadScene(sceneName);
        }
    }
}
