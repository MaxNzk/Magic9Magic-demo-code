using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    private Missions.Status _missionStatus = Missions.Status.TimeOver;
    private float _remainingTime;
    private IFinishMission _gameManager;
    private ITimer _uiManagerWorld;
    private bool _isInitialized;
    private bool isWorking;
    private int _minutes;
    private int _seconds;

    public void Initialize(IFinishMission gameManager, float duration, ITimer uiManagerWorld)
    {
        if (_isInitialized) return;

        _gameManager = gameManager;
        _uiManagerWorld = uiManagerWorld;
        isWorking = true;
        _remainingTime = duration;

        _isInitialized = true;
    }

    private void Update()
    {
        if (isWorking)
        {
            CountdownTimer();
            UpdateUI();
        }
    }

    private void CountdownTimer()
    {
        if (_remainingTime > 0)
        {
            _remainingTime -= Time.deltaTime;
            _minutes = Mathf.FloorToInt(_remainingTime / 60f);
            _seconds = Mathf.RoundToInt(_remainingTime % 60f);
            _minutes = Mathf.Clamp(_minutes, 0, 59);
            _seconds = Mathf.Clamp(_seconds, 0, 59);
        }
        else
        {
            TimerOver();
        }
    }

    private void TimerOver()
    {
        isWorking = false;
        _isInitialized = false;
        _uiManagerWorld.HideTimer();
        _gameManager.FinishMission(_missionStatus);
    }

    private void UpdateUI()
    {
        _uiManagerWorld.SetTimerText(_minutes, _seconds);
    }

}
