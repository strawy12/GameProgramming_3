using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionTimerUI : MonoBehaviour
{
    [SerializeField] private BuildingConstruction buildingConstruction;

    private Image constructionProgressImage;

    private void Awake()
    {
        constructionProgressImage = transform.Find("mask/image").GetComponent<Image>();
        Debug.Log(constructionProgressImage.name);
    }

    private void Update()
    {
        constructionProgressImage.fillAmount = buildingConstruction.GetConstructionTimerNormalized();
    }
}
