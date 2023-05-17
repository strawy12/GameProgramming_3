using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyWaveUI : MonoBehaviour
{

    float commonPosy = -300f;
    [SerializeField]
    private EnemyWaveManager enemyWaveManager;

    private RectTransform enemyWaveSpawnPositionIndicator;
    private RectTransform enemyClosestPositionIndicator;

    private TMP_Text waveNumberText;
    private TMP_Text waveMessageText;

    private Camera mainCam;

    private Enemy targetEnemy;

    private void Awake()
    {
        mainCam = Camera.main;
        waveNumberText = transform.Find("waveNumberText").GetComponent<TMP_Text>();
        waveMessageText = transform.Find("waveMessageText").GetComponent<TMP_Text>();
        enemyWaveSpawnPositionIndicator = transform.Find("enemyWaveSpawnPositionIndicator").GetComponent<RectTransform>();
        enemyClosestPositionIndicator = transform.Find("enemyClosestPositionIndicator").GetComponent<RectTransform>();
    }

    private void Start()
    {
        enemyWaveManager.OnWaveNumberChanged += EnemyWaveManager_OnWaveNumberChanged;
    }

    private void EnemyWaveManager_OnWaveNumberChanged(object sender, EventArgs e)
    {
        SetWaveNumberText("Wave" + enemyWaveManager.WaveNumber);
    }

    private void Update()
    {
        HandleNextWaveMessage();
        HandleEnemyWaveSpawnPositionIndicator();
        HandleEnemyClosestPositionIndicator();

    }

    private void HandleNextWaveMessage()
    {

        float nextTimer = enemyWaveManager.NextWaveSpawnTimer;
        if (nextTimer <= 0f)
        {
            SetWaveMessageText("");
        }
        else
        {
            SetWaveMessageText("Next Wave in " + nextTimer.ToString("F1") + "s");
        }
    }

    private void HandleEnemyClosestPositionIndicator()
    {
        float targetMaxRadius = 20f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) > Vector3.Distance(transform.position, enemy.transform.position))
                    {
                        targetEnemy = enemy;
                    }
                }
            }
        }

        if (targetEnemy != null)
        {
            Vector3 dirToClosetEnemy = (enemyWaveManager.GetSpawnPosition() - mainCam.transform.position).normalized;

            enemyClosestPositionIndicator.anchoredPosition = dirToClosetEnemy * 250f;
            enemyClosestPositionIndicator.eulerAngles = new Vector3(0, 0, UtilClass.GetAngleFromVector(dirToClosetEnemy));

            float distToClosetEnemy = Vector3.Distance(enemyWaveManager.GetSpawnPosition(), mainCam.transform.position);
            enemyClosestPositionIndicator.gameObject.SetActive(distToClosetEnemy > mainCam.orthographicSize * 1.5f);
        }

        else
        {
            enemyClosestPositionIndicator.gameObject.SetActive(false);
        }
    }

    private void HandleEnemyWaveSpawnPositionIndicator()
    {
        Vector3 dirToNextSpawnPos = (enemyWaveManager.GetSpawnPosition() - mainCam.transform.position).normalized;

        enemyWaveSpawnPositionIndicator.anchoredPosition = dirToNextSpawnPos * 300f;
        enemyWaveSpawnPositionIndicator.eulerAngles = new Vector3(0, 0, UtilClass.GetAngleFromVector(dirToNextSpawnPos));

        float distToNextSpawnPos = Vector3.Distance(enemyWaveManager.GetSpawnPosition(), mainCam.transform.position);
        enemyWaveSpawnPositionIndicator.gameObject.SetActive(distToNextSpawnPos > mainCam.orthographicSize * 1.5f);
    }

    private void SetWaveNumberText(string text)
    {
        waveNumberText.text = text;
    }

    private void SetWaveMessageText(string text)
    {
        waveMessageText.text = text;
    }
}
