using DG.Tweening;
using UnityEngine;
using System.Collections;

public class CenterLightManager : MonoBehaviour
{

    [SerializeField] Renderer rend;
    [SerializeField] Color emissionColor = Color.white;
    [Range(0f, 5f)]
    [SerializeField] float emissionIntensity = 1f;
    [SerializeField] float[] intervals;
    [SerializeField] int[] blinkTimes;

    float interval = 0;
    float blinkCount = 0;
    private Material mat;
    private bool isOn = false;


    void Start()
    {
        mat = rend.material;
        mat.EnableKeyword("_EMISSION");

        LightFlash(0);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LightFlash(1);

        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            StopLightFlash();   

        }

    }

    public void LightFlash(int mode)
    {
        StopAllCoroutines();
        interval = intervals[mode];
        switch (mode)
        {
            case 0:
                blinkCount = -1;
                break;
            case 1:
                blinkCount = blinkTimes[0];
                break;
            case 2:
                blinkCount = blinkTimes[1];
                break;


        }

        StartCoroutine(BlinkEmission());
    }



    IEnumerator BlinkEmission()
    {
        while (true)
        {
            isOn = !isOn;
            if (isOn)
            {
                // ‹P“x‚ğ’²®‚µ‚ÄON
                mat.SetColor("_EmissionColor", emissionColor * emissionIntensity);
            }
            else
            {
                // OFF
                mat.SetColor("_EmissionColor", Color.black);
            }

            yield return new WaitForSeconds(interval);
            if (blinkCount > 0)
            {
                blinkCount--;
                if (blinkCount <= 0)
                    StopLightFlash();


            }
        }
    }
    public void StopLightFlash()
    {
        StopAllCoroutines();

    }
}
