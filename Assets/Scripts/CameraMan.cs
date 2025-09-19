using System;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    public Transform target;
    public Transform anchor;
    public float zoom = 4;

    float zoomLast = 2;
    Transform targetLast;
    Transform anchorLast;

    public float ratio;

    Globals globals;

    void Start()
    {
        targetLast = target;
        anchorLast = anchor;
        globals = Globals.Instance;
    }

    float Easing(float from, float to)
    {
        float cos = (float)Math.Cos(globals.tickProgression * Math.PI);
        float t = 1 - (cos + 1) / 2;
        return (1 - t) * from + t * to;
    }

    void Update()
    {
        float targetX = target.position.x;
        float targetY = target.position.y;
        float anchorX = anchor.position.x;
        float anchorY = anchor.position.y;
        float cameraX = (1 - ratio) * anchorX + ratio * targetX;
        float cameraY = (1 - ratio) * anchorY + ratio * targetY;
        float targetLastX = targetLast.position.x;
        float targetLastY = targetLast.position.y;
        float anchorLastX = anchorLast.position.x;
        float anchorLastY = anchorLast.position.y;
        float cameraLastX = (1 - ratio) * anchorLastX + ratio * targetLastX;
        float cameraLastY = (1 - ratio) * anchorLastY + ratio * targetLastY;
        cameraX = Easing(cameraLastX, cameraX);
        cameraY = Easing(cameraLastY, cameraY);
        Vector3 cameraPos = new Vector3(cameraX, cameraY, transform.position.z);
        GetComponent<Transform>().position = cameraPos;
        GetComponent<Camera>().orthographicSize = Easing(zoomLast, zoom);
    }

    public void TickSystem()
    {
        targetLast = target;
        anchorLast = anchor;
        zoomLast = zoom;
    }
}