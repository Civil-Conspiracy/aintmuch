using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraController : MonoBehaviour
{
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
        if (collision.CompareTag("Player"))
        {
            foreach (Camera c in Camera.allCameras)
            {
                c.enabled = false;
                c.transform.Find("Foreground Lighting").GetComponent<Light2D>().enabled = false;
                c.transform.Find("Background Lighting").GetComponent<Light2D>().enabled = false;
            }
            GetComponent<Camera>().enabled = true;
            transform.Find("Foreground Lighting").GetComponent<Light2D>().enabled = true;
            transform.Find("Background Lighting").GetComponent<Light2D>().enabled = true;
        }
    }
}
