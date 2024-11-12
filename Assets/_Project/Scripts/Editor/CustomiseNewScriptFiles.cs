using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;

namespace DaftAppleGames.Darskerry.Editor
{
    public class CustomiseNewScriptFiles : AssetModificationProcessor
    {
        private const string RootNamespace = "DaftAppleGames.Darskerry.Core";

        private static string _fullNamespace = "";
        private static string _scriptFriendlyName = "";
        private static string _parentNamespace = "";
        private static string _editorClass = "";
        private static string _editorWindow = "";

        /// <summary>
        /// Manage the creation of new script files
        /// </summary>
        /// <param name="path"></param>
        public static void OnWillCreateAsset(string path)
        {
            // Check to see that the file exists first
            if (!path.EndsWith(".cs.meta"))
            {
                return;
            }

            string originalFilePath = AssetDatabase.GetAssetPathFromTextMetaFilePath(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            fileName = fileName.Substring(0, fileName.IndexOf(@".", StringComparison.Ordinal));
            string fileContents = File.ReadAllText(originalFilePath);

            // Derive custom properties
            DeriveCustomProperties(path, fileName);

            // Replace placeholders with custom properties
            fileContents = UpdateFile(fileContents);

            // Write the contents back to the file
            File.WriteAllText(originalFilePath, fileContents);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Derive properties that we need for further customisations
        /// </summary>
        private static void DeriveCustomProperties(string path, string fileName)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            if (di.Parent != null) _parentNamespace = di.Parent.Name;
            _fullNamespace = path.Substring(path.IndexOf("Assets", StringComparison.Ordinal)).Substring(0, path.Substring(path.IndexOf("Assets", StringComparison.Ordinal)).LastIndexOf('/')).Replace('/', '.');
            _fullNamespace = _fullNamespace.Replace("Assets._Project.Scripts.Core", RootNamespace);
            _scriptFriendlyName = Regex.Replace(fileName, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");

            int editorIndex = fileName.IndexOf("Editor", StringComparison.Ordinal);
            if (editorIndex != -1)
            {
                _editorClass = fileName.Substring(0, editorIndex);
            }

            int windowIndex = fileName.IndexOf("Window", StringComparison.Ordinal);
            if (windowIndex != -1)
            {
                _editorWindow = fileName.Substring(0, windowIndex);
                _editorWindow = Regex.Replace(_editorWindow, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
            }
        }

        /// <summary>
        /// Updates the root namespace and namespace path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        private static string UpdateFile(string file)
        {
            // Derive and replace the correct Namespace for the file and path
            string newFile = file;
            newFile = newFile.Replace("#NAMESPACE#", _fullNamespace);
            newFile = newFile.Replace("#SCRIPTFRIENDLYNAME#", _scriptFriendlyName);
            newFile = newFile.Replace("#PARENTNAMESPACE#", _parentNamespace);
            newFile = newFile.Replace("#EDITORCLASS#", _editorClass);
            newFile = newFile.Replace("%EDITORCLASSLOWER%", _editorClass.ToLower());
            newFile = newFile.Replace("%EDITORWINDOW%", _editorWindow);
            return newFile;
        }
    }
}