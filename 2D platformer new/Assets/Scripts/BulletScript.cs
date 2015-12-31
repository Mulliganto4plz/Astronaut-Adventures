using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    float maxSpeed = 7f;
    Transform player;
    Vector3 dir;
    Rigidbody2D rb;
    int damageIDoToThePlayer;
    Color32 color;
    float startTime;

    public void DamageToDo(int dmg)
    {
        damageIDoToThePlayer = dmg;
    }

    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        player = obj.transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
        //color = gameObject.GetComponent<SpriteRenderer>().color;
        startTime = Time.time;
        //StartCoroutine(SelfDestruct());
        //Destroy(gameObject, 7f);
        OnEnable();
    }

    void OnEnable()
    {
        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            player = obj.transform;
        }
        dir = player.position - transform.position;
        //color = gameObject.GetComponent<SpriteRenderer>().color;
        //startTime = Time.time;
        StartCoroutine(SelfDestruct());
    }

    public IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(7);
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        rb.velocity = dir.normalized * maxSpeed;
        float t = (Time.time - startTime) / 10;
        //gameObject.GetComponent<SpriteRenderer>().color = new Color32(color.r, color.g, color.b, (byte)Mathf.SmoothStep(color.a, 0, t));
    }

    void OnTriggerEnter2D(Collider2D _colInfo)
    {

        Player _player = _colInfo.GetComponent<Collider2D>().GetComponent<Player>();
        if (_player != null)
        {
            if (!_player.isInvul)
                _player.DamagePlayer(damageIDoToThePlayer);
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}