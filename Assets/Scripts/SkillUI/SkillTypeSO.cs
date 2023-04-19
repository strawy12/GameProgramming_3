using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SkillType")]
public class SkillTypeSO : ScriptableObject
{
    public string nameString;
    public Transform prefab;

    public Sprite sprite;
    public ResourceAmount[] constructionCostArray;
    public bool isnotUseCost;
    public bool isUseWorld;

    public float constructionTimerMax;

    public string GetConstructionCostString()
    {
        string str = "";
        if (isnotUseCost) str = "None";
        else
        {
            foreach (ResourceAmount resourceAmount in constructionCostArray)
            {
                str += "<color=#" + resourceAmount.resourceType.colorHex + ">" +
                    resourceAmount.resourceType.nameShort + resourceAmount.amount
                    + "</color>";
            }
        }

        return str;
    }
}
