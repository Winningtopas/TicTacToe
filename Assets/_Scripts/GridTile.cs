using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridTile
{
    public enum TileType { EMPTY, CROSS, CIRCLE }

    private Vector3Int position;
    private TileType type;
    private Image image;

    public Vector3Int Position => position;
    public TileType Type => type;
    public Image Image => image;

    public GridTile(Vector3Int position, TileType type, Image image)
    {
        this.position = position;
        this.type = type;
        this.image = image;
    }

    public void SetType(TileType type)
    {
        this.type = type;

        switch (type)
        {
            case TileType.EMPTY:
                image.color = Color.white;
                break;
            case TileType.CIRCLE:
                image.color = Color.red;
                break;
            case TileType.CROSS:
                image.color = Color.green;
                break;
        }
    }

    public void SetPosition(Vector3Int position)
    {
        this.position = position;
    }
}
