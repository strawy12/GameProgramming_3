using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    private float constructionTimer;
    private float constructionTimerMax;

    private BuildingTypeSO buildingType;
    private BoxCollider2D boxCol;
    private SpriteRenderer spriteRenderer;
    private BuildingTypeHolder buildingTypeHolder;

    private Material currentMat;


    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO type)
    {
        Transform pfBuildingConstruction = Resources.Load<Transform>("pfBuildingConstruction");
        Transform buildingConstructionTransform = Instantiate(pfBuildingConstruction, position, Quaternion.identity);

        BuildingConstruction buildingConstruction = buildingConstructionTransform.GetComponent<BuildingConstruction>();
        buildingConstruction.SetBuildingType(type);

        return buildingConstruction;
    }

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
        buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();
        currentMat = spriteRenderer.material;
    }


    private void Update()
    {
        constructionTimer -= Time.deltaTime;

        currentMat.SetFloat("_Progress", GetConstructionTimerNormalized());

        if (constructionTimer <= 0f)
        {
            Debug.Log("Ding!");
            Instantiate(buildingType.prefab, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
            Destroy(gameObject);
        }
    }

    private void SetBuildingType(BuildingTypeSO type)
    {
        buildingType = type;

        constructionTimerMax = type.constructionTimerMax;
        constructionTimer = constructionTimerMax;
        Debug.Log(constructionTimerMax);
        boxCol.offset = buildingType.prefab.GetComponent<BoxCollider2D>().offset;
        boxCol.size = buildingType.prefab.GetComponent<BoxCollider2D>().size;

        spriteRenderer.sprite = buildingType.sprite;
        buildingTypeHolder.buildingType = buildingType;

    }

    public float GetConstructionTimerNormalized()
    {
        return 1f - constructionTimer / constructionTimerMax;
    }
}
