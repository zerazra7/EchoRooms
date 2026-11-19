using UnityEngine;

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
            PlayerPrefs.SetInt("HasKeycard", 1);
            PlayerPrefs.Save();

            // start the timer
            showMessage = true;
            timer = 2f;

            // hide the mesh and turn off the collider (invisible, cannot trigger again)
            var rend = GetComponent<MeshRenderer>();
            if (rend) rend.enabled = false;
            var col = GetComponent<Collider>();
            if (col) col.enabled = false;

            // destroy the object after 2 seconds
            Destroy(gameObject, 2f);
        }
    }

    private void Update()
    {
        if (showMessage)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
                showMessage = false;
        }
    }

    // show a label on screen with OnGUI
    private void OnGUI()
    {
        if (showMessage)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 35;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.black;

            // a box that covers the whole screen
            Rect fullScreenRect = new Rect(0, 0, Screen.width, Screen.height);
            GUI.Label(fullScreenRect, "Keycard collected!", style);
        }
    }
}
