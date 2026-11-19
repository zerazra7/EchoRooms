using UnityEngine;

public class ResetKeycardOnStart : MonoBehaviour
{
    // Static flag: false when the game starts, true after the first reset
    private static bool hasReset = false;

    void Awake()
    {
        // only clear it if it has not been reset yet
        if (!hasReset)
        {
            PlayerPrefs.DeleteKey("HasKeycard");
            // or: PlayerPrefs.SetInt("HasKeycard", 0);
            PlayerPrefs.Save();
            hasReset = true;
        }
    }
}
