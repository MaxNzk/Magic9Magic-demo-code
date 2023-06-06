using UnityEngine;

[CreateAssetMenu(fileName = "DefaultSOList", menuName = "Scriptable Objects/DefaultSOList")]
public class DefaultSOListSO : ScriptableObject
{
    public CurrentAndDefaultSO[] CurrentAndDefaultSOList;

    [System.Serializable]
    public class CurrentAndDefaultSO
    {
        public Object CurrentSO;
        public Object DefaultSO;
    }
}
