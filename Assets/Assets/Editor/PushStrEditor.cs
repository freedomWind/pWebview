using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEditor;

/// <summary>
/// 消息推送
/// </summary>
public class PushStrEditor
{
    [MenuItem("Tool/PushStr/ToJsonFromExcel")]
    public static void LoadJsonFromExcel_pushStr()
    {
        string epath = Application.streamingAssetsPath + "/pushStrConfig.xlsx";
        string jpath = Application.dataPath + "/Resources/pushStr.json";
        PushStr ps = null;
        Dictionary<string, string[]> strs = ExcelTool.ToArrayListFromExcel(epath);
        if (strs != null)
        {
            ps = new PushStr();
            foreach (var tem in strs)
            {
                Debug.Log(tem.Value.Length+" str = "+tem.Value[0] +" key = "+tem.Key);
                ps.Add(tem.Key);
            }
        }
        if (ps != null)
        {
            try
            {
                string str = JsonMapper.ToJson(ps);
                Debug.Log("json str = "+str);
                if (System.IO.File.Exists(jpath))
                    System.IO.File.Delete(jpath);
                System.IO.FileStream fs = new System.IO.FileStream(jpath, System.IO.FileMode.CreateNew);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);
                sw.WriteLine(str);
                sw.Close();
                fs.Close();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("ex = "+ex.ToString());
            }
        }
    }
}
