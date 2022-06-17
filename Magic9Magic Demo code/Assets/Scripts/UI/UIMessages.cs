using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIMessages : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _surviveXminutes;
    [SerializeField] private Color _surviveXminutesColor;
    [SerializeField] private float _surviveXminutesTime;
    [SerializeField] private GameObject _missionCompleted;
    [SerializeField] private Color _missionCompletedColor;
    [SerializeField] private float _missionCompletedTime;
    [SerializeField] private GameObject _missionFailed;
    [SerializeField] private Color _missionFailedColor;
    [SerializeField] private float _missionFailedTime;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        if (_panel == null)
            Tools.LogError("GameObject _panel = NULL");
        if (_image == null)
            Tools.LogError("Image _image = NULL");
        if (_surviveXminutes == null)
            Tools.LogError("GameObject _surviveXminutes = NULL");
        if (_missionCompleted == null)
            Tools.LogError("GameObject _missionCompleted = NULL");
        if (_missionFailed == null)
            Tools.LogError("GameObject _missionFailed = NULL");
        if (_surviveXminutesTime <= 0)
            Tools.LogError("_surviveXminutesTime <= 0");
        if (_missionCompletedTime <= 0)
            Tools.LogError("_missionCompletedTime <= 0");
        if (_missionFailedTime <= 0)
            Tools.LogError("_missionFailedTime <= 0");
    }

    public void ShowSurviveXminutes()
    {
        _panel.SetActive(true);
        _image.color = _surviveXminutesColor;
        _surviveXminutes.SetActive(true);
        StartCoroutine(HideSurviveXminutes());
    }

    private IEnumerator HideSurviveXminutes()
    {
        yield return new WaitForSeconds(_surviveXminutesTime);
        _surviveXminutes.SetActive(false);
        _panel.SetActive(false);
    }

    public void ShowMissionCompleted()
    {
        _panel.SetActive(true);
        _image.color = _missionCompletedColor;
        _missionCompleted.SetActive(true);
        StartCoroutine(HideMissionCompleted());
    }

    private IEnumerator HideMissionCompleted()
    {
        yield return new WaitForSeconds(_missionCompletedTime);
        _missionCompleted.SetActive(false);
        _panel.SetActive(false);
    }

    public void ShowMissionFailed()
    {
        _panel.SetActive(true);
        _image.color = _missionFailedColor;
        _missionFailed.SetActive(true);
        StartCoroutine(HideMissionFailed());
    }

    private IEnumerator HideMissionFailed()
    {
        yield return new WaitForSeconds(_missionFailedTime);
        _missionFailed.SetActive(false);
        _panel.SetActive(false);
    }




}
