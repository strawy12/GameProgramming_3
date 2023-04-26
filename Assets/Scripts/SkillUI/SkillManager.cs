using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class SkillManager : MonoBehaviour
{
    Camera Cam;
    public SkillTypeListSO SkillTypeList;
    public static SkillManager Instance { get; private set; }
    public SkillTypeSO activeSkillType;
    public event EventHandler<onActiveSkillTypeEventArgs> onActiveSkillTypeChanged;
    public GhostSkill ghostSkill;
    int index = 0;

    public Skill[] skillList;
    public Skill activeSkill;

    public class onActiveSkillTypeEventArgs : EventArgs
    {
        public SkillTypeSO Type;
    }

    void Awake()
    {
        Cam = Camera.main;
        ghostSkill = GameObject.Find("SkillGhost").GetComponent<GhostSkill>();
        SkillTypeList = Resources.Load<SkillTypeListSO>(typeof(SkillTypeListSO).Name);
        activeSkillType = SkillTypeList.list[0];
        skillList = GetComponents<Skill>();
        activeSkill = skillList[0];

        Instance = this;
    }

    private void Update()
    {
        InputKey();
    }

    void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            index = 0;
            SetActiveSkillType(SkillTypeList.list[index]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            index = 1;
            SetActiveSkillType(SkillTypeList.list[index]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            index = 2;
            SetActiveSkillType(SkillTypeList.list[index]);
        }
        if (BuildingManager.Instance.GetActiveBuildingType() != null) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (ResourceManager.Instance.CanAfford(activeSkillType.constructionCostArray) || activeSkillType.isnotUseCost)
            {
                ResourceManager.Instance.SpendResources(activeSkillType.constructionCostArray);
                skillList[index].UseSkill(Cam.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }

    public SkillTypeSO GetActiveSkillType()
    {
        return activeSkillType;
    }
    public void SetActiveSkillType(SkillTypeSO type)
    {
        activeSkillType = type;
        onActiveSkillTypeChanged?.Invoke(this, new onActiveSkillTypeEventArgs { Type = type });
        activeSkill = skillList[index];
        ghostSkill.InitSkill();
    }

}
