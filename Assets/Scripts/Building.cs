using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Building : MonoBehaviour
{
    private HealthSystem healthSystem;
    private BuildingTypeSO buildingType;

    private Transform buildingDemolishBtn;
    private Transform buildingRepairBtn;

    private void Awake()
    {
        buildingDemolishBtn = transform.Find("pfBuildingDemolishBtn");
        buildingRepairBtn = transform.Find("pfBuildingRepairBtn");
        HideBuildingDemolishBtn();
        HideBuildingRepairBtn();
    }

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;

        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);
        healthSystem.OnDied += HealthSystem_OnDied;

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
    }

    private void HealthSystem_OnHealed(object sender, EventArgs e)
    {
        if(healthSystem.IsFullHealthAmount())
        {
            HideBuildingRepairBtn();
        }
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
        ShowBuildingRepairBtn();
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
        Destroy(gameObject);
    }

    private void OnMouseEnter()
    {
        ShowBuildingDemolishBtn();

    }

    private void OnMouseExit()
    {
        HideBuildingDemolishBtn();
    }

    private void ShowBuildingDemolishBtn()
    {
        if (buildingDemolishBtn != null)
            buildingDemolishBtn.gameObject.SetActive(true);
    }

    private void HideBuildingDemolishBtn()
    {
        if (buildingDemolishBtn != null)
            buildingDemolishBtn.gameObject.SetActive(false);
    }

    private void ShowBuildingRepairBtn()
    {
        if (buildingRepairBtn != null)
            buildingRepairBtn.gameObject.SetActive(true);
    }

    private void HideBuildingRepairBtn()
    {
        if (buildingRepairBtn != null)
            buildingRepairBtn.gameObject.SetActive(false);
    }
}

