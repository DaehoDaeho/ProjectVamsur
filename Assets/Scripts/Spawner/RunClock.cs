using UnityEngine;

/// <summary>
/// '런 시작 이후 실제 플레이 경과시간(초)'을 제공.
/// - 일시정지 구간은 제외.
/// - 난이도/통계/진단의 공통 기준 시계.
/// </summary>
public class RunClock : MonoBehaviour
{
    private float startTime = 0.0f;        // [단위] 초(Time.time)
    private bool isPaused = false;
    private float pausedAt = 0.0f;         // [단위] 초
    private float accumulatedPause = 0.0f; // [단위] 초

    private void OnEnable()
    {
        // [무엇] 런 시작 시각 초기화.
        // [왜] 경과시간 = 현재 - 시작 - 누적정지.
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
                pausedAt = Time.time; // [무엇] 정지 진입 시각.
            }
        }
        else
        {
            if (isPaused == true)
            {
                isPaused = false;
                // [무엇] 누적정지 += 현재 - 정지시작.
                // [단위] 초.
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
            // [무엇] 정지 중에도 시간이 흐르므로 임시 합산.
            pausedSum = pausedSum + (now - pausedAt);
        }

        // [무엇] 경과 = 현재 - 시작 - 누적정지.
        // [단위] 초.
        float elapsed = now - startTime - pausedSum;

        if (elapsed < 0.0f)
        {
            elapsed = 0.0f;
        }

        return elapsed;
    }
}
