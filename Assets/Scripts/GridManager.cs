using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject molePrefab;
    public GameObject holePrefab;
    public int rows = 3;          // Default grid rows
    public int cols = 3;          // Default grid columns
    public float cellSize = 2.0f; // Spacing between cells

    public GameObject[,] grid;

    [SerializeField]
    private float holePadding = -0.3f;

    public void GenerateGrid()
    {
        grid = new GameObject[rows, cols];
        Vector2 startPosition = new Vector2(-(cols - 1) / 2f * cellSize, -(rows - 1) / 2f * cellSize);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector2 position = startPosition + new Vector2(col * cellSize, row * cellSize);
                Instantiate(holePrefab, position + new Vector2(0, holePadding), Quaternion.identity, transform);
                GameObject mole = Instantiate(molePrefab, position, Quaternion.identity, transform);
                mole.SetActive(false); // Moles are initially inactive
                grid[row, col] = mole;
            }
        }
    }
}
