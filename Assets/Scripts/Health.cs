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
        UpdateHP();
    }

    void UpdateHP()
    {

        if (heartPrefab)
        {
            if(hearts.Count < currentHitPoints)
            {
                int size = currentHitPoints - hearts.Count;
                for (int i = 0; i < size; i++)
                {
                    GameObject heart = Instantiate(heartPrefab);
                    heart.transform.SetParent(gameObject.transform, false);
                    hearts.Push(heart);
                }
            }
            else
            {
                int size = hearts.Count - currentHitPoints;
                for (int i = 0; i < size; i++)
                    Destroy((GameObject)hearts.Pop());
            }
            object[] array = hearts.ToArray();
            for (int i = 0; i < hearts.Count; i++)
                ((GameObject)array[i]).transform.localPosition = new Vector3((i - array.Length / 2f) * heartSize, 0f, 0f) + heartOffset;
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

        while (Time.time < time + invulnerableTime && isInvulnerable)
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
            isInvulnerable = false;
            life.Die();
        }
    }

    public void Damage(GameObject source, Damage sourceDamage)
    {
        if (sourceDamage.instaKill)
            Kill();
        else if (isInvulnerable)
            return;
        else if (sourceDamage.hitPointDamage < 0 || currentHitPoints - sourceDamage.hitPointDamage < 1)
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
            UpdateHP();
            Invulnerable();
        }
    }
}
