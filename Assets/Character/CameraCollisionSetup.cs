using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionSetup : MonoBehaviour
{
    public EdgeCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        Vector2[] points = new Vector2[5];

        points[0] = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        points[1] = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
        points[2] = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        points[3] = Camera.main.ViewportToWorldPoint(new Vector2(1, 0));
        points[4] = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        collider.points = points;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
