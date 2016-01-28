using UnityEngine;
using System.Collections;


public class Health : MonoBehaviour
{
    public GameObject heartPrefab;
    public float heartSize;
    public Vector3 heartOffset;
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
    private Stack hearts;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        life = GetComponent<Life>();
        Renderer renderer = GetComponent<SpriteRenderer>();
        if (renderer == null)
            renderer = GetComponent<MeshRenderer>();
            
        if (renderer)
            originalMaterial = renderer.material;
        if (!heartPrefab)
            return;

        hearts = new Stack();
        for (int i = 0; i < maxHitPoints; i++)
        {
            GameObject heart = Instantiate(heartPrefab);
            heart.transform.SetParent(gameObject.transform, false);
            heart.transform.localPosition = new Vector3(i * heartSize, 0, 0) + heartOffset;
            hearts.Push(heart);
        }
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
        if (!life)
            Destroy(gameObject);
        else
        {
            currentHitPoints = maxHitPoints;
            life.Die();
        }
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

            if (heartPrefab)
                for (int i = 0; i < sourceDamage.hitPointDamage; i++)
                    Destroy((GameObject)hearts.Pop());

            currentHitPoints -= sourceDamage.hitPointDamage;
            Invulnerable();
        }
    }
}
