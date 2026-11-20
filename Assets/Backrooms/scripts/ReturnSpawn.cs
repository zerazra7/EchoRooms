using UnityEngine;

// level 0: if the player came back from level 1 through the ring,
// put them on a random spawn point instead of the starting corner
public class ReturnSpawn : MonoBehaviour
{
    // RingTeleporter sets this before loading level 0
    public static bool cameBackThroughRing = false;

    [Tooltip("The player object (FPSController)")]
    public Transform player;

    [Tooltip("Possible places to appear after coming back from Level 1")]
    public Transform[] spawnPoints;

    void Start()
    {
        // new game or restart: start the escape timer, player stays in the corner
        if (!cameBackThroughRing)
        {
            RunTimer.Begin();
            return;
        }

        // only once, a restart should start from the corner again
        cameBackThroughRing = false;

        if (player == null || spawnPoints == null || spawnPoints.Length == 0) return;

        // pick a random spawn point
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // move the player there
        // the character controller has to be off, or it puts the player back
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        player.position = point.position;

        if (controller != null) controller.enabled = true;
    }
}
