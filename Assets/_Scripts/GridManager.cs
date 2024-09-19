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
    private GridTile[,] gridTiles;
    //private List<GridTile> gridTiles = new List<GridTile>();

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
        gridTiles = new GridTile[gridDimensions.x, gridDimensions.y];

        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                gridTiles[x, y] = new GridTile(new Vector3Int(x, y), -1, gridImages[index]);
                //gridTiles.Add(new GridTile(new Vector3Int(x, y, 0),);
                index++;
                //CheckNeighbourTiles(x,y,0);
            }
        }
    }

    public void CheckNeighbourTiles(int x, int y)
    {
        GridTile baseTile = gridTiles[x, y];
        GridTile targetTile;

        if (baseTile.TileOwner == -1)
            return;

        if (x > 0 && x < gridDimensions.x)
        {
            targetTile = gridTiles[x - 1, y];
            if (targetTile.TileOwner != -1)
            {
                baseTile.AddNeighbour(targetTile);
                targetTile.AddNeighbour(baseTile);
            }
        }
        if (y > 0 && y < gridDimensions.y)
        {
            targetTile = gridTiles[x, y - 1];
            if (targetTile.TileOwner != -1)
            {
                baseTile.AddNeighbour(targetTile);
                targetTile.AddNeighbour(baseTile);
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
        int x = Mathf.FloorToInt(index / gridDimensions.x);
        int y = index % gridDimensions.y;

        if (gridTiles[x, y].TileOwner == -1)
        {
            gridTiles[x, y].SetTileOwner(currentPlayerIndex);
            turnManager.ChangeTurn();
        }
    }
}
