using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarForceUI : MonoBehaviour
{
    public static StarForceUI Inst;
    private BuildingTypeSO currentBuilding;

    private Dictionary<string, int> buildingLevelList = new Dictionary<string, int>();
    [SerializeField]
    private Image buildingImage;

    private readonly Vector2 MAXSIZE = new Vector2(1173.333f, 660f);

    private const float RATIO = 1.636363636363636f;

    private void Awake()
    {
        Inst = this;
        buildingLevelList = new Dictionary<string, int>();

    }

    private void Start()
    {
        foreach(var buildingType in BuildingManager.Instance.BuildingTypeList.list)
        {
            buildingLevelList.Add(buildingType.nameString, 1);
            
        }
    }

    public void Open(BuildingTypeSO buildingType)
    {
        currentBuilding = buildingType;
        SetSprite
    }

    public void SetSprite(Sprite sprite)
    {
        buildingImage.sprite = sprite;

        Vector2 size = sprite.rect.size;
        Vector2 originSize = size;

        size.x /= RATIO;
        size.y /= RATIO;

        buildingImage.rectTransform.sizeDelta = size;
    }
}
