using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
   
    public void StartGame() 
    {
        SceneManager.LoadScene("GameScene");
    
    }

    public void ExitGame() 
    {

        Application.Quit();
    }

    public void BackToMain() 
    {
        SceneManager.LoadScene(0);
    
    }

}
