using System;
using UnityEngine;

public static class Face
{
    public const Faces UP = Faces.UP;
    public const Faces DOWN = Faces.DOWN;
    public const Faces LEFT = Faces.LEFT;
    public const Faces RIGHT = Faces.RIGHT;
    public const Faces NONE = Faces.NONE;


    public enum Faces
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        NONE
    }

    public static Vector2Int ToVector2Int(Faces face)
    {
        return face switch
        {
            Faces.UP => Vector2Int.up,
            Faces.DOWN => Vector2Int.down,
            Faces.LEFT => Vector2Int.left,
            Faces.RIGHT => Vector2Int.right,
            _ => Vector2Int.zero
        };
    }
    public static Vector2 ToVector2(Faces face)
    {
        return face switch
        {
            Faces.UP => Vector2.up,
            Faces.DOWN => Vector2.down,
            Faces.LEFT => Vector2.left,
            Faces.RIGHT => Vector2.right,
            _ => Vector2.zero
        };
    }
    public static int ToInt(Faces face)
    {
        return face switch
        {
            Faces.UP => 0,
            Faces.DOWN => 1,
            Faces.LEFT => 2,
            Faces.RIGHT => 3,
            _ => 0
        };
    }
    public static Vector3 ToRotation(Faces face)
    {
        return face switch
        {
            Faces.UP => new Vector3(0, 0, 0),
            Faces.DOWN => new Vector3(0, 0, 180),
            Faces.LEFT => new Vector3(0, 0, 90),
            Faces.RIGHT => new Vector3(0, 0, -90),
            _ => new Vector3(0, 0, 0)
        };
    }
    public static Faces FromString(string face)
    {
        return face switch
        {
            "up" => Faces.UP,
            "down" => Faces.DOWN,
            "left" => Faces.LEFT,
            "right" => Faces.RIGHT,
            _ => Faces.NONE
        };
    }
    public static string ToString(Faces face)
    {
        return face switch
        {
            Faces.UP => "up",
            Faces.DOWN => "down",
            Faces.LEFT => "left",
            Faces.RIGHT => "right",
            _ => "none"
        };
    }

    public static T Select<T>(Faces face, T up, T down, T left, T right)
    {
        return face switch
        {
            Faces.UP => up,
            Faces.DOWN => down,
            Faces.LEFT => left,
            Faces.RIGHT => right,
            _ => up
        };
    }

    public static bool NotNone(Faces face)
    {
        return !face.Equals(Faces.NONE);
    }
    public static bool IsNone(Faces face)
    {
        return face.Equals(Faces.NONE);
    }
    public static Faces FromInt(int face)
    {
        return face switch
        {
            0 => Faces.UP,
            1 => Faces.DOWN,
            2 => Faces.LEFT,
            3 => Faces.RIGHT,
            _ => Faces.NONE
        };
    }

    public static Faces Inverse(Faces face)
    {
        return face switch
        {
            Faces.DOWN => Faces.UP,
            Faces.UP => Faces.DOWN,
            Faces.RIGHT => Faces.LEFT,
            Faces.LEFT => Faces.RIGHT,
            _ => Faces.NONE
        };
    }

    public static Faces RandomFace()
    {
        int randint = (int)UnityEngine.Random.Range(0.0f, 4.0f);
        randint = randint == 4 ? 0 : randint;
        return FromInt(randint);
    }

    public static Faces PathFind(Vector2Int path)
    {
        bool horizontal = Math.Abs(path.x) >= (Math.Abs(path.y) + UnityEngine.Random.Range(-0.5f, 0.5f));
        if (path == Vector2Int.zero)
        {
            return Faces.NONE;
        }
        else if (horizontal)
        {
            if (path.x > 0) return Faces.RIGHT;
            else return Faces.LEFT;
        }
        else
        {
            if (path.y > 0) return Faces.UP;
            else return Faces.DOWN;
        }
    }
}



