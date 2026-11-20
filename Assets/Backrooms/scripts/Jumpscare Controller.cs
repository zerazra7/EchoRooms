using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// on the player: plays a jumpscare when walking into one of the jumpscare areas
public class JumpscareController : MonoBehaviour
{
    public Image jumpscareImage1, jumpscareImage2;
    public AudioClip jumpscareSound1, jumpscareSound2, jumpscareSound3, jumpscareSound4;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        // area 1: picture on the screen + sound, picture goes away after 1 second
        if (other.gameObject.CompareTag("JumpscareArea"))
        {
            jumpscareImage1.enabled = true;
            audioSource.PlayOneShot(jumpscareSound1);
            StartCoroutine(CloseJumpscare());
        }

        // areas 2-4: only a sound
        if (other.gameObject.CompareTag("JumpscareArea2"))
        {
            audioSource.PlayOneShot(jumpscareSound2);
        }

        if (other.gameObject.CompareTag("JumpscareArea3"))
        {
            audioSource.PlayOneShot(jumpscareSound3);
        }

        if (other.gameObject.CompareTag("JumpscareArea4"))
        {
            audioSource.PlayOneShot(jumpscareSound4);
        }
    }

    // hides the jumpscare pictures after 1 second
    private IEnumerator CloseJumpscare()
    {
        yield return new WaitForSeconds(1);
        jumpscareImage1.enabled = false;
        jumpscareImage2.enabled = false;
    }
}
