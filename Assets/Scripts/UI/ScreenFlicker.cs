using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlicker : MonoBehaviour
{
    public Image overlayImage;       // �������� ������ UI Image
    public float maxAlpha = 1f;      // ������ ���� �ִ� Alpha �� (0~1 ����)
    public float minAlpha = 0f;      // ������ ���� �ּ� Alpha �� (0~1 ����)
    public float flickerSpeed = 0.2f;// ������ �ӵ� (�� �� �����ӿ� �ɸ��� �ð�)
    public float flickerInterval = 0.1f; // ������ ���� ���� (�� �� ������ ��)
    public float offDuration = 3f;   // ���� ���¿��� ��� �ð�

    void Start()
    {
        // ������ �ڷ�ƾ ����
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // �� �� ������ (�����ٰ� ������, �ٽ� �����ٰ� ����)
            yield return StartCoroutine(FlickerEffect());
            yield return new WaitForSeconds(flickerInterval); // ù ��°�� �� ��° ������ ���� ����
            yield return StartCoroutine(FlickerEffect());

            // ������ �� ���� �ð� ���� ���� ����
            yield return new WaitForSeconds(offDuration);
        }
    }

    // ȭ���� �Ѱ� ���� ������ ȿ��
    IEnumerator FlickerEffect()
    {
        // ȭ�� ���� (�ִ� Alpha��)
        yield return StartCoroutine(FadeToAlpha(maxAlpha, flickerSpeed));

        // ȭ�� ���� (�ּ� Alpha��)
        yield return StartCoroutine(FadeToAlpha(minAlpha, flickerSpeed));
    }

    // Alpha ���� ������ �����Ͽ� ȭ���� �Ѱ� ���� ȿ��
    IEnumerator FadeToAlpha(float targetAlpha, float duration)
    {
        float startAlpha = overlayImage.color.a;  // ���� ���� ��
        float elapsedTime = 0f;

        // ���� ���� ������ ����
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            overlayImage.color = new Color(overlayImage.color.r, overlayImage.color.g, overlayImage.color.b, newAlpha);
            yield return null;
        }

        // ���������� ��ǥ ���� ���� ����
        overlayImage.color = new Color(overlayImage.color.r, overlayImage.color.g, overlayImage.color.b, targetAlpha);
    }
}
