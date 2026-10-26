using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TextAnimations1 : MonoBehaviour
{
    public TextMeshProUGUI TextPause;

    private Color defaultColor = Color.white;
    private Color highColor = Color.lightGreen;

    private float DurationAnimation = 0.2f;
    private float ScaleTextSize = 1.2f;

    private bool isHighlighter = false;
    private Vector3 initialScale;

    public bool startAnim;

    public Image Frame;
    public Sprite FrameTimerFalse;
    public Sprite FrameTimerTrue;




    private IEnumerator AnimateHigh()
    {
        Frame.sprite = FrameTimerTrue;
        float elapsedtime = 0f;
        Vector3 startscale = TextPause.rectTransform.localScale;
        Vector3 endScale = startscale * ScaleTextSize;
        Color startColor1 = TextPause.color;

        while (elapsedtime < DurationAnimation)
        {
            elapsedtime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedtime / DurationAnimation);

            TextPause.rectTransform.localScale = Vector3.Lerp(startscale, endScale, t);

            TextPause.color = Color.Lerp(startColor1, highColor, t);

            if (Time.timeScale==0) { startAnim = false; break;}


            yield return null;

        }

        TextPause.rectTransform.localScale = endScale;
        TextPause.color = highColor;
        startAnim = false;
    }


    private IEnumerator AnimateLow()
    {
        
        float elapsedtime = 0f;
        Vector3 startscale = TextPause.rectTransform.localScale;
        Vector3 endScale = initialScale;
        Color startColor1 = TextPause.color;

        while (elapsedtime < DurationAnimation)
        {
            elapsedtime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedtime / DurationAnimation);

            TextPause.rectTransform.localScale = Vector3.Lerp(startscale, endScale, t);

            TextPause.color = Color.Lerp(startColor1, defaultColor, t);

            yield return null;

        }


        TextPause.rectTransform.localScale = endScale;
        TextPause.color = defaultColor;
        Frame.sprite = FrameTimerFalse;

    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialScale = TextPause.rectTransform.localScale;

        TextPause.color = defaultColor;
        //TextPause.outlineWidth = 0.2f;

        Frame.sprite = FrameTimerFalse;
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
