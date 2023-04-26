using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSkill : MonoBehaviour
{
    private SpriteRenderer sRenderer;
    private SkillTypeSO ActiveSkill;
    Camera cam;

    private void Start()
    {
        ActiveSkill = SkillManager.Instance.GetActiveSkillType();
        sRenderer = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    public void InitSkill()
    {
        ActiveSkill = SkillManager.Instance.GetActiveSkillType();
        SetSprite();
        SetPos();
        SetScale();
    }

    private void Update()
    {
        SetPos();
    }

    public void SetSprite()
    {
        if (ActiveSkill.isUseWorld)
        {
            sRenderer.sprite = ActiveSkill.sprite;
        }
    }

    void SetPos()
    {
        Vector2 setPos = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = setPos;
    }


    void SetScale()
    {
        float scale = SkillManager.Instance.activeSkill.GetSkillScale();
        sRenderer.size = new Vector2(scale, scale);
    }
}
