using UnityEngine;

// escape timer + best time (saved in PlayerPrefs)
// a run starts when level 0 is opened from the start and ends at the exit door
public static class RunTimer
{
    const string BestKey = "BestEscapeTime";
    const string LastKey = "LastEscapeTime";
    const string LastWasRecordKey = "LastEscapeWasRecord";

    static float startTime;
    static bool running;

    // starts the timer
    // Time.time stops while a note is open, so reading doesn't count
    public static void Begin()
    {
        startTime = Time.time;
        running = true;
    }

    // stops the timer and saves the time, also saves it as best if it's a record
    public static void Finish()
    {
        // game was loaded from a save, so this is not a full run
        if (!running) return;
        running = false;

        float time = Time.time - startTime;
        float best = PlayerPrefs.GetFloat(BestKey, 0f);
        bool isRecord = best <= 0f || time < best;

        PlayerPrefs.SetFloat(LastKey, time);
        PlayerPrefs.SetInt(LastWasRecordKey, isRecord ? 1 : 0);
        if (isRecord) PlayerPrefs.SetFloat(BestKey, time);
        PlayerPrefs.Save();
    }

    // saved times, 0 if there are none yet
    public static float Best => PlayerPrefs.GetFloat(BestKey, 0f);
    public static float Last => PlayerPrefs.GetFloat(LastKey, 0f);
    public static bool LastWasRecord => PlayerPrefs.GetInt(LastWasRecordKey, 0) == 1;

    // the menu shows the last run only once, then deletes it
    public static void ClearLast()
    {
        PlayerPrefs.DeleteKey(LastKey);
        PlayerPrefs.DeleteKey(LastWasRecordKey);
        PlayerPrefs.Save();
    }

    // seconds -> "m:ss"
    public static string Format(float seconds)
    {
        int total = Mathf.FloorToInt(seconds);
        return $"{total / 60}:{total % 60:00}";
    }
}
