using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Building : MonoBehaviour
{
    private HealthSystem healthSystem;
    private BuildingTypeSO buildingType;

    [SerializeField]
    private StarUI starUI;

    public BuildingTypeSO BuildingType => buildingType;

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
        StarForceUI.Inst.AddBuilding(gameObject.name);

        healthSystem = GetComponent<HealthSystem>();
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;

        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);
        buildingType.OnChangeMaxHealth += () => healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, false);
        healthSystem.OnDied += HealthSystem_OnDied;

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;

        buildingType = Instantiate(buildingType);
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
        CinemachineShake.Instance.ShakeCamera(7f, .15f);
        ChromaticAberrationEffect.Instance.SetWeight(1f);

    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Instantiate(GameAssets.Instance.pfBuildingDestroyParticlse, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
        CinemachineShake.Instance.ShakeCamera(10f, .2f);
        ChromaticAberrationEffect.Instance.SetWeight(1f);


        Destroy(gameObject);
    }

    private void OnMouseEnter()
    {
        ShowStar();
        ShowBuildingDemolishBtn();

    }

   

    private void OnMouseExit()
    {
        HideStar();
        HideBuildingDemolishBtn();
    }

    private void ShowStar()
    {
        int level = StarForceUI.Inst.GetLevel(gameObject.name);
        string text = StarForceUI.Inst.GetStatText(gameObject.name, BuildingType);
        starUI.gameObject.SetActive(true);
        starUI.Open(level, text);
    }

    private void HideStar()
    {
        starUI.gameObject.SetActive(false);
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

