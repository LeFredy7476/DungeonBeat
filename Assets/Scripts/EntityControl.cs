
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

    public int currentX;
    public int currentY;
    public float lastX;
    public float lastY;

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

    public string face;

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
        lastX = currentX;
        lastY = currentY;
    }

    public void ApplyLook()
    {
        if (face.Equals("up"))
        {
            GetComponent<SpriteRenderer>().sprite = upSprite;
            selfBg.GetComponent<SpriteRenderer>().sprite = upBgSprite;
        }
        else if (face.Equals("down"))
        {
            GetComponent<SpriteRenderer>().sprite = downSprite;
            selfBg.GetComponent<SpriteRenderer>().sprite = downBgSprite;
        }
        else if (face.Equals("left"))
        {
            GetComponent<SpriteRenderer>().sprite = leftSprite;
            selfBg.GetComponent<SpriteRenderer>().sprite = leftBgSprite;
        }
        else if (face.Equals("right"))
        {
            GetComponent<SpriteRenderer>().sprite = rightSprite;
            selfBg.GetComponent<SpriteRenderer>().sprite = rightBgSprite;
        }
    }

    public int ApplyMovement(string direction, int distance, bool ignoreEntity)
    {
        int actualDistance = 0;
        direction = direction.ToLower();
        if (direction.Equals("up"))
        {
            for (int i = 1; i <= distance; i++)
            {
                if (Globals.Instance.CheckTile(currentX, currentY + i, ignoreEntity))
                {
                    actualDistance = i;
                }
                else break;
            }
            if (actualDistance != 0)
            {
                currentY += actualDistance;
            }
        }
        else if (direction.Equals("down"))
        {
            for (int i = 1; i <= distance; i++)
            {
                if (Globals.Instance.CheckTile(currentX, currentY - i, ignoreEntity))
                {
                    actualDistance = i;
                }
                else break;
            }
            if (actualDistance != 0)
            {
                currentY -= actualDistance;
            }
        }
        else if (direction.Equals("left"))
        {
            for (int i = 1; i <= distance; i++)
            {
                if (Globals.Instance.CheckTile(currentX - i, currentY, ignoreEntity))
                {
                    actualDistance = i;
                }
                else break;
            }
            if (actualDistance != 0)
            {
                currentX -= actualDistance;
            }
        }
        else if (direction.Equals("right"))
        {
            for (int i = 1; i <= distance; i++)
            {
                if (Globals.Instance.CheckTile(currentX + i, currentY, ignoreEntity))
                {
                    actualDistance = i;
                }
                else break;
            }
            if (actualDistance != 0)
            {
                currentX += actualDistance;
            }
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
        float x = (1 - currentEase) * lastX + currentEase * currentX;
        float y = (1 - currentEase) * lastY + currentEase * currentY;
        GetComponent<Transform>().position = new Vector2(x, y);
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
        if (face.Equals("up"))
        {
            return new Vector2Int(currentX, currentY + distance);
        }
        else if (face.Equals("down"))
        {
            return new Vector2Int(currentX, currentY - distance);
        }
        else if (face.Equals("left"))
        {
            return new Vector2Int(currentX - distance, currentY);
        }
        else if (face.Equals("right"))
        {
            return new Vector2Int(currentX + distance, currentY);
        }
        else
        {
            return new Vector2Int(currentX, currentY);
        }
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