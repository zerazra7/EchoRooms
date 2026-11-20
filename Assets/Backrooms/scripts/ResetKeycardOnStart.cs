using UnityEngine;

// the keycard is saved in PlayerPrefs, so it would still be there next time the game opens
// this removes it once when the game starts
public class ResetKeycardOnStart : MonoBehaviour
{
    // false when the game starts, true after the first reset
    private static bool hasReset = false;

    void Awake()
    {
        // only the first time, not every time a scene loads
        if (!hasReset)
        {
            PlayerPrefs.DeleteKey("HasKeycard");
            PlayerPrefs.Save();
            hasReset = true;
        }
    }
}
