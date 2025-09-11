using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField]
    private float targetSurvivalSeconds = 300.0f;

    [SerializeField]
    private bool isRunning = true;

    private float elapsedSeconds = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if(isRunning == true)
        {
            elapsedSeconds += Time.deltaTime;

            if(elapsedSeconds >= targetSurvivalSeconds)
            {
                Debug.Log("목표 생존 시간 도달!!!!");
                isRunning = false;
            }
        }
    }

    public void PauseTimer()
    {
        if(isRunning == true)
        {
            isRunning = false;
        }
    }

    public void ResumeTimer()
    {
        if(isRunning == false)
        {
            isRunning = true;
        }
    }

    public float GetElapsedSeconds()
    {
        return elapsedSeconds;
    }
}
