using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTree : MonoBehaviour, IDamageable
{
    [Header("Tree object.  Spawns one on death.")]
    [SerializeField] GameObject go_deadTree;
    [Header("Tree object.  Spawns two on death.")]
    [SerializeField] GameObject go_splitTree;
    [Header("BaseItem object.")]
    [SerializeField] GameObject go_defaultItem;
    [Space(5)]
    [Header("Properties")]
    [SerializeField] int m_MaxHealth;
    [SerializeField] Item[] m_lootTable;
    [SerializeField] Color m_Color;
    [SerializeField] Transform m_DeadPoint;
    [SerializeField] FillBar m_healthBar;
    [SerializeField] Material m_outlineMat;
    Material defaultMat;

    HealthSystem healthSystem;    

    public void Damage(int dmg)
    {
        healthSystem.Damage(dmg);
    }

    private void Awake()
    {
        defaultMat = GetComponent<SpriteRenderer>().material;
        healthSystem = new HealthSystem(m_MaxHealth);
        healthSystem.OnHealthChanged += UpdateHealthBar;
        healthSystem.OnDead += OnDeadTree;
    }

    private void OnValidate()
    {
        SpriteRenderer thisRend = GetComponent<SpriteRenderer>();
        if (thisRend.color != m_Color)
            thisRend.color = m_Color;
    }

    public void SetGlow()
    {
        SpriteRenderer thisRend = GetComponent<SpriteRenderer>();
        if (thisRend.material != m_outlineMat)
            thisRend.material = m_outlineMat;
    }

    public void RemoveGlow()
    {
        SpriteRenderer thisRend = GetComponent<SpriteRenderer>();
        if (thisRend.material != defaultMat)
            thisRend.material = defaultMat;
    }

    private void OnDeadTree(object sender, System.EventArgs e)
    {
        if(go_deadTree != null)
            SpawnDeadTree();
        else if(go_splitTree != null)
            SpawnSplitTree();

        if(go_defaultItem != null && m_lootTable != null)
            SpawnLootTable(m_lootTable);

        Destroy(gameObject);
    }

    private void UpdateHealthBar(object sender, System.EventArgs e)
    {
        m_healthBar.SetBarAtPercent(healthSystem.GetCurrentHealthPercentage());
    }

    private void SpawnDeadTree()
    {
        Vector2 playerPos = m_DeadPoint.position - GameObject.FindGameObjectWithTag("Player").transform.position;

        float playerDir = playerPos.normalized.x;
        float spawnRot;

        if (playerDir > 0)
            spawnRot = -12f;
        else if (playerDir < 0)
            spawnRot = 12f;
        else
            spawnRot = -90f;

        Instantiate(go_deadTree, m_DeadPoint.position, Quaternion.Euler(0f, 0f, spawnRot));
    }

    private void SpawnSplitTree()
    {
        Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        float spawnRot = 5f;
        Vector2 spawnPoint1 = m_DeadPoint.position;
        Vector2 spawnPoint2 = m_DeadPoint.position;

        spawnPoint1.x -= 0.5f;
        spawnPoint2.x += 0.5f;

        spawnPoint1.y = playerPos.y + 3f;
        spawnPoint2.y = playerPos.y + 3f;

        GameObject split1 = Instantiate(go_splitTree, spawnPoint1, Quaternion.Euler(0f, 0f, spawnRot));
        Instantiate(go_splitTree, spawnPoint2, Quaternion.Euler(0f, 0f, -spawnRot));

        split1.GetComponent<SpriteRenderer>().flipX = true;
    }

    private void SpawnLootTable(Item[] items)
    {
        Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector2 spawnPos = m_DeadPoint.position;

        float treeLength = GetComponent<CapsuleCollider2D>().size.y - -0.25f;
        float spacing = (treeLength / items.Length);
        spawnPos.x += treeLength / 2;
        spawnPos.y = playerPos.y + 1.5f;

        for (int i = 0; i < items.Length; i++)
        {
            if(items[i] != null)
            {
                spawnPos.x -= spacing;
                SpawnItem(items[i], spawnPos);
            }
        }
    }

    private void SpawnItem(Item item, Vector2 spawnPos)
    {
        Debug.Log("Spawning " + item.ItemName);
        GameObject lootDrop = Instantiate(go_defaultItem, spawnPos, Quaternion.identity);
        lootDrop.GetComponent<FloorItem>().SetInfo(item, 1);
    }
}
