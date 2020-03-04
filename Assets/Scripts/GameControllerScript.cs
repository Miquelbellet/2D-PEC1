using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    public GameObject complimentBtnPrefab, parentPrefab, panelResponses, scrollBar, panelText;
    public Animator player1Animator, player2Animator;
    public AudioSource audioSource;
    public AudioClip winRound, looseRound, claps, boo, backMusic; //Audioclips cogidos de la web gratuita: https://freesound.org/
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
        //Aqui se inicializan tadas las varibales i se activan o desactivan los elementos necessarios en la escena
        InitializeObjects();
        //Aquí se cojen los piropos del fichero externo i se ponen en listas.
        SetComplimentsAndResponses();
    }

    private void InitializeObjects()
    {
        stateMachine = GetComponent<StateMachineScript>();
        scrollBar.GetComponent<Scrollbar>().value = 1;
    }
    private void SetComplimentsAndResponses()
    {
        //Cojer el listado de complidos del JSON externo
        var compliments = Resources.Load("Compliments") as TextAsset;
        var list = compliments.text.Split('\n');
        for (int i = 0; i < list.Length - 1; i += 2)
        {
            //Separar los cumplidos y las respuestas en 2 listas diferentes ordenadamente (i=cumplido y i+1=respuesta del cumplido)
            listCompliments.Add(list[i]);
            listResponses.Add(list[i + 1]);
        }

        //Mezclar la lista con todos los piropos, extraido de: https://answers.unity.com/questions/486626/how-can-i-shuffle-alist.html
        for (int i = 0; i < list.Length - 1; i++)
        {
            string temp = list[i];
            int randomIndex = Random.Range(i, list.Length - 1);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        //Inicializar los botones con los textox de los cumplidos en la escena
        foreach (var item in list)
        {
            GameObject compPrefab = Instantiate(complimentBtnPrefab);
            compPrefab.transform.GetChild(0).GetComponent<Text>().text = item;
            compPrefab.transform.parent = parentPrefab.transform;
        }
    }
    public void AskQuestion(string ask)
    {
        //Poner el texto en rojo y escribir la pregunta
        actionText.GetComponent<TextMeshProUGUI>().color = new Color32(211, 46, 46, 255);
        actionText.text = ask;
        //comprobar si la pregunta es un cumplido o una resposta
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
        //Poner computerAsked a false para que no vuelva a pasar por aqui en este turno
        computerAsked = false;
        //Seleccionar un cumpido random del listado de cumplidos
        var randomComp = Random.Range(0, listCompliments.Count);
        var resp = listCompliments[randomComp];
        //Ponerla en una variable global para que pueda ser cogida cuando se responda
        question = resp;
        //Poner el texto en verde i escribir la pregunta del player2
        actionText.GetComponent<TextMeshProUGUI>().color = new Color32(52, 105, 30, 255);
        actionText.text = resp;
        //Esperar respuesta del jugador
        stateMachine.playerState = StateMachineScript.playerStates.PlayerResponding;
    }
    public void ComputerAsking()
    {
        //Esperar un tiempo en escojer una pregunta para darle realismo al ordenador
        if (!computerAsked)
        {
            computerAsked = true;
            Invoke("ComputerQuestion", waitingResponseTime);
        }
    }
    public void CheckResponse(string response, string player)
    {
        //Poner el texto del color del jugador y escribir respuesta
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
        //El ordenador escoje una respuesta random del listado de respuestas
        var randomResponse = Random.Range(0, listResponses.Count);
        CheckResponse(listResponses[randomResponse], "player2");
    }
    private void CorrectResponse(string player)
    {
        //Cuando la respuesta es la correcta para la preguna efectuada, saber si la ha escrito el jugador o el ordenador para darle la victoria
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
        //Cuando la respuesta es la incorrecta para la preguna efectuada, saber si la ha escrito el jugador o el ordenador para darle la victoria al otro jugador
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
        //Gana la ronda el jugador,se le suma un punto de victoria
        pointsPlayer1++;
        //comprobar que aun no haya ganado la partida y esperar pregunta del jugador
        if (pointsPlayer1 <= 2)
        {
            stateMachine.playerState = StateMachineScript.playerStates.PlayerAsking;
            audioSource.PlayOneShot(winRound);
            actionText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            actionText.text = "Your turn";
        }
        //Si ya ha ganado la partida, Activar animacion final, activar sonido victoria, ir a la pantalla final
        else
        {
            stateMachine.playerState = StateMachineScript.playerStates.Waiting;
            actionText.GetComponent<TextMeshProUGUI>().color = new Color32(52, 105, 30, 255);
            actionText.text = "Wow! You are a realy nice guy. Bye";
            player2Animator.SetTrigger("loser");
            audioSource.PlayOneShot(claps);
            Invoke("GameOverP1Wins", finalAnimTime);
        }
        //Recorrer el listado de imagenes de "happy faces" y activar el siguiente (si es posible)
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
        //Gana la ronda el ordenador,se le suma un punto de victoria
        pointsPlayer2++;
        //comprobar que aun no haya ganado la partida y esperar pregunta del ordenador
        if (pointsPlayer2 <= 2)
        {
            stateMachine.playerState = StateMachineScript.playerStates.ComputerAsking;
            audioSource.PlayOneShot(looseRound);
            actionText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            actionText.text = "Player2 turn";
        }
        //Si ya ha ganado la partida, Activar animacion final, activar sonido victoria, ir a la pantalla final
        else
        {
            stateMachine.playerState = StateMachineScript.playerStates.Waiting;
            actionText.GetComponent<TextMeshProUGUI>().color = new Color32(211, 46, 46, 255);
            actionText.text = "You made me happyer! See you!";
            player1Animator.SetTrigger("loser");
            audioSource.PlayOneShot(boo);
            Invoke("GameOverP2Wins", finalAnimTime);
        }
        //Recorrer el listado de imagenes de "happy faces" y activar el siguiente (si es posible)
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
        //Guardar como ganador al jugador e ir a la escena final
        PlayerPrefs.SetString("Winner", "Player");
        SceneManager.LoadScene("EndScene");
    }
    private void GameOverP2Wins()
    {
        //Guardar como ganador al ordenador e ir a la escena final
        PlayerPrefs.SetString("Winner", "Computer");
        SceneManager.LoadScene("EndScene");
    }
}
