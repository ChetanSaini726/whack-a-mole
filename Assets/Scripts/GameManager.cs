using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public TextMeshProUGUI scoreText;

    private int score = 0;
    private float spawnInterval = 1.5f;
    private bool gameRunning = true;
    private InputAction inputActions;
    private GameObject activeMole;

    [SerializeField]
    [Range(0.1f, 1.5f)]
    private float difficulty = 0.02f;

    private void Awake()
    {
        inputActions = new InputAction();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        gridManager.GenerateGrid();
        UpdateScoreText();
        StartCoroutine(SpawnMoles());
    }

    private IEnumerator SpawnMoles()
    {
        while (gameRunning)
        {
            // Ensure only one mole is active at a time
            if (activeMole == null || !activeMole.activeSelf)
            {
                ActivateRandomMole();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void ActivateRandomMole()
    {
        int randomRow = Random.Range(0, gridManager.rows);
        int randomCol = Random.Range(0, gridManager.cols);

        GameObject mole = gridManager.grid[randomRow, randomCol];
        if (!mole.activeSelf)
        {
            mole.SetActive(true);
            activeMole = mole;
            StartCoroutine(DeactivateMole(mole, 1.0f));
        }
    }

    private IEnumerator DeactivateMole(GameObject mole, float delay)
    {
        yield return new WaitForSeconds(delay);
        mole.SetActive(false);

        // Reset activeMole after it's deactivated
        if (activeMole == mole)
        {
            activeMole = null;
        }
    }

    public void MoleHit(GameObject mole)
    {
        mole.SetActive(false);
        score++;
        if (spawnInterval > 0.1f)
            spawnInterval -= difficulty;
        UpdateScoreText();

        // Reset activeMole after mole is hit
        if (activeMole == mole)
        {
            activeMole = null;
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}
