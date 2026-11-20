using UnityEngine;
using UnityEngine.SceneManagement;

// saves the current level every few seconds, so "load game" can open it later
public class SaveLevel : MonoBehaviour
{
    public GameObject SaveGameUI;
    [Range(1, 30)] public float timeBetweenSaves;

    // start saving again and again
    void Awake()
    {
        InvokeRepeating(nameof(SaveGame), timeBetweenSaves, timeBetweenSaves);
    }

    // saves the level name and shows the "saved" text for 3 seconds
    public void SaveGame()
    {
        SaveGameUI.SetActive(true);

        PlayerPrefs.SetString("loaded level", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();

        Invoke(nameof(hideUI), 3);
    }

    public void hideUI()
    {
        SaveGameUI?.SetActive(false);
    }
}
