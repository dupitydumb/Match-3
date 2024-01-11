using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class CheckTileMap : MonoBehaviour
{
    public Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(tilemap.GetUsedTilesCount());
        for (int i = 0; i < tilemap.GetUsedTilesCount(); i++) {
            //Debug.Log("AAAAAAA:"+ tilemap.GetT);
        }

        //foreach (var position in tilemap.cellBounds.allPositionsWithin) {
        //    Debug.Log(position);
        //}
        Debug.Log("tilemap.cellBounds.xMin:"+ tilemap.cellBounds.xMin);
        Debug.Log("tilemap.cellBounds.xMax:"+ tilemap.cellBounds.xMax);
        Debug.Log("tilemap.cellBounds.yMin:"+ tilemap.cellBounds.yMin);
        Debug.Log("tilemap.cellBounds.yMax:"+ tilemap.cellBounds.yMax);
        for (int i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++) {
            for (int j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++) {
                //Debug.Log("i:" + i + "   j:" + j);
                if (tilemap.HasTile(new Vector3Int(i, j, 0))) {
                    Debug.Log("HasTile    "+ "i:" + i + "   j:" + j);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    
}
