using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileKind
    {
        Blank,
        Mine,
        Clue
    }

    public bool isCovered = true;
    public bool didCheck = false;

    public Sprite coveredSprite;

    public TileKind tileKind = TileKind.Blank;

    private Sprite defaultSprite;

    private void Start()
    {
        defaultSprite = GetComponent<SpriteRenderer>().sprite;

        GetComponent<SpriteRenderer>().sprite = coveredSprite;
    }

    public void SetIsCovered(bool isCovered)
    {
        isCovered = false;
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }
}
