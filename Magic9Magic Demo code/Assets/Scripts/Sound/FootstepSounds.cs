using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    [System.Serializable]
    private struct TextureSound
    {
        public Texture Albedo;
        public SoundManager.SoundNames SoundName;
    }

    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private bool _isBlendTerrainSounds;
    [SerializeField] private TextureSound[] _textureWalkSounds;
    [SerializeField] private TextureSound[] _textureRunSounds;
    [SerializeField] private TextureSound[] _textureJumpSounds;
    private SoundManager _soundManager;

    public void Initialize(SoundManager soundManager)
    {
        _soundManager = soundManager;
    }

    private void TakeStep()
    {
        // Debug.Log("----------------------------------");
        // Debug.Log("TakeStep()");
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f, _groundLayer))
        {
            if (hit.collider.TryGetComponent<Terrain>(out Terrain terrain))
            {
                // Debug.Log("hit Terrain");
                PlayFootstepSoundFromTerrain(_textureWalkSounds, terrain, hit.point);
            }
            else if (hit.collider.TryGetComponent<Renderer>(out Renderer renderer))
            {
                // Debug.Log("hit Renderer");
                PlayFootstepSoundFromRenderer(_textureWalkSounds, renderer);
            }
        }
    }

    private void TakeRun()
    {
        // Debug.Log("----------------------------------");
        // Debug.Log("TakeRun()");
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f, _groundLayer))
        {
            if (hit.collider.TryGetComponent<Terrain>(out Terrain terrain))
            {
                // Debug.Log("hit Terrain");
                PlayFootstepSoundFromTerrain(_textureRunSounds, terrain, hit.point);
            }
            else if (hit.collider.TryGetComponent<Renderer>(out Renderer renderer))
            {
                // Debug.Log("hit Renderer");
                PlayFootstepSoundFromRenderer(_textureRunSounds, renderer);
            }
        }
    }

    private void TakeJump()
    {
        // Debug.Log("----------------------------------");
        // Debug.Log("TakeJump()");
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f, _groundLayer))
        {
            if (hit.collider.TryGetComponent<Terrain>(out Terrain terrain))
            {
                // Debug.Log("hit Terrain");
                PlayFootstepSoundFromTerrain(_textureJumpSounds, terrain, hit.point);
            }
            else if (hit.collider.TryGetComponent<Renderer>(out Renderer renderer))
            {
                // Debug.Log("hit Renderer");
                PlayFootstepSoundFromRenderer(_textureJumpSounds, renderer);
            }
        }
    }

    private void PlayFootstepSoundFromTerrain(TextureSound[] textureSounds, Terrain terrain, Vector3 hitPoint)
    {
        Vector3 terrainPosition = hitPoint - terrain.transform.position;
        Vector3 splatMapPosition = new Vector3(
            terrainPosition.x / terrain.terrainData.size.x,
            0,
            terrainPosition.z / terrain.terrainData.size.z
        );
        int x = Mathf.FloorToInt(splatMapPosition.x * terrain.terrainData.alphamapWidth);
        int z = Mathf.FloorToInt(splatMapPosition.z * terrain.terrainData.alphamapHeight);
        float[,,] alphaMap = terrain.terrainData.GetAlphamaps(x, z, 1, 1);

        if (_isBlendTerrainSounds)
        {
            // Debug.Log("_isBlendTerrainSounds == true");
            for (int i = 0; i < alphaMap.Length; i++)
                if (alphaMap[0, 0 , i] > 0)
                    foreach (TextureSound textureSound in textureSounds)
                        if (textureSound.Albedo == terrain.terrainData.terrainLayers[i].diffuseTexture)
                        {
                            // Debug.Log(textureSound.SoundName.ToString());
                            _soundManager.Play(textureSound.SoundName.ToString());
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
            foreach (TextureSound textureSound in textureSounds)
                if (textureSound.Albedo == terrain.terrainData.terrainLayers[primaryIndex].diffuseTexture)
                {
                    // Debug.Log(textureSound.SoundName.ToString());
                    _soundManager.Play(textureSound.SoundName.ToString());
                }
        }

    }

    private void PlayFootstepSoundFromRenderer(TextureSound[] textureSounds, Renderer renderer)
    {
        foreach (TextureSound textureSound in textureSounds)
        {
            if (textureSound.Albedo == renderer.material.GetTexture("_MainTex"))
            {
                // Debug.Log(textureSound.SoundName.ToString());
                _soundManager.Play(textureSound.SoundName.ToString());
            }
        }
    }

}
