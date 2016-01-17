using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Life))]
public class Health : MonoBehaviour
{

    public int maxHitPoints = 3;
    public int currentHitPoints = 3;
    public bool isInvulnerable = false;
    public float invulnerableTime = 2f;
    public float fadeAlpha = 0.2f;
    public float fadeTime = 0.1f;

    private Life life;
    private Rigidbody2D rigid;
    private float timer;
    private Material originalMaterial;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        life = GetComponent<Life>();
        Renderer renderer = GetComponent<SpriteRenderer>();
        if (renderer == null)
            renderer = GetComponent<MeshRenderer>();
            
        if (renderer)
            originalMaterial = renderer.material;
    }

    void Invulnerable()
    {
        StartCoroutine("InvulnerableEffect");
    }

    IEnumerator InvulnerableEffect()
    {
        isInvulnerable = true;
        float time = Time.time;
        Color c = originalMaterial.color;
        float originalAlpha = c.a;

        while (Time.time < time + invulnerableTime)
        {
            c.a = fadeAlpha;
            originalMaterial.color = c;
            yield return new WaitForSeconds(fadeTime);
            c.a = originalAlpha;
            originalMaterial.color = c;
            yield return new WaitForSeconds(fadeTime);
        }

        isInvulnerable = false;
    }

    void Kill()
    {
        if (life)
            life.Die();
        currentHitPoints = maxHitPoints;
    }

    public void Damage(GameObject source, Damage sourceDamage)
    {
        if (isInvulnerable)
            return;
        if (sourceDamage.hitPointDamage < 0 || currentHitPoints - sourceDamage.hitPointDamage < 1)
            Kill();
        else
        {
            if (rigid)
            {
                Vector2 force = transform.position - source.transform.position;
                force.Normalize();
                force *= sourceDamage.pushForce;
                rigid.AddForce(force, ForceMode2D.Impulse);
            }
            currentHitPoints -= sourceDamage.hitPointDamage;
            Invulnerable();
        }
    }
}
