using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerprefs : MonoBehaviour
{
    public enum PlayerKeys
    {
        TOKEN,
        NAME,
        DAY_MONTH,
        YEAR
    }
    public static void setplayerprefs(PlayerKeys key ,string value)
    {
        PlayerPrefs.SetString(key.ToString(),value);
        PlayerPrefs.Save();
    }

    public static string getplayerprefs(PlayerKeys key)
    {
        return PlayerPrefs.GetString(key.ToString());
    }
}
