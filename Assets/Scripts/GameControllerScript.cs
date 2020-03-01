using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    public GameObject complimentBtnPrefab, parentPrefab;
    public TextMeshProUGUI actionText, pointsPlayer1Txt, pointsPlayer2Txt;

    [HideInInspector] public List<string> listCompliments = new List<string>();
    [HideInInspector] public List<string> listResponses = new List<string>();

    private StateMachineScript stateMachine;
    private string question;
    private int waitingResponseTime = 3, pointsPlayer1 = 0, pointsPlayer2 = 0;
    private bool computerAsked = false;

    void Start()
    {
        InitializeObjects();
        SetComplimentsAndResponses();
    }

    private void InitializeObjects()
    {
        stateMachine = GetComponent<StateMachineScript>();
        pointsPlayer1Txt.text = pointsPlayer1.ToString();
        pointsPlayer2Txt.text = pointsPlayer2.ToString();
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
            stateMachine.playerState = StateMachineScript.playerStates.Listening;
            Invoke("ComputerResponse", waitingResponseTime);
        }
        else
        {
            stateMachine.playerState = StateMachineScript.playerStates.Incorrect;
            Invoke("Player2Wins", waitingResponseTime);
        }
    }
    private void AskQuestionComputer()
    {
        actionText.text = question;
        stateMachine.playerState = StateMachineScript.playerStates.Responding;
    }
    private void ComputerQuestion(string ask)
    {
        computerAsked = true;
        actionText.text = "Computer turn";
        question = ask;
        Invoke("AskQuestionComputer", waitingResponseTime);
    }
    public void ComputerAsking()
    {
        if (!computerAsked)
        {
            var randomCompliment = Random.Range(0, listCompliments.Count);
            ComputerQuestion(listResponses[randomCompliment]);
        }
    }
    public void CheckResponse(string response, string player)
    {
        computerAsked = false;
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
            stateMachine.playerState = StateMachineScript.playerStates.Correct;
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
            stateMachine.playerState = StateMachineScript.playerStates.Incorrect;
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
        pointsPlayer1Txt.text = pointsPlayer1.ToString();
        actionText.text = "Your turn";
        stateMachine.playerState = StateMachineScript.playerStates.Asking;
    }
    private void Player2Wins()
    {
        pointsPlayer2++;
        pointsPlayer2Txt.text = pointsPlayer2.ToString();
        actionText.text = "Player2 turn";
        stateMachine.playerState = StateMachineScript.playerStates.Listening;
    }
}
