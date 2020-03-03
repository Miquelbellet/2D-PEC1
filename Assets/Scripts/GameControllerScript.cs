using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    public GameObject complimentBtnPrefab, parentPrefab, panelResponses, scrollBar, panelText;
    public GameObject[] playerPonitsSprites, computerPonitsSprites;
    public TextMeshProUGUI actionText;
    public float waitingResponseTime, initAnimTime, finalAnimTime;

    [HideInInspector] public List<string> listCompliments = new List<string>();
    [HideInInspector] public List<string> listResponses = new List<string>();

    private StateMachineScript stateMachine;
    private string question;
    private int pointsPlayer1 = 0, pointsPlayer2 = 0;
    private bool computerAsked = false;

    void Start()
    {
        InitializeObjects();
        SetComplimentsAndResponses();
    }

    private void InitializeObjects()
    {
        stateMachine = GetComponent<StateMachineScript>();
        scrollBar.GetComponent<Scrollbar>().value = 1;
    }
    private void SetComplimentsAndResponses()
    {
        //Get the list of compliments and responses from external  JSON
        var compliments = Resources.Load("Compliments") as TextAsset;
        var list = compliments.text.Split('\n');
        for (int i = 0; i < list.Length - 1; i += 2)
        {
            //Separate the compliments and the responses in 2 separeted lists ordered (i=compliment and i+1=response for that compliment)
            listCompliments.Add(list[i]);
            listResponses.Add(list[i + 1]);
        }

        //Code extracted to https://answers.unity.com/questions/486626/how-can-i-shuffle-alist.html
        for (int i = 0; i < list.Length - 1; i++)
        {
            string temp = list[i];
            int randomIndex = Random.Range(i, list.Length - 1);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        //Instantate button prefabs with the compliments in the document
        foreach (var item in list)
        {
            GameObject compPrefab = Instantiate(complimentBtnPrefab);
            compPrefab.transform.GetChild(0).GetComponent<Text>().text = item;
            compPrefab.transform.parent = parentPrefab.transform;
        }
    }
    public void AskQuestion(string ask)
    {
        actionText.GetComponent<TextMeshProUGUI>().color = new Color32(211, 46, 46, 255);
        actionText.text = ask;
        //comprobar si la pregunta es un compliment o una resposta
        var isCompliment = false;
        foreach (var comp in listCompliments)
        {
            if (comp == ask) isCompliment = true;
        }

        //si es un compliment que l'ordinador respongui i si es una resposta que conti com a incorrecte
        if (isCompliment)
        {
            question = ask;
            stateMachine.playerState = StateMachineScript.playerStates.ComputerResponding;
            Invoke("ComputerResponse", waitingResponseTime);
        }
        else
        {
            stateMachine.playerState = StateMachineScript.playerStates.PlayerIncorrect;
            Invoke("Player2Wins", waitingResponseTime);
        }
    }
    private void ComputerQuestion()
    {
        computerAsked = false;
        var randomComp = Random.Range(0, listCompliments.Count);
        var resp = listCompliments[randomComp];
        question = resp;
        actionText.GetComponent<TextMeshProUGUI>().color = new Color32(52, 105, 30, 255);
        actionText.text = resp;
        stateMachine.playerState = StateMachineScript.playerStates.PlayerResponding;
    }
    public void ComputerAsking()
    {
        if (!computerAsked)
        {
            computerAsked = true;
            Invoke("ComputerQuestion", waitingResponseTime);
        }
    }
    public void CheckResponse(string response, string player)
    {
        if(player == "player1") actionText.GetComponent<TextMeshProUGUI>().color = new Color32(211, 46, 46, 255);
        else actionText.GetComponent<TextMeshProUGUI>().color = new Color32(52, 105, 30, 255);
        actionText.text = response;
        //Check response if correct or incorrect
        for (var i=0;i<listCompliments.Count;i++)
        {
            if(question == listCompliments[i])
            {
                if (response == listResponses[i])
                {
                    CorrectResponse(player);
                }
                else
                {
                    IncorrectResponse(player);
                }
            }
        }
    }
    private void ComputerResponse()
    {
        var randomResponse = Random.Range(0, listResponses.Count);
        CheckResponse(listResponses[randomResponse], "player2");
    }
    private void CorrectResponse(string player)
    {
        if (player == "player1")
        {
            stateMachine.playerState = StateMachineScript.playerStates.PlayerCorrect;
            Invoke("Player1Wins", waitingResponseTime);
        }
        else if (player == "player2")
        {
            stateMachine.playerState = StateMachineScript.playerStates.ComputerCorrect;
            Invoke("Player2Wins", waitingResponseTime);
        }
    }
    private void IncorrectResponse(string player)
    {
        if (player == "player1")
        {
            stateMachine.playerState = StateMachineScript.playerStates.PlayerIncorrect;
            Invoke("Player2Wins", waitingResponseTime);
        }
        else if (player == "player2")
        {
            stateMachine.playerState = StateMachineScript.playerStates.ComputerInorrect;
            Invoke("Player1Wins", waitingResponseTime);
        }
    }
    private void Player1Wins()
    {
        pointsPlayer1++;
        actionText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        actionText.text = "Your turn";
        stateMachine.playerState = StateMachineScript.playerStates.PlayerAsking;
        if(pointsPlayer1 >= 3) Invoke("GameOverP1Wins", 2f);
        foreach (var point in playerPonitsSprites)
        {
            if (!point.activeSelf)
            {
                point.SetActive(true);
                return;
            }
        }
    }
    private void Player2Wins()
    {
        pointsPlayer2++;
        actionText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        actionText.text = "Player2 turn";
        stateMachine.playerState = StateMachineScript.playerStates.ComputerAsking;
        if(pointsPlayer2 >= 3) Invoke("GameOverP2Wins", 2f);
        foreach (var point in computerPonitsSprites)
        {
            if (!point.activeSelf)
            {
                point.SetActive(true);
                return;
            }
        }
    }
    private void GameOverP1Wins()
    {
        PlayerPrefs.SetString("Winner", "Player");
        SceneManager.LoadScene("EndScene");
    }
    private void GameOverP2Wins()
    {
        PlayerPrefs.SetString("Winner", "Computer");
        SceneManager.LoadScene("EndScene");
    }
}
