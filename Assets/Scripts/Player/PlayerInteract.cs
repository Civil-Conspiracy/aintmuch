using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    PlayerStateMachine player;
    private void Start()
    {
        InputManager.instance.OnInteract += args => Interact_performed(args);
        player = GetComponent<PlayerStateMachine>();
    }

    private void Interact_performed(InputManager.InputArgs args)
    {
        Debug.Log("wow");
        Interact();
    }

    private void Interact()
    {
        LayerMask mask = LayerMask.GetMask("Interactables");

        Vector2 dir = Vector2.zero;
        dir.x = (player.IsFacingRight) ? 1 : -1;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1.3f, mask);

        if (hit.collider.gameObject.GetComponent<IInteractable>() != null)
        {
            hit.collider.gameObject.GetComponent<IInteractable>().Interact(gameObject);
        }
    }
}
