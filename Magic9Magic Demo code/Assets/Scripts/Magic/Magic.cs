using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    [Header("0 = Fire, 1 = Water, 2 = Earth")]
    [Header("3 = Wind, 4 = Metal, 5 = Lightning")]
    [Header("6 = Life, 7 = Death, 8 = Space")]
    [SerializeField] private Color[] _magicColors;
    public Color[] MagicColors { get => _magicColors; }

}
