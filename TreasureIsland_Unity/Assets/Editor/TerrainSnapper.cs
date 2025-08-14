using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace TreasureDragon.Tools
{
    public class TerrainSnapper : EditorWindow
    {
        private bool snapToNormal = false;
        private float heightOffset = 0f;
        private bool maintainRelativeHeight = false;
        private LayerMask terrainLayer = 1; // Default layer
        
        [MenuItem("Tools/Terrain Snapper/Snap Objects to Terrain")]
        public static void Init()
        {
            TerrainSnapper window = (TerrainSnapper)EditorWindow.GetWindow(typeof(TerrainSnapper));
            window.titleContent = new GUIContent("Terrain Snapper");
            window.Show();
        }
        
        void OnGUI()
        {
            GUILayout.Label("Terrain Object Snapper", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            EditorGUILayout.HelpBox("Select objects in the scene, then click 'Snap to Terrain' to reposition them to the terrain surface.", MessageType.Info);
            EditorGUILayout.Space();
            
            snapToNormal = EditorGUILayout.Toggle("Align to Terrain Normal", snapToNormal);
            heightOffset = EditorGUILayout.FloatField("Height Offset", heightOffset);
            maintainRelativeHeight = EditorGUILayout.Toggle("Maintain Relative Height", maintainRelativeHeight);
            terrainLayer = EditorGUILayout.LayerField("Terrain Layer", terrainLayer);
            
            EditorGUILayout.Space();
            
            GUI.enabled = Selection.gameObjects.Length > 0;
            if (GUILayout.Button("Snap Selected Objects to Terrain", GUILayout.Height(30)))
            {
                SnapSelectedObjectsToTerrain();
            }
            GUI.enabled = true;
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Selected Objects: {Selection.gameObjects.Length}");
            
            if (Selection.gameObjects.Length == 0)
            {
                EditorGUILayout.HelpBox("No objects selected. Select objects in the scene hierarchy to enable snapping.", MessageType.Warning);
            }
        }
        
        void SnapSelectedObjectsToTerrain()
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            
            if (selectedObjects.Length == 0)
            {
                EditorUtility.DisplayDialog("No Selection", "Please select objects to snap to terrain.", "OK");
                return;
            }
            
            // Record undo for all selected objects
            Undo.RecordObjects(selectedObjects.Select(obj => obj.transform).ToArray(), "Snap Objects to Terrain");
            
            int snappedCount = 0;
            List<string> failedObjects = new List<string>();
            
            foreach (GameObject obj in selectedObjects)
            {
                if (SnapObjectToTerrain(obj))
                {
                    snappedCount++;
                }
                else
                {
                    failedObjects.Add(obj.name);
                }
            }
            
            // Show results
            string message = $"Successfully snapped {snappedCount} objects to terrain.";
            if (failedObjects.Count > 0)
            {
                message += $"\nFailed to snap {failedObjects.Count} objects: {string.Join(", ", failedObjects)}";
            }
            
            Debug.Log($"[TerrainSnapper] {message}");
            EditorUtility.DisplayDialog("Terrain Snapping Complete", message, "OK");
        }
        
        bool SnapObjectToTerrain(GameObject obj)
        {
            Vector3 originalPosition = obj.transform.position;
            
            // Try multiple methods to find terrain height
            float terrainHeight = GetTerrainHeight(originalPosition);
            
            if (float.IsNaN(terrainHeight))
            {
                // Fallback: Raycast from above
                terrainHeight = GetHeightFromRaycast(originalPosition);
            }
            
            if (float.IsNaN(terrainHeight))
            {
                Debug.LogWarning($"[TerrainSnapper] Could not find terrain height for {obj.name}");
                return false;
            }
            
            // Calculate new position
            Vector3 newPosition = originalPosition;
            
            if (maintainRelativeHeight)
            {
                float currentHeightAboveTerrain = originalPosition.y - GetTerrainHeight(originalPosition);
                newPosition.y = terrainHeight + currentHeightAboveTerrain + heightOffset;
            }
            else
            {
                newPosition.y = terrainHeight + heightOffset;
            }
            
            // Apply position
            obj.transform.position = newPosition;
            
            // Align to terrain normal if requested
            if (snapToNormal)
            {
                Vector3 terrainNormal = GetTerrainNormal(newPosition);
                if (terrainNormal != Vector3.zero)
                {
                    obj.transform.rotation = Quaternion.FromToRotation(Vector3.up, terrainNormal) * obj.transform.rotation;
                }
            }
            
            return true;
        }
        
        float GetTerrainHeight(Vector3 worldPosition)
        {
            // Method 1: Unity Terrain component
            Terrain terrain = Terrain.activeTerrain;
            if (terrain != null)
            {
                return terrain.SampleHeight(worldPosition);
            }
            
            // Method 2: Find terrain in scene
            Terrain[] terrains = FindObjectsOfType<Terrain>();
            foreach (Terrain t in terrains)
            {
                if (IsPositionInTerrain(worldPosition, t))
                {
                    return t.SampleHeight(worldPosition);
                }
            }
            
            return float.NaN;
        }
        
        float GetHeightFromRaycast(Vector3 worldPosition)
        {
            Vector3 rayStart = new Vector3(worldPosition.x, worldPosition.y + 1000f, worldPosition.z);
            RaycastHit hit;
            
            int layerMask = 1 << terrainLayer;
            
            if (Physics.Raycast(rayStart, Vector3.down, out hit, Mathf.Infinity, layerMask))
            {
                return hit.point.y;
            }
            
            // Try without layer mask as fallback
            if (Physics.Raycast(rayStart, Vector3.down, out hit, Mathf.Infinity))
            {
                // Check if hit object is terrain-like
                if (hit.collider.GetComponent<Terrain>() != null || 
                    hit.collider.name.ToLower().Contains("terrain") ||
                    hit.collider.name.ToLower().Contains("ground"))
                {
                    return hit.point.y;
                }
            }
            
            return float.NaN;
        }
        
        Vector3 GetTerrainNormal(Vector3 worldPosition)
        {
            Vector3 rayStart = new Vector3(worldPosition.x, worldPosition.y + 10f, worldPosition.z);
            RaycastHit hit;
            
            if (Physics.Raycast(rayStart, Vector3.down, out hit, 20f))
            {
                return hit.normal;
            }
            
            return Vector3.up; // Default normal
        }
        
        bool IsPositionInTerrain(Vector3 worldPosition, Terrain terrain)
        {
            Vector3 terrainPosition = terrain.transform.position;
            TerrainData terrainData = terrain.terrainData;
            
            return worldPosition.x >= terrainPosition.x && 
                   worldPosition.x <= terrainPosition.x + terrainData.size.x &&
                   worldPosition.z >= terrainPosition.z && 
                   worldPosition.z <= terrainPosition.z + terrainData.size.z;
        }
    }
    
    // Quick menu item for immediate snapping without opening window
    public class QuickTerrainSnap
    {
        [MenuItem("Tools/Terrain Snapper/Quick Snap to Terrain %t")]
        static void QuickSnapToTerrain()
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            
            if (selectedObjects.Length == 0)
            {
                EditorUtility.DisplayDialog("No Selection", "Please select objects to snap to terrain.", "OK");
                return;
            }
            
            Undo.RecordObjects(selectedObjects.Select(obj => obj.transform).ToArray(), "Quick Snap to Terrain");
            
            int snappedCount = 0;
            
            foreach (GameObject obj in selectedObjects)
            {
                Vector3 position = obj.transform.position;
                Terrain terrain = Terrain.activeTerrain;
                
                if (terrain != null)
                {
                    float terrainHeight = terrain.SampleHeight(position);
                    position.y = terrainHeight;
                    obj.transform.position = position;
                    snappedCount++;
                }
                else
                {
                    // Fallback raycast
                    RaycastHit hit;
                    Vector3 rayStart = new Vector3(position.x, position.y + 1000f, position.z);
                    
                    if (Physics.Raycast(rayStart, Vector3.down, out hit, Mathf.Infinity))
                    {
                        position.y = hit.point.y;
                        obj.transform.position = position;
                        snappedCount++;
                    }
                }
            }
            
            Debug.Log($"[QuickTerrainSnap] Snapped {snappedCount} objects to terrain.");
        }
        
        [MenuItem("Tools/Terrain Snapper/Quick Snap to Terrain %t", true)]
        static bool ValidateQuickSnapToTerrain()
        {
            return Selection.gameObjects.Length > 0;
        }
    }
}