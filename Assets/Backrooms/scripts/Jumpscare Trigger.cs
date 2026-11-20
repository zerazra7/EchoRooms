using UnityEngine;

// jumpscare that plays when the mouse is over this object, only once
public class JumpscareTrigger : MonoBehaviour
{
    public Animation JumpscareAnimation;

    public AudioSource JumpscareAudio;

    public AudioClip AudioClip;

    public void OnMouseOver()
    {
        // play the animation and the sound
        JumpscareAnimation.Play();

        if (JumpscareAudio != null)
        {
            JumpscareAudio.PlayOneShot(AudioClip);
        }

        // turn the trigger off so it doesn't play again
        this.gameObject.SetActive(false);
    }
}
