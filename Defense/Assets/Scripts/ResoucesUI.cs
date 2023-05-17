using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ResoucesUI : MonoBehaviour
{
    private ResourceTypeListSO resourceTypeList;
    [SerializeField]
    [Tooltip("Wood Stone Gold 순으로 두기")]
    private List<Transform> resourceTransformList;
    private Dictionary<ResourceTypeSO, Transform> resourceTransformDictionary;

    private void Awake()
    {
        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        resourceTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();

        for(int i = 0; i < 3; i++)
        {
            ResourceTypeSO type = resourceTypeList.list[i];
            Transform trm = resourceTransformList[i];

            resourceTransformDictionary.Add(type, trm);
        }
    }
    private void Start()
    {
        ResourceManager.Instance.onResourceAmountChanged += ResourceManager_onResourceAmountChanged;
    }

    private void ResourceManager_onResourceAmountChanged(object sender, System.EventArgs e)
    {
        UpdateResouceAmount();
    }

    private void UpdateResouceAmount()
    {
        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            Transform resourceTransform = resourceTransformDictionary[resourceType];
            int resourceAmount = ResourceManager.Instance.GetResouceAmount(resourceType);
            resourceTransform.Find("text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
        }
    }
}
