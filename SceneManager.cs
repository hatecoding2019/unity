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
    public int thisGroup; //0:A�飬1B��
    public int currentRound = 0;//��ǰ����
    public int deskRotateCount = 0; //����ת������
    public int notdeskRotateCount = 0; //����ת������
    //public int rotate_changeItemCount = 0;//��Ʒ��λ�ô���
    public int notrotate_changeItemCount = 0;
    public int rotate_changeItemCount = 0;
    float startAngle = 0;
    int[] indexs = new int[5];
    bool isTimer = false;
    float checkTimer = 0;
    //int intTimer = 0;
    float rotaTimer = 0;
    bool isDestRotate = false;//��û�з���ת��
    bool isChangeItem = false;//��û����Ʒ����λ��
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

        //��ʾ�ڼ���
        if (texthint2.transform.parent.gameObject.activeInHierarchy)
        {
            if (Input.anyKeyDown)
            {
                texthint2.transform.parent.gameObject.SetActive(false);
                RandomItem();
            }
        }

        //��ʼ��ʱ
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
                print("��ͬ");
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
                    texthint2.text = string.Format("Trial {0} of 60��Press any key to proceed", currentRound);
                    texthint2.transform.parent.gameObject.SetActive(true);
                }

            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                print("��ͬ");
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
                    texthint2.text = string.Format("Trial {0} of 60��Press any key to proceed", currentRound);
                    texthint2.transform.parent.gameObject.SetActive(true);
                    //answer.gameObject.SetActive(true);
                    //Invoke("HideAnswer", 1f);
                }

            }
        }
        //�����ӵ���Ʒ��ʧ��ת���
        if (itemTrans.activeInHierarchy == false)
        {
            rotaTimer += Time.deltaTime;
            if (rotaTimer < 3)
            {
                //B��ת���
                if (thisGroup == 1)
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
                //A��ת���
                else
                {
                    if (rotaTimer < 1.5f)
                    {
                        newcamera.transform.RotateAround(cameraPos.transform.position, cameraPos.transform.up, -Time.deltaTime * 30);
                    }
                    else
                    {
                        newcamera.transform.RotateAround(cameraPos.transform.position, cameraPos.transform.up, Time.deltaTime * 30);
                    }
                }
            }
        }
        //�����ⰴ��ȥ����ʾ�ı�
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
    //��ʼ��Ϸ
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

    //������ʾ�ı�
    void HintText()
    {
        text1.transform.parent.gameObject.SetActive(false);
        texthint2.text = string.Format("Trial {0} of 60��Press any key to proceed", currentRound);
        texthint2.transform.parent.gameObject.SetActive(true);
    }
    //�����ʼ�Ƕ�
    void RandomCameraAngle()
    {
        if (thisGroup == 0)
        {//A�� 
            print("Group:A��");
        }
        else
        {
            print("Group:B��");
            //startAngle = Random.Range(0, 2);
            //startAngle = startAngle > 1 ? -60 : 60;
        }
        startAngle = Random.Range(0, 360);
        newcamera.transform.RotateAround(cameraPos.transform.localPosition, cameraPos.transform.up, startAngle);
    }

    //�����Ʒ
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
        Vector3[] pos = new Vector3[5]; 
        for (int i = 0; i < indexs.Length; i++)
        {
            items[indexs[i]].gameObject.SetActive(true);
            pos[i] = items[indexs[i]].transform.localPosition;
        }
        items[indexs[0]].transform.localPosition = pos[4];
        items[indexs[1]].transform.localPosition = pos[3];
        items[indexs[2]].transform.localPosition = pos[0];
        items[indexs[3]].transform.localPosition = pos[1];
        items[indexs[4]].transform.localPosition = pos[2];
        //�����ʼ�Ƕ�
        RandomCameraAngle();
        Invoke("HideItem", 3f);
    }

    //��Ϸ��ʼ��������Ʒ
    void HideItem()
    {
        itemTrans.SetActive(false);
        startRotation = itemTrans.transform.localRotation;
        RandomDeskRotate();
        RandomChangeItemPos();
        Invoke("ShowItem", 3f);
    }

    //��ʾ��Ʒ
    void ShowItem()
    {
        itemTrans.SetActive(true);
        isTimer = true;
        checkTimer = 0;
        //intTimer = 0;
    }

    /// <summary>
    /// ������������������Ʒ��λ��
    /// </summary>
    public void ChangeItemsPos(Transform itemOne, Transform itemTwo)
    {
        print("�ı���Ʒλ��");
        isChangeItem = true;
        Vector3 tempPos = itemOne.transform.position;
        itemOne.transform.position = itemTwo.transform.position;
        itemTwo.transform.position = tempPos;
    }

    //���������Ʒλ��
    void RandomChangeItemPos()
    {
        if (currentRound > DataManager.totalRounds) return;
        if (isDestRotate)
        {
            //����Ʒλ�õĴ���С��������ʱ��ʣ�µ��������û�
            if ((DataManager.changeItemPosCount - rotate_changeItemCount) > 0 && (DataManager.changeItemPosCount - rotate_changeItemCount) >= (DataManager.totalDeskRotateCount - deskRotateCount))
            {
                rotate_changeItemCount += 1;
                isChangeItem = true;
                print("�ı���Ʒλ��");
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
                        print("�ı���Ʒλ��");
                        //ChangeItemsPos(items[indexs[Random.Range(0, 2)]], items[indexs[Random.Range(2, indexs.Length)]]);
                    }
                }
                else
                {
                    print("���ı���Ʒλ��");
                }
            }
        }
        else
        {
            //����Ʒλ�õĴ���С��������ʱ��ʣ�µ��������û�
            if ((DataManager.changeItemPosCount - notrotate_changeItemCount) > 0 && (DataManager.changeItemPosCount - notrotate_changeItemCount) >= (DataManager.totalDeskRotateCount - notdeskRotateCount))
            {
                notrotate_changeItemCount += 1;
                isChangeItem = true;
                print("�ı���Ʒλ��");
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
                        print("�ı���Ʒλ��");
                        //ChangeItemsPos(items[indexs[Random.Range(0, 2)]], items[indexs[Random.Range(2, indexs.Length)]]);
                    }
                }
                else
                {
                    print("���ı���Ʒλ��");
                }
            }
        }
    }

    //�������ת
    float rotateDir = 0;
    int indexxxx = 0;
    void RandomDeskRotate()
    {
        if (currentRound > DataManager.totalRounds) return;
        rotaTimer = 0;
        indexxxx = Random.Range(0, 2);
        rotateDir = indexxxx > 0 ? -1 : 1;
        //������ת���Ĵ���С��������ʱ��ʣ�µ���������ת
        if ((DataManager.totalDeskRotateCount - deskRotateCount) > 0 && (DataManager.totalDeskRotateCount - deskRotateCount) >= (DataManager.totalRounds - currentRound))
        {
            print("����ת");
            isDestRotate = true;
            deskRotateCount += 1;
            if (thisGroup == 0)
            {//A�� 
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
            if (index == 0)//ת
            {
                if (deskRotateCount < DataManager.totalDeskRotateCount)
                {
                    print("����ת");
                    isDestRotate = true;
                    deskRotateCount += 1;
                    if (thisGroup == 0)
                    {//A�� 
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
                    print("���Ӳ�ת");
                }
            }
            else
            {
                notdeskRotateCount += 1;
                print("���Ӳ�ת");
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
