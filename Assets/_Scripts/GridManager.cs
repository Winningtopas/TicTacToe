using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.WSA;

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
    [SerializeField]
    private int matchAmount = 3;

    Dictionary<Vector2, int> directionToIndexDictionary = new Dictionary<Vector2, int>()
    {
     {new Vector2(-1,1), 0},
     {new Vector2(0,1), 1},
     {new Vector2(1,1), 2},
     {new Vector2(-1,0), 3},
     {new Vector2(1,0), 4},
     {new Vector2(-1,-1), 5},
     {new Vector2(0,-1), 6},
     {new Vector2(1,-1), 7}
    };

    private int currentPlayerIndex = 0;
    private bool gameHasEnded = false;
    private bool hasMatch = false;
    private bool hasWon = false;

    [SerializeField]
    private int[] neighbourMatchTypeAmount = new int[4];

    //private int neighBourAmount = 0;

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
                index++;
            }
        }
    }

    public void ManageNeighbourTiles(int baseX, int baseY)
    {
        GridTile baseTile = gridTiles[baseX, baseY];

        if (baseTile.TileOwner == -1)
            return;

        // We look into the neighbour tiles in the following order:
        //(-1,-1)(0,-1)(1, -1)      5     3     0
        //(-1, 0)   X  (1,  0)      6     X     1
        //(-1, 1)(0, 1)(1,  1)      7     4     2

        for (int x = baseX - 1; x <= baseX + 1; ++x)
        {
            for (int y = baseY - 1; y <= baseY + 1; ++y)
            {
                int tileIndex = 0;

                //Continue if the tile is outside of the grid area
                if (x < 0 || y < 0 || x == gridDimensions.x || y == gridDimensions.y)
                    continue;

                //Continue if it's comparing the basetile to itself
                if (gridTiles[x, y] == baseTile)
                    continue;

                if (gridTiles[x, y].TileOwner != -1)
                {
                    baseTile.AddNeighbour(gridTiles[x, y], GetIndexFromDirection(new Vector2Int(x, y), new Vector2Int(baseX, baseY)));
                    gridTiles[x, y].AddNeighbour(baseTile, GetIndexFromDirection(new Vector2Int(baseX, baseY), new Vector2Int(x, y)));
                }
            }
        }
        CheckForMatches(baseX, baseY);
    }

    private int GetIndexFromDirection(Vector2Int basePos, Vector2Int newPos)
    {
        Vector2Int direction = basePos - newPos;
        return directionToIndexDictionary[direction];
    }

    private void CheckForMatches(int x, int y)
    {
        GridTile currentTile = gridTiles[x, y];
        GridTile[] neighbours = currentTile.GetNeighbours();
        List<GridTile> adjacentTiles = new List<GridTile>();

        //Horizontal
        StartCoroutine(GoThroughNeighbours(3, 4, currentTile, 0));
        //Vertical
        StartCoroutine(GoThroughNeighbours(1, 6, currentTile, 1));
        //Diagonal Left
        StartCoroutine(GoThroughNeighbours(0, 7, currentTile, 2));
        //Diagonal Right
        StartCoroutine(GoThroughNeighbours(5, 2, currentTile, 3));
    }

    private IEnumerator GoThroughNeighbours(int neighbourIndex1, int neighbourIndex2, GridTile tile, int checkIndex)
    {
        neighbourMatchTypeAmount[checkIndex] = 0;
        GridTile baseTile = tile;

        FindNeighbours(neighbourIndex1, baseTile, tile, checkIndex);
        FindNeighbours(neighbourIndex2, baseTile, tile, checkIndex);

        yield return null;
    }

    private void FindNeighbours(int index, GridTile baseTile, GridTile tile, int checkIndex)
    {
        while (tile != null)
        {
            tile = tile.GetNeighbours()[index];
            if (tile != null)
            {
                //   Debug.Log("tile owner: " + tile.TileOwner + " tile position: " + tile.Position);
                if (tile.TileOwner != baseTile.TileOwner)
                    break;
                else
                {
                    neighbourMatchTypeAmount[checkIndex]++;
                }
            }
        }

        if (neighbourMatchTypeAmount[checkIndex] >= matchAmount - 1)
        {
            EndGame(baseTile.TileOwner);
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
            ManageNeighbourTiles(x, y);
            turnManager.ChangeTurn();
        }
    }

    private void EndGame(int winnerIndex)
    {
        if (!hasWon)
            Debug.Log("Player " + winnerIndex + " wins!");
    }
}
