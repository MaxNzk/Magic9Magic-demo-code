using UnityEngine;

public class PortalPopup : MonoBehaviour
{
    public Portal ActivePortal { get; set; }

    public void ChooseDistance(int variant)
    {
        ActivePortal.ChooseDistance(variant);
    }

}
