using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static int DataKill
    {
        get => PlayerPrefs.GetInt("KillCount", 0);
        set => PlayerPrefs.SetInt("KillCount", value);
    }

    public static float DataTime
    {
        get => PlayerPrefs.GetFloat("BestTime", 0);
        set => PlayerPrefs.SetFloat("BestTime", value);
    }
}
