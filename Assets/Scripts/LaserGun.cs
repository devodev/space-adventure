using UnityEngine;

public class LaserGun : MonoBehaviour {

    [SerializeField] Bullet BulletPrefab;
    [SerializeField] SpriteRenderer Background;
    [SerializeField] Transform ShootPoint;
    [SerializeField] KeyCode ShootKey = KeyCode.Space;

    [Range(10f, 200f)]
    [SerializeField] float ShootDelayMs = 100f;

    [Range(0.1f, 5f)]
    [SerializeField] float VelocityMultiplier = 0.4f;

    [Range(0.1f, 1f)]
    [SerializeField] float VelocityInterpolationFactor = 0.1f;

    float lastShotPeriod;
    Vector3 lastPos;
    Vector3 velocity;

    protected void Start() {
        lastPos = transform.position;
    }

    protected void Update() {
        computeVelocity();
        if (shouldShoot()) {
            shootBullet();
        }
    }

    void computeVelocity() {
        Vector3 newVelocity = (transform.position - lastPos) / Time.deltaTime;
        velocity = Vector3.Lerp(velocity, newVelocity, VelocityInterpolationFactor);
        lastPos = transform.position;
    }

    bool shouldShoot() {
        if (lastShotPeriod > ShootDelayMs / 1000 && Input.GetKey(ShootKey)) {
            lastShotPeriod = 0;
            return true;
        }
        lastShotPeriod += Time.deltaTime;
        return false;
    }

    void shootBullet() {
        Bullet bullet = Instantiate(BulletPrefab, ShootPoint.position, ShootPoint.rotation);
        bullet.Background = Background;
        bullet.VelocityMultiplier = VelocityMultiplier;
        bullet.VelocityAdder = velocity;
    }
}
