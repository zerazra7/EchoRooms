using UnityEngine;

// Keeps track of how long the current run takes and stores the best escape time.
// A run starts when Level 0 is entered fresh (new game or restart)
// and keeps going through Level 1 until the player escapes.
public static class RunTimer
{
    const string BestKey = "BestEscapeTime";
    const string LastKey = "LastEscapeTime";
    const string LastWasRecordKey = "LastEscapeWasRecord";

    static float startTime;
    static bool running;

    public static void Begin()
    {
        startTime = Time.time; // game time, so paused moments (like reading notes) don't count
        running = true;
    }

    // Call when the player escapes. Saves the time and updates the record.
    public static void Finish()
    {
        if (!running) return; // e.g. the run started from "Load Game" halfway through
        running = false;

        float time = Time.time - startTime;
        float best = PlayerPrefs.GetFloat(BestKey, 0f);
        bool isRecord = best <= 0f || time < best;

        PlayerPrefs.SetFloat(LastKey, time);
        PlayerPrefs.SetInt(LastWasRecordKey, isRecord ? 1 : 0);
        if (isRecord) PlayerPrefs.SetFloat(BestKey, time);
        PlayerPrefs.Save();
    }

    public static float Best => PlayerPrefs.GetFloat(BestKey, 0f);
    public static float Last => PlayerPrefs.GetFloat(LastKey, 0f);
    public static bool LastWasRecord => PlayerPrefs.GetInt(LastWasRecordKey, 0) == 1;

    // Clears the "last run" info so it is only shown once on the menu
    public static void ClearLast()
    {
        PlayerPrefs.DeleteKey(LastKey);
        PlayerPrefs.DeleteKey(LastWasRecordKey);
        PlayerPrefs.Save();
    }

    public static string Format(float seconds)
    {
        int total = Mathf.FloorToInt(seconds);
        return $"{total / 60}:{total % 60:00}";
    }
}
