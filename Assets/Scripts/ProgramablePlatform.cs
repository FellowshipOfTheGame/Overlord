using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ProgramablePlatform : MonoBehaviour
{
    private SpriteRenderer render;
    private SpriteRenderer[] renders;
    private BoxCollider2D boxCollider;
    private Material originalMaterial;
    private Material[] originalMaterials;
    private float originalAlpha;
    private float[] originalAlphas;

    [Header("Fade")]
    [Tooltip("Platform will fade on trigger or use material?")]
    public bool fade = true;
    [Tooltip("Fading alpha target")]
    public float fadeAlpha = 0.5f;
    [Tooltip("Material, if it doesn't fade")]
    public Material material = null;

    [Header("Collision")]
    [Tooltip("Platform will disappear on collision?")]
    public bool onCollide = true;
    [Tooltip("Delay time to disappear in seconds")]
    public float disappearTime = 0.1f;
    [Tooltip("Fade time to disappear in seconds")]
    public float fadeOut = 0.1f;
    [Tooltip("Delay time to reappear in seconds")]
    public float reappearTime = 0.1f;
    [Tooltip("Fade time to reappear in seconds")]
    public float fadeIn = 0.1f;

    [Header("Timer")]
    [Tooltip("Platform will disappear over an specific time?")]
    public bool onTimer = false;
    [Tooltip("Timer period in seconds")]
    public float timerPeriod = 1f;
    [Tooltip("Timer pattern (false for no collision, true for collision)")]
    public bool[] timerPattern;

    private bool timerState;
    private float timerStart;

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Get original material and alpha
        if (render)
        {
            originalMaterial = render.material;
            originalAlpha = originalMaterial.color.a;
        }
        else
        {
            renders = GetComponentsInChildren<SpriteRenderer>();
            if (renders != null && renders.Length > 0)
            {
                originalMaterials = new Material[renders.Length];
                originalAlphas = new float[renders.Length];
                for (int i = 0; i < originalMaterials.Length; i++)
                {
                    originalMaterials[i] = renders[i].material;
                    originalAlphas[i] = originalMaterials[i].color.a;
                }
            }

        }


        // If is timer based and size > 0
        if (timerPattern.Length > 0 && onTimer)
        {
            timerState = timerPattern[0];
            if (timerState)
                Reappear();
            else
                Disappear();
        }
        timerStart = Time.timeSinceLevelLoad;
    }

    void Update()
    {
        if (onTimer)
        {
            // Get current time based on awake
            float time = Time.timeSinceLevelLoad - timerStart;
            int patternSize = timerPattern.Length;
            // Calculate poisition based on time and pattern
            time = time * patternSize;
            int timerPosition = Mathf.FloorToInt(time / timerPeriod) % patternSize;

            // If the current state is different from the timer state
            if (timerState != timerPattern[timerPosition])
            {
                timerState = timerPattern[timerPosition];
                if (timerState)
                    Reappear();
                else
                    Disappear();
            }
        }

    }

    void OnCollisionEnter2D()
    {
        // If collision based, than start coroutine
        if (onCollide)
        {
            StartCoroutine("Fade");
        }
    }

    void Disappear()
    {
        if (!fade)
        {
            if (render)
                render.material = material;
            else for (int i = 0; i < renders.Length; i++)
                    renders[i].material = material;
        }
        else
        {
            if (render)
            {
                Color c = originalMaterial.color;
                c.a = fadeAlpha;
                originalMaterial.color = c;
            }
            else for (int i = 0; i < renders.Length; i++)
                {
                    Color c = originalMaterials[i].color;
                    c.a = fadeAlpha;
                    originalMaterials[i].color = c;
                }

        }
        boxCollider.enabled = false;
    }

    void Reappear()
    {
        if (!fade)
        {
            if (render)
                render.material = originalMaterial;
            else for (int i = 0; i < renders.Length; i++)
                    renders[i].material = originalMaterials[i];
        }
        else
        {
            if (render)
            {
                Color c = originalMaterial.color;
                c.a = fadeAlpha;
                originalMaterial.color = c;
            }
            else for (int i = 0; i < renders.Length; i++)
                {
                    Color c = originalMaterials[i].color;
                    c.a = originalAlphas[i];
                    originalMaterials[i].color = c;
                }
        }
        boxCollider.enabled = true;
    }

    IEnumerator Fade()
    {
        bool timer = onTimer;
        onTimer = false;

        // Wait some time before start disappearing
        yield return new WaitForSeconds(disappearTime);

        float startTime = Time.timeSinceLevelLoad;

        // Fade out time
        while (fade && fadeOut > (Time.timeSinceLevelLoad - startTime))
        {
            if (fade)
            {
                float alpha = Mathf.Lerp(originalAlpha, fadeAlpha, (Time.timeSinceLevelLoad - startTime) / fadeOut);
                if (render)
                {
                    Color c = originalMaterial.color;
                    c.a = alpha;
                    originalMaterial.color = c;
                }
                else for (int i = 0; i < renders.Length; i++)
                    {
                        Color c = originalMaterials[i].color;
                        c.a = alpha;
                        originalMaterials[i].color = c;
                    }
            }
            yield return new WaitForEndOfFrame();
        }

        // Fully disappear and disable collision
        Disappear();

        // Wait some time before start reappearing
        yield return new WaitForSeconds(reappearTime);

        startTime = Time.timeSinceLevelLoad;

        // Fade in time
        while (fade && fadeIn > (Time.timeSinceLevelLoad - startTime))
        {
            if (fade)
            {
                float alpha = Mathf.Lerp(fadeAlpha, originalAlpha, (Time.timeSinceLevelLoad - startTime) / fadeIn);
                if (render)
                {
                    Color c = originalMaterial.color;
                    c.a = alpha;
                    originalMaterial.color = c;
                }
                else for (int i = 0; i < renders.Length; i++)
                    {
                        Color c = originalMaterials[i].color;
                        c.a = alpha;
                        originalMaterials[i].color = c;
                    }
            }
            yield return new WaitForEndOfFrame();
        }

        // Fully reappear and enable collision
        Reappear();

        // Unexpected beahviour if using both timer and collision, just in case
        onTimer = timer;
        timerStart = Time.timeSinceLevelLoad;
    }
}
