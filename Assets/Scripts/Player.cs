using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class AttackInfo
{
    public GridEntity sender;
    public Vector2Int zone;
    public Face.Faces origin;
    public int damages;
    public Vector2 knockback;
    public AttackInfo(GridEntity sender, Vector2Int zone, Face.Faces origin, int damages, Vector2 knockback)
    {
        this.sender = sender;
        this.zone = zone;
        this.origin = origin;
        this.damages = damages;
        this.knockback = knockback;
    }
}

public delegate void PlayerAttackEvent(AttackInfo Attack);

public class Player : MovableGridEntity
{
    private Face.Faces nextMove = Face.NONE;
    private Face.Faces nextAttack = Face.NONE;
    protected SwordHolder swordHolder;
    protected CharacterSpriteManager characterSpriteManager;
    
    public int hp = 10;
    public int armor = 0;

    protected override void Start()
    {
        base.Start();
        characterSpriteManager = GetComponent<CharacterSpriteManager>();
        swordHolder = GetComponent<SwordHolder>();
        Enemy.OnEnemyAttack += OnEnemyAttack;
    }

    public static event PlayerAttackEvent OnPlayerAttack;

    public void QueueMove(Face.Faces face)
    {
        nextMove = face;
    }

    public void QueueAttack(Face.Faces face)
    {
        nextAttack = face;
    }

    public void TickPlayer()
    {
        // Debug.Log("nextMove:" + nextMove + " nextAttack:" + nextAttack);
        if (Face.NotNone(nextMove))
        {
            // TODO: apply move
            int possibleMove = AttemptMove(nextMove, 1);
            // do possible move verif
            if (possibleMove != 0)
            {
                ApplyMove(nextMove, possibleMove);
            }
            if (Face.IsNone(nextAttack))
            {
                characterSpriteManager.Turn(nextMove);
                swordHolder.Turn(nextMove);
            }
        }
        if (Face.NotNone(nextAttack))
        {
            // TODO: do a Attack
            characterSpriteManager.Turn(nextAttack);
            swordHolder.Attack(nextAttack);
            OnPlayerAttack?.Invoke(new AttackInfo(
                this,
                CurrentPos + Face.ToVector2Int(nextAttack),
                Face.Inverse(nextAttack),
                1,
                Face.ToVector2(nextAttack) * 0.2f
            ));
        }
        nextMove = Face.NONE;
        nextAttack = Face.NONE;
    }

    private void OnEnemyAttack(AttackInfo attackInfo)
    {
        if (attackInfo.zone == CurrentPos)
        {
            OnHurt(attackInfo);
        }
    }

    protected virtual void OnHurt(AttackInfo attackInfo)
    {
        int damages = attackInfo.damages - armor;
        if (damages > 0)
        {
            hp -= damages;
            LastPos = CurrentPos + attackInfo.knockback;
        }
        if (hp <= 0)
        {
            SceneManager.LoadScene(0);
        }
        characterSpriteManager.hit = true;
    }
}