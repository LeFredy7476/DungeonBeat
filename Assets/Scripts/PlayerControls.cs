
using UnityEngine;

public class PlayerControls : EntityControls
{

    public AudioSource audioSource;
    public AudioClip[] slashes;
    [Range(0f, 1f)] public float slashVol;
    public AudioClip[] footsteps;

    Face.Faces nextMove = Face.NONE;
    Face.Faces nextSlash = Face.NONE;
    bool nextDash = false;

    public GameObject sword;
    public SwordHolder swordHolder;

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
        if (Face.IsNone(nextSlash))
        {
            if (Face.NotNone(nextMove)) face = nextMove;
            ApplyLook();
            swordHolder.Turn(face);
        }
        else
        {
            face = nextSlash;
            ApplyLook();
            swordHolder.Turn(face);
            swordHolder.Slash(face);
            MapPresence facingPresence = globals.TestForPresence(GetFacingTile());
            if (facingPresence != null && facingPresence.entity != null)
            {
                if (facingPresence.entity.alignment == Alignment.EVIL)
                {
                    bool hit = facingPresence.entity.ReceiveDamage(1, this);
                }
            }
        }
        nextMove = Face.NONE;
        nextSlash = Face.NONE;
        nextDash = false;
    }

    public void OnMoveUp()
    {
        nextMove = Face.UP;
    }
    public void OnMoveDown()
    {
        nextMove = Face.DOWN;
    }
    public void OnMoveLeft()
    {
        nextMove = Face.LEFT;
    }
    public void OnMoveRight()
    {
        nextMove = Face.RIGHT;
    }

    public void OnSlashUp()
    {
        nextSlash = Face.UP;
    }
    public void OnSlashDown()
    {
        nextSlash = Face.DOWN;
    }
    public void OnSlashLeft()
    {
        nextSlash = Face.LEFT;
    }
    public void OnSlashRight()
    {
        nextSlash = Face.RIGHT;
    }

    public void OnDash()
    {
        nextDash = true;
    }

    public void ApplySword()
    {
        
    }

    public void TurnSword()
    {
        
    }

    public void Tick8()
    {
        if (globals.currentTick8 == 2 || globals.currentTick8 == 6)
        {
            hit = false;
        }
    }
}
