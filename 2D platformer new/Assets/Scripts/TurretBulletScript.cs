using UnityEngine;

public class TurretBulletScript : MonoBehaviour
{

    Vector2 direction;
    Rigidbody2D rb;
    public float mSpeed = 3;
    int damageIDoToThePlayer = 4;

    // Use this for initialization
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

    }

    public void SetDirectionAndSpeed(float x, float y, float speed)
    {
        direction = new Vector2(x, y);
        mSpeed = speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = direction * mSpeed;
    }

    void OnTriggerEnter2D(Collider2D _colInfo)
    {
        Player _player = _colInfo.GetComponent<Collider2D>().GetComponent<Player>();
        if (_player != null)
        {
            if (!_player.isInvul)
                _player.DamagePlayer(damageIDoToThePlayer);
        }
        if (!_colInfo.isTrigger)
            gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
