using UnityEngine;

class Cell
{
    public int x;
    public int y;
    public bool isMine;
    public bool opened;
    public bool flaged;
    public int mines;
    public int[] previousCell = new int[2];
    public GameObject cellObject;

    public Cell(GameObject cellObject)
    {
        this.cellObject = cellObject;
    }
}
