using UnityEngine;

// put next to an AudioSource in level 0: the sound plays when the game starts,
// but not again when the player comes back from level 1 through the ring
[RequireComponent(typeof(AudioSource))]
public class PlayOnlyOnFirstEntry : MonoBehaviour
{
    void Awake()
    {
        // awake runs before ReturnSpawn.Start resets the flag
        if (!ReturnSpawn.cameBackThroughRing) return;

        AudioSource source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.Stop();
    }
}
