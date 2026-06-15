using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("--- 점프대 설정 ---")]
    [Tooltip("플레이어를 얼마나 높이 날릴지 설정합니다. (기본 점프보다 높은 값 추천)")]
    public float launchForce = 15f;

    private void OnTriggerEnter(Collider other)
    {
        // 밟은 대상이 플레이어인지 확인
        if (other.CompareTag("Player"))
        {
            // Starter Assets의 ThirdPersonController 컴포넌트를 가져옵니다.
            var playerController = other.GetComponent<StarterAssets.ThirdPersonController>();

            if (playerController != null)
            {
                // 플레이어를 강제로 날려버리는 함수 호출
                LaunchPlayer(playerController);
            }
        }
    }

    private void LaunchPlayer(StarterAssets.ThirdPersonController player)
    {
        // 리플렉션이나 컴포넌트 내부 수정을 피하기 위해 가장 안전한 우회 방법을 씁니다.
        // Starter Assets 내부의 수직 속도(VerticalVelocity)를 조작하기 위해
        // 플레이어 스크립트에 직접 함수를 하나 추가해 주는 것이 가장 깔끔합니다.
        player.LaunchFromPad(launchForce);
    }
}