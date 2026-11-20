using UnityEngine;
using UnityEngine.AI;

// the monster: follows the player on the navmesh and kills them when it gets close
public class enemyController : MonoBehaviour
{
    public Transform target;
    public float killDistance = 1.2f;

    [Header("UI")]
    [Tooltip("Drag the GameOverPanel object here")]
    public GameObject gameOverPanel;

    private NavMeshAgent agent;
    private Animator anim;
    private bool gameOverTriggered = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        // player is already dead, nothing to do
        if (gameOverTriggered)
            return;

        // walk to the player, crawling animation only while moving
        agent.SetDestination(target.position);
        bool isMoving = agent.velocity.sqrMagnitude > 0.01f;
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
}
