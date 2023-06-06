using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : MonoBehaviour, IAddressableManager
{
    [SerializeField] private ReferenceListSO[] _referenceLists;
    private Dictionary<string, AssetReference> ReferenceDictionary = new Dictionary<string, AssetReference>();
    private AsyncOperationHandle<GameObject> AOhandle;
    private bool IsInitialized;

    private void Awake()
    {
        if (_referenceLists.Length == 0)
        {
            Tools.LogError("AddressableManager: _referenceLists is Empty");
        }
        for (int i = 0; i < _referenceLists.Length; i++)
        {
            Tools.CheckNull<ReferenceListSO>(_referenceLists[i], nameof(_referenceLists), gameObject);            
        }
    }

    public GameObject Get(string name)
    {
        Initialize();
        AOhandle = Addressables.LoadAssetAsync<GameObject>(ReferenceDictionary[name]);
        return AOhandle.WaitForCompletion();
    }

    private void Initialize()
    {
        if (IsInitialized)
            return;
        
        // Debug.Log("ReferenceDictionary -----------------------------");
        for (int i = 0; i < _referenceLists.Length; i++)
        {
            for (int k = 0; k < _referenceLists[i].ObjectList.Length; k++)
            {
                ReferenceDictionary.Add(_referenceLists[i].ObjectList[k].ObjectName.ReferenceName, _referenceLists[i].ObjectList[k].AssetRef);
                // Debug.Log(_referenceLists[i].ObjectList[k].ObjectName.ReferenceName);
            }
        }
        // Debug.Log("-------------------------------------------------");

        IsInitialized = true;
    }

    private void OnDestroy()
    {
        Addressables.Release(AOhandle);
    }

}
