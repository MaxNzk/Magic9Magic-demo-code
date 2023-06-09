﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] private LayerMask _fireableLayers;
    [SerializeField] private float _shotOffsetY = 2.0f;
    [SerializeField] private ManaMaker _manaMaker;
    [SerializeField] private SoundManager _soundManager;

    [SerializeField] private GameObject[] _weapons;
    [SerializeField] private Image[] _iconSkillSlots;
    [SerializeField] private GameObject[] _manaContainers;
    private TextMeshProUGUI[] _manaTexts;
    [SerializeField] private GameObject[] _focusSlotImages;
    [SerializeField] private Image[] _frameImage;
    [SerializeField] private Image[] _cooldownSlotImages;
    private float[] _cooldownSlotsTime;
    private float[] _currentCooldownSlotsTime;
    private ISkillSlot[] _iSkillSlots;
    private bool[] _isWeaponReady;
    private int[] _manaWeapons;
    private int _activeWeaponIndex;
    private PlayerTakeDamage _playerTakeDamageScript;
    private PlayerMovement _playerMovementScript;

    private Camera _camera;

    private Animator _animator;
    private int _animIdAttack = Animator.StringToHash("attack");
    private int _animIdAttackType = Animator.StringToHash("attackType");

    private InputManager _inputManager;

    public void Initialize(InputManager inputManager)
    {
        _inputManager = inputManager;
        FindAndTestComponents();
        InitializeWeapons();
    }

    private void FindAndTestComponents()
    {
        _camera = Camera.main;
        _playerTakeDamageScript = GetComponent<PlayerTakeDamage>();
        _playerMovementScript = GetComponent<PlayerMovement>();
        _animator = GetComponentInChildren<Animator>();
        if (_animator == null)
            Tools.LogError("Animator _animator = NULL");
        _manaTexts = new TextMeshProUGUI[_manaContainers.Length];
        for (int i = 0; i < _manaContainers.Length; i++)
            _manaTexts[i] = _manaContainers[i].GetComponentInChildren<TextMeshProUGUI>();

        if (_manaMaker == null)
            Tools.LogError("ManaMaker _manaMaker = NULL");
        if (_soundManager == null)
            Tools.LogError("SoundManager _soundManager = NULL");
        if (_playerTakeDamageScript == null)
            Tools.LogError("PlayerTakeDamage _playerTakeDamageScript = NULL");

        for (int i = 0; i < _weapons.Length; i++)
        {
            if (_weapons[i] == null)
                Tools.LogError("GameObject[] _weapons = NULL");
            if (_iconSkillSlots[i] == null)
                Tools.LogError("Image[] _iconSkillSlots = NULL");
            if (_manaContainers[i] == null)
                Tools.LogError("GameObject[] _manaContainers = NULL");
            if (_manaTexts[i] == null)
                Tools.LogError("TextMeshProUGUI[] _manaTexts = NULL");
            if (_focusSlotImages[i] == null)
                Tools.LogError("GameObject[] _focusSlotImages = NULL");
            if (_frameImage[i] == null)
                Tools.LogError("Image[] _frameImage = NULL");
            if (_cooldownSlotImages[i] == null)
                Tools.LogError("Image[] _cooldownSlotImages = NULL");
        }
    }

    private void InitializeWeapons()
    {
        _iSkillSlots = new ISkillSlot[_weapons.Length];
        _manaWeapons = new int[_weapons.Length];
        _cooldownSlotsTime = new float[_weapons.Length];
        _currentCooldownSlotsTime = new float[_weapons.Length];
        _isWeaponReady = new bool[_weapons.Length];
        for (int i = 0; i < _weapons.Length; i++)
        {
            _iSkillSlots[i] = _weapons[i].GetComponent<ISkillSlot>();
            _iconSkillSlots[i].gameObject.SetActive(true);
            _iconSkillSlots[i].sprite = _iSkillSlots[i].Icon;
            _manaContainers[i].SetActive(true);
            _manaTexts[i].text = _iSkillSlots[i].Mana.ToString();
            _manaWeapons[i] = _iSkillSlots[i].Mana;
            _frameImage[i].color = new Color(255, 255, 255, 255);
            _cooldownSlotsTime[i] = _iSkillSlots[i].CooldownTime;
        }
        _focusSlotImages[0].SetActive(true);
        _frameImage[0].color = new Color(255, 0, 0, 255);
    }

    private void Update()
    {
        if (_playerTakeDamageScript.IsDead()) return;
        
        if (_inputManager.CanActivateActiveSlot) ActivateActiveSlot();

        if (_inputManager.GetIsActiveSlot(0)) SetActiveSlot(0);
        if (_inputManager.GetIsActiveSlot(1)) SetActiveSlot(1);
        if (_inputManager.GetIsActiveSlot(2)) SetActiveSlot(2);
        if (_inputManager.GetIsActiveSlot(3)) SetActiveSlot(3);
        if (_inputManager.GetIsActiveSlot(4)) SetActiveSlot(4);
        if (_inputManager.GetIsActiveSlot(5)) SetActiveSlot(5);
        if (_inputManager.GetIsActiveSlot(6)) SetActiveSlot(6);
        if (_inputManager.GetIsActiveSlot(7)) SetActiveSlot(7);
        if (_inputManager.GetIsActiveSlot(8)) SetActiveSlot(8);
        if (_inputManager.GetIsActiveSlot(9)) SetActiveSlot(9);

        CooldownWeapons();
    }

    // Canvas > UILayerHUD > SkillPanel >
    public void SetActiveSlot(int index)
    {
        _activeWeaponIndex = index;
        SetIconFocus();
    }

    private void SetIconFocus()
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            _focusSlotImages[i].SetActive(_activeWeaponIndex == i);
            if (_activeWeaponIndex == i)
                _frameImage[i].color = new Color(255, 0, 0, 255);
            else
                _frameImage[i].color = new Color(255, 255, 255, 255);
        }
    }

    private void ActivateActiveSlot()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (_isWeaponReady[_activeWeaponIndex]) return;

        if (_manaMaker.HasMana(_manaWeapons[_activeWeaponIndex]))
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(_inputManager.MousePosition), out RaycastHit hit, 100, _fireableLayers))
            {
                if (_playerMovementScript != null)
                    _playerMovementScript.StopAndLookAtTarget(hit.point);
                GameObject go = Instantiate(_weapons[_activeWeaponIndex], transform.position, _weapons[_activeWeaponIndex].transform.rotation);
                ISkillSlot iss = go.GetComponent<ISkillSlot>();
                WeaponOnShoot weaponOnShoot = go.GetComponent<WeaponOnShoot>();
                _animator.SetFloat(_animIdAttackType, iss.AttackAnimationType);
                _animator.SetTrigger(_animIdAttack);                
                if (weaponOnShoot)
                {
                    Vector3 playerPos = new Vector3(transform.position.x, transform.position.y + _shotOffsetY, transform.position.z);
                    Vector3 targetPos = new Vector3(hit.point.x, hit.point.y + _shotOffsetY, hit.point.z);
                    iss.Use(transform, playerPos, targetPos, _soundManager);
                }
                else
                {
                    iss.Use(transform, transform.position, hit.point, _soundManager);
                }                
                _manaMaker.GetMana(_manaWeapons[_activeWeaponIndex]);
                _currentCooldownSlotsTime[_activeWeaponIndex] = _cooldownSlotsTime[_activeWeaponIndex];
                _isWeaponReady[_activeWeaponIndex] = true;
            }
        }
    }

    private void CooldownWeapons()
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            if (_isWeaponReady[i] == false) continue;

            if (_currentCooldownSlotsTime[i] > 0)
            {
                _currentCooldownSlotsTime[i] -= Time.deltaTime;
                _cooldownSlotImages[i].fillAmount = Mathf.Lerp(0, 1f, _currentCooldownSlotsTime[i] / _cooldownSlotsTime[i]);
            }
            else
            {
                _cooldownSlotImages[i].fillAmount = 0f;
                _isWeaponReady[i] = false;
            }
        }
    }
}
