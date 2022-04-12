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
    }
    public bool AttackPressed
    {
        get { return m_attackPressed; }
    }
    public bool AttackFinished
    {
        get { return m_attackDone; }
    }
    public bool RequireNewPress
    {
        get { return m_RequireNewPress; }
    }
    private void Start()
    {
        m_attacking = false;
        PlayerManager.Instance.Input.Player.Attack.performed += Attack_changed;
        PlayerManager.Instance.Input.Player.Attack.canceled += Attack_changed;
    }

    private void Update()
    {

    }

    private void Attack_changed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        m_attackPressed = obj.ReadValueAsButton();
        if(m_attackPressed)
           m_RequireNewPress = false;
    }

    public IEnumerator Attack()
    {
        if (m_attacking || !m_attackPressed || m_RequireNewPress)
            yield break;

        m_attacking = true;
        m_RequireNewPress = true;
        m_attackDone = false;
        float timeToImpact = swingTime - 0.6f;
        yield return new WaitForSeconds(timeToImpact);
        DetectHit();
        yield return new WaitForSeconds(0.3f);
        m_attacking = false;
        yield return new WaitForSeconds(0.3f);
        m_attackDone = true;
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
