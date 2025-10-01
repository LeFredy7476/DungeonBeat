using UnityEngine;
using System.Collections.Generic;

public class MovableGridEntity : GridEntity
{
    public Vector2 LastPos { get; private set; }
    protected void Start()
    {
        base.Start();
        LastPos = CurrentPos;
    }
}