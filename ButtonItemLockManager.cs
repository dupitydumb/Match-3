using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonItemLockManager : MonoBehaviour
{
    public List<GameObject> listObj_Opens;
    public List<GameObject> listObj_Locks;
    public GameObject addObj;
    // Start is called before the first frame update
    void Start()
    {
        addObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowButtonItem_Lock(bool isLock) {
        for (int i = 0; i < listObj_Locks.Count; i++)
        {
            listObj_Locks[i].gameObject.SetActive(isLock);
        }

        for (int i = 0; i < listObj_Opens.Count; i++)
        {
            listObj_Opens[i].gameObject.SetActive(!isLock);
        }
        addObj.SetActive(false);
    }

    public void ShowAddIcon() {
        for (int i = 0; i < listObj_Locks.Count; i++)
        {
            listObj_Locks[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < listObj_Opens.Count; i++)
        {
            listObj_Opens[i].gameObject.SetActive(false);
        }

        addObj.SetActive(true);
    }
}
