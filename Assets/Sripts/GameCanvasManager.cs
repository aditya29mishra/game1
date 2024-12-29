using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameCanvasManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject threeDotMenuButton;
    public Button playNowButton;
    public Button newGameButton;
    public Button replayGameButton;
    public Button resumeGameButton;
    public Button exitGameButton;

    private bool gameStarted = false;
    private int totalClickableAnchors = 0;
    private int clickedAnchors = 0;

    private int lastRows = 1; // Stores the last played grid rows
    private int lastColumns = 1; // Stores the last played grid columns

    void Start()
    {
        menuPanel.SetActive(true);

        playNowButton.onClick.AddListener(StartPlayNowGame);
        newGameButton.onClick.AddListener(StartNewGame);
        replayGameButton.onClick.AddListener(ReplayGame);
        resumeGameButton.onClick.AddListener(ResumeGame);
        exitGameButton.onClick.AddListener(ExitGame);

        threeDotMenuButton.GetComponent<Button>().onClick.AddListener(ToggleMenu);

        replayGameButton.gameObject.SetActive(false);
        newGameButton.gameObject.SetActive(false);
        exitGameButton.gameObject.SetActive(false);
        resumeGameButton.gameObject.SetActive(false);
    }

    public void StartPlayNowGame()
    {
        menuPanel.SetActive(false);
        Debug.Log("Starting Play Now game with grid size: 1x1");

        clickedAnchors = 0;
        lastRows = 1;
        lastColumns = 1;

        InitializeGame(lastRows, lastColumns);
        playNowButton.gameObject.SetActive(false);
        replayGameButton.gameObject.SetActive(true);
        newGameButton.gameObject.SetActive(true);
        exitGameButton.gameObject.SetActive(true);
        resumeGameButton.gameObject.SetActive(true);
    }

    public void StartNewGame()
    {
        menuPanel.SetActive(false);

        int rows = Random.Range(1, 4);
        int columns = Random.Range(1, 5);
        Debug.Log($"Starting new game with grid size: {rows}x{columns}");

        clickedAnchors = 0;
        lastRows = rows;
        lastColumns = columns;

        InitializeGame(rows, columns);
    }

    public void ReplayGame()
    {
        if (gameStarted)
        {
            clickedAnchors = 0;
            menuPanel.SetActive(false);
            Debug.Log($"Replaying game with grid size: {lastRows}x{lastColumns}");
            InitializeGame(lastRows, lastColumns);
        }
    }

    public void ResumeGame()
    {
        menuPanel.SetActive(false);
        Debug.Log("Resuming game...");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        replayGameButton.gameObject.SetActive(gameStarted);
        newGameButton.gameObject.SetActive(gameStarted);
        exitGameButton.gameObject.SetActive(gameStarted);
        resumeGameButton.gameObject.SetActive(gameStarted);
    }

    private void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    private void InitializeGame(int rows, int columns)
    {
        gameStarted = true;
        GraphGenerator graphGenerator = FindObjectOfType<GraphGenerator>();
        graphGenerator.GenerateGraphWithSize(rows, columns);

        totalClickableAnchors = graphGenerator.GetTotalClickableAnchors();
        Debug.Log($"Total clickable anchors in the game: {totalClickableAnchors}");
    }

    public void OnAnchorTriggered()
    {
        clickedAnchors++;
        Debug.Log($"Anchor triggered. Total clicked: {clickedAnchors}/{totalClickableAnchors}");

        if (clickedAnchors >= totalClickableAnchors)
        {
            StartCoroutine(HandleGameOver());
        }
    }

    private IEnumerator HandleGameOver()
    {
        clickedAnchors = 0;
        Debug.Log("Game Over! All anchors triggered.");
        yield return new WaitForSeconds(1);
        ShowMenu();
    }
}
