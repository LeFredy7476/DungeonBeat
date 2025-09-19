using UnityEngine;

public class Key : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Door door;
    public Collider2D player;

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<BoxCollider2D>().IsTouching(player))
        {
            GetComponent<Animator>().SetTrigger("Pick");
            door.Unlock();
        }
    }
}
