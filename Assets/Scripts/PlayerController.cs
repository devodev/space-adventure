using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Range(4f, 16f)]
    [SerializeField] float Speed = 8f;
    [SerializeField] SpriteRenderer Background;

    Collider2D col;
    Rect bounds;

    protected void Start() {
        col = GetComponentInChildren<Collider2D>();

        bounds = computeBounds();
    }

    protected void Update() {
        movePlayer();
        limitPlayer();
    }

    void movePlayer() {
        var xAxis = Input.GetAxisRaw("Horizontal");
        var yAxis = Input.GetAxisRaw("Vertical");

        var movement = new Vector3(xAxis, yAxis, 0);
        movement *= Time.deltaTime * Speed;

        this.transform.position += movement;
    }

    void limitPlayer() {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, bounds.xMin, bounds.xMax);
        position.y = Mathf.Clamp(position.y, bounds.yMin, bounds.yMax);

        transform.position = position;
    }

    Rect computeBounds() {
        return new Rect(
            Background.bounds.min.x + col.bounds.size.x / 2,
            Background.bounds.min.y + col.bounds.size.y / 2,
            Background.size.x - col.bounds.size.x,
            Background.size.y - col.bounds.size.y
        );
    }
}
