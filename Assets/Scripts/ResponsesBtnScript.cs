using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponsesBtnScript : MonoBehaviour
{
    private GameControllerScript gameController;
    private StateMachineScript stateController;
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();
        stateController = GameObject.FindWithTag("GameController").GetComponent<StateMachineScript>();
    }

    void Update()
    {
        
    }

    public void PlayerResponse(Text text)
    {
        if (stateController.playerState == StateMachineScript.playerStates.Responding)
        {
            gameController.CheckResponse(text.text, "player1");
        }
        else if (stateController.playerState == StateMachineScript.playerStates.Asking)
        {
            gameController.AskQuestion(text.text);
        }
        
    }
}
