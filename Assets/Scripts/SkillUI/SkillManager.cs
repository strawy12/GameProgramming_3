using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }
    SkillTypeSO activeSkillType;
    public event EventHandler<onActiveSkillTypeEventArgs> onActiveSkillTypeChanged;

    public class onActiveSkillTypeEventArgs : EventArgs
    {
        public SkillTypeSO Type;
    }

    void Awake()
    {
        Instance = this;
    }

    public SkillTypeSO GetActiveSkillType()
    {
        return activeSkillType;
    }
    public void SetActiveSkillType(SkillTypeSO type)
    {
        activeSkillType = type;
        onActiveSkillTypeChanged?.Invoke(this, new onActiveSkillTypeEventArgs { Type = type });
    }

}
