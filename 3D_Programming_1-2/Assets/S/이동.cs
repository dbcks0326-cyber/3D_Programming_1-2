using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("--- 이동 경로 설정 ---")]
    public Transform pointA; // 시작 지점 위치 (빈 오브젝트 배치)
    public Transform pointB; // 도착 지점 위치 (빈 오브젝트 배치)
    public float speed = 3f;  // 플랫폼 이동 속도

    private Vector3 targetPosition;

    void Start()
    {
        // 시작할 때 목표 지점을 Point A로 설정
        if (pointA != null)
        {
            transform.position = pointA.position;
            targetPosition = pointB.position;
        }
    }

    void Update()
    {
        if (pointA == null || pointB == null) return;

        // 목표 지점을 향해 매 프레임 부드럽게 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 목표 지점에 거의 도달했다면 타겟을 반대편으로 변경 (왕복)
        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            targetPosition = targetPosition == pointA.position ? pointB.position : pointA.position;
        }
    }

    // --- 플레이어를 플랫폼과 함께 움직이게 만드는 탑승 로직 ---
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 발판 위에 올라타면
        if (other.CompareTag("Player"))
        {
            // 플레이어 오브젝트를 플랫폼의 자식으로 등록하여 함께 움직이게 만듭니다.
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 발판에서 점프하거나 벗어나면
        if (other.CompareTag("Player"))
        {
            // 자식 관계를 해제하여 독립적으로 움직이게 만듭니다.
            other.transform.SetParent(null);

            // 만약 하이어라키 창이 복잡해지는 걸 막으려면 아래 코드로 씬 최상단 원위치 시킵니다.
            DontDestroyOnLoad(other.gameObject);
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(other.gameObject, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
    }
}