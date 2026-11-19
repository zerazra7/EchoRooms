using UnityEngine;
using System.Collections;

public class SoundsJS : MonoBehaviour
{
    public AudioSource source; // audio source that plays the sound
    public AudioClip screamSound; // the scream sound    

    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = screamSound;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Scream sound played");
            StartCoroutine(playSound());
        }
    }

    IEnumerator playSound()
    {
        source.Play();
        yield return new WaitForSeconds(5.25f);
        Destroy(gameObject);
    }

}
