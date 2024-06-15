using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform chaseTarget;
    public Transform[] spawnPoints;
    public Wave[] waves;
    public float waveTime = 5f;
    private float waveCountdown;
    private int nextWave = 0;
    private SpawnState state = SpawnState.COUNTDOWN;
    private float searchCountdown = 1f;

    private static WaveSpawner instance;
    public static WaveSpawner Instance
    {
        get => instance;
    }

    private void Awake()
    {
        if(instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        waveCountdown = waveTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == SpawnState.WAITING)
        {
            if(!IsEnemyAlive())
            {
                WaveComplete();
            } else {
                return;
            }
        }
        
        if(waveCountdown <= 0)
        {
            if(state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            } 
        } else {
            waveCountdown -= Time.deltaTime;
        }
    }

    private void WaveComplete()
    {
        Debug.Log("Wave completed!");
        state = SpawnState.COUNTDOWN;
        waveCountdown = waveTime;
        if(nextWave + 1 > waves.Length-1)
        {
            nextWave = 0;
            waves[nextWave].count += 3;
            Debug.Log("hết tất cả wave, lặp lại ...");
        } else
        {
            nextWave++;
        }
    }

    bool IsEnemyAlive()
    {
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if(GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    void SpawnEnemy(Transform enemy)
    {
        Transform spawnLocation = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        var enemyPref = Instantiate(enemy, spawnLocation.position, spawnLocation.rotation);

        enemyPref.GetComponent<AIDestinationSetter>().target = chaseTarget;
        enemyPref.GetComponentInChildren<EnemyAim>().playerPosition = chaseTarget;
        Debug.Log("Spawning Enemy: "+enemy.name);
    }

    IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("Spawning wave: "+wave.name);
        state = SpawnState.SPAWNING;
        for(int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(wave.spawnDelay);
        }
        state = SpawnState.WAITING;
        yield break;
    }
}


