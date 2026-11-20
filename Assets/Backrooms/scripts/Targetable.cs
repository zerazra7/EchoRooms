using UnityEngine;

// an object the player can look at, shows an info text on it
public class Targetable : MonoBehaviour
{
    public GameObject infoObject;

    // turns the info text on or off
    public void ToggleHiglight(bool status)
    {
        infoObject.SetActive(status);
    }
}
