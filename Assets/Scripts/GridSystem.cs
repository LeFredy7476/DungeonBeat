using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MyMonoBehaviour
{
    public Tilemap worldTilemap;
    public List<GridEntity> gridEntities = new();

    public bool CanWalk(Vector2Int pos, MovableGridEntity entity)
    {
        Vector3Int tilepos = new Vector3Int(pos.x, pos.y, 0);
        TileBase tile = worldTilemap.GetTile(tilepos);
        bool noWalls = tile == null;
        bool noEntity = true;
        for (int e = 0; e < gridEntities.Count; e++)
        {
            GridEntity gridEntity = gridEntities[e];
            if (gridEntity.CurrentPos == pos)
            {
                if (gridEntity.alive)
                {
                    if (entity is Player)
                    {
                        noEntity = noEntity && (gridEntity.playerOnlyPass || gridEntity.ghosted);
                    }
                    else
                    {
                        noEntity = noEntity && gridEntity.ghosted;
                    }
                }
            }
        }
        return noWalls && noEntity;
    }
}