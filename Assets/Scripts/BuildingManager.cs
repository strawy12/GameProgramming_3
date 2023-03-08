using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BuildingManager : MonoBehaviour
{
    private BuildingTypeListSO buildingTypeList;

    private BuildingTypeSO buildingType;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        buildingType = buildingTypeList.list[0];
    }


    private void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            CreateBuiling();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            buildingType = buildingTypeList.list[0];
        }

        else if (Input.GetKeyDown(KeyCode.W))
        {
            buildingType = buildingTypeList.list[1];
        }

        else if (Input.GetKeyDown(KeyCode.E))
        {
            buildingType = buildingTypeList.list[2];
        }
    }

    private void CreateBuiling()
    {
        GameObject building = Instantiate(buildingType.prefab);
        building.transform.position = GetMouseWorldPosition();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        return mousePos;
    }
}
