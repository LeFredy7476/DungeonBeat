using UnityEngine;
using System;

public class MovableGridEntity : GridEntity
{
    public Vector2 LastPos { get; protected set; }
    public Face.Faces CurrentFace { get; protected set; }

    protected override void Start()
    {
        base.Start();
        LastPos = CurrentPos;
        CurrentFace = Face.UP;
    }

    protected virtual void UpdatePosition()
    {
        float ease = GetEase();
        Vector2 pos = (1 - ease) * LastPos + ease * (Vector2)CurrentPos;
        if (dying)
        {
            Vector2 newLast = (LastPos - CurrentPos) * 10.0f + CurrentPos;
            pos = (1 - ease) * (Vector2)CurrentPos + ease * newLast;
            transform.eulerAngles = new Vector3(0, 0, ease * 30.0f);
            float scale = 1.0f + ease * 0.5f;
            transform.localScale = new Vector3(scale, scale, scale);
        }
        transform.position = pos;
    }

    float GetEase()
    {
        float tickProgression = (float)globals.tickProgression;
        float easeProgression = Math.Min(tickProgression * 2, 1);
        return 1 - (float)Math.Pow(1 - easeProgression, 4);
    }

    public virtual void ApplyMove(Face.Faces face, int length)
    {
        CurrentPos += Face.ToVector2Int(face) * length;
    }

    public virtual int AttemptMove(Face.Faces face, int length)
    {
        Vector2Int directionVector = Face.ToVector2Int(face);
        int trueLength = 0;
        for (int i = 1; i <= length; i++)
        {
            Vector2Int posToCheck = CurrentPos + i * directionVector;
            if (globals.gridSystem.CanWalk(posToCheck, this))
            {
                trueLength = i;
            }
            else break;
        }
        return trueLength;
    }

    public void TickSystem()
    {
        if (!dying)
        {
            LastPos = CurrentPos;
        }
    }

    protected virtual void Update()
    {
        UpdatePosition();
    } 
}