using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Range(4f, 16f)]
    [SerializeField] float Speed = 8f;
    [SerializeField] SpriteRenderer Background;
    [SerializeField] GameObject explosion;

    Collider2D col;
    Rect bounds;
    bool canMove = true;

    void Awake() {
        col = GetComponentInChildren<Collider2D>();
        bounds = computeBounds();
    }

    void Update() {
        movePlayer();
        limitPlayer();
    }

    void OnCollisionEnter2D(Collision2D _) {
        die();
    }

    void die() {
        canMove = false;
        GetComponentInChildren<Renderer>().enabled = false;
        StartCoroutine(restartScene());
        Instantiate(explosion, this.transform.position, Quaternion.identity);
    }

    // TODO: update to send a player died event
    // TODO: in GameManager, listen for this event
    // TODO: and reload the scene or do something else
    IEnumerator restartScene() {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.RestartScene();
    }

    void movePlayer() {
        if (!canMove) {
            return;
        }

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
