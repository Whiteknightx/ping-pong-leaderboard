using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public TMP_InputField playerNameInput;
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI aiScoreText;
    public TextMeshProUGUI leaderboardText;
    public Button startButton; // Add this in inspector

    private string playerName;
    private int playerScore = 0;
    private int aiScore = 0;
    private const int WIN_SCORE = 10;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Freeze game
        Time.timeScale = 0;

        // Ensure panels are in correct state
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // Add listener to start button
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        else
        {
            Debug.LogError("Start Button is not assigned in the inspector!");
        }

        // Optional: Add listener to input field to start on submit
        if (playerNameInput != null)
        {
            playerNameInput.onSubmit.AddListener(HandleSubmit);
        }
    }

    private void HandleSubmit(string input)
    {
        StartGame();
    }

    public void StartGame()
    {
        // Debug checks
        if (playerNameInput == null)
        {
            Debug.LogError("Player Name Input is not assigned!");
            return;
        }

        // Trim and validate input
        playerName = playerNameInput.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Please enter a player name!");
            return;
        }

        // Reset scores
        playerScore = 0;
        aiScore = 0;

        // Toggle panels
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        gameOverPanel.SetActive(false);

        // Unpause game
        Time.timeScale = 1;

        // Update score display
        UpdateScoreDisplay();

        Debug.Log($"Game started with player: {playerName}");
    }

    // Rest of the script remains the same as in previous version

public void AddScore(string scorer)
    {
        if (scorer == "Player")
        {
            playerScore++;
        }
        else if (scorer == "AI")
        {
            aiScore++;
        }

        UpdateScoreDisplay();
        CheckGameOver();
    }

    private void UpdateScoreDisplay()
    {
        playerScoreText.text = $"{playerName}: {playerScore}";
        aiScoreText.text = $"AI: {aiScore}";
    }

    private void CheckGameOver()
    {
        if (playerScore >= WIN_SCORE || aiScore >= WIN_SCORE)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);

        string winner = playerScore >= WIN_SCORE ? playerName : "AI";
        FindObjectOfType<PlayerDataManager>().SavePlayerData(playerName, playerScore);
        UpdateLeaderboard();
    }

    private void UpdateLeaderboard()
    {
        PlayerDataManager.PlayerList list = FindObjectOfType<PlayerDataManager>().GetLeaderboard();
        string leaderboardContent = "LEADERBOARD\n\n";

        for (int i = 0; i < Mathf.Min(list.players.Count, 5); i++)
        {
            leaderboardContent += $"{i + 1}. {list.players[i].name}: {list.players[i].score}\n";
        }

        leaderboardText.text = leaderboardContent;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
