using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridTile
{
    private Vector3Int position;
    private int tileOwner;
    private Image image;

    public Vector3Int Position => position;
    public int TileOwner => tileOwner;

    public Image Image => image;

    public GridTile(Vector3Int position, int tileOwner, Image image)
    {
        this.position = position;
        this.tileOwner = tileOwner;
        this.image = image;
    }

    public void SetTileOwner(int tileOwner)
    {
        this.tileOwner = tileOwner;

        switch (tileOwner)
        {
            case -1:
                image.color = Color.white;
                break;
            case 0:
                image.color = Color.red;
                break;
            case 1:
                image.color = Color.green;
                break;
        }
    }

    public void SetPosition(Vector3Int position)
    {
        this.position = position;
    }
}
