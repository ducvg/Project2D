using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Canvas GameOverScreen;

    public UnityEvent levelUpEvent;
    public UnityEvent killCountEvent;
    public UnityEvent<int> UIkillCountEvent;
    public UnityEvent GameOverEvent;
    public UnityEvent UIGameOverEvent;


    public int killCount { get; set; }
    public int totalKill { get; set; }
    public float playTime { get; set; }
    public int damageDealt { get; set; }
    public int damageReceived { get; set; }
    public int level 
    { 
        get => level;
        set
        {
            level=value;
        } 
    }

    private static GameManager instance;
    public static GameManager Instance
    {
        get => instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        if(killCountEvent == null) killCountEvent = new();
        if(levelUpEvent == null) killCount = new();
        if(GameOverEvent == null) GameOverEvent = new();

    }
    // Start is called before the first frame update
    void Start()
    {
        GameOverScreen.enabled = false;
        totalKill = DataManager.DataKill;
        killCountEvent.AddListener(CountKill);
        UIkillCountEvent?.Invoke(killCount);
        levelUpEvent.AddListener(LevelUp);
        GameOverEvent.AddListener(EndGame);

        playTime = 0;
        damageDealt = 0;
        damageReceived = 0;
        level = 1;
        killCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EndGame()
    {
        playTime = DataManager.DataTime;
        if(playTime < Time.time)
        {
            playTime = Time.time;
            DataManager.DataTime = Time.time;
        }
        UIGameOverEvent.Invoke();
        GameOverScreen.enabled = true;
        Debug.Log("Game over. !");
    }

    void LevelUp()
    {
        Debug.Log("levelup");
    }

    void CountKill()
    {
        totalKill++;
        killCount++;
        DataManager.DataKill = totalKill;
        UIkillCountEvent?.Invoke(killCount);
    }
}
