using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueNotInteractive : MonoBehaviour
{
    public enum DialogueNames
    {
        FirstCamp = 0,
        FirstDeath = 5,
        PickUpFirstItem = 10
    }

    [SerializeField] private GameObject _DialoguePanel;
    private TextBackground _textBackground;
    [SerializeField] private TextMeshProUGUI _replicaText;

    [Space(10)]
    [Header("----- ActorName: -----------------------------")]
    [SerializeField] private TextMeshProUGUI _replicaActorName;
    [SerializeField] private bool _isActorNameInReplica;
    [SerializeField] private string _actorNameStartPart;
    [SerializeField] private string _actorNameEndPart;
    [SerializeField] private bool _isShowPlayerName;
    [SerializeField] private string _playerName;

    [Space(10)]
    [SerializeField] private List<DialogueDataSO> _dialogueList;

    private string _lang;
    private DialogueDataSO _currentDialogue;
    private ISoundManager _soundManager;
    private int _replicaIndex;
    private IEnumerator _playCoroutine;
    private bool _isPlaying;

    public void Initialize(ISoundManager soundManager)
    {
        _soundManager = soundManager;
    }

    private void Awake()
    {
        FindAndTestComponents();
        _playCoroutine = Play();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckNull<GameObject>(_DialoguePanel, nameof(_DialoguePanel), gameObject);
        _textBackground = Tools.GetComponentWithAssertion<TextBackground>(_DialoguePanel);

        Tools.CheckNull<TextMeshProUGUI>(_replicaText, nameof(_replicaText), gameObject);
        if (_isActorNameInReplica == false)
            Tools.CheckNull<TextMeshProUGUI>(_replicaActorName, nameof(_replicaActorName), gameObject);
    }

    private void OnEnable()
    {
        MenuManager.OnShow += ShowMenu;
        MenuManager.OnHide += HideMenu;
    }

    private void OnDisable()
    {
        MenuManager.OnShow -= ShowMenu;
        MenuManager.OnHide -= HideMenu;
    }

    private void ShowMenu()
    {
        if (_isPlaying)
        {
            _soundManager.Pause(GetReplicaSoundName());
            StopCoroutine(_playCoroutine);
        }
    }

    private void HideMenu()
    {
        if (_isPlaying)
        {
            _replicaIndex--;
            StartCoroutine(_playCoroutine);
        }
    }

    public void StartDialogue(DialogueNames dialogueName)
    {
        _lang = Lean.Localization.LeanLocalization.GetFirstCurrentLanguage();
        
        for (int i = 0; i < _dialogueList.Count; i++)
        {
            if (_dialogueList[i].name == dialogueName.ToString())
            {
                _currentDialogue = _dialogueList[i];
                StartCoroutine(_playCoroutine);
                break;
            }
        }
    }

    private IEnumerator Play()
    {
        _isPlaying = true;
        _DialoguePanel.SetActive(true);
        for (_replicaIndex = 0; _replicaIndex < _currentDialogue.Replicas.Length; _replicaIndex++)
        {
            if (_isActorNameInReplica)
            {
                if (_isShowPlayerName)
                {
                    _replicaText.text = GetReplicaActorName();
                    _replicaText.text += GetReplicaText();
                }
                else
                {
                    _replicaText.text = "";
                    if (_playerName != _currentDialogue.Replicas[_replicaIndex].ActorName)
                    {
                        _replicaText.text = GetReplicaActorName();
                    }
                    _replicaText.text += GetReplicaText();
                }
            }
            else
            {
                _replicaActorName.text = GetReplicaActorName();
                _replicaText.text = GetReplicaText();
            }
            _textBackground.UpdateSize();
            _soundManager.Play(GetReplicaSoundName());
            yield return new WaitForSeconds(GetReplicaDuration());
        }
        _DialoguePanel.SetActive(false);
        _isPlaying = false;
    }

    private string GetReplicaActorName()
    {
        string actorName = _actorNameStartPart;
        switch (_lang)
        {
            case "English": actorName += _currentDialogue.Replicas[_replicaIndex].ActorName; break;
            case "Ukraine": actorName += _currentDialogue.ReplicasUkraine[_replicaIndex].ActorName; break;
            case "Russian": actorName += _currentDialogue.ReplicasRussian[_replicaIndex].ActorName; break;
            default: actorName += _currentDialogue.Replicas[_replicaIndex].ActorName; break;
        }

        if (_currentDialogue.Replicas[_replicaIndex].ActorName == "")
            Tools.LogError(_currentDialogue.name + ": replica actor name (en) is empty.");
        if (_currentDialogue.ReplicasUkraine[_replicaIndex].ActorName == "")
            Tools.LogError(_currentDialogue.name + ": replica actor name (uk) is empty.");
        if (_currentDialogue.ReplicasRussian[_replicaIndex].ActorName == "")
            Tools.LogError(_currentDialogue.name + ": replica actor name (ru) is empty.");

        actorName += _actorNameEndPart;
        return actorName;
    }

    private string GetReplicaText()
    {
        string replica;
        switch (_lang)
        {
            case "English": replica = _currentDialogue.Replicas[_replicaIndex].Text; break;
            case "Ukraine": replica = _currentDialogue.ReplicasUkraine[_replicaIndex].Text; break;
            case "Russian": replica = _currentDialogue.ReplicasRussian[_replicaIndex].Text; break;
            default: replica = _currentDialogue.Replicas[_replicaIndex].Text; break;
        }
        if (replica == "")
        {
            replica = _currentDialogue.Replicas[_replicaIndex].Text;
            Tools.LogError(_currentDialogue.name + ": replica text (" + _lang + ") is empty.");
        }
        return replica;
    }

    private string GetReplicaSoundName()
    {
        string soundName;
        switch (_lang)
        {
            case "English": soundName = _currentDialogue.Replicas[_replicaIndex].SoundName.ToString(); break;
            case "Ukraine": soundName = _currentDialogue.ReplicasUkraine[_replicaIndex].SoundName.ToString(); break;
            case "Russian": soundName = _currentDialogue.ReplicasRussian[_replicaIndex].SoundName.ToString(); break;
            default: soundName = _currentDialogue.Replicas[_replicaIndex].SoundName.ToString(); break;
        }
        if (soundName == "")
        {
            soundName = _currentDialogue.Replicas[_replicaIndex].SoundName.ToString();
            Tools.LogError(_currentDialogue.name + ": replica soundName (" + _lang + ") is empty.");
        }
        return soundName;
    }

    private float GetReplicaDuration()
    {
        float duration;
        switch (_lang)
        {
            case "English": duration = _currentDialogue.Replicas[_replicaIndex].Duration; break;
            case "Ukraine": duration = _currentDialogue.ReplicasUkraine[_replicaIndex].Duration; break;
            case "Russian": duration = _currentDialogue.ReplicasRussian[_replicaIndex].Duration; break;
            default: duration = _currentDialogue.Replicas[_replicaIndex].Duration; break;
        }
        if (duration <= 0)
        {
            duration = _currentDialogue.Replicas[_replicaIndex].Duration;
            Tools.LogError(_currentDialogue.name + ": replica (" + _lang + ") duration <= 0.");
        }
        return duration;
    }

}
