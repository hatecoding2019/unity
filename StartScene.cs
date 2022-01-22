using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    //开始游戏 跳转到室内
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    //退出游戏
    public void QuitGame()
    {
        Application.Quit();
    }
}
