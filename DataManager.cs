using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static string fileName = "";
    public static int totalRounds = 60;
    public static int totalDeskRotateCount = 30;
    //public static int totalnotDeskRotateCount = 30;
    //public static int doRotate_changeItemPosCount = 30;
    public static int changeItemPosCount = 15;
    public static float doRotate_allTime = 0;
    public static float notRotate_allTime = 0;
    public static int groupIndex = -1; //1：A，2：B
    public static string groupName = "";
    public static int notRotate = 0;
    public static int notRotate_hits = 0;
    public static int notRotate_misses = 0;
    public static int notRotate_falseAlarms = 0;
    public static int notRotate_rejections = 0;
    public static float notRotate_reactionTime = 0;

    public static int doRotate = 0;
    public static int doRotate_hits = 0;
    public static int doRotate_misses = 0;
    public static int doRotate_falseAlarms = 0;
    public static int doRotate_rejections = 0;
    public static float doRotate_reactionTime = 0;

    public static string data = string.Format(
        "Experiment Condition {0}\n" +
        "Table does not rotate - \n" +
        "Hits:{1}\n" +
        "Misses:{2}\n" +
        "False Alarms:{3}\n" +
        "Rejection:{4}\n" +
        "Mean reaction time:{5}\n" +
        "Table does rotate - \n" +
        "Hits:{6}\n" +
        "Misses:{7}\n" +
        "False Alarms:{8}\n" +
        "Rejection:{9}\n" +
        "Mean reaction time:{10}\n",
        groupName,
        notRotate_hits,
        notRotate_misses,
        notRotate_falseAlarms,
        notRotate_rejections,
        notRotate_reactionTime,
        doRotate_hits,
        doRotate_misses,
        doRotate_falseAlarms,
        doRotate_rejections,
        doRotate_reactionTime
        );



    private void Start()
    {
        ResetData();
        //FileManager.Instance.SaveData("nameB", data);
    }


    //------------------------unlit------------

    //保存组别
    public static void SetGroup(int index)
    {
        groupIndex = index;
        if (groupIndex == 0)
        {
            groupName = "A";
        }
        else
        {
            groupName = "B";
        }
    }
    //保存数据到txt 
    public static void SaveData()
    {
        print("notRotate_allTime" + notRotate_allTime);
        print("doRotate_allTime" + notRotate_allTime);
        notRotate_reactionTime = notRotate_allTime / totalDeskRotateCount;
        doRotate_reactionTime = doRotate_allTime/(totalRounds- totalDeskRotateCount);
        fileName = fileName + "_" + groupName;
           data = string.Format(
          "Experiment Condition {0}\n" +
          "Table does not rotate - \n" +
          "Hits:{1}\n" +
          "Misses:{2}\n" +
          "False Alarms:{3}\n" +
          "Rejection:{4}\n" +
          "Mean reaction time:{5}\n" +
          "Table does rotate - \n" +
          "Hits:{6}\n" +
          "Misses:{7}\n" +
          "False Alarms:{8}\n" +
          "Rejection:{9}\n" +
          "Mean reaction time:{10}\n",
          groupName,
          notRotate_hits,
          notRotate_misses,
          notRotate_falseAlarms,
          notRotate_rejections,
          notRotate_reactionTime,
          doRotate_hits,
          doRotate_misses,
          doRotate_falseAlarms,
          doRotate_rejections,
          doRotate_reactionTime
          );

        FileManager.Instance.SaveData(fileName, data);
    }
    //从txt读数据
    public static void ResetData()
    {
        notRotate = 0;
        notRotate_hits = 0;
        notRotate_misses = 0;
        notRotate_falseAlarms = 0;
        notRotate_rejections = 0;
        notRotate_reactionTime = 0;

        doRotate = 0;
        doRotate_hits = 0;
        doRotate_misses = 0;
        doRotate_falseAlarms = 0;
        doRotate_rejections = 0;
        doRotate_reactionTime = 0;

        data = string.Format(
       "Experiment Condition {0}\n" +
       "Table does not rotate - \n" +
       "Hits:{1}\n" +
       "Misses:{2}\n" +
       "False Alarms:{3}\n" +
       "Rejection:{4}\n" +
       "Mean reaction time:{5}\n" +
       "Table does rotate - \n" +
       "Hits:{6}\n" +
       "Misses:{7}\n" +
       "False Alarms:{8}\n" +
       "Rejection:{9}\n" +
       "Mean reaction time:{10}\n",
       groupName,
       notRotate_hits,
       notRotate_misses,
       notRotate_falseAlarms,
       notRotate_rejections,
       notRotate_reactionTime,
       doRotate_hits,
       doRotate_misses,
       doRotate_falseAlarms,
       doRotate_rejections,
       doRotate_reactionTime
       );
    }
}
