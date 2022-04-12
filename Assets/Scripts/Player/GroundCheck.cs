using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    PlayerMotor motor;

    private void Awake()
    {
        motor = transform.parent.GetComponent<PlayerMotor>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent != null)
        {
            if (collision.transform.parent.CompareTag("Ground") || collision.transform.parent.CompareTag("Platform"))
            {
                motor.Grounded = true;
            }
        }
        if (collision.gameObject.CompareTag("Damageable") && transform.position.y > collision.transform.position.y)
        {
            motor.Grounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent != null && (collision.transform.parent.CompareTag("Ground") || collision.transform.parent.CompareTag("Platform")))
        {
            motor.Grounded = false;
        }
        else if (collision.gameObject.CompareTag("Damageable"))
        {
            if (transform.position.y >= collision.transform.position.y)
            {
                motor.Grounded = false;
            }
            else 
                motor.Grounded = true;
        }
    }
}
