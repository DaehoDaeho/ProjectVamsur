using UnityEngine;
using UnityEngine.UI;
using System.Text;

/// <summary>
/// 좌상단 텍스트 HUD: Time MM:SS / FPS(EMA) / Enemies.
/// </summary>
public class DebugMiniHUD : MonoBehaviour
{
    [SerializeField]
    private RunClock runClock;

    [SerializeField]
    private SpawnerEnemyRing spawner;

    [SerializeField]
    private Text uiText;

    private float fpsSmoothed = 60.0f;

    private void Update()
    {
        float instant = 0.0f;

        if (Time.deltaTime > 0.0f)
        {
            // [무엇] 즉시 FPS = 1/dt.
            // [단위] 프레임/초.
            instant = 1.0f / Time.deltaTime;
        }

        // [무엇] EMA 평활(α=0.1).
        fpsSmoothed = Mathf.Lerp(fpsSmoothed, instant, 0.1f);

        int mm = 0;
        int ss = 0;

        if (runClock != null)
        {
            float elapsed = runClock.GetElapsedSeconds();
            mm = Mathf.FloorToInt(elapsed / 60.0f);
            ss = Mathf.FloorToInt(elapsed - mm * 60.0f);
        }

        int alive = 0;

        if (spawner != null)
        {
            alive = spawner.GetAliveCount();
        }

        StringBuilder sb = new StringBuilder();
        sb.Append("Time ");
        sb.Append(mm.ToString("00"));
        sb.Append(":");
        sb.Append(ss.ToString("00"));
        sb.AppendLine();
        sb.Append("FPS ~");
        sb.Append(Mathf.RoundToInt(fpsSmoothed).ToString());
        sb.AppendLine();
        sb.Append("Enemies ");
        sb.Append(alive.ToString());

        if (uiText != null)
        {
            uiText.text = sb.ToString();
        }
    }
}
