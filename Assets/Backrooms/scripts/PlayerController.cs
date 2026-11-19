using UnityEngine;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float turnSpeed;
    public AudioClip[] FootstepClips; // footstep sound clips
    public float StepInterval = 1f; // time between steps (seconds)

    private CharacterController _characterController;
    private AudioSource _audioSource;
    private float _stepTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();
        _stepTimer = StepInterval;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        transform.Translate(Vector3.right * Time.deltaTime * turnSpeed);
        // play footsteps while the character is moving
        if (_characterController.isGrounded && _characterController.velocity.magnitude > 0.1f)
        {
            _stepTimer -= Time.deltaTime;

            if (_stepTimer <= 0f)
            {
                PlayFootstep();
                _stepTimer = StepInterval; // reset the timer
            }
        }
        else
        {
            // character is standing still, reset the timer
            _stepTimer = StepInterval;
        }
    }

    private void PlayFootstep()
    {
        if (FootstepClips.Length > 0)
        {
            int index = Random.Range(0, FootstepClips.Length); // pick a random clip
            _audioSource.PlayOneShot(FootstepClips[index]);    // play it
        }
    }
}
