using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationPopup : MonoBehaviour
{
    public static NotificationPopup instance;

    public ItemNotification itemNotificationPrefab;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNotification(string content) {
        SoundManager.instance.PlaySound_Notification();
        ItemNotification itemNotification = Instantiate(itemNotificationPrefab, gameObject.transform);
        itemNotification.ShowNotification(content);
    }
}
