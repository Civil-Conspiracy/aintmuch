using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float swingTime;
    private bool m_attacking, m_attackPressed;

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
        if (m_attackPressed && !m_attacking)
            StartCoroutine(Attack());
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

        yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        Debug.Log(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        // add an event in the swing animation that fires on the frame that the axe hits the tree
        // This event fires the code below.

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, GetComponent<PlayerMotor>().CurrentDirection * transform.right, 1.15f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Damageable"))
            {
                Camera.main.GetComponent<CameraController>().Shake(0.05f);

                hit.collider.gameObject.GetComponent<IDamageable>().Damage(damage);
            }
        }

        m_attacking = false;
    }
}
