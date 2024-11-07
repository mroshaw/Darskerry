using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.PlayerController.FootSteps
{
    public class FootstepTrigger : CharacterTrigger
    {
        #region Class Variables
        private AudioSource _audioSource;
        private float _cooldownCounter = 0.0f;
        public FootstepManager FootstepManager { get; set; }
        private TerrainData _terrainData;
        private bool _terrainDetected;

        private FootstepSurface _defaultSurface;
        private FootstepSurface[] _footstepSurfaces;

        #endregion

        #region Startup
        private void Awake()
        {
            if (!FootstepManager || !FootstepManager.FootstepsEnabled)
            {
                GetComponent<SphereCollider>().enabled = false;
                return;
            }

            _audioSource = GetComponent<AudioSource>();

            if (!_audioSource)
            {
                Debug.LogError($"FootstepTrigger: no AudioSource on this gameobject! {gameObject}");
            }

            _terrainDetected = !(Terrain.activeTerrain == null);

            if (_terrainDetected)
            {
                _terrainData = Terrain.activeTerrain.terrainData;
            }

            _defaultSurface = FootstepManager.GetDefaultSurface();
            _footstepSurfaces = FootstepManager.GetAllSurfaces();

        }
        #endregion

        #region Class methods
        public override void TriggerEnter(Collider other)
        {
            if (_cooldownCounter > 0.0f || !FootstepManager.decalPool || !FootstepManager.particleFxPool)
            {
                return;
            }

            GetSurfaceFromCollision(transform, other, out FootstepSurface footstepSurface,
                out Vector3 spawnPosition);

            // Spawn particles
            if (footstepSurface.spawnParticle)
            {
                SpawnFootStepParticleFx(spawnPosition, FootstepManager.transform.rotation);
            }

            // Spawn decal
            if (footstepSurface.spawnDecal)
            {
                SpawnFootStepDecal(spawnPosition, FootstepManager.transform.rotation);
            }

            // Play random audio
            System.Random randomAudio = new System.Random();
            int audioIndex = randomAudio.Next(0, footstepSurface.audioClips.Length);
            AudioClip audioClip = footstepSurface.audioClips[audioIndex];
            _audioSource.Stop();
            _audioSource.PlayOneShot(audioClip);

            _cooldownCounter = 0.5f;

        }

        private void Update()
        {
            _cooldownCounter -= Time.deltaTime;
        }

        public override void TriggerExit(Collider other)
        {
        }
        #endregion

        #region Class methods
        public void SpawnFootStepParticleFx(Vector3 spawnPosition, Quaternion spawnRotation)
        {
            GameObject particleFxInstance = FootstepManager.particleFxPool.SpawnInstance(spawnPosition, spawnRotation);
        }

        public void SpawnFootStepDecal(Vector3 spawnPosition, Quaternion spawnRotation)
        {
            GameObject decalInstance = FootstepManager.decalPool.SpawnInstance(spawnPosition, spawnRotation);
        }

        public void GetSurfaceFromCollision(Transform footTransform, Collider otherCollider,
            out FootstepSurface footstepSurface, out Vector3 spawnPosition)
        {

            if (otherCollider is TerrainCollider)
            {
                Vector3 collisionPosition = footTransform.position;
                if (!FindTerrainTextureAtPosition(footTransform.position, out var terrainTextureName))
                {
                    footstepSurface = _defaultSurface;
                    spawnPosition = collisionPosition;
                }
                float terrainHeight = Terrain.activeTerrain.SampleHeight(collisionPosition);
                spawnPosition = new Vector3(collisionPosition.x, terrainHeight, collisionPosition.z);
                footstepSurface = FindSurfaceFromTexture(terrainTextureName);
                return;

            }
            spawnPosition = otherCollider is MeshCollider { convex: true } or BoxCollider or SphereCollider or CapsuleCollider ? otherCollider.ClosestPoint(footTransform.position) : footTransform.position;

            if (FindMaterialTextureFromCollider(otherCollider, out var meshTextureName))
            {
                footstepSurface = FindSurfaceFromTexture(meshTextureName);
                return;
            }
            footstepSurface = _defaultSurface;
            spawnPosition = footTransform.position;
        }

        private FootstepSurface FindSurfaceFromTexture(string textureName)
        {
            foreach (FootstepSurface currSurface in _footstepSurfaces)
            {
                if (currSurface.ContainsTextureName(textureName) && currSurface.audioClips.Length > 0)
                {
                    return currSurface;
                }
            }
            return _defaultSurface;
        }

        private bool FindMaterialTextureFromCollider(Collider other, out string textureName)
        {
            textureName = "";

            MeshRenderer meshRender = other.GetComponent<MeshRenderer>();
            if (!meshRender)
            {
                return false;
            }
            Material meshMaterial = meshRender.material;
            if (!meshMaterial || !meshMaterial.mainTexture)
            {
                return false;
            }
            textureName = meshMaterial.mainTexture.name;
            if (FootstepManager.DebugTextureName)
            {
                Debug.Log($"FootstepManager: Mesh texture is : {textureName}");
            }
            return true;
        }

        private bool FindTerrainTextureAtPosition(Vector3 collisionPosition, out string textureName)
        {
            textureName = "";

            Vector3 terrainSize = Terrain.activeTerrain.terrainData.size;
            Vector2 textureSize = new Vector2(Terrain.activeTerrain.terrainData.alphamapWidth,
                Terrain.activeTerrain.terrainData.alphamapHeight);

            int alphaX = (int)((collisionPosition.x / terrainSize.x) * textureSize.x + 0.5f);
            int alphaY = (int)((collisionPosition.z / terrainSize.z) * textureSize.y + 0.5f);

            float[,,] terrainMaps = Terrain.activeTerrain.terrainData.GetAlphamaps(alphaX, alphaY, 1, 1);

            float[] textures = new float[terrainMaps.GetUpperBound(2) + 1];

            for (int n = 0; n < textures.Length; n++)
            {
                textures[n] = terrainMaps[0, 0, n];
            }

            if (textures.Length == 0)
            {
                return false;
            }

            // Looking for the texture with the highest 'mix'
            float textureMaxMix = 0;
            int textureMaxIndex = 0;

            for (int currTexture = 0; currTexture < textures.Length; currTexture++)
            {
                if (textures[currTexture] > textureMaxMix)
                {
                    textureMaxIndex = currTexture;
                    textureMaxMix = textures[currTexture];
                }
            }

            // Texture is at index textureMaxIndex
            textureName = (_terrainData != null && _terrainData.terrainLayers.Length > 0 && _terrainData.terrainLayers[textureMaxIndex].diffuseTexture) ? (_terrainData.terrainLayers[textureMaxIndex]).diffuseTexture.name : "";

            if (FootstepManager.DebugTextureName)
            {
                Debug.Log($"FootstepManager: Terrain texture is : {textureName}");
            }
            return true;
        }
        #endregion
    }
}