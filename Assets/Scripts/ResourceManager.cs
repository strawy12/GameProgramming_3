using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;

    public static ResourceManager Inst { get; private set; }
    private void Awake()
    {
        Inst = this;

        resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            resourceAmountDictionary[resourceType] = 0;
        }

        TestLogResourceAmountDictionary();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
            AddResource(resourceTypeList.list[0], 1);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
            AddResource(resourceTypeList.list[1], 1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
            AddResource(resourceTypeList.list[2], 1);
        }
    }

    private void TestLogResourceAmountDictionary()
    {
       foreach(ResourceTypeSO resourceType in resourceAmountDictionary.Keys)
        {
            Debug.Log(resourceType.nameString + " : " + resourceAmountDictionary[resourceType]);
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        resourceAmountDictionary[resourceType] += amount;
        TestLogResourceAmountDictionary();
    }
}
