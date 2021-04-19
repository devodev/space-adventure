using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Damageable : MonoBehaviour {

    [Range(0.01f, 10f)]
    [SerializeField] float damageTakenAnimationDelay = 0.05f;
    [SerializeField] GameObject hitAnimation;
    [SerializeField] GameObject explosion;

    public float StartHealth { get; set; } = 100f;
    public float Health { get; private set; }

    List<(float, Color)> thresholds;
    bool damageTaken = false;
    Color initialColor;
    float damageTakenColorSetSince = 0f;

    Renderer rend;

    // TakeDamage substracts damage from health
    // and returns the health value.
    public float TakeDamage(float damage, Vector3 position) {
        if (Health <= 0f) {
            return 0f;
        }

        showHit(position);

        Health -= damage;

        if (Health <= 0f) {
            Health = 0f;
            die();
        } else {
            updateColor();
        }

        return Health;
    }

    void Awake() {
        rend = GetComponentInChildren<Renderer>();

        thresholds = new List<(float, Color)>();
        thresholds.Add((StartHealth / 4, Color.red));
        thresholds.Add((StartHealth / 2, Color.yellow));
        thresholds.Add((StartHealth, Color.gray));

        initialColor = rend.material.color;
    }

    void Start() {
        Health = StartHealth;
    }

    void Update() {
        if (damageTaken) {
            damageTakenColorSetSince += Time.deltaTime;

            if (damageTakenColorSetSince >= damageTakenAnimationDelay) {
                damageTaken = false;
                damageTakenColorSetSince = 0f;
                rend.material.color = initialColor;
            }
        }
    }

    void die() {
        GetComponentInChildren<Renderer>().enabled = false;
        StartCoroutine(delayDestroy());
        Instantiate(explosion, this.transform.position, Quaternion.identity);
    }

    IEnumerator delayDestroy() {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void updateColor() {
        foreach ((float, Color) threshold in thresholds) {
            if (Health <= threshold.Item1) {
                damageTaken = true;
                damageTakenColorSetSince = 0f;
                rend.material.color = threshold.Item2;
                break;
            }
        }
    }

    void showHit(Vector3 position) {
        Instantiate(hitAnimation, position, Quaternion.identity);
    }
}
