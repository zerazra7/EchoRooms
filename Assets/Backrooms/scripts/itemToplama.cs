using UnityEngine;

// keycard in level 1: walking into it picks it up and shows "Keycard collected!" for 2 seconds
public class itemToplama : MonoBehaviour
{
    public static bool hasKeycard = false;

    bool showMessage = false;
    float timer = 0f;
    bool picked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!picked && other.CompareTag("Player"))
        {
            picked = true;

            // save the keycard, the exit door in level 0 checks this
            PlayerPrefs.SetInt("HasKeycard", 1);
            PlayerPrefs.Save();

            // show the message for 2 seconds
            showMessage = true;
            timer = 2f;

            // hide the keycard and turn off its collider so it can't be picked up again
            var rend = GetComponent<MeshRenderer>();
            if (rend) rend.enabled = false;
            var col = GetComponent<Collider>();
            if (col) col.enabled = false;

            // remove it after the message is gone
            Destroy(gameObject, 2f);
        }
    }

    // counts down the message time
    private void Update()
    {
        if (showMessage)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
                showMessage = false;
        }
    }

    // draws the message in the middle of the screen
    private void OnGUI()
    {
        if (showMessage)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 35;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.black;

            Rect fullScreenRect = new Rect(0, 0, Screen.width, Screen.height);
            GUI.Label(fullScreenRect, "Keycard collected!", style);
        }
    }
}
