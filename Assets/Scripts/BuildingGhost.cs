using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BuildingGhost : MonoBehaviour
{
    private GameObject spriteGameObject;

    private void Awake()
    {
        spriteGameObject = transform.Find("Sprite").gameObject;
        Hide();
    }
    private void Start()
    {
        BuildingManager.Inst.OnActiveBuildingTypeChanged += OnActiveBuildingTypeChanged;
    }

    private void Update()
    {
        transform.position = UtilClass.GetMouseWorldPosition();
    }

    private void Hide()
    {
        spriteGameObject.SetActive(false);
    }

    private void Show(Sprite ghostSprite)
    {
        spriteGameObject.SetActive(true);
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }
    private void OnActiveBuildingTypeChanged(object sender, EventArgs e)
    {
        BuildingTypeSO activeBuildingType = BuildingManager.Inst.GetActiveBuildingType();

        if(activeBuildingType == null )

        {
            Hide();
        }
        else
        {
            Show(activeBuildingType.profile);
        }
    }
}
