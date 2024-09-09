using Sirenix.OdinInspector;
using UnityEngine;

namespace DaftAppleGames.Darskerry.Core.TimeAndWeather
{



    [ExecuteInEditMode]
    public class NmWind : MonoBehaviour
    {

        [BoxGroup("General Parameters")] [Tooltip("Wind Speed in Kilometers per hour")] [SerializeField] private float windSpeed = 30;
        [BoxGroup("General Parameters")] [Range(0.0f, 2.0f)] [Tooltip("Wind Turbulence in percentage of wind Speed")] [SerializeField] private float turbulence = 0.25f;
        [BoxGroup("Noise Parameters")] [Tooltip("Texture used for wind turbulence")]  [SerializeField] private Texture2D noiseTexture;
        [BoxGroup("Noise Parameters")] [Tooltip("Size of one world tiling patch of the Noise Texture, for bending trees")] [SerializeField] private float flexNoiseWorldSize = 175.0f;
        [BoxGroup("Noise Parameters")] [Tooltip("Size of one world tiling patch of the Noise Texture, for leaf shivering")] [SerializeField] private float shiverNoiseWorldSize = 10.0f;
        [BoxGroup("Gust Parameters")] [Tooltip("Texture used for wind gusts")] [SerializeField] private Texture2D gustMaskTexture;
        [BoxGroup("Gust Parameters")] [Tooltip("Size of one world tiling patch of the Gust Texture, for leaf shivering")] [SerializeField] private float gustWorldSize = 600.0f;
        [BoxGroup("Gust Parameters")] [Tooltip("Wind Gust Speed in Kilometers per hour")] [SerializeField] private float gustSpeed = 50;
        [BoxGroup("Gust Parameters")] [Tooltip("Wind Gust Influence on trees")] [SerializeField] private float gustScale = 1.0f;
        [BoxGroup("Wind Spherical")] [Tooltip("Wind Gust Influence on trees")] [SerializeField] private WindZone point1;
        [BoxGroup("Wind Spherical")] public WindZone point2;
        [BoxGroup("Wind Spherical")] public WindZone point3;
        [BoxGroup("Wind Spherical")] public WindZone point4;

        private Vector4 _pos1 = new();
        private Vector4 _pos2 = new();
        private Vector4 _pos3 = new();
        private Vector4 _pos4 = new();
        private Vector4 _radius = new();

        private void Start()
        {
            ApplySettings();
        }

        // Update is called once per frame
        private void Update()
        {
            ApplySettings();
        }

        private void OnValidate()
        {
            ApplySettings();
        }

        private void ApplySettings()
        {
            Shader.SetGlobalTexture("WIND_SETTINGS_TexNoise", noiseTexture);
            Shader.SetGlobalTexture("WIND_SETTINGS_TexGust", gustMaskTexture);
            Shader.SetGlobalVector("WIND_SETTINGS_WorldDirectionAndSpeed", GetDirectionAndSpeed());
            Shader.SetGlobalFloat("WIND_SETTINGS_FlexNoiseScale", 1.0f / Mathf.Max(0.01f, flexNoiseWorldSize));
            Shader.SetGlobalFloat("WIND_SETTINGS_ShiverNoiseScale", 1.0f / Mathf.Max(0.01f, shiverNoiseWorldSize));
            Shader.SetGlobalFloat("WIND_SETTINGS_Turbulence", windSpeed * turbulence);
            Shader.SetGlobalFloat("WIND_SETTINGS_GustSpeed", gustSpeed);
            Shader.SetGlobalFloat("WIND_SETTINGS_GustScale", gustScale);
            Shader.SetGlobalFloat("WIND_SETTINGS_GustWorldScale", 1.0f / Mathf.Max(0.01f, gustWorldSize));

            if (point1 != null)
            {
                _pos1 = new Vector4(point1.transform.position.x, point1.transform.position.y,
                    point1.transform.position.z, point1.windMain * 0.2777f);
                _radius[0] = point1.radius;

            }
            else
            {
                _pos1 = new Vector4(0, 0, 0, 0);
                _radius[0] = 0.1f;
            }

            if (point2 != null)
            {
                _pos2 = new Vector4(point2.transform.position.x, point2.transform.position.y,
                    point2.transform.position.z, point2.windMain * 0.2777f);
                _radius[1] = point2.radius;

            }
            else
            {
                _pos2 = new Vector4(0, 0, 0, 0);
                _radius[1] = 0.1f;
            }

            if (point3 != null)
            {
                _pos3 = new Vector4(point3.transform.position.x, point3.transform.position.y,
                    point3.transform.position.z, point3.windMain * 0.2777f);
                _radius[2] = point3.radius;

            }
            else
            {
                _pos3 = new Vector4(0, 0, 0, 0);
                _radius[2] = 0.1f;
            }

            if (point4 != null)
            {
                _pos4 = new Vector4(point4.transform.position.x, point4.transform.position.y,
                    point4.transform.position.z, point4.windMain * 0.2777f);
                _radius[3] = point4.radius;

            }
            else
            {
                _pos4 = new Vector4(0, 0, 0, 0);
                _radius[3] = 0.1f;
            }
            Shader.SetGlobalMatrix("WIND_SETTINGS_Points", new Matrix4x4(_pos1, _pos2, _pos3, _pos4));
            Shader.SetGlobalVector("WIND_SETTINGS_Points_Radius", _radius);
        }

        private Vector4 GetDirectionAndSpeed()
        {
            Vector3 dir = transform.forward.normalized;
            return new Vector4(dir.x, dir.y, dir.z, windSpeed * 0.2777f);
        }
    }
}