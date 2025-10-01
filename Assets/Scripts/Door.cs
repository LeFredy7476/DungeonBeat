using UnityEngine;

public class Door : EntityControls
{
    Globals globals;



    void Start()
    {
        globals = Globals.Instance;
        current = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        last = new Vector2(transform.position.x, transform.position.y);
        mapPresence = new MapPresence(this, false);
        globals.mapPresences.Add(mapPresence);
        alignment = Alignment.NEUTRAL;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Unlock()
    {
        mapPresence.passable = true;
        GetComponent<Animator>().SetTrigger("Unlock");
    }
}
