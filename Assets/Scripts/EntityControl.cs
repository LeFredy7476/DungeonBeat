using System;
using UnityEngine;

public enum Alignment
{
    GOOD,
    NEUTRAL,
    EVIL
}

public class EntityControls : MonoBehaviour
{
    public int health;
    public Alignment alignment;

    public Vector2Int current;
    public Vector2 last;

    readonly Color WHITE = Color.white;
    readonly Color BLACK = new Color(24.0f / 255, 20.0f / 255, 37.0f / 255);

    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    public Sprite upBgSprite;
    public Sprite downBgSprite;
    public Sprite leftBgSprite;
    public Sprite rightBgSprite;

    public GameObject selfBg;

    public Color highColor;
    public Color lowColor;
    public bool flashing = false;
    public bool hit = false;

    public MapPresence mapPresence;

    public Face.Faces face;

    public virtual bool ReceiveDamage(int amount, EntityControls source)
    {
        return ReceiveDamage(amount, Vector2Int.zero, source);
    }
    public virtual bool ReceiveDamage(int amount, Vector2Int direction, EntityControls source)
    {
        health -= amount;
        health = Math.Max(0, health);
        hit = true;
        return true;
    }

    public void FlushLast()
    {
        last = current;
        current = new Vector2Int(current.x, current.y);
    }

    public void ApplyLook()
    {
        GetComponent<SpriteRenderer>().sprite = 
            Face.Select(face, upSprite, downSprite, leftSprite, rightSprite);
        selfBg.GetComponent<SpriteRenderer>().sprite = 
            Face.Select(face, upBgSprite, downBgSprite, leftBgSprite, rightBgSprite);
        
    }

    public int ApplyMovement(Face.Faces direction, int distance, bool ignoreEntity)
    {
        Vector2Int directionVector = Face.ToVector2Int(direction);
        int actualDistance = 0;
        for (int i = 1; i <= distance; i++)
        {
            Vector2Int posToCheck = current + i * directionVector;
            if (Globals.Instance.CheckTile(posToCheck.x, posToCheck.y, ignoreEntity))
            {
                actualDistance = i;
            }
            else break;
        }
        if (actualDistance != 0)
        {
            current += actualDistance * directionVector;
        }
        
        return actualDistance;
    }

    float CurrentEase()
    {
        float tickProgression = (float)Globals.Instance.tickProgression;
        float easeProgression = Math.Min(tickProgression * 2, 1);
        return 1 - (float)Math.Pow(1 - easeProgression, 4);
    }

    public void UpdatePosition()
    {
        float currentEase = CurrentEase();
        Vector2 pos = (1 - currentEase) * (Vector2)last + currentEase * (Vector2)current;
        GetComponent<Transform>().position = pos;
    }

    public void UpdateColor()
    {
        if (hit)
        {
            GetComponent<SpriteRenderer>().color = WHITE;
            selfBg.GetComponent<SpriteRenderer>().color = WHITE;
        }
        else
        {
            float tickProgression = (float)Globals.Instance.tickProgression;
            float easeProgression = tickProgression;//Math.Min(tickProgression * 2, 1);
            float colorProgression = (float)Math.Pow(easeProgression, 1);
            if (flashing)
            {
                colorProgression = Globals.Instance.currentTick8 % 2;
            }
            GetComponent<SpriteRenderer>().color = Color.Lerp(highColor, flashing ? WHITE : lowColor, colorProgression);
            selfBg.GetComponent<SpriteRenderer>().color = BLACK;
        }
    }

    public Vector2Int GetFacingTile()
    {
        return GetFacingTile(1);
    }
    public Vector2Int GetFacingTile(int distance)
    {
        return current + Face.ToVector2Int(face) * distance;
    }

    public void Refresh()
    {
        if (health <= 0)
        {
            Globals.Instance.mapPresences.Remove(mapPresence);
            Destroy(gameObject);
        }
    }
}