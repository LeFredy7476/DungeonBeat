using System;
using Unity.VisualScripting;
using UnityEngine;

public class TrollControls : EntityControls
{
    readonly Face.Faces[] moves = { Face.UP, Face.DOWN, Face.LEFT, Face.RIGHT };
    Face.Faces nextMove = Face.NONE;

    public EntityControls aggro = null;
    public int aggroCooldown = 0;


    public int attackCooldown { get; private set; } = 0;
    public bool attack { get; private set; } = false;

    public MapPresence[] ennemies;

    Globals globals;

    void Start()
    {
        globals = Globals.Instance;
        current = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        last = new Vector2(transform.position.x, transform.position.y);
        mapPresence = new MapPresence(this, false);
        globals.mapPresences.Add(mapPresence);
        alignment = Alignment.EVIL;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        UpdateColor();
        Refresh();
    }

    public override bool ReceiveDamage(int amount, Vector2Int direction, EntityControls source)
    {
        health -= amount;
        health = Math.Max(0, health);
        hit = true;
        aggroCooldown = 10;
        aggro = source;
        return true;
    }

    public void TickMonster()
    {
        FlushLast();
        if (health != 0)
        {
            if (attack)
            {
                last += 0.5f * Face.ToVector2(face);
                MapPresence facingPresence = globals.TestForPresence(GetFacingTile());
                if (facingPresence != null && facingPresence.entity != null)
                {
                    if (facingPresence.entity.alignment == Alignment.GOOD)
                    {
                        facingPresence.entity.ReceiveDamage(1, this);
                        aggro = facingPresence.entity;
                        aggroCooldown = 10;
                    }
                }
                flashing = false;
                attack = false;
                attackCooldown = 3;
            }
            else
            {
                int moveScale = 1;
                int delta = 0;
                if (aggro != null)
                {
                    if (aggro.current.x < current.x)
                    {
                        nextMove = Face.LEFT;
                        delta = ApplyMovement(nextMove, moveScale, false);
                    }
                    else if (aggro.current.x > current.x)
                    {
                        nextMove = Face.RIGHT;
                        delta = ApplyMovement(nextMove, moveScale, false);
                    }

                    if (delta == 0)
                    {
                        if (aggro.current.y < current.y)
                        {
                            nextMove = Face.DOWN;
                            delta = ApplyMovement(nextMove, moveScale, false);
                        }
                        else if (aggro.current.y > current.y)
                        {
                            nextMove = Face.UP;
                            delta = ApplyMovement(nextMove, moveScale, false);
                        }
                    }
                }
                else
                {
                    int rng = (int)UnityEngine.Random.Range(0.0f, 8.0f);
                    if (rng < 4)
                    {
                        nextMove = moves[rng];
                    }
                    delta = ApplyMovement(nextMove, moveScale, false);
                }
                if (Face.NotNone(nextMove)) face = nextMove;
                ApplyLook();
                if (delta == 0 && attackCooldown <= 0)
                {
                    MapPresence facingPresence = globals.TestForPresence(GetFacingTile());
                    if (facingPresence != null && facingPresence.entity != null)
                    {
                        if (facingPresence.entity.alignment == Alignment.GOOD)
                        {
                            attack = true;
                            flashing = true;
                            aggro = facingPresence.entity;
                        }
                    }
                }
            }
            nextMove = Face.NONE;
            attackCooldown -= 1;
            aggroCooldown -= 1;
            if (aggroCooldown <= 0)
            {
                aggro = null;
            }
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