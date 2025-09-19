using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapPresence
{
    public MapPresence(EntityControls entity, bool passable)
    {
        this.entity = entity;
        this.passable = passable;
    }
    public EntityControls entity;
    public bool passable;
}

public class Globals : MonoBehaviour
{

    public Tilemap worldTilemap;
    public static Globals Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }

    public List<MapPresence> mapPresences = new List<MapPresence>() {};

    public bool inGame = true;
    public double timeSinceLastTick { get; private set; } = 0.0;
    public double tickProgression { get; private set; } = 0.0;
    public int currenTick { get; private set; } = 0;
    public int currentTick8 { get; private set; } = 0;
    public bool tickFrame { get; private set; } = false;
    public double bpmDuration = 60.0 / 90.0;
    public const double easingRatio = 0.000001;
    public double currentEasingRatio { get; private set; } = 0.0;

    public double EaseExp(double from, double to)
    {
        return to - (to - from) * currentEasingRatio;
    }
    public float EaseExp(float from, float to)
    {
        float ease = (float)currentEasingRatio;
        return to - (to - from) * ease;
    }
    public Vector2 EaseExp(Vector2 from, Vector2 to)
    {
        float ease = (float)currentEasingRatio;
        return to - (to - from) * new Vector2(ease, ease);
    }
    public Vector3 EaseExp(Vector3 from, Vector3 to)
    {
        float ease = (float)currentEasingRatio;
        Vector3 result = to - from;
        result.Scale(new Vector3(ease, ease, ease));
        return to - result;
    }
    public Vector4 EaseExp(Vector4 from, Vector4 to)
    {
        float ease = (float)currentEasingRatio;
        Vector4 result = to - from;
        result.Scale(new Vector4(ease, ease, ease, ease));
        return to - result;
    }

    public bool CheckTile(int x, int y)
    {
        return CheckTile(x, y, false); // TODO : link to the grid
    }
    public bool CheckTile(int x, int y, bool ignoreEntity)
    {
        Vector3Int tilepos = new Vector3Int(x, y);
        TileBase tile = worldTilemap.GetTile(tilepos);
        bool passable = tile == null; // TODO : link to the grid
        for (int e = 0; e < mapPresences.Count; e++)
        {
            if (x == mapPresences[e].entity.currentX && y == mapPresences[e].entity.currentY)
            {
                passable = passable && mapPresences[e].passable;
            }
        }
        return passable;
    }

    public MapPresence TestForPresence(Vector2Int pos)
    {
        for (int e = 0; e < mapPresences.Count; e++)
        {
            if (pos.x == mapPresences[e].entity.currentX && pos.y == mapPresences[e].entity.currentY)
            {
                if (!mapPresences[e].passable)
                {
                    return mapPresences[e];
                }
            }
        }
        return null;
    }

    void Update()
    {
        if (inGame)
        {
            timeSinceLastTick += Time.deltaTime;
            if (timeSinceLastTick >= bpmDuration)
            {
                timeSinceLastTick -= bpmDuration;
                tickFrame = true;
                currenTick++;
                BroadcastMessage("TickSystem", SendMessageOptions.DontRequireReceiver);
                BroadcastMessage("TickPlayer", SendMessageOptions.DontRequireReceiver);
                BroadcastMessage("TickMonster", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                tickFrame = false;
            }
            currentEasingRatio = math.pow(easingRatio, Time.deltaTime);
            tickProgression = timeSinceLastTick / bpmDuration;

            int actualTick8 = (int)(tickProgression * 8);
            if (currentTick8 != actualTick8)
            {
                currentTick8 = actualTick8;
                BroadcastMessage("Tick8", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
