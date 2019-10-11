using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverColor : MonoBehaviour
{
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color hoverColor = Color.white;
    [SerializeField] private Color clickColor = Color.white;
    //[SerializeField] private float colorChangeMultiplier = 1.0f;
    private TextMeshProUGUI text;

    private Coroutine fadeColorCoroutine;
    private Color lerpedColor;
    private float t;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void OnEnter()
    {
        text.color = hoverColor;

        //if (fadeColorCoroutine != null)
        //{
        //    StopCoroutine(fadeColorCoroutine);
        //}

        //fadeColorCoroutine = StartCoroutine(FadeColors(text.color, hoverColor));
    }

    public void OnExit()
    {
        text.color = defaultColor;

        //if (fadeColorCoroutine != null)
        //{
        //    StopCoroutine(fadeColorCoroutine);
        //}

        //fadeColorCoroutine = StartCoroutine(FadeColors(text.color, defaultColor));
    }

    public void OnClick()
    {
        text.color = clickColor;
        EventAnnouncer.OnButtonPressed?.Invoke(EnumSound.BUTTON_PRESS);
    }

    //private IEnumerator FadeColors(Color start, Color end)
    //{
    //    t = 0.0f;

    //    while (t <= 1)
    //    {
    //        text.color = Color.Lerp(start, end, t);
    //        t += Time.deltaTime * colorChangeMultiplier;
    //        yield return null;
    //    }

    //    fadeColorCoroutine = null;
    //}
}
