using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager
{
    private Tilemap _mainMap;
    private Tilemap _collisionMap;

    public static MapManager Instance;

    public MapManager(Transform tilemapObject)
    {
        _collisionMap = tilemapObject.Find("Collisions").GetComponent<Tilemap>();
        _mainMap = tilemapObject.Find("Background").GetComponent<Tilemap>();
        _mainMap.CompressBounds(); //외곽 테두리 딱맞게 조여주기
    }
    public bool CanMove(Vector3Int pos)
    {
        BoundsInt mapBounds = _mainMap.cellBounds;
        if(pos.x < mapBounds.xMin || pos.x > mapBounds.xMax || pos.y < mapBounds.yMin || pos.y > mapBounds.yMax)
            return false;

        return _collisionMap.GetTile(pos) == null;
    }
    public Vector3Int GetTilePos(Vector3 worldPos)
    {
        return _mainMap.WorldToCell(worldPos);
    }

    public Vector3 GetWorldPos(Vector3Int cellPos)
    {
        return _mainMap.GetCellCenterWorld(cellPos);
    }
}
