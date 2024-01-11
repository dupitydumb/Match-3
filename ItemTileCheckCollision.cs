using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTileCheckCollision : MonoBehaviour
{
    public ItemTile itemTile;
    public int countCollision = 0;
    public List<GameObject> listCollisionObjecs = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        countCollision = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        //Debug.Log("OnTriggerEnter2D:"+ collision.tag);
        if (collision.CompareTag("TileCheckCollision")) {
            if (itemTile.itemTileState == Config.ITEMTILE_STATE.FLOOR)
            {
                if (collision.GetComponent<ItemTileCheckCollision>().itemTile.floorIndex > itemTile.floorIndex)
                {
                    listCollisionObjecs.Add(collision.gameObject);
                    countCollision++;
                    itemTile.SetTouch_Avaiable(false);
                    itemTile.SetShadow_Avaiable(true);
                }
            }
            
        }
    }
    

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("TileCheckCollision"))
        {
            if (itemTile.itemTileState == Config.ITEMTILE_STATE.FLOOR)
            {
                if (collision.GetComponent<ItemTileCheckCollision>().itemTile.floorIndex > itemTile.floorIndex)
                {
                    countCollision--;
                }

                if (countCollision <= 0)
                {
                    //Debug.Log("OnTriggerExit2DOnTriggerExit2D:"+ itemTile.gameObject.name);
                    itemTile.SetTouch_Enable();
                    //itemTile.SetShadow_Avaiable(false);
                }
            }

            
        }
    }

    public void ResetCollisionCount() {
        countCollision = 0;
    }
}
