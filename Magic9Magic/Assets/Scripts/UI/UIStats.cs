using UnityEngine;
using TMPro;

public class UIStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _itemStatsTexts;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        for (int i = 0; i < _itemStatsTexts.Length; i++)
        {
            Tools.CheckNull<TextMeshProUGUI>(_itemStatsTexts[i], nameof(_itemStatsTexts), gameObject);
        }
    }

    public void UpdateItemStatsTexts(int[] attackValues, int[] defenseValues)
    {
        for (int i = 0; i < _itemStatsTexts.Length; i++)
        {
            _itemStatsTexts[i].text = attackValues[i].ToString() + " / " + defenseValues[i].ToString();
        }
    }

}
