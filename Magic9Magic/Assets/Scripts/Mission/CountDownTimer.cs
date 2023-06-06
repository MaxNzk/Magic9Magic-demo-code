using System.Collections;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    private Missions.Status _missionStatus = Missions.Status.TimeOver;
    private float _remainingTime;
    private IFinishMission _gameManager;
    private IUIManagerTimer _IUIManagerTimer;
    private bool _isInitialized;
    private bool isWorking;
    private int _minutes;
    private int _seconds;

    public void Initialize(IFinishMission gameManager, float duration, IUIManagerTimer iUIManagerTimer)
    {
        if (_isInitialized)
            return;

        _gameManager = gameManager;
        _IUIManagerTimer = iUIManagerTimer;
        isWorking = true;
        _remainingTime = duration;

        StartCoroutine(UpdateUI());

        _isInitialized = true;
    }

    private void Update()
    {
        if (isWorking)
        {
            CountdownTimer();
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
        _IUIManagerTimer.HideTimer();
        _gameManager.FinishMission(_missionStatus);
    }

    private IEnumerator UpdateUI()
    {
        WaitForSeconds waitSeconds = new WaitForSeconds(1f);
        while (isWorking)
        {
            yield return waitSeconds;
            _IUIManagerTimer.SetTimerText(_minutes, _seconds);
        }
    }

}
