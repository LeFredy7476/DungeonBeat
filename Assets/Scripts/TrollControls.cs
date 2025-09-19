using System;
using Unity.VisualScripting;
using UnityEngine;

public class TrollControls : EntityControls
{
    string[] moves = { "up", "down", "left", "right" };
    string nextMove = "";

    public EntityControls aggro = null;
    public int aggroCooldown = 0;


    public int attackCooldown { get; private set; } = 0;
    public bool attack { get; private set; } = false;

    public MapPresence[] ennemies;

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
                if (face == "up") lastY += 0.5f;
                else if (face == "down") lastY -= 0.5f;
                else if (face == "left") lastX -= 0.5f;
                else if (face == "right") lastX += 0.5f;
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
                    if (aggro.currentX < currentX)
                    {
                        nextMove = "left";
                        delta = ApplyMovement(nextMove, moveScale, false);
                    }
                    else if (aggro.currentX > currentX)
                    {
                        nextMove = "right";
                        delta = ApplyMovement(nextMove, moveScale, false);
                    }

                    if (delta == 0)
                    {
                        if (aggro.currentY < currentY)
                        {
                            nextMove = "down";
                            delta = ApplyMovement(nextMove, moveScale, false);
                        }
                        else if (aggro.currentY > currentY)
                        {
                            nextMove = "up";
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
                if (nextMove != "") face = nextMove;
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
            nextMove = "";
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