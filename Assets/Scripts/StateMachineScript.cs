using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineScript : MonoBehaviour
{
    [HideInInspector] public enum playerStates { Waiting, Asking, Listening, Responding, Correct, Incorrect, ComputerCorrect, ComputerInorrect };
    [HideInInspector] public playerStates playerState;

    private GameControllerScript gameController;
    private int firstPlayer;
    private bool computerResponded = false;

    void Start()
    {
        gameController = GetComponent<GameControllerScript>();

        //To know who starts first, 0 starts player1 and 1 starts player2
        firstPlayer = Random.Range(0, 2);
        if (firstPlayer == 0) playerState = playerStates.Asking;
        else playerState = playerStates.Listening;
    }

    void Update()
    {
        StateMachine();
    }

    private void StateMachine()
    {
        switch (playerState)
        {
            case playerStates.Waiting:
                Debug.Log("player witing");
                break;
            case playerStates.Asking:
                Debug.Log("player asking");
                break;
            case playerStates.Listening:
                Debug.Log("player listening");
                gameController.ComputerAsking();
                break;
            case playerStates.Responding:
                Debug.Log("player responding");
                computerResponded = false;
                break;
            case playerStates.Correct:
                Debug.Log("player correct");
                break;
            case playerStates.Incorrect:
                Debug.Log("player incorrect");
                break;
            case playerStates.ComputerCorrect:
                Debug.Log("player ComputerCorrect");
                break;
            case playerStates.ComputerInorrect:
                Debug.Log("player ComputerInorrect");
                break;
            default:
                playerState = playerStates.Waiting;
                break;
        }
    }
}
