using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] Transform FollowTarget;
    [SerializeField] SpriteRenderer Background;

    [Range(0.1f, 8f)]
    [SerializeField] float speed = 2f;

    Camera cam;
    Rect bounds;

    protected void Start() {
        cam = GetComponent<Camera>();
        bounds = computeBounds();
    }

    protected void Update() {
        if (FollowTarget == null) {
            return;
        }

        moveCameraLerp();
        clampCamera();
    }

    Rect computeBounds() {
        float vExtent = cam.orthographicSize;
        float hExtent = vExtent * Screen.width / Screen.height;

        float left = Background.bounds.min.x + hExtent;
        float bottom = Background.bounds.min.y + vExtent;
        float width = Background.size.x - hExtent * 2;
        float height = Background.size.y - vExtent * 2;

        return new Rect(left, bottom, width, height);
    }

    void moveCameraLerp() {
        Vector3 position = transform.position;

        position.x = Mathf.Lerp(position.x, FollowTarget.transform.position.x, speed * Time.deltaTime);
        position.y = Mathf.Lerp(position.y, FollowTarget.transform.position.y, speed * Time.deltaTime);

        transform.position = position;
    }

    void clampCamera() {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, bounds.xMin, bounds.xMax);
        position.y = Mathf.Clamp(position.y, bounds.yMin, bounds.yMax);

        transform.position = position;
    }
}
