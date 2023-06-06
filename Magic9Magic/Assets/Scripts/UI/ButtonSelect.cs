using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().Select();
    }
}
