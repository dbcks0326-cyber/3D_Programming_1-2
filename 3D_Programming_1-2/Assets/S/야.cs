using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("--- UI Elements ---")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("--- 타이핑 속도 설정 ---")]
    public float typingSpeed = 0.05f;

    [Header("--- 다이얼로그 사운드 설정 ---")]
    public AudioSource audioSource;   // 사운드를 재생할 오디오 소스 컴포넌트
    public AudioClip typingSound;     // 글자 한 자마다 재생될 조그만 효과음 (띡, 띡)

    [Range(0f, 1f)]
    public float soundVolume = 0.5f;  // 너무 시끄럽지 않게 조절할 볼륨 크기

    private Queue<string> sentences;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private string currentSentence = "";
    private Coroutine typingCoroutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        sentences = new Queue<string>();

        // 만약 오디오 소스를 컴포넌트에 깜빡하고 안 넣었다면 자동으로 찾아 붙여줍니다.
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    void Start()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (isDialogueActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentSentence;
                isTyping = false;
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(string[] lines)
    {
        if (dialoguePanel == null || dialogueText == null) return;

        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        sentences.Clear();

        foreach (string line in lines)
        {
            sentences.Enqueue(line);
        }

        Time.timeScale = 0f;
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
    }

    // ★ 글자가 찍힐 때마다 사운드를 재생하는 코루틴
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        isTyping = true;

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            // 공백(띄어쓰기)일 때는 소리가 안 나게 처리하면 훨씬 자연스럽습니다.
            if (letter != ' ' && typingSound != null && audioSource != null)
            {
                // 글자 찍히는 속도가 빠르므로, 이전 소리를 무시하고 즉시 끊어서 재생하도록 PlayOneShot 사용
                audioSource.PlayOneShot(typingSound, soundVolume);
            }

            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}