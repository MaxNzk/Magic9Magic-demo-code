using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMarket : MonoBehaviour
{
    private UIManagerCamp _uiManagerCamp;
    private UIManagerPlayer _uiManagerPlayer;

    public void Initialize(UIManagerCamp uiManagerCamp, UIManagerPlayer uiManagerPlayer)
    {
        _uiManagerCamp = uiManagerCamp;
        _uiManagerPlayer = uiManagerPlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _uiManagerCamp.ShowMarket();
            _uiManagerPlayer.ShowInventory();
        }
    }

}
