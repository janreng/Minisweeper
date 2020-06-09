﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Tile[,] grid = new Tile[9, 9];

    public List<Tile> tilesToCheck = new List<Tile>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            PlaceMines();
        }
        PlaceClues();

        PlaceBlanks();
    }

    // Update is called once per frame
    void Update()
    {
        InputHandle();
    }

    private void InputHandle()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Mathf.RoundToInt(mousePosition.x);
            int y = Mathf.RoundToInt(mousePosition.y);

            Tile tile = grid[x, y];

            tile.SetIsCovered(false);

            if(tile.tileKind == Tile.TileKind.Blank)
            {
                RevealAdjacentTilesForTileAt(x, y);
            }

        }
    }

    void PlaceMines()
    {
        int x = UnityEngine.Random.Range(0, 9);
        int y = UnityEngine.Random.Range(0, 9);

        if(grid[x, y] == null)
        {
            Tile mineTile = Instantiate(Resources.Load("Prefabs/Mine", typeof(Tile)), new Vector3(x, y, 0), Quaternion.identity) as Tile;

            grid[x, y] = mineTile;

            Debug.Log("(" + x + ", " + y + ")");
        }
        else
        {
            PlaceMines();
        }
    }

    void PlaceClues()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (grid[x, y] == null)
                {
                    int numMines = 0;

                    // North
                    if(y + 1 < 9)
                    {
                        if (grid[x, y + 1] != null && grid[x, y + 1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    // East
                    if(x + 1 < 9)
                    {
                        if (grid[x + 1, y] != null && grid[x + 1, y].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    // South
                    if(y - 1 >= 0)
                    {
                        if(grid[x, y - 1] != null && grid[x, y - 1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    // West
                    if(x - 1 >= 0)
                    {
                        if(grid[x-1,y] != null && grid[x - 1, y].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    // NorthEast
                    if(x+1 < 9 && y+1 < 9)
                    {
                        if(grid[x+1, y+1] != null && grid[x + 1, y + 1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    // NorthWest
                    if(x-1 >= 0 && y+1 < 9)
                    {
                        if(grid[x-1, y+1] != null && grid[x-1, y+1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    // SouthEast
                    if(x+1 < 9 && y - 1 >= 0)
                    {
                        if (grid[x + 1, y - 1] != null && grid[x + 1, y - 1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    // SouthWest
                    if(x-1 >= 0 && y-1 >= 0)
                    {
                        if (grid[x - 1, y - 1] != null && grid[x - 1, y - 1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    if(numMines > 0)
                    {                        
                        Tile clueTile = Instantiate(Resources.Load("Prefabs/" + numMines, typeof(Tile)), new Vector3(x, y, 0), Quaternion.identity) as Tile;

                        grid[x, y] = clueTile;
                    }
                }
            }
        }
    }

    void PlaceBlanks()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if(grid[x,y] == null)
                {
                    Tile blankTile = Instantiate(Resources.Load("Prefabs/blank", typeof(Tile)), new Vector3(x, y, 0), Quaternion.identity) as Tile;

                    grid[x, y] = blankTile;
                }
            }
        }
    }

    void RevealAdjacentTilesForTileAt(int x, int y)
    {
        // North
        if((y + 1) < 9)
        {
            CheckTileAt(x, y + 1);
        }

        // East
        if ((x + 1) < 9)
        {
            CheckTileAt(x + 1, y);
        }

        // South
        if ((y - 1) >= 0)
        {
            CheckTileAt(x, y - 1);
        }

        // West
        if ((x - 1) >= 0)
        {
            CheckTileAt(x - 1, y);
        }

        // North East
        if ((y + 1) < 9 && (x + 1) < 9)
        {
            CheckTileAt(x + 1, y + 1);
        }

        // North West
        if ((y + 1) < 9 && (x - 1) >= 0)
        {
            CheckTileAt(x - 1, y + 1);
        }

        // South East
        if ((y - 1) >= 0 && (x + 1) < 9)
        {
            CheckTileAt(x + 1, y - 1);
        }

        // South West
        if ((y - 1) >= 0 && (x - 1) >= 0)
        {
            CheckTileAt(x - 1, y - 1);
        }

        for (int i = tilesToCheck.Count - 1;  i >= 0; i--)
        {
            if(tilesToCheck[i].didCheck)
            {
                tilesToCheck.RemoveAt(i);
            }
        }

        if (tilesToCheck.Count > 0)
        {
            RevealAdjacentTilesForTiles();
        }
    }

    void RevealAdjacentTilesForTiles()
    {
        for (int i = 0; i < tilesToCheck.Count; i++)
        {
            Tile tile = tilesToCheck[i];

            int x = (int)tile.gameObject.transform.localPosition.x;
            int y = (int)tile.gameObject.transform.localPosition.y;

            tile.didCheck = true;

            tile.SetIsCovered(false);

            RevealAdjacentTilesForTileAt(x, y);
        }
    }

    void CheckTileAt(int x, int y)
    {
        Tile tile = grid[x, y];

        if(tile.tileKind == Tile.TileKind.Blank)
        {
            tilesToCheck.Add(tile);
            Debug.Log("Tile @ (" + x + ", " + y + ") is a Blank Tile");
        }
        else if (tile.tileKind == Tile.TileKind.Clue)
        {
            tile.SetIsCovered(false);
            Debug.Log("Tile @ (" + x + ", " + y + ") is a Clue Tile");
        }
        else if (tile.tileKind == Tile.TileKind.Mine)
        {
            Debug.Log("Tile @ (" + x + ", " + y + ") is a Mine Tile");
        }

    }
}
