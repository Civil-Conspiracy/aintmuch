using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialTextManager : MonoBehaviour
{
   [SerializeField] private TextMeshPro _textMeshPro;

    private void OnValidate()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
    }
    private void Awake()
    {
        _textMeshPro.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _textMeshPro.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _textMeshPro.enabled = false;
        }
    }
}
