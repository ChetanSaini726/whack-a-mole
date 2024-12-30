using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public TextMeshProUGUI scoreText;
    public GameObject finalScorePanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI timerText;
    public float gameDuration = 60.0f;

    [Range(0, int.MaxValue)]
    private int score = 0;
    private float spawnInterval = 1.5f;
    private bool gameRunning = true;
    private InputAction inputActions;
    private GameObject activeMole;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void CallExternalJS(string value);

    public void SendScore(int score)
    {
            CallExternalJS(score.ToString()); // Call the JavaScript function
    }
#endif
    private void Awake()
    {
        inputActions = new InputAction();
        finalScorePanel.SetActive(false);
        timerText.text = gameDuration.ToString("F0");
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
        StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer()
    {
        float remainingTime = gameDuration;
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            remainingTime -= 1.0f;
            timerText.text = remainingTime.ToString("F0");
        }
        gameRunning = false;
        ShowFinalScore();
    }

    private void ShowFinalScore()
    {
        finalScorePanel.SetActive(true);
        finalScoreText.text = "Final Score: " + score.ToString();
#if UNITY_WEBGL && !UNITY_EDITOR
        // Send the score to the JavaScript function
        SendScore(score);
#endif
        Invoke("ReloadScene", 10f); // Wait n seconds before reloading
    }

    // Reload the current scene
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        UpdateScoreText();

        // Reset activeMole after mole is hit
        if (activeMole == mole)
        {
            activeMole = null;
        }
    }

    private void UpdateScoreText()
    {
        if (score == int.MaxValue) return;
        scoreText.text = score.ToString();
    }
}