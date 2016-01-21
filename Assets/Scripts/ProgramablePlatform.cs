using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ProgramablePlatform : MonoBehaviour
{
    private new SpriteRenderer renderer;
    private new BoxCollider2D collider;
    private Material originalMaterial;
    private float originalAlpha;

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

	void Awake ()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();

        // Get original material and alpha
        originalMaterial = renderer.material;
        originalAlpha = originalMaterial.color.a;

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
	
	void Update ()
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
            if(timerState != timerPattern[timerPosition])
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
        if(onCollide)
        {
            StartCoroutine("Fade");
        }
    }

    void Disappear()
    {
        if (!fade)
            renderer.sharedMaterial = material;
        else
        {
            Color c = originalMaterial.color;
            c.a = fadeAlpha;
            originalMaterial.color = c;
        }
        collider.enabled = false;
    }

    void Reappear()
    {
        if (!fade)
            renderer.sharedMaterial = originalMaterial;
        else
        {
            Color c = originalMaterial.color;
            c.a = originalAlpha;
            originalMaterial.color = c;
        }
        collider.enabled = true;
    }

    IEnumerator Fade()
    {
        bool timer = onTimer;
        onTimer = false;

        // Wait some time before start disappearing
        yield return new WaitForSeconds(disappearTime);

        float startTime = Time.timeSinceLevelLoad;
        Color c = originalMaterial.color;

        // Fade out time
        while (fade && fadeOut > (Time.timeSinceLevelLoad - startTime))
        {
            if (fade)
            {
                c.a = Mathf.Lerp(originalAlpha, fadeAlpha, (Time.timeSinceLevelLoad - startTime) / fadeOut);
                originalMaterial.color = c;
            }
            yield return new WaitForEndOfFrame();
        }

        // Fully disappear and disable collision
        Disappear();

        // Wait some time before start reappearing
        yield return new WaitForSeconds(reappearTime);

        startTime = Time.timeSinceLevelLoad;
        c = originalMaterial.color;

        // Fade in time
        while (fade && fadeIn > (Time.timeSinceLevelLoad - startTime))
        {
            if (fade)
            {
                c.a = Mathf.Lerp(fadeAlpha, originalAlpha, (Time.timeSinceLevelLoad - startTime) / fadeIn);
                originalMaterial.color = c;
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
