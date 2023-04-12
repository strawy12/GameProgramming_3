using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairBtn : MonoBehaviour
{
    [SerializeField]
    private HealthSystem healthSystem;

    [SerializeField]
    private ResourceTypeSO goldResouceType;

    public void Awake()
    {
        transform.Find("button").GetComponent<Button>().onClick.AddListener(() =>
        {
            int missingHealth = healthSystem.GetHealthAmountMax() - healthSystem.GetHealthAmount();
            int repairCost = missingHealth / 2;

            ResourceAmount[] resourceAmountCost = new ResourceAmount[]
            {
                new ResourceAmount {resourceType = goldResouceType, amount = repairCost }
            };

            if(ResourceManager.Instance.CanAfford(resourceAmountCost))
            {
                ResourceManager.Instance.SpendResources(resourceAmountCost);
                healthSystem.HealFull();
            }
            else
            {
                TooltipUI.Instance.Show("���� �����Ͽ� ���� ���� ���Ͽ����ϴ�.", new TooltipUI.Timer { timer = 2f });
            }

            healthSystem.HealFull();
        });
    }
}
