using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace TreasureDragon.Tools
{
    [InitializeOnLoad]
    public class DragonFolderIcons
    {
        private static Dictionary<string, string> folderIcons = new Dictionary<string, string>()
        {
            // Main Categories
            { "Audio", "🎵" },
            { "ImportedAssets", "📦" },
            { "AssetStore", "🛍️" },
            { "Materials", "🎨" },
            { "Models", "🗿" },
            { "Prefabs", "🎯" },
            { "Scenes", "🎬" },
            { "Scripts", "💻" },
            { "Textures", "🖼️" },
            { "Shaders", "✨" },
            { "Editor", "🔧" },
            
            // Sub-Categories
            { "Architecture", "🏛️" },
            { "Combat", "⚔️" },
            { "Dragon", "🐉" },
            { "Environment", "🌴" },
            { "Player", "🎮" },
            
            // Architecture Sub-folders
            { "Buildings", "🏠" },
            { "Houses", "🏘️" },
            { "Civilian", "👤" },
            { "Shops", "🏪" },
            { "Infrastructure", "🏗️" },
            { "Temples", "⛩️" },
            { "People", "👥" },
            { "Civilians", "👫" },
            { "Merchants", "💰" },
            { "NPCs", "🎭" },
            { "Workers", "👷" },
            { "Props", "📦" },
            { "Tools", "🛠️" },
            { "Furniture", "🪑" },
            { "Decorations", "🎨" },
            { "Vehicles", "🚗" },
            
            // Military/Combat
            { "Military", "🪖" },
            { "Personnel", "💂" },
            { "Towers", "🗼" },
            { "Weapons", "🗡️" },
            
            // Environment Sub-folders
            { "Trees", "🌳" },
            { "Vegetation", "🌿" },
            { "Grass", "🌾" },
            { "Flowers", "🌸" },
            { "Bushes", "🌱" },
            { "Plants", "🪴" },
            { "Rocks", "🪨" },
            { "Cliffs", "🏔️" },
            { "Terrain", "⛰️" },
            { "Islands", "🏝️" },
            { "Landscapes", "🏞️" },
            { "Heightmaps", "📊" },
            { "Water", "💧" },
            { "Water_Features", "🌊" },
            { "Natural", "🍃" },
            
            // Scene Organization
            { "Archive", "📁" },
            { "Environments", "🌍" },
            { "Masters", "👑" },
            { "Testing", "🧪" },
            { "SampleScene", "📋" },
            
            // Asset types
            { "Animations", "🎬" },
            { "Documentation", "📚" },
            { "Particles", "✨" },
            { "Profiles", "⚙️" },
            { "Settings", "⚙️" },
            { "Skybox", "☁️" },
            
            // Special folders
            { "_Project", "🚀" },
            { "Resources", "📦" },
            { "StreamingAssets", "📡" },
            { "Plugins", "🔌" },
            { "Gizmos", "🎯" },
            
            // Toon Adventure Island specific
            { "Toon", "🎪" },
            { "TAI", "🏝️" },
            { "ToonyTinyPeople", "👶" },
            { "TT_RTS", "🎮" }
        };

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
            
            // Get the emoji icon for this folder
            string icon = GetFolderIcon(folderName);
            
            if (!string.IsNullOrEmpty(icon))
            {
                // Calculate icon position
                Rect iconRect;
                
                // Check if we're in list view or grid view
                if (selectionRect.width > selectionRect.height * 3) // List view
                {
                    iconRect = new Rect(selectionRect.x - 1, selectionRect.y, 16, selectionRect.height);
                }
                else // Grid view
                {
                    iconRect = new Rect(selectionRect.x + selectionRect.width - 20, 
                                       selectionRect.y + 2, 18, 18);
                }
                
                // Draw the emoji icon
                GUIStyle style = new GUIStyle();
                style.normal.textColor = GetIconColor(folderName);
                style.fontSize = 14;
                style.alignment = TextAnchor.MiddleCenter;
                
                // Add subtle background for better visibility
                Color bgColor = style.normal.textColor * 0.2f;
                bgColor.a = 0.3f;
                EditorGUI.DrawRect(iconRect, bgColor);
                
                // Draw the emoji
                GUI.Label(iconRect, icon, style);
            }
        }

        private static string GetFolderIcon(string folderName)
        {
            // Check for exact matches first
            if (folderIcons.ContainsKey(folderName))
                return folderIcons[folderName];
            
            // Then check for partial matches
            foreach (var kvp in folderIcons)
            {
                if (folderName.IndexOf(kvp.Key, System.StringComparison.OrdinalIgnoreCase) >= 0)
                    return kvp.Value;
            }
            
            // Default folder icon if no match
            return "";
        }

        private static Color GetIconColor(string folderName)
        {
            // Color coding for different folder types
            if (folderName.Contains("Combat") || folderName.Contains("Military"))
                return new Color(1f, 0.3f, 0.2f); // Red
            
            if (folderName.Contains("Dragon") || folderName.Contains("Player"))
                return new Color(0.8f, 0.4f, 1f); // Purple
            
            if (folderName.Contains("Environment") || folderName.Contains("Vegetation"))
                return new Color(0.2f, 0.8f, 0.2f); // Green
            
            if (folderName.Contains("Architecture") || folderName.Contains("Buildings"))
                return new Color(0.2f, 0.6f, 1f); // Blue
            
            if (folderName.Contains("Scene") || folderName.Contains("Testing"))
                return new Color(1f, 0.8f, 0.2f); // Yellow
            
            if (folderName.Contains("Scripts") || folderName.Contains("Editor"))
                return new Color(0.7f, 0.7f, 0.7f); // Gray
            
            if (folderName.Contains("Audio"))
                return new Color(1f, 0.5f, 0.8f); // Pink
            
            return Color.white; // Default white
        }
    }
}