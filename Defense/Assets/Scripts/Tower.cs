using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private float shootTimerMax = 0.3f;
    private float shootTimer;

    private float lookForTimer;
    private float lookForTimerMax = 0.2f;

    private Enemy targetEnemy;

    private Vector3 projectileSpawnPosition;

    private void Awake()
    {
        projectileSpawnPosition = transform.Find("projectileSpawnPosition").position;
    }

    private void Update()
    {
        HandleTargetting();
        HandleShooting();
    }

    private void HandleTargetting()
    {
        lookForTimer -= Time.deltaTime;
        if (lookForTimer < 0f)
        {
            lookForTimer += lookForTimerMax;
            LookForTarget();
        }
    }

    private void LookForTarget()
    {
        float targetMaxRadius = 20f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) > Vector3.Distance(transform.position, enemy.transform.position))
                    {
                        targetEnemy = enemy;
                    }
                } 
            }
        }
    }

    private void HandleShooting()
    {
        shootTimer -= Time.deltaTime;
        if(shootTimer <= 0f)
        {
            shootTimer += shootTimerMax;

            if (targetEnemy != null)
            {
                int damageAmount = (int)Mathf.Pow(1.5f, StarForceUI.Inst.GetLevel(gameObject.name)) * 10;
                ArrowProjectile.Create(projectileSpawnPosition, targetEnemy, damageAmount);
            }
        }
    }
}
