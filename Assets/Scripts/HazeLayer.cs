using System.Linq.Expressions;
using System.Collections.Generic;
using UnityEngine;

public class HazeLayer : MonoBehaviour {

    [SerializeField] float ParallaxFactor = 0f;
    [SerializeField] Camera Cam;
    [SerializeField] GameObject HazePrefab;

    List<GameObject> sprites;
    Vector3 spriteBounds;
    float camHeight;
    float camWidth;
    int rowSize;
    int columnSize;

    void Awake() {
        spriteBounds = HazePrefab.GetComponent<Renderer>().bounds.size;

        camHeight = Cam.orthographicSize * 2f;
        camWidth = camHeight * Cam.aspect;

        float camPrefabDeltaX = Mathf.Max(Mathf.Floor(camWidth / spriteBounds.x) + 1, 2);
        float camPrefabDeltaY = Mathf.Max(Mathf.Floor(camHeight / spriteBounds.y) + 1, 2);

        rowSize = (int)camPrefabDeltaX * 2 - 1;
        columnSize = (int)camPrefabDeltaY * 2 - 1;

        sprites = new List<GameObject>();

        for (int i = 0; i < rowSize; i++) {
            for (int j = 0; j < columnSize; j++) {
                Vector3 multiplier = new Vector3(i - (rowSize / 2f), j - (columnSize / 2f), 0f);

                sprites.Add(Instantiate(HazePrefab, transform.position + Vector3.Scale(multiplier, spriteBounds), Quaternion.identity));
            }
        }
    }

    void Update() {
        Vector3 prevPos = transform.position;
        parallaxMove();

        for (int i = 0; i < rowSize; i++) {
            for (int j = 0; j < columnSize; j++) {
                sprites[j + i * columnSize].transform.position += transform.position - prevPos;
            }
        }
    }

    void parallaxMove() {
        Vector3 newPos = Cam.transform.position * ParallaxFactor;
        newPos.z = 0f;
        transform.position = newPos;
    }
}
