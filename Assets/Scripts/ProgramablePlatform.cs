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
        originalMaterial = renderer.material;
        originalAlpha = originalMaterial.color.a;
        collider = GetComponent<BoxCollider2D>();
        if (timerPattern.Length > 0)
        {
            timerState = timerPattern[0];
            if (timerState)
                Reappear();
        }
        timerStart = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (onTimer)
        {
            float time = Time.timeSinceLevelLoad - timerStart;
            int patternSize = timerPattern.Length;
            time = time * patternSize;
            int timerPosition = Mathf.FloorToInt(time / timerPeriod) % patternSize;

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


        yield return new WaitForSeconds(disappearTime);

        float startTime = Time.timeSinceLevelLoad;
        Color c = originalMaterial.color;
        while (fade && fadeOut > (Time.timeSinceLevelLoad - startTime))
        {
            if (fade)
            {
                c.a = Mathf.Lerp(originalAlpha, fadeAlpha, (Time.timeSinceLevelLoad - startTime) / fadeOut);
                originalMaterial.color = c;
            }
            yield return new WaitForEndOfFrame();
        }
        Disappear();

        yield return new WaitForSeconds(reappearTime);

        startTime = Time.timeSinceLevelLoad;
        c = originalMaterial.color;
        while (fade && fadeIn > (Time.timeSinceLevelLoad - startTime))
        {
            if (fade)
            {
                c.a = Mathf.Lerp(fadeAlpha, originalAlpha, (Time.timeSinceLevelLoad - startTime) / fadeIn);
                originalMaterial.color = c;
            }
            yield return new WaitForEndOfFrame();
        }
        Reappear();

        onTimer = timer;
        timerStart = Time.timeSinceLevelLoad;
    }
}
