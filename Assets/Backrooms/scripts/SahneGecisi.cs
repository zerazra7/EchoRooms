using UnityEngine;
using UnityEngine.SceneManagement; // needed for scene loading

public class SceneTransition : MonoBehaviour
{
    public string PoolroomsDemo; // scene to switch to

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // if the object that entered is the player
        {
            SceneManager.LoadScene("Level 1"); // load the Poolrooms level
        }
    }
}