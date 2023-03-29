using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyWaveUI : MonoBehaviour
{
    [SerializeField]
    private EnemyWaveManager enemyWaveManager;

    private TMP_Text waveNumberText;
    private TMP_Text waveMessageText;

    private void Awake()
    {
        waveNumberText = transform.Find("waveNumberText").GetComponent<TMP_Text>();
        waveMessageText = transform.Find("waveMessageText").GetComponent<TMP_Text>();
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

    private void SetWaveNumberText(string text)
    {
        waveNumberText.text = text;
    }

    private void SetWaveMessageText(string text)
    {
        waveMessageText.text = text;
    }
}
