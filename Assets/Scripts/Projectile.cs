using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    public Vector3 direction;
    public float speed ;
    public System.Action destroyed;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void CheckCollision(Collider2D other)
    {
        Bunker bunker = other.gameObject.GetComponent<Bunker>();

        if (bunker == null || bunker.CheckCollision(boxCollider, transform.position))
        {
            Destroy(gameObject);
        }
    }
}
