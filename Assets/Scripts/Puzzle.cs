using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public int ID;
    private Renderer renderer;
    private GameController gameController;

    public void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    private void ReplaceBlocks(int currentX, int currentY, int newX, int newY)
    {
        gameController.grid[currentX, currentY].transform.position = gameController.positions[newX, newY];
        gameController.grid[newX, newY] = gameController.grid[currentX, currentY];
        gameController.grid[currentX, currentY] = null;
        gameController.CheckGameFinish();
    }

    private void OnMouseDown()
    {
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (gameController.grid[x, y])
                {
                    if (gameController.grid[x, y].GetComponent<Puzzle>().ID == ID)
                    {
                        if (x > 0 && gameController.grid[x - 1, y] == null)
                        {
                            ReplaceBlocks(x, y, x - 1, y);
                            return;
                        }

                        if (x < 3 && gameController.grid[x + 1, y] == null)
                        {
                            ReplaceBlocks(x, y, x + 1, y);
                            return;
                        }
                    }
                }
                if (gameController.grid[x, y])
                {
                    if (gameController.grid[x, y].GetComponent<Puzzle>().ID == ID)
                    {
                        if (y > 0 && gameController.grid[x, y - 1] == null)
                        {
                            ReplaceBlocks(x, y, x, y - 1);
                            return;
                        }

                        if (y < 3 && gameController.grid[x, y + 1] == null)
                        {
                            ReplaceBlocks(x, y, x, y + 1);
                            return;
                        }
                    }
                }
            }
        }
    }

    public void InitPuzzle(Material material, GameController gameController)
    {
        this.gameController = gameController;
        renderer.material = material;
    }
}
