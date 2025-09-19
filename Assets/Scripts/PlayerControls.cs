
using UnityEngine;

public class PlayerControls : EntityControls
{

    public AudioSource audioSource;
    public AudioClip[] slashes;
    [Range(0f, 1f)] public float slashVol;
    public AudioClip[] footsteps;

    string nextMove = "";
    string nextSlash = "";
    bool nextDash = false;

    public GameObject sword;

    public Sprite swordUp;
    public Sprite swordDown;
    public Sprite swordLeft;
    public Sprite swordRight;

    Globals globals;
    void Start()
    {
        globals = Globals.Instance;
        mapPresence = new MapPresence(this, false);
        globals.mapPresences.Add(mapPresence);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        UpdateColor();
        Refresh();
    }

    public void TickPlayer()
    {
        FlushLast();
        int moveScale = nextDash ? 2 : 1;
        ApplyMovement(nextMove, moveScale, false);
        if (nextSlash.Equals(""))
        {
            if (nextMove != "") face = nextMove;
            ApplyLook();
            ApplySword();
            TurnSword();
        }
        else
        {
            face = nextSlash;
            ApplyLook();
            TurnSword();
            ApplySword();
            int currentTick = globals.currenTick % 2;
            sword.GetComponent<Animator>().SetBool("Reverse", currentTick == 1);
            sword.GetComponent<Animator>().SetTrigger("Slash");
            audioSource.pitch = Random.Range(0.75f, 1.25f);
            audioSource.PlayOneShot(slashes[currentTick], slashVol);
            MapPresence facingPresence = globals.TestForPresence(GetFacingTile());
            if (facingPresence != null && facingPresence.entity != null)
            {
                if (facingPresence.entity.alignment == Alignment.EVIL)
                {
                    bool hit = facingPresence.entity.ReceiveDamage(1, this);
                }
            }
        }
        nextMove = "";
        nextSlash = "";
        nextDash = false;
    }

    public void OnMoveUp()
    {
        nextMove = "up";
    }
    public void OnMoveDown()
    {
        nextMove = "down";
    }
    public void OnMoveLeft()
    {
        nextMove = "left";
    }
    public void OnMoveRight()
    {
        nextMove = "right";
    }

    public void OnSlashUp()
    {
        nextSlash = "up";
    }
    public void OnSlashDown()
    {
        nextSlash = "down";
    }
    public void OnSlashLeft()
    {
        nextSlash = "left";
    }
    public void OnSlashRight()
    {
        nextSlash = "right";
    }

    public void OnDash()
    {
        nextDash = true;
    }

    public void ApplySword()
    {
        if (face.Equals("up"))
        {
            sword.GetComponent<SpriteRenderer>().sprite = swordUp;
            sword.GetComponent<Animator>().SetInteger("Face", 0);
        }
        else if (face.Equals("down"))
        {
            sword.GetComponent<SpriteRenderer>().sprite = swordDown;
            sword.GetComponent<Animator>().SetInteger("Face", 1);
        }
        else if (face.Equals("left"))
        {
            sword.GetComponent<SpriteRenderer>().sprite = swordLeft;
            sword.GetComponent<Animator>().SetInteger("Face", 2);
        }
        else if (face.Equals("right"))
        {
            sword.GetComponent<SpriteRenderer>().sprite = swordRight;
            sword.GetComponent<Animator>().SetInteger("Face", 3);
        }
    }

    public void TurnSword()
    {
        if (face.Equals("up"))
        {
            sword.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 0);
        }
        else if (face.Equals("down"))
        {
            sword.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 180);
        }
        else if (face.Equals("left"))
        {
            sword.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 90);
        }
        else if (face.Equals("right"))
        {
            sword.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, -90);
        }
    }

    public void Tick8()
    {
        if (globals.currentTick8 == 2 || globals.currentTick8 == 6)
        {
            hit = false;
        }
    }
}
