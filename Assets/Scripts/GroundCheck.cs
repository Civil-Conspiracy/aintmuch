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
            if (collision.transform.parent.CompareTag("Ground"))
            {
                motor.Grounded = true;
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent != null)
        {
            if (collision.transform.parent.CompareTag("Ground"))
            {
                motor.Grounded = false;
            }
        }
    }
}
