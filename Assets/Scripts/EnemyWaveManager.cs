using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance { get; private set; }

    public event EventHandler OnWaveNumberChanged;
    private float nextWaveSpawnTimer;

    private float nextEnemySpawnTimer;
    private int remainingEnemySpawnAmount;

    private int waveNum = 0;

    public int WaveNumber => waveNum;
    public float NextWaveSpawnTimer => nextWaveSpawnTimer;

    [SerializeField]
    private List<Transform> spawnPointList;

    [SerializeField]
    private Transform nextWaveSpawnPosTrm;

    private Vector3 spawnPos;

    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave
    }

    private State currentState;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentState = State.WaitingToSpawnNextWave;
        SetSpawnPos();
        nextWaveSpawnTimer = 3f;
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPos;
    }

    private void SetSpawnPos()
    {
        spawnPos = spawnPointList[UnityEngine.Random.Range(0, spawnPointList.Count)].position;
        nextWaveSpawnPosTrm.position = spawnPos;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer < 0f)
                {
                    SpawnWave();
                }
                break;

            case State.SpawningWave:
                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;

                    if (nextEnemySpawnTimer <= 0f)
                    {
                        nextEnemySpawnTimer = Random.Range(0f, 2f);
                        Enemy.Create(spawnPos + UtilClass.GetRandomDir() * Random.Range(0f, 10f));
                        remainingEnemySpawnAmount--;

                        if (remainingEnemySpawnAmount <= 0)
                        {
                            currentState = State.WaitingToSpawnNextWave;
                            SetSpawnPos();
                            nextWaveSpawnTimer = 3f;
                        }
                    }
                }
                break;
        }
    }

    void SpawnWave()
    {
        currentState = State.SpawningWave;
        remainingEnemySpawnAmount = 5 + 3 * waveNum;
        waveNum++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }
}
