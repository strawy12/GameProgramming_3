using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    private BuildingTypeListSO buildingTypeList;
    public event EventHandler OnActiveBuildingTypeChanged;

    private BuildingTypeSO activeBuildingType = null;

    private Camera mainCam;
    public event EventHandler OnChangeBuildType;
    public static BuildingManager Inst;
    private void Awake()
    {
        Inst = this;
    }
    private void Start()
    {
        mainCam = Camera.main;
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
    }


    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (activeBuildingType != null)
            {
                CreateBuiling();
            }
        }


    }

    private void CreateBuiling()
    {
        GameObject building = Instantiate(activeBuildingType.prefab);
        building.transform.position = UtilClass.GetMouseWorldPosition();
    }

  

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke(this, EventArgs.Empty);
    }

    public BuildingTypeSO GetActiveBuildingType() => activeBuildingType;
}
