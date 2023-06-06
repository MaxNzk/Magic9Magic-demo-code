using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class FPCPlayerWeapons : MonoBehaviour
{
    [SerializeField] private bool _isAnimator;
    private Animator _animator;
    private readonly int _animIdAttack = Animator.StringToHash("attack");
    private readonly int _animIdAttackType = Animator.StringToHash("attackType");

    [Space(10)]
    [SerializeField] private Transform _handsPosTransform;
    
    private ISoundManager _soundManager;
    [SerializeField] private SoundManager.SoundNamesUI soundNameSetActiveSlot;

    [Space(10)]
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
    [SerializeField] private int _altWeaponIndex = 1;
    private int _activeWeaponIndex;
    private PlayerTakeDamage _playerTakeDamageScript;

    private Camera _camera;
    private StarterAssets.StarterAssetsInputs _inputManager;

    private CharacterStats _characterStats;
    private ManaMaker _manaMaker;
    private MagicPoolManager _magicPoolManager;

    public void Initialize(MagicPoolManager magicPoolManager, ISoundManager soundManager, StarterAssets.StarterAssetsInputs inputManager)
    {
        _magicPoolManager = magicPoolManager;
        _soundManager = soundManager;
        _inputManager = inputManager;

        FindAndTestComponents();
        InitializeWeapons();
    }

    private void FindAndTestComponents()
    {
        _camera = Camera.main;
        _playerTakeDamageScript = Tools.GetComponentWithAssertion<PlayerTakeDamage>(gameObject);
        _characterStats = Tools.GetComponentInChildrenWithAssertion<CharacterStats>(gameObject);
        _manaMaker = Tools.GetComponentWithAssertion<ManaMaker>(gameObject);

        if (_isAnimator)
            _animator = Tools.GetComponentInChildrenWithAssertion<Animator>(gameObject);
        
        _manaTexts = new TextMeshProUGUI[_manaContainers.Length];
        for (int i = 0; i < _manaContainers.Length; i++)
        {
            _manaTexts[i] = _manaContainers[i].GetComponentInChildren<TextMeshProUGUI>();
        }

        Tools.CheckNull<Transform>(_handsPosTransform, nameof(_handsPosTransform), gameObject);

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
        if (_playerTakeDamageScript.IsDead())
            return;
        
        if (_inputManager.CanActivateActiveSlot) ActivateActiveSlot(_activeWeaponIndex);
        if (_inputManager.CanActivateAltSlot) ActivateActiveSlot(_altWeaponIndex);

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
        if (_activeWeaponIndex == index)
            return;
        
        _soundManager.Play(soundNameSetActiveSlot.ToString());
        _activeWeaponIndex = index;
        SetIconFocus();
    }

    private void SetIconFocus()
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            _focusSlotImages[i].SetActive(_activeWeaponIndex == i);
            if (_activeWeaponIndex == i)
            {
                _frameImage[i].color = new Color(255, 0, 0, 255);
            }
            else
            {
                _frameImage[i].color = new Color(255, 255, 255, 255);
            }
        }
    }

    private void ActivateActiveSlot(int slotIndex)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (_isWeaponReady[slotIndex])
            return;

        if (_manaMaker.HasMana(_manaWeapons[slotIndex]))
        {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, Mathf.Infinity))
            {
                string nameInPool = "Magic " + _weapons[slotIndex].name;
                GameObject go = _magicPoolManager.GetObject(nameInPool, transform.position);
                ISkillSlot iss = go.GetComponent<ISkillSlot>();

                if (_isAnimator)
                {
                    _animator.SetFloat(_animIdAttackType, iss.AttackAnimationType);
                    _animator.SetTrigger(_animIdAttack);
                }

                WeaponOnShoot weaponOnShoot = go.GetComponent<WeaponOnShoot>();
                if (weaponOnShoot)
                {
                    iss.Use(_characterStats, _camera.transform, _handsPosTransform.position, hit.point);
                }
                else
                {
                    iss.Use(_characterStats, transform, transform.position, hit.point);
                }

                _manaMaker.GetMana(_manaWeapons[slotIndex]);
                _currentCooldownSlotsTime[slotIndex] = _cooldownSlotsTime[slotIndex];
                _isWeaponReady[slotIndex] = true;
            }
        }
    }

    private void CooldownWeapons()
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            if (_isWeaponReady[i] == false)
                continue;

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
