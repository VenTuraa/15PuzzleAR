using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public GameObject[,] grid;
    public Vector3[,] positions;

    public float startPosX;
    public float startPosY;
    public float offsetX;
    public float offsetY;


    private GameObject[] random_cells;
    private bool win = false;
    private int puzzleLength = 0;

    [SerializeField] private Text txtCongratulations;
    [SerializeField] private Button btnRestart;
    [SerializeField] private Material[] puzzleMaterials;
    [SerializeField] private GameObject goPuzzlePrefab;

    private void Awake()
    {
        btnRestart.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }

    private void Start()
    {
        puzzleLength = puzzleMaterials.Length;
        random_cells = new GameObject[puzzleLength];
        float posXreset = startPosX;
        positions = new Vector3[4, 4];
        for (int y = 0; y < 4; y++)
        {
            startPosY -= offsetY;
            for (int x = 0; x < 4; x++)
            {
                startPosX += offsetX;
                positions[x, y] = new Vector3(startPosX, startPosY, 0);
            }
            startPosX = posXreset;

        }
        RandomPuzzle(true);
    }


    private void CreatePuzzle()
    {
        int i = 0;
        int j = 0;
        grid = new GameObject[4, 4];
        int randomHorizontal = Random.Range(0, 3);
        int randomVertical = Random.Range(0, 3);

        GameObject clone = new GameObject();
        grid[randomHorizontal, randomVertical] = clone;

        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {

                if (grid[x, y] == null)
                {
                    startPosX += offsetX;
                    grid[x, y] = random_cells[i];
                    grid[x, y].transform.position = positions[x, y];
                    grid[x, y].name = "ID-" + i;
                    grid[x, y].transform.parent = transform;
                    i++;
                    j++;
                }
                else
                {
                    j++;
                }
            }
        }

        Destroy(clone);
    }

    private void RandomPuzzle(bool isRandom)
    {
        if (isRandom)
        {
            int[] tmp = new int[puzzleLength];
            for (int i = 0; i < puzzleLength; i++)
            {
                tmp[i] = 1;
            }
            int c = 0;
            while (c < puzzleLength)
            {
                int r = Random.Range(0, puzzleLength);
                if (tmp[r] == 1)
                {
                    random_cells[c] = Instantiate(goPuzzlePrefab, new Vector3(0, 5, 0), new Quaternion(0, 0, 180, 0)) as GameObject;
                    Puzzle newPuzzle = random_cells[c].GetComponent<Puzzle>();
                    newPuzzle.InitPuzzle(puzzleMaterials[r], this);
                    newPuzzle.ID = c;
                    tmp[r] = 0;
                    c++;
                }
            }
            CreatePuzzle();
        }
        else
        {
            CreatePuzzle();
        }
    }

    public void CheckGameFinish()
    {
        int i = 1;
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (grid[x, y])
                {
                    if (grid[x, y].GetComponent<Puzzle>().ID == i)
                        i++;
                } 
                else i--;
            }
        }
        if (i == 15)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (grid[x, y]) Destroy(grid[x, y].GetComponent<Puzzle>());
                }
            }
            win = true;
            txtCongratulations.color = new Color(0, 0, 0, 1);

            Debug.Log("Finish!");
        }
    }

}
