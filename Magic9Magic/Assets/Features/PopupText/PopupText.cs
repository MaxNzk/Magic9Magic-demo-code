using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _showTime;
    private float _elapsedTime;
    [SerializeField] private float _offsetX;
    [SerializeField] private Color _criticalDamageColor;
    [SerializeField] private Color[] _colors;
    private PopupTextPool _poolManager;
    private TextMeshProUGUI _text;
    private Transform _transform;

    public void Initialize(PopupTextPool poolManager)
    {
        _poolManager = poolManager;
    }

    public void SetParams(Vector3 position, int damage, int maxHealth, bool isCriticalDamage)
    {
        SetPosition(position);
        SetColor(damage, maxHealth, isCriticalDamage);
        SetText(damage);
        _elapsedTime = 0;
    }

    private void SetPosition(Vector3 position)
    {
        _transform.position = new Vector3(position.x + Random.Range(-_offsetX, _offsetX), position.y, position.z);
    }

    private void SetColor(int damage, int maxHealth, bool isCriticalDamage)
    {
        if (isCriticalDamage)
        {
            _text.color = _criticalDamageColor;
        }
        else
        {
            int index = damage * 10 / maxHealth;
            index = Mathf.Clamp(index, 0, 9);
            _text.color = _colors[index];
        }
    }

    private void SetText(int damage)
    {
        _text.text = damage.ToString();
    }

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _transform = GetComponent<Transform>();
        if (_text == null)
            Tools.LogError("TextMeshProUGUI _text = NULL");
    }

    private void Update()
    {
        if (_elapsedTime < _showTime)
        {
            _elapsedTime += Time.deltaTime;
            _transform.position += _speed * Time.deltaTime * Vector3.up;
        }
        else
        {
            _poolManager.ReturnObject(gameObject.name, gameObject);
        }
    }

}
