using UnityEngine;
using System.Collections;

// plays a scream when the player walks into this area, only once
public class SoundsJS : MonoBehaviour
{
    public AudioSource source;
    public AudioClip screamSound;

    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = screamSound;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(playSound());
        }
    }

    // plays the sound, then removes the area so it can't play again
    IEnumerator playSound()
    {
        source.Play();
        yield return new WaitForSeconds(5.25f);
        Destroy(gameObject);
    }
}
