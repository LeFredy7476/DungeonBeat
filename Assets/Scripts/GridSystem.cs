using System.Collections.Generic;
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

public class GridSystem : MonoBehaviour
{

    public List<MapPresence> mapPresences = new() { };
    public Tilemap worldTilemap;

    void Start()
    {

    }

    public bool CheckTile(int x, int y, bool ignoreEntity)
    {
        Vector3Int tilepos = new Vector3Int(x, y);
        TileBase tile = worldTilemap.GetTile(tilepos);
        bool passable = tile == null;
        for (int e = 0; e < mapPresences.Count; e++)
        {
            if (x == mapPresences[e].entity.current.x && y == mapPresences[e].entity.current.y)
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
            if (pos.x == mapPresences[e].entity.current.x && pos.y == mapPresences[e].entity.current.y)
            {
                if (!mapPresences[e].passable)
                {
                    return mapPresences[e];
                }
            }
        }
        return null;
    }
}