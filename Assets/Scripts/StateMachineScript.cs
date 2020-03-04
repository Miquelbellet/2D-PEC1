using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineScript : MonoBehaviour
{
    //Todos los estados posibles de la maquina de estados
    [HideInInspector] public enum playerStates { Waiting, PlayerAsking, ComputerResponding, ComputerAsking, PlayerResponding, PlayerCorrect, PlayerIncorrect, ComputerCorrect, ComputerInorrect };
    [HideInInspector] public playerStates playerState;

    private GameControllerScript gameController;
    private int firstPlayer;
    private float time;
    private bool isDebug = false;

    void Start()
    {
        //Aqui se inicializan tadas las varibales i se activan o desactivan los elementos necessarios en la escena
        initVariables();
        //Para saber quin empieza primero un integer random, '0' empieza player1 y '1' empieza player2
        firstPlayer = Random.Range(0, 2);
        if (firstPlayer == 0) playerState = playerStates.PlayerAsking;
        else
        {
            gameController.actionText.gameObject.SetActive(false);
            gameController.actionText.text = "Player2 turn";
            playerState = playerStates.ComputerAsking;
        }
    }

    void Update()
    {
        //Mientras se esté ejecutando la animación inicial, no entrar en la máquina de estados
        time += Time.deltaTime;
        if (time >= gameController.initAnimTime)
        {
            gameController.actionText.gameObject.SetActive(true);
            gameController.panelText.SetActive(true);
            StateMachine();
        }
    }

    private void initVariables()
    {
        gameController = GetComponent<GameControllerScript>();
        gameController.panelResponses.SetActive(false);
        gameController.panelText.SetActive(false);
    }
    private void StateMachine()
    {
        //Mientras el jugador no tiene que enviar una pregunta o respuesta, que no se enseñe el panel con las preguntas y respuestas
        if (playerState == playerStates.PlayerAsking || playerState == playerStates.PlayerResponding) gameController.panelResponses.SetActive(true);
        else gameController.panelResponses.SetActive(false);

        switch (playerState)
        {
            case playerStates.Waiting:
                if(isDebug) Debug.Log("player waiting");
                break;
            case playerStates.PlayerAsking:
                if (isDebug) Debug.Log("player asking");
                gameController.actionText.text = "Your turn";
                break;
            case playerStates.ComputerResponding:
                if (isDebug) Debug.Log("computer responding");
                break;
            case playerStates.ComputerAsking:
                if (isDebug) Debug.Log("computer asking");
                gameController.ComputerAsking();
                break;
            case playerStates.PlayerResponding:
                if (isDebug) Debug.Log("player responding");
                break;
            case playerStates.PlayerCorrect:
                if (isDebug) Debug.Log("player correct");
                break;
            case playerStates.PlayerIncorrect:
                if (isDebug) Debug.Log("player incorrect");
                break;
            case playerStates.ComputerCorrect:
                if (isDebug) Debug.Log("Computer Correct");
                break;
            case playerStates.ComputerInorrect:
                if (isDebug) Debug.Log("Computer Inorrect");
                break;
            default:
                playerState = playerStates.Waiting;
                break;
        }
    }
}
