using System.Collections.Generic;
using UnityEngine;

    public class CVDProfilesSO : ScriptableObject
    {
        [field: SerializeField] public List<VisionTypeInfo> VisionTypes { get; private set; }
    }