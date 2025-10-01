using UnityEngine;
using System.Collections.Generic;

public class GridEntity : MonoBehaviour
{
    public Vector2Int CurrentPos { get; private set; }

    protected void Start()
    {
        Vector3 pos = transform.position;
        CurrentPos = new((int)pos.x, (int)pos.y);
    }
}