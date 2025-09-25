using UnityEngine;

/// <summary>
/// '�� ���� ���� ���� �÷��� ����ð�(��)'�� ����.
/// - �Ͻ����� ������ ����.
/// - ���̵�/���/������ ���� ���� �ð�.
/// </summary>
public class RunClock : MonoBehaviour
{
    private float startTime = 0.0f;        // [����] ��(Time.time)
    private bool isPaused = false;
    private float pausedAt = 0.0f;         // [����] ��
    private float accumulatedPause = 0.0f; // [����] ��

    private void OnEnable()
    {
        // [����] �� ���� �ð� �ʱ�ȭ.
        // [��] ����ð� = ���� - ���� - ��������.
        startTime = Time.time;
        isPaused = false;
        pausedAt = 0.0f;
        accumulatedPause = 0.0f;
    }

    public void SetPaused(bool pause)
    {
        if (pause == true)
        {
            if (isPaused == false)
            {
                isPaused = true;
                pausedAt = Time.time; // [����] ���� ���� �ð�.
            }
        }
        else
        {
            if (isPaused == true)
            {
                isPaused = false;
                // [����] �������� += ���� - ��������.
                // [����] ��.
                accumulatedPause = accumulatedPause + (Time.time - pausedAt);
            }
        }
    }

    public float GetElapsedSeconds()
    {
        float now = Time.time;
        float pausedSum = accumulatedPause;

        if (isPaused == true)
        {
            // [����] ���� �߿��� �ð��� �帣�Ƿ� �ӽ� �ջ�.
            pausedSum = pausedSum + (now - pausedAt);
        }

        // [����] ��� = ���� - ���� - ��������.
        // [����] ��.
        float elapsed = now - startTime - pausedSum;

        if (elapsed < 0.0f)
        {
            elapsed = 0.0f;
        }

        return elapsed;
    }
}
