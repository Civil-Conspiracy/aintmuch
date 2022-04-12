using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    static readonly float swingTime = 1.72f;
    private bool m_attacking;
    bool m_attackPressed;
    bool m_RequireNewPress;
    bool m_attackDone;
    [SerializeField] float m_dirModifier;
    // TEMP DAMAGE
    private int damage = 2;
    public bool Attacking
    {
        get { return m_attacking; }
        set { m_attacking = value; }
    }
    public bool AttackPressed
    {
        get { return m_attackPressed; }
    }
    public bool AttackFinished
    {
        get { return m_attackDone; }
        set { m_attackDone = value; }
    }
    public bool RequireNewPress
    {
        get { return m_RequireNewPress; }
    }

    private Coroutine c;

    public Coroutine C
    {
        get { return c; }
        set { c = value; }
    }

    private void Start()
    {
        m_attacking = false;
        PlayerManager.Instance.Input.Player.Attack.performed += Attack_changed;
        PlayerManager.Instance.Input.Player.Attack.canceled += Attack_changed;
    }

    private void Update()
    {
        if (m_attackPressed && !m_attacking && GetComponent<PlayerMotor>().Direction == 0)
            C = StartCoroutine(Attack());
    }

    private void Attack_changed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        m_attackPressed = obj.ReadValueAsButton();
    }

    public IEnumerator Attack()
    {

        if (m_attacking || !m_attackPressed)
            yield break;

        m_attacking = true;

        m_RequireNewPress = true;
        float timeToImpact = swingTime - 0.9f;
        yield return new WaitForSeconds(timeToImpact);
        GetComponent<PlayerMotor>().DirectionLocked = true;
        DetectHit();
        yield return new WaitForSeconds(0.4f);
        GetComponent<PlayerMotor>().DirectionLocked = false;
        yield return new WaitForSeconds(0.5f);
        m_attacking = false;
    }

    private void DetectHit()
    {
        Vector2 newPos = transform.position;
        newPos.y -= 0.5f;

        Vector2 dir = new Vector2(GetComponent<PlayerMotor>().CurrentDirection, m_dirModifier);
        
        Debug.DrawRay(newPos, dir, Color.red, 1.15f);

        RaycastHit2D[] hits = Physics2D.RaycastAll(newPos, dir, 1.15f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Damageable"))
            {
                Camera.main.GetComponent<CameraController>().Shake(0.05f);
                hit.collider.gameObject.GetComponent<IDamageable>().Damage(damage);
            }
        }
    }
}
