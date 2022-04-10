using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IDamageable
{
    [SerializeField] int m_MaxHealth;
    [SerializeField] FillBar healthBar;
    [SerializeField] GameObject go_deadTree;
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
        SpawnDeadTree();
        Destroy(gameObject);
    }

    private void UpdateHealthBar(object sender, System.EventArgs e)
    {
        healthBar.SetBarAtPercent(healthSystem.GetCurrentHealthPercentage());
    }

    private void SpawnDeadTree()
    {
        Instantiate(go_deadTree, m_DeadPoint.position, Quaternion.Euler(0f, 0f, -90f));
    }
}
