using UnityEngine;
using UnityEngine.AI;

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

        // the panel starts hidden
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (gameOverTriggered)
            return;  // already triggered once, skip everything

        agent.SetDestination(target.position);
        bool isMoving = agent.velocity.sqrMagnitude > 0.01f;
        anim.SetBool("isMoving", isMoving);

        float dist = Vector3.Distance(transform.position, target.position);
        if (dist <= killDistance)
        {
            gameOverTriggered = true;

            // stop moving
            agent.isStopped = true;
            anim.SetBool("isMoving", false);

            // show the panel and stop time
            DeathScreen.Show(gameOverPanel, "It caught you...");
        }
    }
}
