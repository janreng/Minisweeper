using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellState
{
    Mine,
    Opened,
    Flag,
    Marked
}

public class Cell : MonoBehaviour
{
    public bool IsMine = false;
    public bool IsOpened = false;
    public bool IsFlag = false;
    public bool IsMarked = false;
    public int MinesAround = 0;
}
