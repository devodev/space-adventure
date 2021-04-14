using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [Range(20f, 50f)]
    [SerializeField] float speed = 30f;

    [Range(1, 100f)]
    [SerializeField] float damageAmount = 10f;

    public SpriteRenderer Background { get; set; }
    public Vector2 VelocityAdder { get; set; }
    public float VelocityMultiplier { get; set; }

    Rigidbody2D rb;
    Collider2D col;
    Rect bounds;

    void Start() {
        bounds = computeBounds();
        col = GetComponent<Collider2D>();

        Vector3 velocity = transform.right * VelocityMultiplier * speed;
        Vector3 velocityToAdd = transform.right.normalized * VelocityAdder * VelocityAdder.normalized;
        Vector2 newVelocity = new Vector2(velocity.x, velocity.y) + new Vector2(velocityToAdd.x, velocityToAdd.y);

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = newVelocity;
    }

    protected void Update() {
        destroyOutsideBounds();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // we collided with a damageable object, inflict damage!
        if (collision.collider.TryGetComponent<Damageable>(out Damageable damageable)) {
            ContactPoint2D[] contacts = new ContactPoint2D[1];
            collision.GetContacts(contacts);

            damageable.TakeDamage(damageAmount, contacts[0].point);
            Destroy(gameObject);
        }
    }

    Rect computeBounds() {
        return new Rect(
            Background.bounds.min.x,
            Background.bounds.min.y,
            Background.size.x,
            Background.size.y
        );
    }

    void destroyOutsideBounds() {
        if (
            col.bounds.max.x <= bounds.xMin
         || col.bounds.min.x >= bounds.xMax
         || col.bounds.max.y <= bounds.yMin
         || col.bounds.min.y >= bounds.yMax) {
            Destroy(gameObject);
        }
    }
}
