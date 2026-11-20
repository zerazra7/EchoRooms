using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// the monster: follows the player on the navmesh and kills them when it gets close
// it can also jump down from a ledge (NavMesh Link), then needs a moment to get up
public class enemyController : MonoBehaviour
{
    public Transform target;
    public float killDistance = 1.2f;

    [Header("UI")]
    [Tooltip("Drag the GameOverPanel object here")]
    public GameObject gameOverPanel;

    [Header("Jumping down")]
    [Tooltip("How high it hops up before falling (0 = just slides off the edge)")]
    public float jumpHeight = 0.2f;
    [Tooltip("How fast it moves away from the edge. Higher = gets clear of the stairs sooner, lower = rounder curve")]
    [Range(1f, 5f)] public float edgePush = 3f;
    [Tooltip("How fast it falls, higher = faster")]
    public float fallGravity = 20f;
    [Tooltip("How long it stays on the ground before chasing again. The player can run past it in this time")]
    public float getUpTime = 1.8f;

    private NavMeshAgent agent;
    private Animator anim;
    private bool gameOverTriggered = false;
    private bool jumping = false;

    // true while jumping or getting up, the monster can't kill in this time
    public bool IsHarmless => jumping;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // we do the jump ourselves instead of the agent sliding over the link
        agent.autoTraverseOffMeshLink = false;

        // keep crawling all the way to the player
        // (with a big stopping distance it slowed down early and slid in without animation)
        agent.stoppingDistance = Mathf.Min(agent.stoppingDistance, killDistance * 0.5f);
        agent.autoBraking = false;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        // player is already dead, nothing to do
        if (gameOverTriggered)
            return;

        // reached a jump point: jump down
        if (agent.isOnOffMeshLink && !jumping)
            StartCoroutine(JumpDown());

        if (jumping)
            return;

        // walk to the player, crawling animation only while moving
        agent.SetDestination(target.position);
        // "wants to move" instead of "is fast", so the crawl doesn't stop while it's still sliding
        bool isMoving = agent.desiredVelocity.sqrMagnitude > 0.01f || agent.velocity.sqrMagnitude > 0.01f;
        anim.SetBool("isMoving", isMoving);

        // close enough: stop and show the death screen
        float dist = Vector3.Distance(transform.position, target.position);
        if (dist <= killDistance)
        {
            gameOverTriggered = true;

            agent.isStopped = true;
            anim.SetBool("isMoving", false);

            DeathScreen.Show(gameOverPanel, "It caught you...");
        }
    }

    IEnumerator JumpDown()
    {
        jumping = true;
        anim.SetBool("isMoving", false);

        // falling animation while in the air (if the animator has a "fall" trigger)
        if (HasTrigger("fall"))
            anim.SetTrigger("fall");

        OffMeshLinkData link = agent.currentOffMeshLinkData;
        Vector3 start = transform.position;
        Vector3 end = link.endPos + Vector3.up * agent.baseOffset;

        // look where it's jumping
        Vector3 flatDir = end - start;
        flatDir.y = 0f;
        if (flatDir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(flatDir);

        // one smooth curve from the ledge to the floor:
        // up/down works like real falling (small hop, then faster and faster)
        // sideways it moves fast at the start and slows down, so it gets away
        // from the stairs before it has dropped much and doesn't cut through them
        float upSpeed = Mathf.Sqrt(2f * fallGravity * Mathf.Max(jumpHeight, 0f));
        float drop = Mathf.Max(start.y - end.y, 0f);
        float airTime = (upSpeed + Mathf.Sqrt(upSpeed * upSpeed + 2f * fallGravity * drop)) / fallGravity;

        Vector3 startFlat = new Vector3(start.x, 0f, start.z);
        Vector3 endFlat = new Vector3(end.x, 0f, end.z);

        for (float t = 0f; t < airTime; t += Time.deltaTime)
        {
            float k = t / airTime;
            float side = 1f - Mathf.Pow(1f - k, edgePush);
            float y = start.y + upSpeed * t - 0.5f * fallGravity * t * t;
            Vector3 flat = Vector3.Lerp(startFlat, endFlat, side);
            transform.position = new Vector3(flat.x, y, flat.z);
            yield return null;
        }
        transform.position = end;
        agent.CompleteOffMeshLink();

        // landed: play the getting up animation and wait
        if (HasTrigger("getUp"))
            anim.SetTrigger("getUp");
        agent.isStopped = true;
        yield return new WaitForSeconds(getUpTime);
        agent.isStopped = false;

        jumping = false;
    }

    // so the game doesn't complain if the animator has no getUp trigger yet
    bool HasTrigger(string name)
    {
        foreach (AnimatorControllerParameter p in anim.parameters)
            if (p.name == name && p.type == AnimatorControllerParameterType.Trigger)
                return true;
        return false;
    }
}
