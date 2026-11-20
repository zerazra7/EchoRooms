using UnityEngine;

// exit door in level 0, only opens if the player has the keycard
public class EscapeDoor : MonoBehaviour
{
    public AudioClip escapeSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // player with the keycard walks in: play the door sound and start the ending
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerPrefs.GetInt("HasKeycard", 0) == 1)
        {
            if (escapeSound != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(escapeSound);
            }

            EndGameManager.Instance.ShowEndGame();
        }
    }
}
