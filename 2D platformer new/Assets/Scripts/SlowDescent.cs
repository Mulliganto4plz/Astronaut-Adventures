using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class SlowDescent : MonoBehaviour {

    Vector2 direction = new Vector2(0, -1);
    Rigidbody2D rb;
    public float speed = 0.9f;
    public float descentEnd;

    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

	void FixedUpdate ()
    {
        rb.velocity = direction * speed;
    }
}
