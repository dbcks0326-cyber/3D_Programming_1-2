using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("--- 대화 내용 설정 ---")]
    [TextArea(3, 5)]
    public string[] dialogueLines;

    [Header("--- 안내 UI (선택사항) ---")]
    public GameObject pressFNoticeUI; // "Press F to Talk" 안내 UI

    private bool isPlayerInside = false; // 플레이어가 구역 안에 있는지 체크
    private bool isTalking = false;      // 현재 대화가 진행 중인지 체크
    private GameObject playerObject;     // 플레이어 참조 저장용

    void Start()
    {
        if (pressFNoticeUI != null) pressFNoticeUI.SetActive(false);
    }

    void Update()
    {
        // 1. 플레이어가 구역 안에 있고, '현재 대화 중이 아닐 때' F 키를 누르면 대화 시작
        if (isPlayerInside && !isTalking && Input.GetKeyDown(KeyCode.F))
        {
            StartDialogueSequence();
        }
        // 2. 만약 대화 중인데 대화창이 꺼졌다면? (DialogueManager가 대화를 끝내서 Time.timeScale이 1이 된 상태)
        else if (isTalking && Time.timeScale == 1f)
        {
            // 대화 상태를 종료하여, 다시 F를 누를 수 있게 리셋합니다.
            isTalking = false;

            // 구역 안에 여전히 있다면 "Press F" 안내창을 다시 켜줍니다.
            if (isPlayerInside && pressFNoticeUI != null) pressFNoticeUI.SetActive(true);
        }
    }

    // 대화 시퀀스를 시작하는 함수
    void StartDialogueSequence()
    {
        isTalking = true;

        if (pressFNoticeUI != null) pressFNoticeUI.SetActive(false); // 대화 중엔 안내창 끄기

        // 플레이어 움직임 일시정지 (입력값 초기화)
        if (playerObject != null)
        {
            var input = playerObject.GetComponent<StarterAssets.StarterAssetsInputs>();
            if (input != null)
            {
                input.MoveInput(Vector2.zero);
                input.JumpInput(false);
                input.SprintInput(false);
            }
        }

        // 다이얼로그 매니저 실행 (여기서 타임스케일이 0이 됨)
        DialogueManager.Instance.StartDialogue(dialogueLines);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerObject = other.gameObject;

            // 대화 중이 아닐 때만 F키 안내를 띄웁니다.
            if (!isTalking && pressFNoticeUI != null)
                pressFNoticeUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            isTalking = false; // 구역을 완전히 나가면 대화 상태도 안전하게 리셋

            if (pressFNoticeUI != null) pressFNoticeUI.SetActive(false);
        }
    }
}