using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControllerScript : MonoBehaviour
{
    public void GoToGame()
    {
        //Ir al juego
        SceneManager.LoadScene("GameScene");
    }

    public void Exit()
    {
        //Salir del juego
        Application.Quit();
    }
}
