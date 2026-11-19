using UnityEngine;

// Put this on an empty object in Level 0.
// When the player comes back from Level 1 through the ring,
// they are moved to a random spawn point instead of the starting corner.
public class ReturnSpawn : MonoBehaviour
{
    // Set by RingTeleporter right before Level 0 is loaded
    public static bool cameBackThroughRing = false;

    [Tooltip("The player object (FPSController)")]
    public Transform player;

    [Tooltip("Possible places to appear after coming back from Level 1")]
    public Transform[] spawnPoints;

    void Start()
    {
        if (!cameBackThroughRing)
        {
            // Fresh start of Level 0 (new game or restart): start timing a new run
            RunTimer.Begin();
            return;
        }

        // Use the flag only once, so restarting the level starts from the beginning again
        cameBackThroughRing = false;

        if (player == null || spawnPoints == null || spawnPoints.Length == 0) return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // CharacterController overrides position changes, so turn it off while moving the player
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        player.position = point.position;

        if (controller != null) controller.enabled = true;
    }
}
