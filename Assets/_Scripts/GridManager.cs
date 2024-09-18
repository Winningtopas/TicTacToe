using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private Vector2Int gridDimensions = new Vector2Int(3,3);
    [SerializeField]
    private List<Image> gridImages = new List<Image>();
    [SerializeField]
    private List<GridTile> gridTiles = new List<GridTile>();

    private GridTile.TileType currentTileType;

    // Start is called before the first frame update
    void Start()
    {
        CreatGrid();
    }

    private void CreatGrid()
    {
        int index = 0;

        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                gridTiles.Add(new GridTile(new Vector3Int(x, y, 0), GridTile.TileType.EMPTY, gridImages[index]));
                index++;
            }
        }
    }

    public void ChangeCurrentType(GridTile.TileType type)
    {
        currentTileType = type;
    }

    public void OnButtonPress(int index)
    {
        gridTiles[index].SetType(currentTileType);
    }
}
