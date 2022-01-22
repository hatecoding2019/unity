using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class FileManager : MonoBehaviour
{
    public static FileManager Instance;


    //string tableA = "Experiment Condition A\n";
    //string tableB = "Experiment Condition B\n";
    //string normalTablenot = "Table does not rotate - \n";
    //string normalTable = "Table does rotate - \n";
    //string Hits = "Hits:\n";
    //string Misses = "Misses:\n";
    //string FalseAlarms = "False Alarms:\n";
    //string Rejection = "Rejection:\n";
    //string reactionTime = "Mean reaction time:\n";

    //List<byte> strLists = new List<byte>();

    private void Awake()
    {
        Instance = this;
    }
    //加数据
    void AddByte(List<byte> testlis,string strr)
    {
        byte[] temp = Encoding.UTF8.GetBytes(strr);
        for (int i = 0; i < temp.Length; i++)
        {
            testlis.Add(temp[i]);
        }
    }
    //保存数据
    public void SaveData(string name, string info)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(info.ToCharArray());

        string normalPath = "";
        normalPath = Application.dataPath + "/Code/data/"+ name+".txt";
        //if (isA)
        //{
        //    normalPath = Application.dataPath + "/Code/data/filename_A.txt";
        //    AddByte(strLists, tableA);
        //}
        //else
        //{
        //    normalPath = Application.dataPath + "/Code/data/filename_B.txt";
        //    AddByte(strLists, tableB);
        //}
 
        //AddByte(strLists, normalTablenot);
        //AddByte(strLists, Hits);
        //AddByte(strLists, Misses);
        //AddByte(strLists, FalseAlarms);
        //AddByte(strLists, Rejection);
        //AddByte(strLists, reactionTime);
        //AddByte(strLists, normalTable);
        //AddByte(strLists, Hits);
        //AddByte(strLists, Misses);
        //AddByte(strLists, FalseAlarms);
        //AddByte(strLists, Rejection);
        //AddByte(strLists, reactionTime);

        //byte[] bts = strLists.ToArray();
        bool isExist = File.Exists(normalPath);
        //print(isExist);
        if (isExist)
        {
            FileStream file = new FileStream(normalPath, FileMode.Open);
            file.Write(bytes, 0, bytes.Length);
            if (file != null)
            {
                file.Flush();
                file.Close();
                file.Dispose();
            }
        }
        else
        {
            // 文件流创建一个文本文件
            FileStream file = new FileStream(normalPath, FileMode.Create);
            print(bytes.Length);
            // 文件写入数据流
            file.Write(bytes, 0, bytes.Length);
            if (file != null)
            {
                //清空缓存
                file.Flush();
                // 关闭流
                file.Close();
                //销毁资源
                file.Dispose();
            }
        }
    }
}
