using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public int playerAmount = 2;

    private GridManager gridManager;
    private int playerTurnIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        gridManager = GetComponent<GridManager>();
    }

    public void ChangeTurn()
    {
        playerTurnIndex++;

        if (playerTurnIndex == playerAmount)
            playerTurnIndex = 0;

        gridManager.ChangeCurrentType(playerTurnIndex);
    }
}
