using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    public Transform[] items;
    public Camera newcamera;
    public Transform cameraPos;
    public GameObject itemTrans;
    Quaternion startRotation;
    //UI
    public GameObject canvasTrans;
    public Dropdown dropdown;
    public InputField input;
    public Text timeText;
    public GameObject hint;
    public Text text1;
    public Text texthint2;
    public Text text2;
    //public Text answer;
    //data
    [HideInInspector]
    public int thisGroup;
    public int currentRound = 0;//当前轮数
    public int deskRotateCount = 0; //桌子转动次数
    public int notdeskRotateCount = 0; //桌子转动次数
    //public int rotate_changeItemCount = 0;//物品换位置次数
    public int notrotate_changeItemCount = 0;
    public int rotate_changeItemCount = 0;
    float startAngle = 0;
    int[] indexs = new int[5];
    bool isTimer = false;
    float checkTimer = 0;
    //int intTimer = 0;
    float rotaTimer = 0;
    bool isDestRotate = false;
    bool isChangeItem = false;
    List<int> itemIndex = new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].gameObject.SetActive(false);
        }
        newcamera.transform.LookAt(cameraPos);
        hint.SetActive(false);
        text1.transform.parent.gameObject.SetActive(false);
        text2.transform.parent.gameObject.SetActive(false);
        texthint2.transform.parent.gameObject.SetActive(false);
        //answer.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu();
        }
        //TODO
        //newcamera.transform.RotateAround(cameraPos.transform.localPosition, cameraPos.transform.up, Time.deltaTime * 20);

        if (texthint2.transform.parent.gameObject.activeInHierarchy)
        {
            if (Input.anyKeyDown)
            {
                texthint2.transform.parent.gameObject.SetActive(false);
                RandomItem();
            }
        }

        if (isTimer)
        {
            checkTimer += Time.deltaTime;
            timeText.text = checkTimer.ToString() + "s";
            //if (checkTimer >= 1)
            //{
            //    intTimer += 1;
            //    timeText.text = intTimer.ToString() + "s";
            //    checkTimer = 0;
            //}

            if (Input.GetKeyDown(KeyCode.S))
            {
                print("相同");
                if (isDestRotate && isChangeItem)
                {
                    print("doRotate_misses");
                    //answer.text = "doRotate_misses";
                    DataManager.doRotate_allTime += checkTimer;
                    DataManager.doRotate_misses += 1;
                }
                if (isDestRotate && !isChangeItem)
                {
                    print("doRotate_rejections");
                    //answer.text = "doRotate_rejections";
                    DataManager.doRotate_allTime += checkTimer;
                    DataManager.doRotate_rejections += 1;
                }
                if (!isDestRotate && isChangeItem)
                {
                    print("notRotate_misses");
                    //answer.text = "notRotate_misses";
                    DataManager.notRotate_allTime += checkTimer;
                    DataManager.notRotate_misses += 1;
                }
                if (!isDestRotate && !isChangeItem)
                {
                    print("notRotate_rejections");
                    //answer.text = "notRotate_rejections";
                    DataManager.notRotate_allTime += checkTimer;
                    DataManager.notRotate_rejections += 1;
                }

                isTimer = false;
                currentRound += 1;
                if (currentRound > DataManager.totalRounds)
                {
                    //Invoke("HideAnswer", 1f);
                    text2.transform.parent.gameObject.SetActive(true);
                    Invoke("Menu", 5f);
                    DataManager.SaveData();
                    return;
                }
                else
                {
                    //answer.gameObject.SetActive(true);
                    //Invoke("HideAnswer", 1f);
                    texthint2.text = string.Format("Trial {0} of 60，Press any key to proceed", currentRound);
                    texthint2.transform.parent.gameObject.SetActive(true);
                }
               
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                print("不同");
                if (isDestRotate && isChangeItem)
                {
                    print("doRotate_hits");
                    //answer.text = "doRotate_hits";
                    DataManager.doRotate_allTime += checkTimer;
                    DataManager.doRotate_hits += 1;
                }
                if (isDestRotate && !isChangeItem)
                {
                    print("doRotate_falseAlarms");
                    //answer.text = "doRotate_falseAlarms";
                    DataManager.doRotate_allTime += checkTimer;
                    DataManager.doRotate_falseAlarms += 1;
                }
                if (!isDestRotate && isChangeItem)
                {
                    print("notRotate_hits");
                    //answer.text = "notRotate_hits";
                    DataManager.notRotate_allTime += checkTimer;
                    DataManager.notRotate_hits += 1;
                }
                if (!isDestRotate && !isChangeItem)
                {
                    print("notRotate_falseAlarms");
                    //answer.text = "notRotate_falseAlarms";
                    DataManager.notRotate_allTime += checkTimer;
                    DataManager.notRotate_falseAlarms += 1;
                }
                currentRound += 1;
                isTimer = false;
                if (currentRound > DataManager.totalRounds)
                {
                    //Invoke("HideAnswer", 1f);
                    text2.transform.parent.gameObject.SetActive(true);
                    Invoke("Menu", 5f);
                    DataManager.SaveData();
                }
                else
                {
                    texthint2.text = string.Format("Trial {0} of 60，Press any key to proceed", currentRound);
                    texthint2.transform.parent.gameObject.SetActive(true);
                    //answer.gameObject.SetActive(true);
                    //Invoke("HideAnswer", 1f);
                }
              
            }
        }
        if (itemTrans.activeInHierarchy == false)
        {
            //if (isDestRotate)
            //{
                rotaTimer += Time.deltaTime;
                if (rotaTimer < 3)
                {
                    if (rotateDir == -1)
                    {
                        newcamera.transform.RotateAround(cameraPos.transform.position, cameraPos.transform.up, -Time.deltaTime * 30);
                    }
                    else
                    {
                        newcamera.transform.RotateAround(cameraPos.transform.position, cameraPos.transform.up, Time.deltaTime * 30);
                    }
                }
            //}  
        }

        if (text1.transform.parent.gameObject.activeInHierarchy)
        {
            if (Input.anyKeyDown)
            {
                HintText();
            }
        }
    }

    void HideAnswer()
    {
        //answer.gameObject.SetActive(false);
    }
    public void StartGame()
    {
        if (input.text == "") { Hint(); return; }
        else
        {
            DataManager.fileName = input.text;
        }
        currentRound += 1;
        thisGroup = dropdown.value;
        DataManager.SetGroup(thisGroup);
        canvasTrans.SetActive(false);
        text1.transform.parent.gameObject.SetActive(true);
        //Invoke("HintText", 2f);
    }

    void HintText()
    {
        text1.transform.parent.gameObject.SetActive(false);
        texthint2.text = string.Format("Trial {0} of 60，Press any key to proceed", currentRound);
        texthint2.transform.parent.gameObject.SetActive(true);
    }

    void RandomCameraAngle()
    {
        if (thisGroup == 0)
        {//A组 
            print("Group:A组");
        }
        else
        {
            print("Group:B组");
            //startAngle = Random.Range(0, 2);
            //startAngle = startAngle > 1 ? -60 : 60;
        }
        startAngle = Random.Range(0, 360);
        newcamera.transform.RotateAround(cameraPos.transform.localPosition, cameraPos.transform.up, startAngle);
    }

    void RandomItem()
    {
        print("currentRound" + currentRound);
        
        isDestRotate = false;
        isChangeItem = false;
        timeText.text = "0s";

        for (int i = 0; i < items.Length; i++)
        {
            itemIndex.Add(i);
        }
        for (int i = 0; i < indexs.Length; i++)
        {
            int temp = Random.Range(0, itemIndex.Count);
            indexs[i] = temp;
            itemIndex.Remove(temp);
        }
        indexs[0] = Random.Range(0, 3);
        indexs[1] = Random.Range(3, 6);
        indexs[2] = Random.Range(6, 9);
        indexs[3] = Random.Range(9, 12);
        indexs[4] = Random.Range(12, items.Length);
        for (int i = 0; i < items.Length; i++)
        {
            items[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < indexs.Length; i++)
        {
            items[indexs[i]].gameObject.SetActive(true);
        }
        RandomCameraAngle();
        Invoke("HideItem", 3f);
    }

    void HideItem()
    {
        itemTrans.SetActive(false);
        startRotation = itemTrans.transform.localRotation;
        RandomDeskRotate();
        RandomChangeItemPos();
        Invoke("ShowItem", 3f);
    }

    void ShowItem()
    {
        itemTrans.SetActive(true);
        isTimer = true;
        checkTimer = 0;
        //intTimer = 0;
    }

    /// <summary>
    /// 交换传进来的两个物品的位置
    /// </summary>
    public void ChangeItemsPos(Transform itemOne, Transform itemTwo)
    {
        print("改变物品位置");
        isChangeItem = true;
        Vector3 tempPos = itemOne.transform.position;
        itemOne.transform.position = itemTwo.transform.position;
        itemTwo.transform.position = tempPos;
    }

    void RandomChangeItemPos()
    {
        if (currentRound > DataManager.totalRounds) return;
        if (isDestRotate)
        {
            //换物品位置的次数小于总轮数时，剩下的轮数都得换
            if ((DataManager.changeItemPosCount - rotate_changeItemCount) > 0 && (DataManager.changeItemPosCount - rotate_changeItemCount) >= (DataManager.totalDeskRotateCount - deskRotateCount))
            {
                rotate_changeItemCount += 1;
                isChangeItem = true;
                print("改变物品位置");
                // ChangeItemsPos(items[indexs[Random.Range(0, 2)]], items[indexs[Random.Range(2, indexs.Length)]]);
            }
            else
            {
                int index = Random.Range(0, 2);
                if (index == 0)
                {
                    if (rotate_changeItemCount < DataManager.changeItemPosCount)
                    {
                        rotate_changeItemCount += 1;
                        isChangeItem = true;
                        print("改变物品位置");
                        //ChangeItemsPos(items[indexs[Random.Range(0, 2)]], items[indexs[Random.Range(2, indexs.Length)]]);
                    }
                }
                else
                {
                    print("不改变物品位置");
                }
            }
        }
        else
        {
            //换物品位置的次数小于总轮数时，剩下的轮数都得换
            if ((DataManager.changeItemPosCount - notrotate_changeItemCount) > 0 && (DataManager.changeItemPosCount - notrotate_changeItemCount) >= (DataManager.totalDeskRotateCount - notdeskRotateCount))
            {
                notrotate_changeItemCount += 1;
                isChangeItem = true;
                print("改变物品位置");
                // ChangeItemsPos(items[indexs[Random.Range(0, 2)]], items[indexs[Random.Range(2, indexs.Length)]]);
            }
            else
            {
                int index = Random.Range(0, 2);
                if (index == 0)
                {
                    if (notrotate_changeItemCount < DataManager.changeItemPosCount)
                    {
                        notrotate_changeItemCount += 1;
                        isChangeItem = true;
                        print("改变物品位置");
                        //ChangeItemsPos(items[indexs[Random.Range(0, 2)]], items[indexs[Random.Range(2, indexs.Length)]]);
                    }
                }
                else
                {
                    print("不改变物品位置");
                }
            }
        }
        //    //换物品位置的次数小于总轮数时，剩下的轮数都得换
        //if ((DataManager.changeItemPosCount - changeItemCount) > 0 && (DataManager.changeItemPosCount - changeItemCount) >= (DataManager.totalRounds - currentRound))
        //{
        //    changeItemCount += 1;
        //    isChangeItem = true;
        //    print("改变物品位置");
        //    // ChangeItemsPos(items[indexs[Random.Range(0, 2)]], items[indexs[Random.Range(2, indexs.Length)]]);
        //}
        //else
        //{
        //    int index = Random.Range(0, 2);
        //    if (index == 0)
        //    {
        //        if (changeItemCount < DataManager.changeItemPosCount)
        //        {
        //            changeItemCount += 1;
        //            isChangeItem = true;
        //            print("改变物品位置");
        //            //ChangeItemsPos(items[indexs[Random.Range(0, 2)]], items[indexs[Random.Range(2, indexs.Length)]]);
        //        }
        //    }
        //    else
        //    {
        //        print("不改变物品位置");
        //    }
        //}
    }

    float rotateDir = 0;
    int indexxxx = 0;
    void RandomDeskRotate()
    {
        if (currentRound > DataManager.totalRounds) return;
        rotaTimer = 0;
        indexxxx = Random.Range(0, 2);
        rotateDir = indexxxx > 0 ? -1 : 1;
        //桌子需转动的次数小于总轮数时，剩下的轮数都得转
        if ((DataManager.totalDeskRotateCount - deskRotateCount) > 0 && (DataManager.totalDeskRotateCount - deskRotateCount) >= (DataManager.totalRounds - currentRound))
        {
            print("桌子转");
            isDestRotate = true;
            deskRotateCount += 1;
            if (thisGroup == 0)
            {//A组 
                //float angle = Random.Range(0, 2);
                startAngle = startAngle > 1 ? -30 : 30;
                itemTrans.transform.RotateAround(itemTrans.transform.position, itemTrans.transform.up, startAngle);
            }
            else
            {
                //float angle = Random.Range(0, 2);
                startAngle = startAngle > 1 ? -60 : 60;
                itemTrans.transform.RotateAround(itemTrans.transform.position, itemTrans.transform.up, startAngle);
            }
            //rotateDir = Random.Range(0, 3);
            //rotateDir = startAngle > 1 ? -1 : 1;
        }
        else
        {
            int index = Random.Range(0, 2);
            if (index == 0)//转
            {
                if (deskRotateCount < DataManager.totalDeskRotateCount)
                {
                    print("桌子转");
                    isDestRotate = true;
                    deskRotateCount += 1;
                    if (thisGroup == 0)
                    {//A组 
                        //float angle = Random.Range(0, 2);
                        startAngle = startAngle > 1 ? -30 : 30;
                        itemTrans.transform.RotateAround(itemTrans.transform.position, itemTrans.transform.up, startAngle);
                    }
                    else
                    {
                        //float angle = Random.Range(0, 2);
                        startAngle = startAngle > 1 ? -60 : 60;
                        itemTrans.transform.RotateAround(itemTrans.transform.position, itemTrans.transform.up, startAngle);
                    }
                    //rotateDir = Random.Range(0, 2);
                    //rotateDir = startAngle > 1 ? -1 : 1;
                }
                else
                {
                    notdeskRotateCount += 1;
                    print("桌子不转");
                }
            }
            else
            {
                notdeskRotateCount += 1;
                print("桌子不转");
            }
        }
    }

    void Hint()
    {
        hint.SetActive(true);
        CancelInvoke("HideHint");
        Invoke("HideHint", 1f);
    }

    void HideHint()
    {
        hint.SetActive(false);
    }

    public void ResetGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
