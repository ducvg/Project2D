using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Text killCount;
    public TMP_Text totalKill;
    public TMP_Text roundKill;
    public TMP_Text damageDealt;
    public TMP_Text damageReceived;
    public TMP_Text bestTime;
    public TMP_Text roundTime;
    public TMP_Text Timer;

    private float playTime;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.UIkillCountEvent.AddListener(UpdateKillCount);
        GameManager.Instance.UIGameOverEvent.AddListener(GameOverStatistic);
    }

    private void FixedUpdate()
    {
        UpdatePlayTime();

    }

    private void UpdateKillCount(int count)
    {
        killCount.text = count.ToString();
    }
    
    private void UpdatePlayTime()
    {
        playTime += Time.deltaTime;
        int minute = Mathf.FloorToInt(playTime / 60);
        int second = Mathf.FloorToInt(playTime % 60);
        Timer.text = String.Format("{0:00} : {1:00}",minute,second);
    }

    private void GameOverStatistic()
    {
        totalKill.text = "Totak kill:\n"+GameManager.Instance.totalKill;
        roundKill.text = "Kill this round:\n" + GameManager.Instance.killCount;
        damageDealt.text = "Damage Dealth:\n" + GameManager.Instance.damageDealt;
        damageReceived.text = "Damage Received:\n"+ GameManager.Instance.damageReceived;
        int minute = Mathf.FloorToInt(GameManager.Instance.playTime / 60);
        int second = Mathf.FloorToInt(GameManager.Instance.playTime % 60);
        bestTime.text = String.Format("Longest Time:\n {0:00} : {1:00}", minute, second);
        minute = Mathf.FloorToInt(playTime / 60);
        second = Mathf.FloorToInt(playTime % 60);
        roundTime.text = String.Format("Survive Time:\n {0:00} : {1:00}", minute, second);
    }
}
