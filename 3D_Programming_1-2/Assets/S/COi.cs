using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    [Header("--- 아이템 설정 ---")]
    public int scoreValue = 10;   // 플레이어가 이 아이템을 먹었을 때 증가할 점수
    public float rotateSpeed = 100f; // 아이템이 빙글빙글 도는 속도

    [Header("--- 오디오 설정 ---")]
    public AudioClip coinSound;   // 인스펙터창에서 넣을 코인 효과음 파일

    void Update()
    {
        // 제자리에서 회전만 담당
        transform.Rotate(Vector2.up * rotateSpeed * Time.deltaTime);
    }
}