using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Animator loadingBar;

    public AnimatorStateInfo StateInfo
    {
        get => loadingBar.GetCurrentAnimatorStateInfo(0);
    }

    public void PlayLoadingBar()
    {
        loadingBar.Play("loading_bar_");
    }
}
