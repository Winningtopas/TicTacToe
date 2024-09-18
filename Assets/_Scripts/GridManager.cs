using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    //References
    private TurnManager turnManager;

    //Variables
    [SerializeField]
    private Vector2Int gridDimensions = new Vector2Int(3, 3);
    [SerializeField]
    private List<Image> gridImages = new List<Image>();
    [SerializeField]
    private List<GridTile> gridTiles = new List<GridTile>();

    private int currentPlayerIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        turnManager = GetComponent<TurnManager>();
        CreatGrid();
    }

    private void CreatGrid()
    {
        int index = 0;

        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                gridTiles.Add(new GridTile(new Vector3Int(x, y, 0), -1, gridImages[index]));
                index++;
            }
        }
    }

    public void ChangeCurrentType(int playerIndex)
    {
        currentPlayerIndex = playerIndex;
    }

    public void OnButtonPress(int index)
    {
        // -1 means a tile without an owner.
        if (gridTiles[index].TileOwner == -1)
        {
            gridTiles[index].SetTileOwner(currentPlayerIndex);
            turnManager.ChangeTurn();
        }
    }
}
