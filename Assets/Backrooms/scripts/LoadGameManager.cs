using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadGameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI LoadGameText;  // which level will be loaded

    [SerializeField] Button LoadButton;


    void Awake()
    {
        if (string.IsNullOrWhiteSpace(PlayerPrefs.GetString("loaded level", " ")))
        { 
            LoadButton.interactable = false;   // no saved level, so the button cannot be used

        }
        else
        {
            LoadButton.interactable = true;
            LoadGameText.text = PlayerPrefs.GetString("loaded level", " ");

        }
        
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("loaded level"));
    }
}
