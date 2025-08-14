using UnityEngine;
using UnityEditor;
using System.IO;

namespace TreasureDragon.Tools
{
    [InitializeOnLoad]
    public class DragonFolderIcons
    {
        private static readonly string[] CombatFolders = { "Combat", "Military", "Towers", "Personnel", "Weapons" };
        private static readonly string[] ArchitectureFolders = { "Architecture", "Buildings", "Civilian", "People", "Civilians" };
        private static readonly string[] DragonFolders = { "Dragon", "Dragons", "Player" };
        private static readonly string[] SceneFolders = { "Scenes", "Environments", "Masters", "Testing" };
        private static readonly string[] CoreFolders = { "Scripts", "Core", "Editor", "Tools" };
        private static readonly string[] AssetFolders = { "Models", "Materials", "Textures", "Audio", "Prefabs" };

        static DragonFolderIcons()
        {
            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
        }

        private static void ProjectWindowItemOnGUI(string guid, Rect selectionRect)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            
            if (!AssetDatabase.IsValidFolder(assetPath))
                return;

            string folderName = Path.GetFileName(assetPath);
            
            // Get the icon color based on folder type
            Color iconColor = GetFolderColor(folderName);
            
            if (iconColor != Color.clear)
            {
                // Draw colored background
                Rect iconRect = new Rect(selectionRect.x, selectionRect.y, 16, 16);
                EditorGUI.DrawRect(iconRect, iconColor * 0.3f);
                
                // Draw colored border
                GUI.color = iconColor;
                GUI.DrawTexture(iconRect, EditorGUIUtility.IconContent("Folder Icon").image);
                GUI.color = Color.white;
            }
        }

        private static Color GetFolderColor(string folderName)
        {
            // Combat folders - Red/Orange theme
            foreach (string folder in CombatFolders)
            {
                if (folderName.Contains(folder))
                    return new Color(1f, 0.3f, 0.2f, 1f); // Red-Orange
            }

            // Architecture folders - Blue/Green theme
            foreach (string folder in ArchitectureFolders)
            {
                if (folderName.Contains(folder))
                    return new Color(0.2f, 0.6f, 1f, 1f); // Blue
            }

            // Dragon folders - Purple/Gold theme
            foreach (string folder in DragonFolders)
            {
                if (folderName.Contains(folder))
                    return new Color(0.8f, 0.4f, 1f, 1f); // Purple
            }

            // Scene folders - Yellow theme
            foreach (string folder in SceneFolders)
            {
                if (folderName.Contains(folder))
                    return new Color(1f, 0.9f, 0.2f, 1f); // Yellow
            }

            // Core/Scripts folders - Gray theme
            foreach (string folder in CoreFolders)
            {
                if (folderName.Contains(folder))
                    return new Color(0.6f, 0.6f, 0.6f, 1f); // Gray
            }

            // Asset folders - Green theme
            foreach (string folder in AssetFolders)
            {
                if (folderName.Contains(folder))
                    return new Color(0.3f, 0.8f, 0.3f, 1f); // Green
            }

            return Color.clear; // No custom color
        }
    }

    // Note: All Dragon Toolkit functionality has been moved to UnifiedDragonToolkit.cs
    // Access via: Tools → Dragon Toolkit
}