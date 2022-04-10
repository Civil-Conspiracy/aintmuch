using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float swingTime;
    private bool m_attacking;

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
    }

    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        if (m_attacking)
            yield break;

        m_attacking = true;

        // Start the animation
        // wait for the animation length
        // send out a raycast for damage

        yield return new WaitForSeconds(swingTime);
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, GetComponent<PlayerMotor>().CurrentDirection * transform.right, 10f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Damageable"))
            {
                hit.collider.gameObject.GetComponent<IDamageable>().Damage(damage);
            }
        }

        m_attacking = false;
    }

}
