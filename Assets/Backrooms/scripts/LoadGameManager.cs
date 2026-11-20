using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// load game button in the main menu, opens the last saved level
public class LoadGameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI LoadGameText; // shows which level will be loaded

    [SerializeField] Button LoadButton;

    void Awake()
    {
        // no saved level -> button can't be clicked
        // saved level -> button works and shows the level name
        if (string.IsNullOrWhiteSpace(PlayerPrefs.GetString("loaded level", " ")))
        {
            LoadButton.interactable = false;
        }
        else
        {
            LoadButton.interactable = true;
            LoadGameText.text = PlayerPrefs.GetString("loaded level", " ");
        }
    }

    // opens the saved level
    public void LoadGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("loaded level"));
    }
}
