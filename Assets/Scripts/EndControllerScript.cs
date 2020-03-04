using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndControllerScript : MonoBehaviour
{
    public TextMeshProUGUI winnerText;
    private string winner;
    void Start()
    {
        //Buscar quien ha ganado la partida y escirbir una cosa u otra.
        winner = PlayerPrefs.GetString("Winner", "Player");
        if (winner == "Player") winnerText.text = "Congrats you won the kindness battle!";
        else winnerText.text = "Oh no, you lose the kindness battle!";
    }

    public void RestartGame()
    {
        //Volver a repetir el juego
        SceneManager.LoadScene("GameScene");
    }
    public void Exit()
    {
        //Salir del juego
        Application.Quit();
    }
}
