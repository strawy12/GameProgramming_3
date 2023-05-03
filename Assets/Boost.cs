using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : Skill
{
    [SerializeField]
    SkillInfo info;
    WaitForSeconds skillSec = new WaitForSeconds(10f);
    public override int GetSkillScale()
    {
        return info.spriteScale;
    }

    public override void InitInfo()
    {
        info.scale = 1f;
        info.spriteScale = 1;
    }

    private void Awake()
    {
        InitInfo();
    }

    public override void UseSkill(Vector2 point)
    {
        StartCoroutine(UseSkill());
    }

    private IEnumerator UseSkill()
    {
        foreach (Tower tower in BuildingManager.Instance.towers)
        {
            if (tower == null)
            {
                BuildingManager.Instance.towers.Remove(tower);
                continue;
            }
            tower.SetTimerMax(0.2f);
            info.particleObj = Instantiate(SkillManager.Instance.GetActiveSkillType().prefab, tower.transform.position, Quaternion.identity);
            info.particleObj.transform.localScale *= info.scale;
            info.particle = info.particleObj.GetComponent<ParticleSystem>();
            info.particle.Play();
        }
        yield return skillSec;
        foreach (Tower tower in BuildingManager.Instance.towers)
        {
            tower.SetTimerMax(-0.2f);
        }
    }
}
