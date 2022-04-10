using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillBar : MonoBehaviour
{
    [SerializeField]
    private FillBarType barType;
    [SerializeField]
    private Transform barFill;

    public void SetBarAtPercent(float p)
    {
        if (barType == FillBarType.HORIZONTAL)
        {
            barFill.localScale = new Vector2(p, barFill.localScale.y);
        }
        else if (barType == FillBarType.VERTICAL)
        {
            barFill.localScale = new Vector2(barFill.localScale.x, p);
        }
    }

    public void ResetBarToZero()
    {
        SetBarAtPercent(0f);
    }
}

public enum FillBarType
{
    HORIZONTAL,
    VERTICAL
}
