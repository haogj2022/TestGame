using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera mainCam;

    private float shakeAmount = 0f;

    private void Awake()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
    }

    public void Shake(float amt, float length)
    {
        shakeAmount = amt;
        InvokeRepeating("DoShake", 0f, 0.01f);
        Invoke("StopShake", length);
    }

    private void DoShake()
    {
        if (shakeAmount > 0f)
        {
            Vector3 camPos = mainCam.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x += offsetX;
            camPos.y += offsetY;

            mainCam.transform.position = camPos;
        }
    }

    private void StopShake()
    {
        CancelInvoke("DoShake");
    }
}
