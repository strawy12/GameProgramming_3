using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/ResourceTypeList")]
public class ResourceTypeListSO : ScriptableObject
{
    public List<ResourceTypeSO> list;
}
