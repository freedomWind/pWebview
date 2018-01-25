using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

/// <summary>
/// 消息推送结构体
/// </summary>
public class PushStr
{
    public List<string> pushStrs;
    public PushStr()
    {
        pushStrs = new List<string>();
    }
    public void Add(string str)
    {
        pushStrs.Add(str);
    }
    public void AddRange(string[] strs)
    {
        pushStrs.AddRange(strs);
    }
    public string GetStr(int index)
    {
        if (index < 0 || index > pushStrs.Count - 1)
            return "index is illegal";
        return pushStrs[index];
    }
    public static PushStr LoadFromJson(TextAsset ta)
    {
        return JsonMapper.ToObject<PushStr>(ta.text);
    }
}
