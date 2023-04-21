using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ResourceGenerator : MonoBehaviour
{

    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position)
    {
        int nearbyResourceAmount = 0;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();
            if (resourceNode != null)
            {
                if (resourceNode.resourceType == resourceGeneratorData.resourceType)
                {
                    nearbyResourceAmount++;
                }

            }
        }
        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);

        return nearbyResourceAmount;
    }


    private float timer;
    private float timerMax;
    private ResourceGeneratorData resourceGeneratorData;


    private void Awake()
    {
        BuildingTypeSO type = GetComponent<BuildingTypeHolder>().buildingType;
        resourceGeneratorData = type.resourceGeneratorData;
        timerMax = resourceGeneratorData.timerMax;

        type.OnChangeResourceGenerateTime += ChangeTimeMax;
    }

    private void ChangeTimeMax()
    {
        int nearbyResourceAmount = GetNearbyResourceAmount(resourceGeneratorData, transform.position);
        if (nearbyResourceAmount == 0) return;

        timerMax = (resourceGeneratorData.timerMax / 2) +
                resourceGeneratorData.timerMax *
                (1 - (float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount);

        timer = 0f;
    }
    private void Start()
    {

        int nearbyResourceAmount = GetNearbyResourceAmount(resourceGeneratorData, transform.position);
        if (nearbyResourceAmount == 0)
        {
            transform.Find("pfResourceGeneratorOverlay").gameObject.SetActive(false);
            transform.GetComponentInChildren<Animator>().enabled = false;
            enabled = false;
        }
        else
        {
            timerMax = (resourceGeneratorData.timerMax / 2) +
        resourceGeneratorData.timerMax *
        (1 - (float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount);
        }

    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = timerMax;
            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, 1);
        }
    }

    public ResourceGeneratorData GetResourceGeneratorData()
    {
        return resourceGeneratorData;
    }

    public float GetTimerNormalized()
    {
        return timer / timerMax;
    }

    public float GetAmountGeneratedPerSecond()
    {
        return 1 / timerMax;
    }
}
