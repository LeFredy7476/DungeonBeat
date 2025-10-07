using System;
using UnityEngine;

public class Troll : Enemy
{

    private int aggressivity = 0;
    public AudioClip attackAudio;
    [Range(0f, 1f)] public float attackVol;
    public AudioClip hurtAudio;
    [Range(0f, 1f)] public float hurtVol;
    AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    void TickMonster()
    {
        if (hp > 0)
        {
            // --- execute attack ---
            if (Face.NotNone(nextAttack))
            {
                Attack(new AttackInfo(
                    this,
                    CurrentPos + Face.ToVector2Int(nextAttack),
                    Face.Inverse(nextAttack),
                    1,
                    Face.ToVector2(nextAttack) * 0.5f
                ));
                audioSource.PlayOneShot(attackAudio, attackVol);
                LastPos += Face.ToVector2(CurrentFace) * 0.5f;
                nextAttack = Face.NONE;
            }
            // --- follow player ---
            else if (aggressivity > 0)
            {
                Vector2Int diff = player.CurrentPos - CurrentPos;
                int dist = Math.Abs(diff.x) + Math.Abs(diff.y);
                Face.Faces face;
                if (dist == 1)
                {
                    face = Face.PathFind(diff);
                    QueueAttack(face);
                }
                else
                {
                    int delta = 0;
                    int attempt = 3;
                    do
                    {
                        face = Face.PathFind(diff);
                        delta = AttemptMove(face, 1);
                        attempt--;
                    }
                    while (delta == 0 && attempt > 0);
                    ApplyMove(face, delta);
                    characterSpriteManager.Turn(face);
                }
                --aggressivity;
            }
            // --- random move ---
            else
            {
                if (Watch())
                {
                    aggressivity = 5;
                    QueueAttack(CurrentFace);
                }
                else
                {
                    if (Face.NotNone(nextMove))
                    {
                        int delta = AttemptMove(nextMove, 1);
                        ApplyMove(nextMove, delta);
                        characterSpriteManager.Turn(nextMove);
                        CurrentFace = nextMove;
                        if (delta == 0 && Watch())
                        {
                            aggressivity = 5;
                            QueueAttack(CurrentFace);
                        }
                        nextMove = Face.NONE;
                    }
                    int moveChance = (int)UnityEngine.Random.Range(0.0f, 2.0f); // chance to move (50%)
                    if (moveChance < 1)
                    {
                        nextMove = Face.RandomFace();
                    }
                }
            }
        }
        else
        {
            if (alive)
            {
                alive = false;
                globals.gridSystem.gridEntities.Remove(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    protected override void OnHurt(AttackInfo attackInfo)
    {
        base.OnHurt(attackInfo);
        aggressivity = 10;
        audioSource.PlayOneShot(hurtAudio, hurtVol);
        
    }
}