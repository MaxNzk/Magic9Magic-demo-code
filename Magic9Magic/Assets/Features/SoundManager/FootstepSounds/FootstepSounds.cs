using System.Collections;
using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    private enum MoveType { Walk, Run, Jump };

    [SerializeField] private bool _isEnable;
    [SerializeField] private Transform _footPosition;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private bool _isBlendTerrainSounds;
    [SerializeField] private FootstepSoundsSO[] _footstepSounds;
    private ISoundManager _soundManager;
    private float _speedForFPC;

    // For debugging
    // private void Update() => Debug.DrawRay(_footPosition.position, Vector3.down, Color.red, 99);

    public void Initialize(ISoundManager soundManager)
    {
        _soundManager = soundManager;
    }

    // -----------------------------------------------------------------------------
    // --- For FirstPersonController -----------------------------------------------
    public void StartTakeStep(float speed)
    {
        if (_isEnable == false) return;

        _speedForFPC = speed;
        StartCoroutine("TakeStepCoroutine");
    }

    public void StopTakeStep()
    {
        if (_isEnable == false) return;

        StopCoroutine("TakeStepCoroutine");
    }

    private IEnumerator TakeStepCoroutine()
    {
        while(true)
        {
            TakeStep();
            float speedModifier = Random.Range(10, 12);
            _speedForFPC /= speedModifier;
            yield return new WaitForSeconds(_speedForFPC);
        }
    }

    public void StartTakeRun(float speed)
    {
        if (_isEnable == false) return;

        _speedForFPC = speed;
        StartCoroutine("TakeRunCoroutine");
    }

    public void StopTakeRun()
    {
        if (_isEnable == false) return;

        StopCoroutine("TakeRunCoroutine");
    }

    private IEnumerator TakeRunCoroutine()
    {
        while(true)
        {
            TakeRun();
            float speedModifier = Random.Range(16, 18);
            _speedForFPC /= speedModifier;
            yield return new WaitForSeconds(_speedForFPC);
        }
    }

    public void StartTakeJump()
    {
        if (_isEnable == false) return;
        
        TakeJump();
    }
    
    // -----------------------------------------------------------------------------
    // --- For Animation Events ----------------------------------------------------

    private void TakeStep()
    {
        // Debug.Log("----------------------------------");
        // Debug.Log("TakeStep()");
        if (Physics.Raycast(_footPosition.position, Vector3.down, out RaycastHit hit, 1f, _groundLayer))
        {
            if (hit.collider.TryGetComponent<Terrain>(out Terrain terrain))
            {
                PlayFootstepSoundFromTerrain(MoveType.Walk, terrain, hit.point);
            }
            else if (hit.collider.TryGetComponent<Renderer>(out Renderer renderer))
            {
                PlayFootstepSoundFromRenderer(MoveType.Walk, renderer);
            }
        }
    }

    private void TakeRun()
    {
        // Debug.Log("----------------------------------");
        // Debug.Log("TakeRun()");
        if (Physics.Raycast(_footPosition.position, Vector3.down, out RaycastHit hit, 1f, _groundLayer))
        {
            // Debug.Log("hit gameObject name = " + hit.collider.gameObject.name);
            if (hit.collider.TryGetComponent<Terrain>(out Terrain terrain))
            {
                PlayFootstepSoundFromTerrain(MoveType.Run, terrain, hit.point);
            }
            else if (hit.collider.TryGetComponent<Renderer>(out Renderer renderer))
            {
                PlayFootstepSoundFromRenderer(MoveType.Run, renderer);
            }
        }
    }

    private void TakeJump()
    {
        // Debug.Log("----------------------------------");
        // Debug.Log("TakeJump()");
        if (Physics.Raycast(_footPosition.position, Vector3.down, out RaycastHit hit, 1f, _groundLayer))
        {
            if (hit.collider.TryGetComponent<Terrain>(out Terrain terrain))
            {
                PlayFootstepSoundFromTerrain(MoveType.Jump, terrain, hit.point);
            }
            else if (hit.collider.TryGetComponent<Renderer>(out Renderer renderer))
            {
                PlayFootstepSoundFromRenderer(MoveType.Jump, renderer);
            }
        }
    }

    private void PlayFootstepSoundFromTerrain(MoveType moveType, Terrain terrain, Vector3 hitPoint)
    {
        Vector3 terrainPosition = hitPoint - terrain.transform.position;
        Vector3 splatMapPosition = new Vector3(terrainPosition.x / terrain.terrainData.size.x, 0, terrainPosition.z / terrain.terrainData.size.z);
        int x = Mathf.FloorToInt(splatMapPosition.x * terrain.terrainData.alphamapWidth);
        int z = Mathf.FloorToInt(splatMapPosition.z * terrain.terrainData.alphamapHeight);
        float[,,] alphaMap = terrain.terrainData.GetAlphamaps(x, z, 1, 1);

        if (_isBlendTerrainSounds)
        {
            // Debug.Log("_isBlendTerrainSounds == true");
            for (int i = 0; i < alphaMap.Length; i++)
                if (alphaMap[0, 0 , i] > 0)
                    foreach (FootstepSoundsSO footstepSound in _footstepSounds)
                        if (footstepSound.Albedo == terrain.terrainData.terrainLayers[i].diffuseTexture)
                        {
                            // Debug.Log("Terrain " + moveType.ToString() + " Albedo name = " + footstepSound.name);
                            if (moveType == MoveType.Walk) _soundManager.Play(footstepSound.WalkSoundName.ToString());
                            if (moveType == MoveType.Run) _soundManager.Play(footstepSound.RunSoundName.ToString());
                            if (moveType == MoveType.Jump) _soundManager.Play(footstepSound.JumpSoundName.ToString());
                            break;
                        }
        }
        else
        {
            // Debug.Log("_isBlendTerrainSounds == false");
            int primaryIndex = 0;
            for (int i = 1; i < alphaMap.Length; i++)
                if (alphaMap[0, 0, i] > alphaMap[0, 0, primaryIndex])
                    primaryIndex = i;
            foreach (FootstepSoundsSO footstepSound in _footstepSounds)
                if (footstepSound.Albedo == terrain.terrainData.terrainLayers[primaryIndex].diffuseTexture)
                {
                    // Debug.Log("Terrain " + moveType.ToString() + " Albedo name = " + footstepSound.name);
                    if (moveType == MoveType.Walk) _soundManager.Play(footstepSound.WalkSoundName.ToString());
                    if (moveType == MoveType.Run) _soundManager.Play(footstepSound.RunSoundName.ToString());
                    if (moveType == MoveType.Jump) _soundManager.Play(footstepSound.JumpSoundName.ToString());
                    break;
                }
        }
    }

    private void PlayFootstepSoundFromRenderer(MoveType moveType, Renderer renderer)
    {
        bool isFound = false;
        foreach (FootstepSoundsSO footstepSound in _footstepSounds)
        {
            if (footstepSound.Albedo != null && footstepSound.Albedo == renderer.material.GetTexture("_MainTex"))
            {
                // Debug.Log("Renderer Walk Albedo name = " + footstepSound.name);
                isFound = true;
                if (moveType == MoveType.Walk) _soundManager.Play(footstepSound.WalkSoundName.ToString());
                if (moveType == MoveType.Run) _soundManager.Play(footstepSound.RunSoundName.ToString());
                if (moveType == MoveType.Jump) _soundManager.Play(footstepSound.JumpSoundName.ToString());
            }
            if (footstepSound.Color.IsApproximatelyEqualTo(renderer.material.color, 1))
            {
                // Debug.Log("Renderer Walk Color name = " + footstepSound.name + " | " + footstepSound.Color.ToString());
                isFound = true;
                if (moveType == MoveType.Walk) _soundManager.Play(footstepSound.WalkSoundName.ToString());
                if (moveType == MoveType.Run) _soundManager.Play(footstepSound.RunSoundName.ToString());
                if (moveType == MoveType.Jump) _soundManager.Play(footstepSound.JumpSoundName.ToString());
            }
        }

        if (isFound == false)
        {
            Tools.LogError("Renderer isFound == false");
            if (renderer.material.GetTexture("_MainTex") != null)
                Tools.LogError("texture = " + renderer.material.GetTexture("_MainTex").ToString());
            Tools.LogError("renderer.color = " + renderer.material.color.ToString());
        }
    }
}
