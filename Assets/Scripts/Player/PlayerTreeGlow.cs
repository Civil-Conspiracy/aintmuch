using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTreeGlow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BaseTree>() != null)
            collision.gameObject.GetComponent<BaseTree>().SetGlow();
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BaseTree>() != null)
            collision.gameObject.GetComponent<BaseTree>().RemoveGlow();
    }
}
