using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    //��ʼ��Ϸ ��ת������
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    //�˳���Ϸ
    public void QuitGame()
    {
        Application.Quit();
    }
}
