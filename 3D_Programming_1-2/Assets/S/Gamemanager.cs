using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이동을 위해 반드시 필요합니다!
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("--- UI Settings (TextMeshPro) ---")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject gameOverUI;    // 게임오버 패널은 그대로 유지합니다.

    [Header("--- Game Balance ---")]
    public float timeLimit = 60f;
    public int targetClearScore = 500; // 목표 클리어 점수

    private int currentScore = 0;
    private float timeRemaining;
    private bool isGameFinished = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        timeRemaining = timeLimit;

        // 시작할 때 게임오버 UI만 잘 숨겨줍니다.
        if (gameOverUI != null) gameOverUI.SetActive(false);

        UpdateScoreUI();
    }

    void Update()
    {
        if (isGameFinished)
        {
            // 게임오버 상태일 때만 R키로 재시작
            if (gameOverUI != null && gameOverUI.activeSelf && Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
            return;
        }

        // 타이머 로직
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            timeRemaining = 0;
            TriggerGameOver("시간 초과!");
        }
    }

    public void AddScore(int amount)
    {
        if (isGameFinished) return;
        currentScore += amount;
        UpdateScoreUI();

        // ★ 코인을 먹어서 500점이 되는 순간!
        if (currentScore >= targetClearScore)
        {
            TriggerGameClearDirect();
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + currentScore;
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + timeRemaining.ToString("F1") + "s";
        }
    }

    // ★ [핵심 변경] 패널을 띄우지 않고, 그 즉시 메인화면 씬으로 강제 이동합니다.
    public void TriggerGameClearDirect()
    {
        isGameFinished = true;
        Debug.Log("게임 클리어! 500점 달성 - 메인 화면으로 즉시 이동합니다.");

        // 시간을 혹시 정지했다면 반드시 1f로 풀어주고 넘어가야 다음 씬이 안 멈춥니다.
        Time.timeScale = 1f;

        // "MainMenu" 자리에 유니티에 있는 실제 메인화면 씬 이름을 똑같이 적어주세요.
        SceneManager.LoadScene("MainMenu_1");
    }

    public void TriggerGameOver(string reason)
    {
        if (isGameFinished) return;
        isGameFinished = true;
        Time.timeScale = 0f;

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            TextMeshProUGUI reasonTxt = gameOverUI.GetComponentInChildren<TextMeshProUGUI>();
            if (reasonTxt != null && reasonTxt != scoreText && reasonTxt != timerText)
                reasonTxt.text = reason + "\n'R'키를 눌러 재시작";
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}