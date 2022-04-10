using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private void Start()
    {
        PlayerManager.Instance.Input.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Interact();
    }

    private void Interact()
    {
        LayerMask mask = LayerMask.GetMask("Interactables");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, GetComponent<PlayerMotor>().CurrentDirection * transform.right, 1.3f, mask);

        if (hit.collider.gameObject.GetComponent<IInteractable>() != null)
        {
            hit.collider.gameObject.GetComponent<IInteractable>().Interact();
        }
    }
}
