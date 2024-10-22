using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlicker : MonoBehaviour
{
    public Image overlayImage;       // 깜빡임을 적용할 UI Image
    public float maxAlpha = 1f;      // 켜졌을 때의 최대 Alpha 값 (0~1 사이)
    public float minAlpha = 0f;      // 꺼졌을 때의 최소 Alpha 값 (0~1 사이)
    public float flickerSpeed = 0.2f;// 깜빡임 속도 (한 번 깜빡임에 걸리는 시간)
    public float flickerInterval = 0.1f; // 깜빡임 사이 간격 (두 번 깜빡일 때)
    public float offDuration = 3f;   // 꺼진 상태에서 대기 시간

    void Start()
    {
        // 깜빡임 코루틴 시작
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // 두 번 깜빡임 (켜졌다가 꺼지고, 다시 켜졌다가 꺼짐)
            yield return StartCoroutine(FlickerEffect());
            yield return new WaitForSeconds(flickerInterval); // 첫 번째와 두 번째 깜빡임 사이 간격
            yield return StartCoroutine(FlickerEffect());

            // 깜빡임 후 오랜 시간 꺼진 상태 유지
            yield return new WaitForSeconds(offDuration);
        }
    }

    // 화면을 켜고 끄는 깜빡임 효과
    IEnumerator FlickerEffect()
    {
        // 화면 켜짐 (최대 Alpha로)
        yield return StartCoroutine(FadeToAlpha(maxAlpha, flickerSpeed));

        // 화면 꺼짐 (최소 Alpha로)
        yield return StartCoroutine(FadeToAlpha(minAlpha, flickerSpeed));
    }

    // Alpha 값을 서서히 조정하여 화면을 켜고 끄는 효과
    IEnumerator FadeToAlpha(float targetAlpha, float duration)
    {
        float startAlpha = overlayImage.color.a;  // 현재 알파 값
        float elapsedTime = 0f;

        // 알파 값을 서서히 변경
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            overlayImage.color = new Color(overlayImage.color.r, overlayImage.color.g, overlayImage.color.b, newAlpha);
            yield return null;
        }

        // 최종적으로 목표 알파 값에 도달
        overlayImage.color = new Color(overlayImage.color.r, overlayImage.color.g, overlayImage.color.b, targetAlpha);
    }
}
