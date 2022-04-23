using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform p0;
    [SerializeField] private Transform p1;
    [SerializeField] private float speed;

    // True: p0   False: p1
    private bool target;

    private readonly float buffer = 0.05f;

    private void Awake()
    {
        gameObject.transform.position = p0.position;
    }

    private void FixedUpdate()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        if (target)
        {
            transform.position = Vector2.MoveTowards(transform.position, p0.position, speed * Time.fixedDeltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, p1.position, speed * Time.fixedDeltaTime);
        }
        CheckSwitchTarget();
    }

    private void CheckSwitchTarget()
    {
        if (target)
        {
            if (transform.position == p0.transform.position || Vector2.Distance(transform.position, p1.transform.position) < buffer)
                target = !target;
        }
        else
        {
            if (transform.position == p1.transform.position || Vector2.Distance(transform.position, p1.transform.position) < buffer)
            {
                target = !target;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
            DontDestroyOnLoad(collision.gameObject);
        }
    }
}
