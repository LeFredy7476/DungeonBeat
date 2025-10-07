using UnityEngine;
using System;

public class CharacterSpriteManager : MyMonoBehaviour
{
    public Sprite upFrontSprite;
    public Sprite downFrontSprite;
    public Sprite leftFrontSprite;
    public Sprite rightFrontSprite;
    public Sprite upBackSprite;
    public Sprite downBackSprite;
    public Sprite leftBackSprite;
    public Sprite rightBackSprite;
    public SpriteRenderer front;
    public SpriteRenderer back;
    public Color highColor;
    public Color lowColor;
    public bool flashing { get; set; } = false;
    public bool hit { get; set; } = false;
    public bool dying { get; set; } = false;


    readonly Color WHITE = Color.white;
    readonly Color BLACK = new Color(24.0f / 255, 20.0f / 255, 37.0f / 255);

    public void Turn(Face.Faces face)
    {
        front.sprite = Face.Select(face, upFrontSprite, downFrontSprite, leftFrontSprite, rightFrontSprite);
        back.sprite = Face.Select(face, upBackSprite, downBackSprite, leftBackSprite, rightBackSprite);
    }

    void Update()
    {
        float tickProgression = (float)globals.tickProgression;
        if (dying)
        {
            front.color = Color.Lerp(highColor, new Color(1.0f, 1.0f, 1.0f, 0.0f), tickProgression * 2.0f);
            back.color = Color.Lerp(BLACK, new Color(BLACK.r, BLACK.g, BLACK.b, 0.0f), tickProgression);
        }
        else if (hit && tickProgression <= 0.5)
        {
            front.color = WHITE;
            back.color = WHITE;
        }
        else
        {
            if (flashing)
            {
                tickProgression = globals.currentTick8 % 2;
            }
            front.color = Color.Lerp(highColor, flashing ? WHITE : lowColor, tickProgression);
            back.color = BLACK;
        }
    }

    public void TickSystem()
    {
        flashing = false;
    }
    
    public void Tick8()
    {
        if (globals.currentTick8 == 2 || globals.currentTick8 == 6)
        {
            hit = false;
        }
    }
}