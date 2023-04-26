using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create(Vector3 position)
    {
        Transform pfEnemy = GameAssets.Instance.pfEnemy;
        Transform enemyTransform = Instantiate(pfEnemy, position, Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }

    private Transform targetTransform;
    private Rigidbody2D enemyRigidbody2D;
    private float lookForTimer;
    private float lookForTimerMax = .2f;

    public HealthSystem healthSystem;


    private void Awake()
    {
        enemyRigidbody2D = GetComponent<Rigidbody2D>();
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
        CinemachineShake.Instance.ShakeCamera(5f, .1f);
        ChromaticAberrationEffect.Instance.SetWeight(.5f);

    }

    private void Start()
    {
        if (BuildingManager.Instance.GetHQBuilding() != null)
        {
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        }

        lookForTimer = Random.Range(0f, lookForTimerMax);

    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        CinemachineShake.Instance.ShakeCamera(7f, .15f);
        ChromaticAberrationEffect.Instance.SetWeight(.5f);
        //ResourceManager.Instance.AddResource( ,1)

        Instantiate(GameAssets.Instance.pfEnemyDieParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Update()
    {
        HandleMovement();
        HandleTargetting();
    }

    private void HandleMovement()
    {
        if (targetTransform != null)
        {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;
            float moveSpeed = 8f;
            enemyRigidbody2D.velocity = moveDir * moveSpeed;
        }
        else
        {
            enemyRigidbody2D.velocity = Vector2.zero;
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();
        if (building != null)
        {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(10);
            this.healthSystem.Damage(999);
        }
    }

    private void LookForTarget()
    {
        float targetMaxRadius = 30f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            Building building = collider2D.GetComponent<Building>();
            if (building != null)
            {
                if (targetTransform == null)
                {
                    targetTransform = building.transform;
                }
                else
                {
                    if (Vector3.Distance(transform.position, targetTransform.position) > Vector3.Distance(transform.position, building.transform.position))
                    {
                        targetTransform = building.transform;
                    }
                }
            }
        }

        if (BuildingManager.Instance.GetHQBuilding() != null)
        {
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        }
    }
}
