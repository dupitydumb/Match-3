using System;
using EasyMobile;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Daily_Notifiaction : MonoBehaviour
{
    [Header("Repeat Local Notification")]
    public string repeatTitle = "Demo Repeat Notification";
    public string repeatSubtitle = "Demo Repeat Notification Subtitle";
    public string repeatMessage = "Demo repeat notification message";
    public string repeatCategoryId;
    public int repeatDelayHours, repeatDelayMinutes, repeatDelaySeconds;
    public NotificationRepeat repeatType = NotificationRepeat.EveryMinute;
    void Start() => Check_Notification_Invoke_Status();
    void Check_Notification_Invoke_Status()
    {
        if (PlayerPrefs.HasKey("Is_Notification_Invoked"))
            return;
        else
            Invoke_Notification_Once();
    }
    void Invoke_Notification_Once()
    {
        Invoke("ScheduleRepeatLocalNotification", 1);
        PlayerPrefs.SetInt("Is_Notification_Invoked", 0);
    }
    void ScheduleRepeatLocalNotification()
    {
        var notif = new NotificationContent();
        notif.title = repeatTitle;
        notif.body = repeatMessage;
        notif.subtitle = repeatSubtitle;
        notif.categoryId = repeatCategoryId;
        Notifications.ScheduleLocalNotification(new TimeSpan(repeatDelayHours, repeatDelayMinutes, repeatDelaySeconds), notif, repeatType);
    }
}
