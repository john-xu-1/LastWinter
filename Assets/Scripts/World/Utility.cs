using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{


    public static string FormatTime(float seconds)
    {
        string timeString = "";
        int hours = (int)(seconds / 3600);
        int minutes = (int)((seconds - hours * 3600) / 60);
        seconds = (int)(seconds - hours * 3600 - minutes * 60);

        if (hours > 0) timeString += hours + ":";
        if (minutes > 0 || hours > 0) timeString += minutes + ":";
        timeString += (int)seconds;
        return timeString;
    }
    public static string FormatTime(double seconds)
    {
        string timeString = "";
        int hours = (int)(seconds / 3600);
        int minutes = (int)((seconds - hours * 3600) / 60);
        seconds = (int)(seconds - hours * 3600 - minutes * 60);

        if (hours > 0) timeString += hours + ":";
        if (minutes > 0 || hours > 0) timeString += minutes + ":";
        timeString += (int)seconds;
        return timeString;
    }

    public static void SaveJasonToPlayerPrefs (string json, string key)
    {
        PlayerPrefs.SetString(key, json);
    }

    public static void SaveJasonToPlayerPrefs<T>(T obj, string key)
    {
        string json = JsonUtility.ToJson(obj);
        PlayerPrefs.SetString(key, json);
    }

    public static string LoadFromPlayerPrefs(string key)
    {
        if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetString(key);
        else return null;
    }

    public static T LoadFromPlayerPrefs<T>(string key)
    {
        if (PlayerPrefs.HasKey(key)) return JsonUtility.FromJson<T>( PlayerPrefs.GetString(key));
        else return default;
    }
}
