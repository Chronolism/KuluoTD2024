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
    //������Ϸ����������
    List<SingleGameSetData> allQAData = new List<SingleGameSetData>();
    //��¼���������Ϸ������ʱ��
    public float timeUseAll;

    //���еĵ�ǰ����Ϣ
    //����ʱ��
    public string thisDataStartTime;
    //��ǰ�������ͼ��
    public int playerPattern;
    //��ǰ����Ƿ�Ϊ����
    public bool isPlayerFirst;
    //��¼���ÿһ�ֵ�����ʱ��
    public float timeUse;
    //��¼�����һ��һ����ʱ��
    float m_LastTimeUse;
    //ÿһ����¼,������StackΪ�˷��㳷������
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
            temp += (item.isPlayerFirst ? "\n�������" : "\n��������") + "\n";
            temp += "���Ϊ" + (item.playerPattern == 0 ? "���\n" : "��Ȧ\n");
            temp += "�˾���Ϸ������" + (((int)(item.timeUse * 100)) / 100f) + "�빲" + item.stepRecord.Count + "��\n";
            for (int i = 0; i < item.stepRecord.Count; i++)
            {
                temp += "��" + i + "����" + item.stepRecord[i] + "��λ�� " + "��ʱ" + item.stepTimeRecord[i] + "\n";
            }
            if(item.isRaw) temp += "����Ϊƽ��\n\n";
            else
            {
                temp += "����Ϊ" +( item.isPlayerWin ? "���": "����") + "��ʤ\n\n";
            }
        }
        return temp;
    }
    /// <summary>
    /// ����ǰ������������ʾ����
    /// </summary>
    /// <returns></returns>
    public string GetThisGameSetDataToDirectChinese()
    {
        string temp = "";
        temp += "����������" + (((int)(timeUse * 100)) / 100f) + "��\n";
        temp += "���ֽ�����" + stepRecord.Count + "��\n";
        temp += (isPlayerFirst ? "�������":"��������") + " ���Ϊ"+(playerPattern == 0? "<color=red>���</color>" : "<color=green>��Ȧ</color>");
        return temp;
    }
}
[Serializable]
public class SingleGameSetData
{
    //����ʱ��
    public string playTime;
    //�������ͼ��
    public int playerPattern;
    //����Ƿ�Ϊ����
    public bool isPlayerFirst;
    //��¼���ÿһ�ֵ�����ʱ��
    public float timeUse;
    //ÿһ����¼
    public List<int> stepRecord;
    public List<float> stepTimeRecord;
    //��ƽ����
    public bool isRaw;
    //�����Ӯ����
    public bool isPlayerWin;

}