using UnityEngine;

public class GridEntity : MyMonoBehaviour
{
    public Vector2Int CurrentPos { get; protected set; }
    public bool ghosted;
    public bool alive = true;
    public bool playerOnlyPass;
    public bool dying = false;

    protected virtual void Start()
    {
        Vector3 pos = transform.position;
        CurrentPos = new((int)pos.x, (int)pos.y);
        transform.position = new Vector3(CurrentPos.x, CurrentPos.y, 0);
        globals.gridSystem.gridEntities.Add(this);
    }
}