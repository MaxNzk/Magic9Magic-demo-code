using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TMPLink : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        TMP_Text pTextMeshPro = GetComponent<TMP_Text>();
        if (pTextMeshPro == null)
        {
            Tools.LogError("TMP_Text pTextMeshPro = NULL");
            return;
        }
        
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, eventData.position, Camera.main);
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];
            if (linkInfo.GetLinkID() == "CVD")
            {
                Application.OpenURL("https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/cvd-filter-post-processing-v2-127303");
            }
        }
    }

}
