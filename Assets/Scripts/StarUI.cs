using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StarUI : MonoBehaviour
{
    [SerializeField]
    private GameObject[] starList = new GameObject[25];

    [SerializeField]
    private TMP_Text statText;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Open(int level, string statText)
    {
        foreach(var item in starList)
        {
            item.SetActive(false);
        }

        for(int i = 0; i < level; i++)
        {
            starList[i].SetActive(true);
        }

        this.statText.text = statText;
    }
}
