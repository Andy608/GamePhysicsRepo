using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FlashColor : MonoBehaviour
{
    [SerializeField] private Color baseColor = Color.white;
    [SerializeField] private Color flashColor;
    [SerializeField] private bool randomFlashColor = false;

    [SerializeField] private float baseColorTime = 0.3f;
    [SerializeField] private float flashColorTime = 0.3f;
    //[SerializeField] private Vector3 startingScale = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private float totalFlashDuration = 2.0f;
    private float durationCounter;

    private SpriteRenderer[] flashRenderers = null;

    private Shader guiShader;
    private Shader defaultShader;

    private float timeCounter;
    private bool isCurrentlyFlashing = false;

    private Coroutine active = null;

    private void Start()
    {
        flashRenderers = GetComponentsInChildren<SpriteRenderer>();
        guiShader = Shader.Find("GUI/Text Shader");
        defaultShader = Shader.Find("Sprites/Default");
    }

    public float Activate()
    {
        if (active != null)
        {
            //Debug.Log("ACTIVE AINT NULL");
            return 0;
        }
        else
        {
            active = StartCoroutine(StartFlash());
            //Debug.Log("DURATION: " + totalFlashDuration);
            return totalFlashDuration;
        }
    }

    private IEnumerator StartFlash()
    {
        timeCounter = 0.0f;
        durationCounter = 0.0f;

        //transform.localScale = startingScale;
        if (randomFlashColor)
        {
            RandomizeColor(0.3f, 1.0f, ref flashColor);
        }

        //GetComponent<CollisionHull2D>().enabled = false;

        while (durationCounter < totalFlashDuration)
        {
            //Debug.Log("DURATION COUNT: " + durationCounter);
            durationCounter += Time.deltaTime;

            //Vector3 scale = transform.localScale * scaleShrinkMultiplier;
            //scale.z = 1.0f;
            //transform.localScale = scale;

            timeCounter += Time.deltaTime;

            if (!isCurrentlyFlashing && timeCounter >= baseColorTime)
            {
                timeCounter = 0.0f;
                isCurrentlyFlashing = true;

                foreach (SpriteRenderer s in flashRenderers)
                {
                    s.color = flashColor;
                    s.material.shader = guiShader;
                }
            }
            else if (isCurrentlyFlashing && timeCounter >= flashColorTime)
            {
                timeCounter = 0.0f;
                isCurrentlyFlashing = false;

                foreach (SpriteRenderer s in flashRenderers)
                {
                    s.color = baseColor;
                    s.material.shader = defaultShader;
                }
            }

            yield return null;
        }

        timeCounter = 0.0f;
        durationCounter = 0.0f;
        isCurrentlyFlashing = false;

        foreach (SpriteRenderer s in flashRenderers)
        {
            s.color = baseColor;
            s.material.shader = defaultShader;
        }

        //GetComponent<CollisionHull2D>().enabled = true;
        active = null;
    }

    private void RandomizeColor(float minRange, float maxRange, ref Color c)
    {
        c.r = Random.Range(minRange, maxRange);
        c.g = Random.Range(minRange, maxRange);
        c.b = Random.Range(minRange, maxRange);
        c.a = 1.0f;
    }
}
