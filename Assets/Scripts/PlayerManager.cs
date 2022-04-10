using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    PlayerInputControls input;

    #region Singleton
    private static PlayerManager instance;

    public static PlayerManager Instance
    {
        get => instance;
        set
        {
            if (instance == null)
            {
                instance = value;
            }
            else if (instance != value)
            {
                Debug.Log($"{nameof(PlayerManager)} already exists. Deleting now...");
                Destroy(value);
            }
        }
    }
    #endregion

    public PlayerInputControls Input
    {
        get { return input; }
    }

    private void Awake()
    {
        Instance = this;

        input = new PlayerInputControls();
        input.Enable();
    }


    private void OnDisable()
    {
        input.Disable();
    }
}
