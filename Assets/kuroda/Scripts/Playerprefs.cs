using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerprefs : MonoBehaviour
{
    public enum PlayerKeys
    {
        TOKEN,
        SCORE,
    }
    public static void SetString(PlayerKeys key ,string value)
    {
        PlayerPrefs.SetString(key.ToString(),value);
        PlayerPrefs.Save();
    }

    public static string GetString(PlayerKeys key)
    {
        return PlayerPrefs.GetString(key.ToString());
    }

    public static void SetInt(PlayerKeys key, int value)
    {
        PlayerPrefs.SetInt(key.ToString(), value);
        PlayerPrefs.Save();
    }

    public static int GetInt(PlayerKeys key)
    {
        return PlayerPrefs.GetInt(key.ToString());
    }
}
