using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTree : MonoBehaviour, IDamageable
{
    [SerializeField] int m_MaxHealth;
    [SerializeField] FillBar healthBar;
    [SerializeField] GameObject go_deadTree;
    [SerializeField] GameObject go_splitTree;
    [SerializeField] Transform m_DeadPoint;

    HealthSystem healthSystem;    

    public void Damage(int dmg)
    {
        healthSystem.Damage(dmg);
    }

    private void Awake()
    {
        healthSystem = new HealthSystem(m_MaxHealth);
        healthSystem.OnHealthChanged += UpdateHealthBar;
        healthSystem.OnDead += OnDeadTree;
    }

    private void OnDeadTree(object sender, System.EventArgs e)
    {
        if(go_deadTree != null)SpawnDeadTree();
        else if(go_splitTree != null)SpawnSplitTree();
        Destroy(gameObject);
    }

    private void UpdateHealthBar(object sender, System.EventArgs e)
    {
        healthBar.SetBarAtPercent(healthSystem.GetCurrentHealthPercentage());
    }

    private void SpawnDeadTree()
    {
        Vector2 playerPos = m_DeadPoint.position - GameObject.FindGameObjectWithTag("Player").transform.position;
        float playerDir = playerPos.normalized.x;
        float spawnRot;
        if (playerDir > 0) spawnRot = -12f;
        else if (playerDir < 0) spawnRot = 12f;
        else spawnRot = -90f;
        Instantiate(go_deadTree, m_DeadPoint.position, Quaternion.Euler(0f, 0f, spawnRot));
        Debug.Log("Player Direction from Tree: " + playerDir);
        Debug.Log("Player Position: " + playerPos);
    }
    private void SpawnSplitTree()
    {
        Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        float playerDir = transform.position.normalized.x - playerPos.normalized.x;
        float spawnRot1 = 5f;
        float spawnRot2 = -5f;
        Vector2 spawnPoint1 = m_DeadPoint.position;
        Vector2 spawnPoint2 = m_DeadPoint.position;
        spawnPoint1.x -= 0.5f;
        spawnPoint2.x += 0.5f;
        spawnPoint1.y = playerPos.y + 3f;
        spawnPoint2.y = playerPos.y + 3f;

        GameObject split1 = Instantiate(go_splitTree, spawnPoint1, Quaternion.Euler(0f, 0f, spawnRot1));
        GameObject split2 = Instantiate(go_splitTree, spawnPoint2, Quaternion.Euler(0f, 0f, spawnRot2));
        split1.GetComponent<SpriteRenderer>().flipX = true;
        Debug.Log("Player Direction from Tree: " + playerDir);
        Debug.Log("Player Position: " + playerPos);
    }
}
