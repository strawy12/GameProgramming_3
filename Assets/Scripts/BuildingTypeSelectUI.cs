using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField] private Sprite sprite;
    private Transform arrowBtn;

    private BuildingTypeListSO buildingTypeList;
    private Dictionary<BuildingTypeSO, Transform> buildingTransformDic = new();

    void Awake()
    {
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        Transform btnTemplate = transform.Find("ButtonTemplete");
        btnTemplate.gameObject.SetActive(false);

        int index = 0;

        arrowBtn = Instantiate(btnTemplate, transform);
        arrowBtn.gameObject.SetActive(true);

        float offsetAmount = 130f;
        arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);
        arrowBtn.Find("Image").GetComponent<Image>().sprite = sprite;
        arrowBtn.Find("Image").GetComponent<RectTransform>().sizeDelta += new Vector2(0, -30);

        arrowBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildingManager.Inst.SetActiveBuildingType(null);
            UpdateActiveBuildingTypeBtn();
        });

        index++;

        foreach (var buildingType in buildingTypeList.list)
        {
            Transform btnTransform = Instantiate(btnTemplate, transform);
            buildingTransformDic[buildingType] = btnTransform;
            btnTransform.name = $"{buildingType.nameString} Btn";

             offsetAmount = 160f;
            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);
            btnTransform.gameObject.SetActive(true);

            btnTransform.Find("Image").GetComponent<Image>().sprite = buildingType.profile;
            btnTransform.Find("Selected").gameObject.SetActive(false);

            btnTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildingManager.Inst.SetActiveBuildingType(buildingType);
                UpdateActiveBuildingTypeBtn();
            });

            index++;
        }
    }

    private void UpdateActiveBuildingTypeBtn()
    {
        arrowBtn.Find("Selected").gameObject.SetActive(false);

        foreach (var buildingType in buildingTransformDic.Keys)
        {
            Transform btnTransform = buildingTransformDic[buildingType];
            btnTransform.Find("Selected").gameObject.SetActive(false);
        }
        BuildingTypeSO activeBuildingType = BuildingManager.Inst.GetActiveBuildingType();
       
        if (activeBuildingType == null)
        {
            arrowBtn.Find("Selected").gameObject.SetActive(true);
        }

        else
        {
            Transform activeBtnTrm = buildingTransformDic[activeBuildingType];
            activeBtnTrm.Find("Selected").gameObject.SetActive(true);
        }
    }
}
