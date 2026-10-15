using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TextAnimations1 : MonoBehaviour
{
    public TextMeshProUGUI TextPause;

    private Color defaultColor = Color.lightSkyBlue;
    private Color highColor = Color.lightGreen;

    private float DurationAnimation = 0.2f;
    private float ScaleTextSize = 1.2f;

    private bool isHighlighter = false;
    private Vector3 initialScale;

    public bool startAnim;


    




    private IEnumerator AnimateHigh()
    {
        float elapsedtime = 0f;
        Vector3 startscale = TextPause.rectTransform.localScale;
        Vector3 endScale = startscale * ScaleTextSize;
        Color startColor1 = TextPause.outlineColor;

        TextPause.outlineWidth = 0.4f;

        while (elapsedtime < DurationAnimation)
        {
            elapsedtime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedtime / DurationAnimation);

            TextPause.rectTransform.localScale = Vector3.Lerp(startscale, endScale, t);

            TextPause.outlineColor = Color.Lerp(startColor1, highColor, t);

            if (Time.timeScale==0) { startAnim = false; break;}


            yield return null;

        }

        TextPause.rectTransform.localScale = endScale;
        TextPause.outlineColor = highColor;
        startAnim = false;
    }


    private IEnumerator AnimateLow()
    {
        float elapsedtime = 0f;
        Vector3 startscale = TextPause.rectTransform.localScale;
        Vector3 endScale = initialScale;
        Color startColor1 = TextPause.outlineColor;

        TextPause.outlineWidth = 0.2f;

        while (elapsedtime < DurationAnimation)
        {
            elapsedtime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedtime / DurationAnimation);

            TextPause.rectTransform.localScale = Vector3.Lerp(startscale, endScale, t);

            TextPause.outlineColor = Color.Lerp(startColor1, defaultColor, t);

            yield return null;

        }


        TextPause.rectTransform.localScale = endScale;
        TextPause.outlineColor = defaultColor;


    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialScale = TextPause.rectTransform.localScale;

        TextPause.outlineColor = defaultColor;
        TextPause.outlineWidth = 0.2f;


        isHighlighter = false;
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.O)) { startAnim = true; }


        if (startAnim != isHighlighter)
        {
            isHighlighter = startAnim;
            if (isHighlighter)
            {
                StartCoroutine(AnimateHigh());

            }
            else
            {
                StartCoroutine(AnimateLow());
            }
        }

    }
}
