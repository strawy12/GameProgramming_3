using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SkillTypeList")]
public class SkillTypeListSO : ScriptableObject
{
    public List<SkillTypeSO> list;
}
