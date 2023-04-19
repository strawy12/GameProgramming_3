using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SkillTypeSelectUI : MonoBehaviour
{
    [SerializeField] private Sprite sprite;

    private SkillTypeListSO SkillTypeList;
    private Dictionary<SkillTypeSO, Transform> btnTransformDic;
    Transform arrowBtn;

    private void Awake()
    {
        SkillTypeList = Resources.Load<SkillTypeListSO>(typeof(SkillTypeListSO).Name);
        btnTransformDic = new Dictionary<SkillTypeSO, Transform>();

        Transform btnTemplate = transform.Find("btnTemplate");
        btnTemplate.gameObject.SetActive(false);

        int index = 0;
        float offsetAmount = +130f;
        MouseEnterExitEvents mouseEnterExitEvents;

        foreach (SkillTypeSO SkillType in SkillTypeList.list)
        {
            Transform btnTransform = Instantiate(btnTemplate, transform);
            btnTransform.gameObject.SetActive(true);

            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

            btnTransform.Find("image").GetComponent<Image>().sprite = SkillType.sprite;
            btnTransform.Find("image").GetComponent<RectTransform>().sizeDelta -= Vector2.down * 30f;


            btnTransform.GetComponent<Button>().onClick.AddListener(() => {
                SkillManager.Instance.SetActiveSkillType(SkillType);
            });

            mouseEnterExitEvents = btnTransform.GetComponent<MouseEnterExitEvents>();
            mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) =>
            {
                TooltipUI.Instance.Show(SkillType.nameString + "\n" + SkillType.GetConstructionCostString());
            };

            mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) =>
            {
                TooltipUI.Instance.Hide();
            };


            btnTransformDic[SkillType] = btnTransform;
            index++;
        }
        btnTransformDic.TryGetValue(SkillTypeList.list[0], out arrowBtn);
        arrowBtn.Find("selected").gameObject.SetActive(true);
    }

    private void Start()
    {
        UpdateActiveSkillTypeBtn();
        SkillManager.Instance.onActiveSkillTypeChanged += SkillManager_onActiveSkillTypeChanged;
    }

    private void SkillManager_onActiveSkillTypeChanged(object sender, SkillManager.onActiveSkillTypeEventArgs e)
    {
        UpdateActiveSkillTypeBtn();
    }

    private void UpdateActiveSkillTypeBtn()
    {
        arrowBtn.Find("selected").gameObject.SetActive(false);
        foreach (SkillTypeSO SkillType in btnTransformDic.Keys)
        {
            Transform btnTransform = btnTransformDic[SkillType];
            btnTransform.Find("selected").gameObject.SetActive(false);
        }

        SkillTypeSO activeSkillType = SkillManager.Instance.GetActiveSkillType();


        if (activeSkillType == null)
        {
            arrowBtn.Find("selected").gameObject.SetActive(true);
        }
        else
        {
            Transform activeBtnTransform = btnTransformDic[activeSkillType];
            activeBtnTransform.Find("selected").gameObject.SetActive(true);
        }
    }
}
