using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消息推送代理
/// </summary>
public class NotificationProxy : INotification
{
    INotification realNotify;
    public NotificationProxy(INotification notify)
    {
        realNotify = notify;
    }
    public void NotificationMessage(string message, int hour, bool isRepeatDay)
    {
        realNotify.NotificationMessage(message, hour, isRepeatDay);
    }
    public void NotificationMessage(string message, System.DateTime newDate, bool isRepeatDay)
    {
        realNotify.NotificationMessage(message, newDate, isRepeatDay);
    }
    public void CleanNotification()
    {
        realNotify.CleanNotification();
    }
    public void NotifyMessage()
    {
        //定义要推送的内容             
        if (list == null) list = new List<int>();
        list.Clear();
        randInt();
        TextAsset ta = Resources.Load<TextAsset>("pushStr");
        PushStr pStr = PushStr.LoadFromJson(ta);
        realNotify.NotificationMessage(pStr.GetStr(list[0]), 6, true);
        realNotify.NotificationMessage(pStr.GetStr(list[1]), 12, true);
        realNotify.NotificationMessage(pStr.GetStr(list[2]), 18, true);
        realNotify.NotificationMessage(pStr.GetStr(list[3]), 20, true);
        //test
        realNotify.NotificationMessage(pStr.GetStr(list[0]), System.DateTime.Now.AddSeconds(10), false);
        realNotify.NotificationMessage(pStr.GetStr(list[1]), System.DateTime.Now.AddSeconds(10), false);
        realNotify.NotificationMessage(pStr.GetStr(list[2]), System.DateTime.Now.AddSeconds(10), false);
        realNotify.NotificationMessage(pStr.GetStr(list[3]), System.DateTime.Now.AddSeconds(10), false);
    }

    List<int> list;
    void randInt()
    {
        int t = UnityEngine.Random.Range(0, 10);
        if (list.Count >= 4) return;
        if (!list.Contains(t))
            list.Add(t);
        randInt();
    }
}
