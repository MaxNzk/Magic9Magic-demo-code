using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMissionMessages : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _image;

    [Space(10)]
    [SerializeField] private GameObject _missionAssignment;
    [SerializeField] private Color _missionAssignmentColor;

    [Space(10)]
    [SerializeField] private GameObject _missionCompleted;
    [SerializeField] private Color _missionCompletedColor;
    [SerializeField] private GameObject _missionCompletedEffect;

    [Space(10)]
    [SerializeField] private GameObject _missionFailed;
    [SerializeField] private Color _missionFailedColor;
    [SerializeField] private GameObject _missionFailedEffect;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckNull<GameObject>(_panel, nameof(_panel), gameObject);
        Tools.CheckNull<Image>(_image, nameof(_image), gameObject);
        Tools.CheckNull<GameObject>(_missionAssignment, nameof(_missionAssignment), gameObject);
        Tools.CheckNull<GameObject>(_missionCompleted, nameof(_missionCompleted), gameObject);
        Tools.CheckNull<GameObject>(_missionFailed, nameof(_missionFailed), gameObject);
    }

    public void ShowMissionAssignment(string msg)
    {
        _panel.SetActive(true);
        _image.color = _missionAssignmentColor;
        _missionAssignment.SetActive(true);
        TextMeshProUGUI missionAssignmentText = _missionAssignment.GetComponent<TextMeshProUGUI>();
        missionAssignmentText.text = msg;
    }

    public void HideMissionAssignment()
    {
        _missionAssignment.SetActive(false);
        _missionCompleted.SetActive(false);
        _missionFailed.SetActive(false);
        _panel.SetActive(false);
    }

    public void ShowMissionCompleted()
    {
        _panel.SetActive(true);
        _image.color = _missionCompletedColor;
        _missionCompleted.SetActive(true);
        _missionCompletedEffect.SetActive(true);
    }

    public void ShowMissionFailed()
    {
        _panel.SetActive(true);
        _image.color = _missionFailedColor;
        _missionFailed.SetActive(true);
        _missionFailedEffect.SetActive(true);
    }

}
