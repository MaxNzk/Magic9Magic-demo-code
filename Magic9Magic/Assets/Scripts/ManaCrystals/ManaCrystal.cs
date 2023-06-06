using UnityEngine;

public class ManaCrystal : MonoBehaviour
{
    [SerializeField] private int _manaAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ManaMaker manaMaker = other.gameObject.GetComponentInParent<ManaMaker>();
            if (manaMaker.AddMana(_manaAmount))
            {
                Destroy(gameObject);
            }
        }
    }
}
