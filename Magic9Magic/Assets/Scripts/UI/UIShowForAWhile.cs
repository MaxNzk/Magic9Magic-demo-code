using System.Collections;
using UnityEngine;

public class UIShowForAWhile : MonoBehaviour
{
    [SerializeField] private float _displayTime = 3.0f;

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(_displayTime);
        gameObject.SetActive(false);
    }

}
