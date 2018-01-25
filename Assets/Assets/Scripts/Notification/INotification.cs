using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_IOS
using LocalNotification = UnityEngine.iOS.LocalNotification;
#endif
public interface INotification
{
    void NotificationMessage(string message, int hour, bool isRepeatDay);
    void NotificationMessage(string message, System.DateTime newDate, bool isRepeatDay);
    void CleanNotification();
}
#if UNITY_ANDROID
public class AndriodPush : INotification
{
    bool isStopped;
    //public 
    public void NotificationMessage(string message, int hour, bool isRepeatDay)
    {
        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;
        System.DateTime newdate = new DateTime(year, month, day, hour, 0, 0);
        NotificationMessage(message, newdate, isRepeatDay);
    }
    public void NotificationMessage(string message, System.DateTime newDate, bool isRepeatDay)
    {
        if (newDate > DateTime.Now)
        {
                          
        }
    }
    public void CleanNotification()
    {
        //JPushBinding.ClearNotificationById(1);
        LocalPush.Ins.showText.text = "clean";
    }
}
#endif
#if UNITY_IOS
public class IphonePush :INotification
{
    public void NotificationMessage(string message, int hour, bool isRepeatDay)
    {
        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;
        System.DateTime newdate = new DateTime(year, month, day, hour, 0, 0);
        NotificationMessage(message, newdate, isRepeatDay);
        
    }
    public void NotificationMessage(string message, System.DateTime newDate, bool isRepeatDay)
    {
        if (newDate > DateTime.Now)
        {
            LocalNotification localNotification = new LocalNotification();
            localNotification.fireDate = newDate;
            localNotification.alertBody = message;
            localNotification.applicationIconBadgeNumber = 1;
            localNotification.hasAction = true;
            if (isRepeatDay)
            {
                localNotification.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.ChineseCalendar;
                localNotification.repeatInterval = UnityEngine.iOS.CalendarUnit.Day;
            }
            UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(localNotification);
            UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Sound);
        }
    }
    public void CleanNotification()
    {
        LocalNotification l = new LocalNotification();
        l.applicationIconBadgeNumber = -1;
        UnityEngine.iOS.NotificationServices.PresentLocalNotificationNow(l);
        LocalPush.Ins.StartCoroutine(delayOneFrame());       
    }
    IEnumerator delayOneFrame()
    {
        yield return null;
        UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
        UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
    }

}
#endif