using UnityEngine;

/// <summary>
/// 경과 '분'을 x축으로, 난이도 스칼라 y를 커브로 샘플.
/// - 초반 완만, 후반 가파름 같은 형태를 직관적으로 조정.
/// </summary>
public class DifficultyScalar : MonoBehaviour
{
    [SerializeField]
    private RunClock runClock;

    [SerializeField]
    private AnimationCurve difficultyOverMinutes = AnimationCurve.EaseInOut(0.0f, 1.0f, 4.0f, 1.8f);

    [SerializeField]
    private float minScalar = 0.5f;

    [SerializeField]
    private float maxScalar = 3.0f;

    public float GetScalar()
    {
        if (runClock == null)
        {
            return 1.0f;
        }

        // [무엇] 초 → 분 변환.
        // [단위] 분 = 초/60.
        float minutes = runClock.GetElapsedSeconds() / 60.0f;

        // [무엇] 커브 샘플.
        float s = difficultyOverMinutes.Evaluate(minutes);

        // [무엇] 안전 범위로 클램프.
        if (s < minScalar)
        {
            s = minScalar;
        }

        if (s > maxScalar)
        {
            s = maxScalar;
        }

        return s;
    }
}
