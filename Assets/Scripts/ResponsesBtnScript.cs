using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponsesBtnScript : MonoBehaviour
{
    private GameObject GOControllers;
    private GameControllerScript gameController;
    private StateMachineScript stateController;
    void Start()
    {
        //Buscar los controladores para saber cuando y que tiene que responder el jugador.
        GOControllers = GameObject.FindWithTag("GameController");
        gameController = GOControllers.GetComponent<GameControllerScript>();
        stateController = GOControllers.GetComponent<StateMachineScript>();
    }

    public void PlayerResponse(Text text)
    {
        //Si el jugador está respondiendo que envie la respuesta a la función CheckResponse
        if (stateController.playerState == StateMachineScript.playerStates.PlayerResponding)
        {
            gameController.CheckResponse(text.text, "player1");
        }
        //Si el jugador está preguntando que haga la función AskQuestion
        else if (stateController.playerState == StateMachineScript.playerStates.PlayerAsking)
        {
            gameController.AskQuestion(text.text);
        }
    }
}
