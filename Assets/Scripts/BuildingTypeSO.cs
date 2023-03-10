using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string nameString;
    public GameObject prefab;
    public Sprite profile;
    public ResourceGeneratorData resourceGeneratorData;
}
