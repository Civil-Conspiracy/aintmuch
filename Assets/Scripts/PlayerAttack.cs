using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float swingTime;
    private bool m_attacking;
    bool m_attackPressed;
    [SerializeField] float m_dirModifier;

    // TEMP DAMAGE
    private int damage = 2;

    public bool Attacking
    {
        get { return m_attacking; }
    }

    private void Start()
    {
        m_attacking = false;
        PlayerManager.Instance.Input.Player.Attack.performed += Attack_performed;
        PlayerManager.Instance.Input.Player.Attack.canceled += Attack_performed;
    }

    private void Update()
    {
       if(m_attackPressed && !m_attacking) StartCoroutine(Attack());
    }

    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        m_attackPressed = obj.ReadValueAsButton();
    }

    private IEnumerator Attack()
    {
        if (m_attacking)
            yield break;

        m_attacking = true;
        // Start the animation
        // wait for the animation length
        // send out a raycast for damage
        float timeToImpact = swingTime - 0.6f;
        yield return new WaitForSeconds(timeToImpact);
        DetectHit();
        yield return new WaitForSeconds(0.6f);
        m_attacking = false;
    }

    private void DetectHit()
    {
        Vector2 dir = new Vector2(GetComponent<PlayerMotor>().CurrentDirection, m_dirModifier);
        Debug.DrawRay(transform.position, dir, Color.red, 1.15f);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, 1.15f);
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
