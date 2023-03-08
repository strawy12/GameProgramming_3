using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mono.Cecil;
using Unity.VisualScripting;

public class ResourceUI : MonoBehaviour
{
    ResourceTypeListSO resourceTypeList;
    private Dictionary<ResourceTypeSO, Transform> resourceTrmDict;
    private void Awake()
    {
        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        resourceTrmDict = new Dictionary<ResourceTypeSO, Transform>();

        Transform resourceTemplete = transform.Find("Resource Templete");
        resourceTemplete.gameObject.SetActive(false);

        int index = 0;

        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            Transform resourceTrm = Instantiate(resourceTemplete, transform);

            float offsetAmount = -160f;
            (resourceTrm.transform as RectTransform).anchoredPosition = new Vector2(offsetAmount * index, 0);
            resourceTrm.gameObject.SetActive(true);

            resourceTrm.Find("Image").GetComponent<Image>().sprite = resourceType.sprite;

            resourceTrmDict[resourceType] = resourceTrm;

            index++;
        }
    }

    private void Start()
    {
        ResourceManager.Inst.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;
    }

    private void ResourceManager_OnResourceAmountChanged(object sender, System.EventArgs eventArgs)
    {
        UpdateResourceAmount();
    }
    private void UpdateResourceAmount()
    {
        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            Transform resourceTrm = resourceTrmDict[resourceType];
            int resourceAmount = ResourceManager.Inst.GetResourceAmount(resourceType);
            resourceTrm.Find("Text").GetComponent<TMP_Text>().text = resourceAmount.ToString();
        }
           


    }
}
