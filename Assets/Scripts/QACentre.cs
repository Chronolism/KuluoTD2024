using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class QACentre : MonoBehaviour
{
    //整个游戏的所有数据
    List<SingleGameSetData> allQAData = new List<SingleGameSetData>();
    //记录玩家整个游戏的游玩时长
    public float timeUseAll;

    //运行的当前局信息
    //游玩时刻
    public string thisDataStartTime;
    //当前玩家所持图案
    public int playerPattern;
    //当前玩家是否为先手
    public bool isPlayerFirst;
    //记录玩家每一局的游玩时长
    public float timeUse;
    //记录玩家这一上一步的时点
    float m_LastTimeUse;
    //每一步记录,这里用Stack为了方便撤销操作
    public Stack<int> stepRecord = new Stack<int>();
    public Stack<float> stepTimeRecord = new Stack<float>();
    public void RecordThisGameSet(bool isRaw,bool isPlayerWin)
    {
        timeUseAll += timeUse;
        SingleGameSetData tempData = new SingleGameSetData();
        tempData.playTime = thisDataStartTime;
        tempData.timeUse = timeUse;
        tempData.playerPattern = playerPattern;
        tempData.isPlayerFirst = isPlayerFirst;
        tempData.stepRecord = stepRecord.ToList<int>();
        tempData.stepTimeRecord = stepTimeRecord.ToList<float>();
        tempData.isRaw = isRaw;
        tempData.isPlayerWin = isPlayerWin;
        allQAData.Add(tempData);
        
    }
    public void GameSetStart(bool isPlayerTurn,int patternNow)
    {
        if((isPlayerTurn && (patternNow == 0)) || (!isPlayerTurn && (patternNow == 1)))playerPattern = 0;
        else playerPattern = 1;
        thisDataStartTime = System.DateTime.Now.ToString();
        isPlayerFirst = isPlayerTurn;
        timeUse = 0;
        m_LastTimeUse = 0;
        stepRecord = new Stack<int>();
        stepTimeRecord = new Stack<float>();
    }
    public void AddStep(int where)
    {
        stepRecord.Push(where);
        stepTimeRecord.Push(timeUse - m_LastTimeUse);
        m_LastTimeUse = timeUse;
    }
    public int RemoveStep()
    {
        if(stepRecord.Count > 0)
        {
            return stepRecord.Pop();
        }
        return -1;
    }
    private void Update()
    {
        timeUse += Time.deltaTime;
    }
    public void PrintData(string DirectoryPath)
    {
        File.WriteAllText(DirectoryPath + "/data.txt", SerializeToDirectChinese(allQAData), Encoding.UTF8);
        //using (FileStream fileStream = new FileStream(DirectoryPath + "/data.txt",  FileMode.OpenOrCreate , FileAccess.Write, FileShare.ReadWrite))
        //{
        //    BinaryFormatter binaryFormatter = new BinaryFormatter();
        //    binaryFormatter.Serialize(fileStream, SerializeToDirectChinese(allQAData));
        //    fileStream.Flush();
        //    fileStream.Close();
        //}
    }
    string SerializeToDirectChinese(List<SingleGameSetData> data)
    {
        if (data.Count < 1) return "Empty";
        string temp = "";
        foreach (var item in data)
        {
            temp += item.playTime;
            temp += (item.isPlayerFirst ? "\n玩家先手" : "\n电脑先手") + "\n";
            temp += "玩家为" + (item.playerPattern == 0 ? "红叉\n" : "绿圈\n");
            temp += "此局游戏进行了" + (((int)(item.timeUse * 100)) / 100f) + "秒共" + item.stepRecord.Count + "步\n";
            for (int i = 0; i < item.stepRecord.Count; i++)
            {
                temp += "第" + i + "步于" + item.stepRecord[i] + "号位置 " + "用时" + item.stepTimeRecord[i] + "\n";
            }
            if(item.isRaw) temp += "最终为平局\n\n";
            else
            {
                temp += "最终为" +( item.isPlayerWin ? "玩家": "电脑") + "获胜\n\n";
            }
        }
        return temp;
    }
    /// <summary>
    /// 将当前的数据内容显示出来
    /// </summary>
    /// <returns></returns>
    public string GetThisGameSetDataToDirectChinese()
    {
        string temp = "";
        temp += "本局游玩了" + (((int)(timeUse * 100)) / 100f) + "秒\n";
        temp += "本局进行了" + stepRecord.Count + "步\n";
        temp += (isPlayerFirst ? "玩家先手":"电脑先手") + " 玩家为"+(playerPattern == 0? "<color=red>红叉</color>" : "<color=green>绿圈</color>");
        return temp;
    }
}
[Serializable]
public class SingleGameSetData
{
    //游玩时刻
    public string playTime;
    //玩家所持图案
    public int playerPattern;
    //玩家是否为先手
    public bool isPlayerFirst;
    //记录玩家每一局的游玩时长
    public float timeUse;
    //每一步记录
    public List<int> stepRecord;
    public List<float> stepTimeRecord;
    //是平局吗
    public bool isRaw;
    //是玩家赢了吗
    public bool isPlayerWin;

}