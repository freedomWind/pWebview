using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPush : MonoBehaviour
{
    public static LocalPush Ins;
    public UnityEngine.UI.Text showText;
    NotificationProxy localNotify;
    void SetLocalNotification(NotificationProxy notification)
    {
        localNotify = notification;
    }
    private void Awake()
    {
        Ins = this;
        GameObject.DontDestroyOnLoad(this);
#if UNITY_ANDROID
        SetLocalNotification(new NotificationProxy(new AndriodPush()));
#elif UNITY_IOS
                SetLocalNotification(new NotificationProxy(new IphonePush()));
#endif
        localNotify?.CleanNotification();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            showText.text = "puasee true";

            localNotify?.NotifyMessage();
        }
        else
        {
            showText.text = "pausee false";
            localNotify?.CleanNotification();
        }
    }

}
