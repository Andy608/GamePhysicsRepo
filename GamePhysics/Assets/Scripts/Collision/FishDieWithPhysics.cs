using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Particle2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class FishDieWithPhysics : MonoBehaviour
{
    [SerializeField] private Color baseColor = Color.white;
    public Color flashColor;
    [SerializeField] private float baseColorTime = 0.3f;
    [SerializeField] private float flashColorTime = 0.3f;
    [SerializeField] private Vector3 startingScale = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] [Range(0.1f, 0.999f)] private float scaleShrinkMultiplier = 0.99f;
    [SerializeField] private Vector2 pointApplied = new Vector2(5.0f, 0.0f);
    [SerializeField] private Vector2 forceApplied = new Vector2(50.0f, 0.0f);

    private Particle2D fishyParticle;
    private SpriteRenderer fishySpriteRenderer;

    private Shader guiShader;
    private Shader defaultShader;

    private float timeCounter;
    private bool isCurrentlyFlashing = false;

    private Coroutine active = null;

    private void Start()
    {
        fishyParticle = GetComponent<Particle2D>();
        fishySpriteRenderer = GetComponent<SpriteRenderer>();
        guiShader = Shader.Find("GUI/Text Shader");
        defaultShader = Shader.Find("Sprites/Default");
    }

    public void Activate()
    {
        if (active != null)
        {
            return;
        }
        else
        {
            active = StartCoroutine(StartAnim());
        }
    }

    private IEnumerator StartAnim()
    {
        timeCounter = 0.0f;
        transform.localScale = startingScale;
        RandomizeColor(0.3f, 1.0f, ref flashColor);
        Destroy(GetComponent<CollisionHull2D>());

        while (transform.localScale.x > 0.1f)
        {
            Vector3 scale = transform.localScale * scaleShrinkMultiplier;
            scale.z = 1.0f;
            transform.localScale = scale;

            fishyParticle.ApplyTorque(pointApplied, forceApplied);

            timeCounter += Time.deltaTime;

            if (!isCurrentlyFlashing && timeCounter >= baseColorTime)
            {
                timeCounter = 0.0f;
                isCurrentlyFlashing = true;
                fishySpriteRenderer.color = flashColor;
                fishySpriteRenderer.material.shader = guiShader;
            }
            else if (isCurrentlyFlashing && timeCounter >= flashColorTime)
            {
                timeCounter = 0.0f;
                isCurrentlyFlashing = false;
                fishySpriteRenderer.color = baseColor;
                fishySpriteRenderer.material.shader = defaultShader;
            }

            yield return null;
        }

        active = null;
        Destroy(gameObject);
    }

    private void RandomizeColor(float minRange, float maxRange, ref Color c)
    {
        c.r = Random.Range(minRange, maxRange);
        c.g = Random.Range(minRange, maxRange);
        c.b = Random.Range(minRange, maxRange);
        c.a = 1.0f;
    }
}
