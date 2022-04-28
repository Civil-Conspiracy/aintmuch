using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && TimerController.instance.TimerGoing == false)
        {
            TimerController.instance.BeginTimer();
        }
    }
}
