using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowProjectile : MonoBehaviour
{
    private Enemy targetEnemy;
    private Vector3 lastMoveDir;
    private float timeToDie = 2f;

    public static ArrowProjectile Create(Vector3 position, Enemy enemy)
    {
        Transform pfArrowProjectile = GameAssets.Instance.pfArrowProjectile;
        Transform arrowTransform = Instantiate(pfArrowProjectile, position, Quaternion.identity);

        ArrowProjectile arrow = arrowTransform.GetComponent<ArrowProjectile>();
        arrow.SetTarget(enemy);

        return arrow;
    }


    private void Update()
    {
        Vector3 moveDir;
        if (targetEnemy != null)
        {
            moveDir = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDir = moveDir;
        }
        else
        {
            moveDir = lastMoveDir;
        }
        transform.eulerAngles = new Vector3(0f, 0f, UtilClass.GetAngleFromVector(moveDir));

        float moveSpeed = 20f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        timeToDie -= Time.deltaTime;
        if (timeToDie < 0f)
        {
            Destroy(gameObject);
        }
    }

    private void SetTarget(Enemy targetEnemy)
    {
        this.targetEnemy = targetEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if(enemy !=null)
        {
            int damageAmount = 10;
            enemy.GetComponent<HealthSystem>().Damage(damageAmount);

            Destroy(gameObject);
        }
    }
}
