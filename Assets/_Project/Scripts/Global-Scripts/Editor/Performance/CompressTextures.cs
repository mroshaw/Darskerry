using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DaftAppleGames.Editor.Performance
{
    public class CompressTexturesEditor : OdinEditorWindow
    {
        [MenuItem("Window/Performance/Compress Textures")]
        private static void OpenWindow()
        {
            GetWindow<CompressTexturesEditor>().Show();
        }
        
        [Header("Configuration")]
        [Tooltip("Choose a root GameObject from the hierarchy. If left blank, the whole scene will be used.")]
        public List<Texture> textures;

        [Header("Texture Compression")]
        public TextureImporterFormat textureFormat = TextureImporterFormat.DXT5;
        public TextureImporterFormat textureFormatNormal = TextureImporterFormat.DXT5;
        public TextureCompressionQuality textureQuality = TextureCompressionQuality.Normal;
        
        [Button("Configure")]
        [Tooltip("Run the editor configuration process.")]
        private void ConfigureClick()
        {
            CompressTextures();
        }

        [Button("Clear")]
        [Tooltip("Clear the texture list.")]
        private void ClearListClick()
        {
            textures.Clear();
        }
        
        private void CompressTextures()
        {
            foreach (var texture in textures)
            {
                Texture2D texture2d = (Texture2D)texture;
                SetTextureCompression(texture2d);
            }
        }

        
        /// <summary>
        /// Apply compression settings
        /// </summary>
        /// <param name="texture2d"></param>
        private void SetTextureCompression(Texture2D texture2d)
        {
            string texturePath = AssetDatabase.GetAssetPath(texture2d);
            TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(texturePath);
            TextureImporterPlatformSettings settings = importer.GetPlatformTextureSettings("Standalone");
            TextureImporterPlatformSettings newSettings = new TextureImporterPlatformSettings();
            newSettings.name = "Standalone";
            newSettings.overridden = true;
            switch (textureQuality)
            {
                case TextureCompressionQuality.Normal:
                    newSettings.compressionQuality = 50;
                    break;
                case TextureCompressionQuality.Best:
                    newSettings.compressionQuality = 100;
                    break;
                case TextureCompressionQuality.Fast:
                    newSettings.compressionQuality = 0;
                    break;
            }

            if (importer.textureType == TextureImporterType.NormalMap)
            {
                Debug.Log($"{texture2d.name} is Normal Map... Compressing from {settings.textureCompression} to {textureFormat}..");

                newSettings.format = textureFormat;
                importer.SetPlatformTextureSettings(newSettings);
                
            }
            if(importer.textureType == TextureImporterType.Default)
            {
                Debug.Log($"{texture2d.name} is Default... Compressing from {settings.textureCompression} to {textureFormatNormal}..");
                
                newSettings.format = textureFormatNormal;
                importer.SetPlatformTextureSettings(newSettings);
            }
        }
    }
}
