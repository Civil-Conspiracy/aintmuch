using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAxeSwing : MonoBehaviour
{
    PlayerStateMachine player;
    [SerializeField] float m_dirModifier;

    private void Start()
    {
        player = GetComponent<PlayerStateMachine>();
    }

    // call this in attack state

}
