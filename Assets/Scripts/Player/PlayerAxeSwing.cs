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

    // moved the attack method to PlayerStateMachine.  you can re-retrieve it whenever, I just didn't feel like adding references when I was testing.

}
