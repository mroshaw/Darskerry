using UnityEngine;

namespace DaftAppleGames.Editor.Common.Terrains
{
    public class TerrainTools
    {
        /// <summary>
        /// Aligns all GameObjects to terrain
        /// </summary>
        /// <param name="targetGameObjects"></param>
        /// <param name="alignToSlope"></param>
        /// <param name="freezeX"></param>
        /// <param name="freezeY"></param>
        /// <param name="freezeZ"></param>
        public static void AlignObjectsToTerrain(GameObject[] targetGameObjects, bool alignToSlope,
            bool freezeX, bool freezeY, bool freezeZ)
        {
            foreach (GameObject targetGameObject in targetGameObjects)
            {
                AlignObjectToTerrain(targetGameObject, alignToSlope, freezeX, freezeY, freezeZ);
            }
        }

        /// <summary>
        /// Aligns given GameObject to terrain
        /// </summary>
        /// <param name="targetGameObject"></param>
        /// <param name="alignToSlope"></param>
        /// <param name="freezeX"></param>
        /// <param name="freezeY"></param>
        /// <param name="freezeZ"></param>
        public static void AlignObjectToTerrain(GameObject targetGameObject, bool alignToSlope,
            bool freezeX, bool freezeY, bool freezeZ)
        {
            float terrainHeight = GetTerrainHeightAtTransform(targetGameObject.transform);
            Quaternion newRotation = GetTerrainSlopeAtTransform(targetGameObject.transform);
            AlignObjectToTerrain(targetGameObject, terrainHeight, newRotation, freezeX, freezeY, freezeZ);
        }

        /// <summary>
        /// Sets the height of the given GameObject
        /// </summary>
        /// <param name="targetGameObject"></param>
        /// <param name="height"></param>
        /// <param name="slopeAngle"></param>
        /// <param name="freezeX"></param>
        /// <param name="freezeY"></param>
        /// <param name="freezeZ"></param>
        private static void AlignObjectToTerrain(GameObject targetGameObject, float height, Quaternion slopeAngle,
            bool freezeX, bool freezeY, bool freezeZ)
        {
            Vector3 newPosition = targetGameObject.transform.position;
            newPosition.y = height;
            Debug.Log($"Setting height of: {targetGameObject.name} to: {height}");
            targetGameObject.transform.position = newPosition;
            Debug.Log($"Setting rotation of: {targetGameObject.name} to: {slopeAngle}");

            if (freezeX)
            {
                slopeAngle.x = targetGameObject.transform.rotation.x;
            }

            if (freezeY)
            {
                slopeAngle.y = targetGameObject.transform.rotation.y;
            }

            if (freezeZ)
            {
                slopeAngle.z = targetGameObject.transform.rotation.z;
            }
            targetGameObject.transform.rotation = slopeAngle;
        }

        /// <summary>
        /// Gets the angle of the slope beneath the transform
        /// </summary>
        /// <param name="targetTransform"></param>
        /// <returns></returns>
        private static Quaternion GetTerrainSlopeAtTransform(Transform targetTransform)
        {
            RaycastHit[] hits = Physics.RaycastAll( targetTransform.position + (Vector3.up * 2.0f) , Vector3.down , maxDistance:10f );
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform != targetTransform)
                {
                    Debug.Log("AlignToTerrain: Hit!");
                    return Quaternion.LookRotation( Vector3.ProjectOnPlane(targetTransform.forward,hit.normal).normalized , hit.normal );
                }
            }

            return targetTransform.rotation;
        }
        
        /// <summary>
        /// Gets the height of the terrain at the given transform
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        private static float GetTerrainHeightAtTransform(Transform targetTransform)
        {
            return Terrain.activeTerrain.SampleHeight(targetTransform.position);
        }
    }
}
