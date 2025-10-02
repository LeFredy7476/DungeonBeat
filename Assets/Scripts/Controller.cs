using UnityEngine;

public class Controller : MyMonoBehaviour
{
    Player player;
    void Start()
    {
        player = GetComponent<Player>();
    }

    public void OnMoveUp() => player.QueueMove(Face.UP);
    public void OnMoveDown() => player.QueueMove(Face.DOWN);
    public void OnMoveLeft() => player.QueueMove(Face.LEFT);
    public void OnMoveRight() => player.QueueMove(Face.RIGHT);

    public void OnSlashUp() => player.QueueAttack(Face.UP);
    public void OnSlashDown() => player.QueueAttack(Face.DOWN);
    public void OnSlashLeft() => player.QueueAttack(Face.LEFT);
    public void OnSlashRight() => player.QueueAttack(Face.RIGHT);
}