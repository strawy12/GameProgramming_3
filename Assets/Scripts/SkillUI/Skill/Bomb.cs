using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Skill
{
    [SerializeField]
    SkillInfo info;

    public override void InitInfo()
    {
        info.scale = 3;
        info.spriteScale = 6;
        info.damage = 30;
    }

    private void Awake()
    {
        InitInfo();
    }

    public override void UseSkill(Vector2 point)
    {
        info.particleObj = Instantiate(SkillManager.Instance.GetActiveSkillType().prefab, point, Quaternion.identity);
        info.particleObj.transform.localScale *= info.scale;
        info.particle = info.particleObj.GetComponent<ParticleSystem>();
        info.enemies = Physics2D.OverlapCircleAll(point, info.scale, info.enemyLayer);
        info.particle.Play();

        foreach (Collider2D enemy in info.enemies)
        {
            enemy.gameObject.GetComponent<Enemy>().healthSystem.Damage(info.damage);
        }
    }
    public override int GetSkillScale()
    {
        return info.spriteScale;
    }
}
