using UnityEngine;

public class Door : EntityControls
{
    Globals globals;



    void Start()
    {
        globals = Globals.Instance;
        currentX = (int)transform.position.x;
        currentY = (int)transform.position.y;
        lastX = (int)transform.position.x;
        lastY = (int)transform.position.y;
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
