using UnityEngine;
using TMPro;

public class VersionDisplay : MonoBehaviour
{

    private void Awake()
    {
        TextMeshProUGUI version = Tools.GetComponentWithAssertion<TextMeshProUGUI>(gameObject);
        version.text = "Version " + Application.version;
    }

}
