using UnityEngine;


public delegate void EnemyAttackEvent(AttackInfo Attack);

public class Enemy : MovableGridEntity
{
    protected Face.Faces nextMove = Face.NONE;
    protected Face.Faces nextAttack = Face.NONE;

    public int hp;
    public int armor = 0;
    protected Player player;

    protected CharacterSpriteManager characterSpriteManager;

    public static event EnemyAttackEvent OnEnemyAttack;

    protected override void Start()
    {
        base.Start();
        characterSpriteManager = GetComponent<CharacterSpriteManager>();
        Player.OnPlayerAttack += OnPlayerAttack;
        characterSpriteManager.Turn(Face.UP);
        player = globals.player;
    }

    public void QueueAttack(Face.Faces face)
    {
        nextAttack = face;
        characterSpriteManager.Turn(face);
        CurrentFace = face;
        characterSpriteManager.flashing = true;
    }

    protected void Attack(AttackInfo attackInfo) {
        OnEnemyAttack?.Invoke(attackInfo);
    }

    protected void OnPlayerAttack(AttackInfo attackInfo)
    {
        if (attackInfo.zone == CurrentPos && alive && !dying)
        {
            OnHurt(attackInfo);
        }
    }

    protected virtual bool Watch()
    {
        return Face.ToVector2Int(CurrentFace) + CurrentPos == player.CurrentPos;
    }

    protected virtual void OnHurt(AttackInfo attackInfo)
    {
        int damages = attackInfo.damages - armor;
        if (damages > 0)
        {
            hp -= damages;
            LastPos = CurrentPos + attackInfo.knockback;
        }
        characterSpriteManager.hit = true;
        if (hp <= 0)
        {
            characterSpriteManager.dying = true;
            dying = true;
        }
    }
}