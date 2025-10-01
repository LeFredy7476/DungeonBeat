using UnityEngine;

public class CameraZone : MonoBehaviour
{

    public CameraMan cameraMan;
    public Transform target;
    public Transform anchor;
    public float zoom = 4;
    public Collider2D player;

    // Update is called once per frame
    public void TickPlayerLate()
    {
        // Debug.Log(GetComponent<BoxCollider2D>().IsTouching(player));
        if (GetComponent<BoxCollider2D>().IsTouching(player))
        {
            cameraMan.anchor = anchor;
            cameraMan.target = target;
            cameraMan.zoom = zoom;
        }

    }
}
