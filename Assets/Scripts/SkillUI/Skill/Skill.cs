using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [System.Serializable]
    public struct SkillInfo
    {
        public Collider2D[] enemies;
        public ParticleSystem particle;
        public GameObject particleObj;
        public LayerMask enemyLayer;
        public float scale;
        public int spriteScale;
        public int damage;
    }

    public abstract void InitInfo();

    public abstract void UseSkill(Vector2 point);
    public abstract int GetSkillScale();
}
