using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera m_LastCamera;

    private IEnumerator DoShake(float intensity)
    {
        Vector3 startPos = transform.position;
        for (int i = 0; i < 3; i++)
        {
            transform.position = new Vector3(startPos.x + Random.Range(intensity, -intensity), startPos.y + Random.Range(intensity, -intensity), -10);
            yield return new WaitForSeconds(0.05f);
        }
        transform.position = startPos;
    }

    public void Shake(float intensity)
    {
        StartCoroutine(DoShake(intensity));
    }
    public void StopShake()
    {
        StopCoroutine(DoShake(0));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach (Camera c in Camera.allCameras)
        {
            c.enabled = false;
        }
        GetComponent<Camera>().enabled = true;
    }
}