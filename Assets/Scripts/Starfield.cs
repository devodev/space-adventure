// https://guidohenkel.com/2018/05/endless_starfield_unity/

using UnityEngine;

public class Starfield : MonoBehaviour {

    [SerializeField] int MaxStars = 100;
    [SerializeField] float StarSize = 0.1f;
    [SerializeField] float StarSizeRange = 0.5f;
    [SerializeField] float ParallaxFactor = 0f;
    [SerializeField] float ScrollFactor = 0f;
    [SerializeField] bool Colorize = false;
    [SerializeField] Camera Cam;

    float fieldWidth;
    float fieldHeight;
    float xOffset;
    float yOffset;

    ParticleSystem Particles;
    ParticleSystem.Particle[] Stars;

    void Awake() {
        Particles = GetComponent<ParticleSystem>();

        fieldHeight = Cam.orthographicSize * 2f;
        fieldWidth = fieldHeight * Cam.aspect;

        xOffset = fieldWidth * 0.5f;
        yOffset = fieldHeight * 0.5f;

        Stars = new ParticleSystem.Particle[MaxStars];

        for (int i = 0; i < MaxStars; i++) {
            float randSize = Random.Range(StarSizeRange, StarSizeRange + 1f);
            float scaledColor = (true == Colorize) ? randSize - StarSizeRange : 1f;

            Stars[i].position = GetRandomPosInRectangle(fieldWidth, fieldHeight) + transform.position;
            Stars[i].startSize = StarSize * randSize;
            Stars[i].startColor = new Color(1f, scaledColor, scaledColor, 1f); // redshift
        }
        Particles.SetParticles(Stars, Stars.Length);
    }

    void Update() {
        parallaxMove();

        for (int i = 0; i < MaxStars; i++) {
            Vector3 pos = Stars[i].position + transform.position;

            // Scroll the stars to the left
            pos += Vector3.left * (ScrollFactor / 100f);

            // Recycle out-of-view stars
            if (pos.x < (Cam.transform.position.x - xOffset)) {
                pos.x += fieldWidth;
            }
            if (pos.x > (Cam.transform.position.x + xOffset)) {
                pos.x -= fieldWidth;
            }
            if (pos.y < (Cam.transform.position.y - yOffset)) {
                pos.y += fieldHeight;
            }
            if (pos.y > (Cam.transform.position.y + yOffset)) {
                pos.y -= fieldHeight;
            }

            Stars[i].position = pos - transform.position;
        }
        Particles.SetParticles(Stars, Stars.Length);
    }

    void parallaxMove() {
        Vector3 newPos = Cam.transform.position * ParallaxFactor;
        newPos.z = 0f;
        transform.position = newPos;
    }

    Vector3 GetRandomPosInRectangle(float width, float height) {
        float x = Random.Range(0, width);
        float y = Random.Range(0, height);
        return new Vector3(x - xOffset, y - yOffset, 0);
    }
}
