using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaveSpawner2 : MonoBehaviour
{
    public Transform chaseTarget;
    public Transform[] spawnPoints;
    public Wave[] waves;
    public Tilemap checkTilemap;

    public float waveTime = 5f;
    private float waveCountdown;
    private int currentWave = 0;
    private SpawnState state = SpawnState.COUNTDOWN;
    private float searchCountdown = 1f;
    private int spawnBorder = 50;

    private static WaveSpawner2 instance;
    public static WaveSpawner2 Instance
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
    }

    // Start is called before the first frame update
    void Start()
    {
        waveCountdown = waves[currentWave].waveDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!IsEnemyAlive())
            {
                WaveComplete();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[currentWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    private void WaveComplete()
    {
        Debug.Log("Wave completed!");
        state = SpawnState.COUNTDOWN;
        waveCountdown = waves[currentWave].waveDelay;
        if (currentWave + 1 > waves.Length - 1)
        {
            currentWave = 0;
            waves[currentWave].count += 3;
            Debug.Log("hết tất cả wave, lặp lại ...");
        }
        else
        {
            currentWave++;
        }
    }

    bool IsEnemyAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    void SpawnEnemy(Transform enemy)
    {
        var spawnPoint = spawnPoints[0].position;
        do
        {
            int side = UnityEngine.Random.Range(0, 4);
            int x = (int)UnityEngine.Random.Range(spawnPoints[0].position.x, spawnPoints[1].position.x);
            int y = (int)UnityEngine.Random.Range(spawnPoints[0].position.y, spawnPoints[1].position.y);
            switch (side)
            {
                case 0:
                    spawnPoint = new Vector3(x, Mathf.RoundToInt(spawnPoints[1].position.y));
                    break;
                case 1:
                    spawnPoint = new Vector3(Mathf.RoundToInt(spawnPoints[1].position.x), y);
                    break;
                case 2:
                    spawnPoint = new Vector3(x, Mathf.RoundToInt(spawnPoints[0].position.y));
                    break;
                case 3:
                    spawnPoint = new Vector3(Mathf.RoundToInt(spawnPoints[0].position.x), y);
                    break;
            }
        } while (checkTilemap.HasTile(checkTilemap.WorldToCell(spawnPoint)) ||
                spawnPoint.x > spawnBorder || spawnPoint.x < -spawnBorder ||
                spawnPoint.y > spawnBorder || spawnPoint.y < -spawnBorder);

        var enemyPref = Instantiate(enemy, spawnPoint, Quaternion.identity);
        enemyPref.GetComponent<AIDestinationSetter>().target = chaseTarget;
        enemyPref.GetComponentInChildren<EnemyAim>().playerPosition = chaseTarget;
        Debug.Log("Wave: " + currentWave + " Spawning Enemy: " + enemy.name);
    }

    IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("Spawning wave: " + wave.name);
        state = SpawnState.SPAWNING;
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(wave.spawnDelay);
        }
        state = SpawnState.WAITING;
        yield break;
    }
}

[Serializable]
public class Wave
{
    public string name;
    public Transform enemy;
    public int count;
    public float spawnDelay; //delay
    public float waveDelay;
}

public enum SpawnState { SPAWNING, WAITING, COUNTDOWN }

