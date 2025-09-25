using UnityEngine;

/// <summary>
/// ��� '��'�� x������, ���̵� ��Į�� y�� Ŀ��� ����.
/// - �ʹ� �ϸ�, �Ĺ� ���ĸ� ���� ���¸� ���������� ����.
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

        // [����] �� �� �� ��ȯ.
        // [����] �� = ��/60.
        float minutes = runClock.GetElapsedSeconds() / 60.0f;

        // [����] Ŀ�� ����.
        float s = difficultyOverMinutes.Evaluate(minutes);

        // [����] ���� ������ Ŭ����.
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
