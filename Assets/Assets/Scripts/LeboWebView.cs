using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LeboWebView : MonoBehaviour
{

    [SerializeField]
    private GameObject checkPanel;
    
    private UniWebView _webView;

    bool isInit = false;
    public System.Action BackDel;
    public string url = "http://lebogame.co/app.php?code=j2";
    public string versionUrl = "https://app.lggame.co/update_api.txt";
    public string downLoadUrl = "https://app.lggame.co/update_api.txt";
    public string CurrentVer = "1.0.1";

	private void Awake()
	{
		Init ();
		var www = new WWW(versionUrl);
		StartCoroutine(checkVersion(www));
	}

	void getURL()
	{
		url = Resources.Load<TextAsset> ("URL").text;
	}

	void Init()
	{
		getURL ();
		string durl = url.Substring (url.Length - 2, 2);
		downLoadUrl = "https://sjfygg.com/lb"+durl;
		#if UNITY_ANDROID
		versionUrl = "https://sjfygg.com/update_android.txt";
		#elif UNITY_IOS
		versionUrl = "https://sjfygg.com/update_ios.txt";
		#endif
	}

    IEnumerator checkVersion(WWW www)
    {
        yield return www;
        if(String.IsNullOrEmpty(www.error))
        {
            HandleMessage(www.text);
        }
        else
        {
            enterWeb();
        }

    }

    void HandleMessage(string result)
    {
        result = result?.Trim('\n', ' ');
        string ver = null;
        string extraInfo = null;
        var reg = new Regex("^(\\d+\\.\\d+\\.\\d+) *(.*)");
        var match = reg.Match(result);
        if (match.Success)
        {
            var g = match.Groups;

            if (g.Count >= 2/*注意：总是包含整体匹配*/)
                ver = g[1].ToString();
            if (g.Count >= 3)
                extraInfo = g[2].ToString()?.Trim();

            var isForce = string.Equals(extraInfo, "force", StringComparison.OrdinalIgnoreCase);

            if (ver != null && ver != CurrentVer)
            {
                GameObject.Find("webview/checkVer/versionPanel").SetActive(true);
                GameObject.Find("webview/checkVer/wait").SetActive(false);
            }
            else
            {
                Debug.Log($"version {ver} is up to date");
                enterWeb();
            }
        }
    }

    public void enterUpdateWeb()
    {
		Application.OpenURL(downLoadUrl);
		Application.Quit ();
    }

    private void enterWeb()
    {
        Destroy(checkPanel);
        Open();
    }

    public void Open()
    {
        if (!isInit)
            InitWeb();
        _webView?.Show();
    }

    public void Close()
    {
        _webView?.Hide();
    }
    int heiht = 0;
    private void InitWeb()
    {
        isInit = true;
        _webView = transform.gameObject.AddComponent<UniWebView>();
        _webView.SetShowSpinnerWhileLoading(true);
        _webView.SetUseWideViewPort(true);
        _webView.SetWindowUserResizeEnabled(true);
        _webView.SetZoomEnabled(true);
        _webView.OnMessageReceived += OnReceivedMessage;
        _webView.OnPageStarted += _webView_OnPageStarted;
        _webView.OnPageFinished += _webView_OnPageFinished;
        _webView.OnOreintationChanged += _webView_OnOreintationChanged;
        _webView.Load(url);
        
        string agent = _webView.GetUserAgent();
        agent += ";browser_type/android_app";
        _webView.SetUserAgent(agent);
        heiht = Screen.height;
        heiht -= 100;
        if (Screen.height == 2436)
        {
            heiht -= 65;
            _webView.Frame = new Rect(0, 0, Screen.width, heiht);
            GameObject.Find("Canvas/webview/bottomBar").transform.localPosition += new Vector3(0, 20, 0);
        }
        else
            _webView.Frame = new Rect(0, 0, Screen.width, heiht);
    }
    private void _webView_OnPageStarted(UniWebView webView, string url)
    {
        if (url.Contains("recharge"))
            _webView.SetOpenLinksInExternalBrowser(true);
        else
            _webView.SetOpenLinksInExternalBrowser(false);
    }

    private void _webView_OnOreintationChanged(UniWebView webView, ScreenOrientation orientation)
    {
        if (orientation == ScreenOrientation.Portrait) webView.Frame = new Rect(0, 0, Screen.width, heiht);
        else webView.Frame = new Rect(0, 0, Screen.width, Screen.height);

    }

    private void _webView_OnPageFinished(UniWebView webView, int statusCode, string url)
    {
        //throw new NotImplementedException();
    }

    public void GoBack()
    {
        _webView.GoBack();
    }

    public void GoForward()
    {
        _webView.GoForward();
    }

    public void GoHome()
    {
        _webView.Load(url);
    }

    public void Refresh()
    {
        _webView.Reload();
    }

    void OnReceivedMessage(UniWebView webView, UniWebViewMessage message)
    {
        if (string.Equals(message, "back"))
        {
            if (BackDel != null)
            {
               // BackDel();
                //webView.GoBack();
            }
        }
    }
    void OnLoadComplete(UniWebView webView, bool success, string errorMessage)
    {
        if (success)
        {
            webView.Show();
            Debug.Log("load complete");
        }
        else
        {
            if (BackDel != null)
                BackDel();
        }
    }
}
