using UnityEngine;

// simple movement with footstep sounds
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float turnSpeed;
    public AudioClip[] FootstepClips;
    public float StepInterval = 1f; // seconds between two steps

    private CharacterController _characterController;
    private AudioSource _audioSource;
    private float _stepTimer;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();
        _stepTimer = StepInterval;
    }

    void Update()
    {
        // move forward and sideways
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        transform.Translate(Vector3.right * Time.deltaTime * turnSpeed);

        // walking on the ground: play a step sound every StepInterval seconds
        if (_characterController.isGrounded && _characterController.velocity.magnitude > 0.1f)
        {
            _stepTimer -= Time.deltaTime;

            if (_stepTimer <= 0f)
            {
                PlayFootstep();
                _stepTimer = StepInterval;
            }
        }
        else
        {
            // standing still or in the air, start the timer over
            _stepTimer = StepInterval;
        }
    }

    // plays a random footstep sound
    private void PlayFootstep()
    {
        if (FootstepClips.Length > 0)
        {
            int index = Random.Range(0, FootstepClips.Length);
            _audioSource.PlayOneShot(FootstepClips[index]);
        }
    }
}
