using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("--- 배경음악 설정 ---")]
    public AudioClip bgmClip;         // 재생할 BGM 음악 파일
    [Range(0f, 1f)]
    public float bgmVolume = 0.4f;    // BGM 볼륨 (기본 0.4)

    void Awake()
    {
        // 1. 오디오 소스 컴포넌트를 자동으로 가져옵니다. 없으면 생성합니다.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 2. 2D 사운드 및 무한 반복(Loop) 설정
        audioSource.playOnAwake = true;
        audioSource.loop = true;
        audioSource.spatialBlend = 0f; // 3D가 아닌 2D로 설정하여 전체 화면에 고르게 들리게 함
    }

    void Start()
    {
        // 3. 음원 파일이 등록되어 있다면 게임 시작과 동시에 재생
        if (bgmClip != null)
        {
            audioSource.clip = bgmClip;
            audioSource.volume = bgmVolume;
            audioSource.Play();
            Debug.Log("[BGM] 배경음악 재생 시작");
        }
        else
        {
            Debug.LogWarning("[BGM] bgmClip에 음악 파일이 비어있습니다!");
        }
    }
}