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
        winner = PlayerPrefs.GetString("Winner", "Player");
        if (winner == "Player") winnerText.text = "Congrats you won the kindness battle!";
        else winnerText.text = "Oh no, you lose the kindness battle!";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
