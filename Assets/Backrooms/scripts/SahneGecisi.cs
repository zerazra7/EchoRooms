using UnityEngine;
using UnityEngine.SceneManagement;

// on the right door in level 0: walking through it opens level 1 (poolrooms)
public class SceneTransition : MonoBehaviour
{
    public string PoolroomsDemo; // not used, the scene name is written below

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Level 1");
        }
    }
}
