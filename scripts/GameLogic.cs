using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public float boxY = 0;
    public float offsetX = 1.3f;
    public float offSetZ = 0;
    public float firstZ;
    public float radius;
    public float power = 50;

    public int V = 0;
    public int bombsCount = 99;
    public int width;
    public int height;
    public int cellsUncovered;
    public int minesToUncover;

    private string _firstClickName;

    private bool _victorious = false;
    private bool hasClicked;

    Vector3 position;

    public GameObject objectsToHide;
    public GameObject objectsToShow;
    public GameObject prefab;
    public GameObject startingTile;
    public GameObject itemsToExplode;
    public GameObject NPCS;
    public GameObject flag;
    public GameObject canvas;
    private GameObject _tile;

    RaycastHit hit;

    private List<List<Cell>> cells;
    private List<GameObject> bombList;
    private List<RuntimeAnimatorController> victories;

    public RuntimeAnimatorController victory1;
    public RuntimeAnimatorController victory2;
    public RuntimeAnimatorController victory3;

    public AudioSource explosionSound;
    public AudioSource ambienceSound;

    void Start()
    {
        victories = new List<RuntimeAnimatorController>();

        victories.Add(victory1);
        victories.Add(victory2);
        victories.Add(victory3);

        cells = new List<List<Cell>>();
        bombList = new List<GameObject>();

        InitiateField();

        minesToUncover = bombsCount;
    }

    private void PlaceBombs()
    {
        Random ran = new Random();
        var bombLocations = new List<string>();

        for (; bombLocations.Count <= bombsCount;)
        {
            var num = Random.Range(0, cells.Count * cells[0].Count);
            if (!bombLocations.Contains(num.ToString()) && num.ToString() != _firstClickName)
            {
                bombLocations.Add(num.ToString());
            }
        }

        for (int x = 0; x < bombLocations.Count; x++)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                for (int j = 0; j < cells[i].Count; j++)
                {
                    if (cells[i][j].cellObject.name == bombLocations[x])
                    {
                        cells[i][j].isMine = true;
                        bombList.Add(cells[i][j].cellObject);
                        cells[i][j].x = j;
                        cells[i][j].y = i;

                        CountNearbyBombs(cells[i][j]);

                        cells[i][j].cellObject.AddComponent<Explosion>();
                        cells[i][j].cellObject.GetComponent<Explosion>().power = power;
                        cells[i][j].cellObject.GetComponent<Explosion>().radius = radius;
                        cells[i][j].cellObject.GetComponent<Explosion>().explosionSound = explosionSound;
                    }
                }
            }
        }
    }

    private void CountNearbyBombs(Cell bomb)
    {
        int x = bomb.x;
        int y = bomb.y;
        int up = y - 1;
        int down = y + 1;
        int right = x + 1;
        int left = x - 1;

        //increase left neighbor cell
        if (left >= 0 && left < cells[0].Count)
        {
            cells[y][left].mines++;
        }

        //increase right neighbor cell
        if (right >= 0 && right < cells[0].Count)
        {
            cells[y][right].mines++;
        }

        // increase top neighbor cell
        if (up >= 0 && up < cells.Count)
        {
            cells[up][x].mines++;
        }

        // increase bottom neighbor cell
        if (down >= 0 && down < cells.Count)
        {
            cells[down][x].mines++;
        }

        // increase diagonally top left neighbor cell
        if (up >= 0 && left >= 0 && up < cells.Count && left < cells[0].Count)
        {
            cells[up][left].mines++;
        }

        // increase diagonally top right neighbor cell
        if (up >= 0 && up < cells.Count && right >= 0 && right < cells[0].Count)
        {
            cells[up][right].mines++;
        }

        // increase diagonally bottom left neighbor cell
        if (down >= 0 && down < cells.Count && left < cells[0].Count && left >= 0)
        {
            cells[down][left].mines++;
        }

        // increase diagonally bottom right neighbor cell
        if (down >= 0 && down < cells.Count && right < cells[0].Count && right >= 0)
        {
            cells[down][right].mines++;
        }
    }

    private void InitiateField()
    {
        int count = 0;
        var rowCellPosition = position;

        for (int i = 0; i < height; i++)
        {
            cells.Add(new List<Cell>());
            offSetZ = firstZ;
            count++;

            position = new Vector3(startingTile.transform.position.x, boxY, startingTile.transform.position.z);

            cells[i].Add(new Cell(SpawnCell(count, position, 0, i)));
            startingTile.transform.position = new Vector3(_tile.transform.position.x, boxY, _tile.transform.position.z - offSetZ);

            for (int j = 0; j < width; j++)
            {
                count++;
                rowCellPosition = new Vector3(_tile.transform.position.x + offsetX, boxY, _tile.transform.position.z);

                cells[i].Add(new Cell(SpawnCell(count, rowCellPosition, j, i)));
            }
        }
    }

    private GameObject SpawnCell(int count, Vector3 position, int x, int y)
    {
        _tile = Instantiate(prefab, position, startingTile.transform.rotation);
        _tile.GetComponentInChildren<TMP_Text>().text = "*";
        _tile.name = count.ToString();

        return _tile;
    }

    public void ExplodeBombs(GameObject selectedMine)
    {
        ambienceSound.Stop();
        // give all items that are supposed to have physics non kinematic effect, so that they will
        // fly around, creating explosion effect
        for (int i = 0; i < itemsToExplode.transform.childCount; i++)
        {
            itemsToExplode.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        // creates effect as if items have exploded via hiding exploded items,
        // and showing the new after explosion objects
        objectsToHide.SetActive(false);
        objectsToShow.SetActive(true);

        // kill npc's
        for (int i = 0; i < NPCS.transform.childCount; i++)
        {
            NPCS.transform.GetChild(i).gameObject.GetComponent<Animator>().enabled = false;
            NPCS.transform.GetChild(i).gameObject.GetComponent<MakeBloodSplatter>().enabled = true;
        }

        // explode every bomb cell
        bombList.ForEach(x => x.GetComponent<Explosion>().explode = true);
    }

    void Update()
    {

        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }


        // Checks for victory
        var openedCells = 0;
        foreach (var cellList in cells)
        {
            foreach (var cell in cellList)
            {
                if (cell.opened)
                {
                    // counts opened cells, if opened cells equals amount of cells on the grid
                    // call victory
                    openedCells++;
                }
                if (openedCells >= cellsUncovered)
                {
                    if (!_victorious)
                    {
                        _victorious = true;
                        canvas.transform.GetChild(1).gameObject.SetActive(true);

                        var num = Random.Range(0, 4);

                        for (int i = 0; i < NPCS.transform.childCount; i++)
                        {
                            num = Random.Range(0, 4);
                            if (num <= 2)
                            {
                                // give random victory animation to npcs
                                NPCS.transform.GetChild(i).gameObject.GetComponent<Animator>().runtimeAnimatorController = victories[num];
                            }
                        }
                    }

                    return;
                }
            }
        }

        // Place a flag
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 200f))
            {
                var hitCell = hit.collider.gameObject.transform.parent;

                for (int i = 0; i < cells.Count; i++)
                {
                    for (int j = 0; j < cells[i].Count; j++)
                    {
                        // find the right cell by name
                        if (cells[i][j].cellObject.name == hitCell.name)
                        {
                            // place flags only if it hasn't been flagged/opened already
                            if (!cells[i][j].flaged && minesToUncover >= 0 && !cells[i][j].opened)
                            {
                                minesToUncover--;
                                cells[i][j].flaged = true;

                                var cell = cells[i][j].cellObject;
                                var flagPosition = new Vector3(cell.transform.position.x, cell.transform.position.y + 1f, cell.transform.position.z);
                                var flagInstance = Instantiate(flag, flagPosition, cells[i][j].cellObject.transform.rotation * Quaternion.Euler(0, 90, 0));

                                flagInstance.transform.parent = cells[i][j].cellObject.transform;
                                flagInstance.name = "flag";
                            }
                            // if there is a flag there already delete it
                            else if (cells[i][j].flaged || cells[i][j].opened)
                            {
                                minesToUncover++;
                                cells[i][j].flaged = false;

                                var count = cells[i][j].cellObject.transform.childCount;

                                if (cells[i][j].cellObject.transform.GetChild(count - 1).gameObject.name == "flag")
                                {
                                    Destroy(cells[i][j].cellObject.transform.GetChild(count - 1).gameObject);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Open a box
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 200f))
            {
                var hitCell = hit.collider.gameObject.transform.parent;
                for (int i = 0; i < cells.Count; i++)
                {
                    for (int j = 0; j < cells[i].Count; j++)
                    {
                        if (cells[i][j].cellObject.name == hitCell.name)
                        {
                            // make sure that you can't hit a bomb on your first click
                            if (!hasClicked)
                            {
                                hasClicked = true;
                                _firstClickName = cells[i][j].cellObject.name;
                                PlaceBombs();
                            }
                            // can't click flagged box
                            if (cells[i][j].flaged)
                            {
                                return;
                            }
                            // if it's a bomb then explode
                            if (cells[i][j].isMine)
                            {
                                ExplodeBombs(cells[i][j].cellObject);
                                canvas.transform.GetChild(2).gameObject.SetActive(true);
                                return;
                            }

                            // if it's just an empty box then reveal it, and reveal
                            // all surrounding empty boxes until the numbered boxes
                            revealCells(i, j);
                        }
                    }
                }
            }
        }
    }

    public void revealCells(int row, int column)
    {
        // goes around the neighboring cells recursively, opening them one by one until the numbered boxes appear
        if (row < 0 || row >= cells.Count || column < 0 || column >= cells[0].Count)
        {
            return;
        }

        Cell cell = cells[row][column];

        if (cell.isMine)
        {
            return;
        }

        if (cell.mines > 0 && !cell.opened)
        {
            OpenBox(row, column);
        }
        else if (cell.mines == 0 && !cell.opened)
        {
            Debug.Log("here");
            OpenBox(row, column);
            // call recursive with coordinates, not cells
            revealCells(row - 1, column - 1);
            revealCells(row - 1, column);
            revealCells(row - 1, column + 1);
            revealCells(row, column - 1);
            revealCells(row, column + 1);
            revealCells(row + 1, column - 1);
            revealCells(row + 1, column);
            revealCells(row + 1, column + 1);
        }
    }

    public void OpenBox(int currentY, int currentX)
    {
        // set the box as opened
        cells[currentY][currentX].opened = true;

        // remove child compenet by name lid, to reveal the number inside the box
        if (cells[currentY][currentX].cellObject.transform.GetChild(2).gameObject.name == "lid")
        {
            cells[currentY][currentX].cellObject.transform.GetChild(2).gameObject.AddComponent<SlowlyDisappear>();
        }

        // give number of surrounding mines to that box
        cells[currentY][currentX].cellObject.GetComponentInChildren<TMP_Text>().text = cells[currentY][currentX].mines == 0? "" : cells[currentY][currentX].mines.ToString();
    }
}
