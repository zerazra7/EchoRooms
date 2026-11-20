using UnityEngine;

// monster sound: plays when the player gets close to the monster
public class CanavarSes : MonoBehaviour
{
    public Transform oyuncu;          // player
    public float sesMesafesi = 10f;   // how close the player has to be
    public AudioClip sesKlip;         // the sound
    private AudioSource sesKaynak;

    public float sesSuresi = 3f;      // how long the sound plays (seconds)

    private bool sesCaldi = false;    // already played for this visit

    void Start()
    {
        sesKaynak = GetComponent<AudioSource>();
        sesKaynak.clip = sesKlip;
    }

    void Update()
    {
        float mesafe = Vector3.Distance(transform.position, oyuncu.position);

        // player came close: play the sound once and stop it after sesSuresi seconds
        if (mesafe <= sesMesafesi && !sesCaldi)
        {
            sesKaynak.Play();
            Invoke("SesiDurdur", sesSuresi);
            sesCaldi = true;
        }
        // player went away: it can play again next time they come close
        else if (mesafe > sesMesafesi)
        {
            sesCaldi = false;
        }
    }

    void SesiDurdur()
    {
        sesKaynak.Stop();
    }
}
