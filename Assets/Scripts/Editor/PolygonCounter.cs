using UnityEngine;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine.UIElements;
using System.Linq;

/// <summary>
/// Scene View Overlay that displays real-time polygon/triangle count
/// Shows total scene count when nothing selected, selection count when something selected
/// Includes separate LOD triangle counting based on camera distance
/// </summary>
[Overlay(typeof(SceneView), "Polygon Counter", true)]
public class PolygonCounterOverlay : Overlay
{
    private static int lastTriangleCount = 0;
    private static int lastVertexCount = 0;
    private static int lastLODGroupCount = 0;
    private static int lastLODLevelCount = 0;
    private static int lastLODTriangleCount = 0;
    private static int lastActiveLODLevel = -1;
    private static bool lastIsCulled = false;
    private static float lastUpdateTime = 0f;
    private const float UPDATE_INTERVAL = 0.5f;

    private Label triangleLabel;
    private Label lodLabel;
    private Label lodTriangleLabel;
    private Label lodModeLabel;
    private Label modeLabel;
    private Label perfLabel;
    private Button refreshBtn;

    public override VisualElement CreatePanelContent()
    {
        var root = new VisualElement();
        root.style.minWidth = 200;
        root.style.paddingTop = 5;
        root.style.paddingBottom = 5;
        root.style.paddingLeft = 8;
        root.style.paddingRight = 8;

        // Title
        var titleLabel = new Label("🔺 Polygon Counter");
        titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        titleLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        titleLabel.style.fontSize = 14;
        root.Add(titleLabel);

        // Mode label  
        modeLabel = new Label("🌍 TOTAL SCENE");
        modeLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        modeLabel.style.fontSize = 11;
        modeLabel.style.unityFontStyleAndWeight = FontStyle.Italic;
        modeLabel.style.marginTop = 5;
        root.Add(modeLabel);

        // Triangle count
        triangleLabel = new Label("Triangles: 0");
        triangleLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        triangleLabel.style.fontSize = 12;
        triangleLabel.style.marginTop = 3;
        root.Add(triangleLabel);

        // LOD count
        lodLabel = new Label("LOD Groups: 0");
        lodLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        lodLabel.style.fontSize = 11;
        lodLabel.style.marginTop = 2;
        root.Add(lodLabel);

        // Performance indicator
        perfLabel = new Label("🟢 Excellent");
        perfLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        perfLabel.style.fontSize = 11;
        perfLabel.style.unityFontStyleAndWeight = FontStyle.Italic;
        perfLabel.style.marginTop = 5;
        root.Add(perfLabel);

        // Separator
        var separator = new Label("─────────────────");
        separator.style.unityTextAlign = TextAnchor.MiddleCenter;
        separator.style.fontSize = 10;
        separator.style.color = Color.gray;
        separator.style.marginTop = 8;
        separator.style.marginBottom = 3;
        root.Add(separator);

        // LOD Triangle section title
        var lodSectionTitle = new Label("🎯 LOD RENDERING");
        lodSectionTitle.style.unityTextAlign = TextAnchor.MiddleCenter;
        lodSectionTitle.style.fontSize = 11;
        lodSectionTitle.style.unityFontStyleAndWeight = FontStyle.Bold;
        lodSectionTitle.style.color = Color.cyan;
        lodSectionTitle.style.marginTop = 2;
        root.Add(lodSectionTitle);

        // LOD mode label
        lodModeLabel = new Label("🎥 CAMERA VIEW");
        lodModeLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        lodModeLabel.style.fontSize = 10;
        lodModeLabel.style.unityFontStyleAndWeight = FontStyle.Italic;
        lodModeLabel.style.marginTop = 3;
        root.Add(lodModeLabel);

        // LOD Triangle count
        lodTriangleLabel = new Label("LOD Triangles: 0");
        lodTriangleLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        lodTriangleLabel.style.fontSize = 12;
        lodTriangleLabel.style.marginTop = 3;
        root.Add(lodTriangleLabel);

        // Refresh button
        refreshBtn = new Button(() => {
            UpdateCounts();
        });
        refreshBtn.text = "🔄 Refresh";
        refreshBtn.style.height = 25;
        refreshBtn.style.marginTop = 5;
        root.Add(refreshBtn);

        // Set up update loop
        EditorApplication.update += UpdateLoop;

        // Initial update
        UpdateCounts();

        return root;
    }

    private void UpdateLoop()
    {
        if (Time.realtimeSinceStartup - lastUpdateTime > UPDATE_INTERVAL)
        {
            UpdateCounts();
            lastUpdateTime = Time.realtimeSinceStartup;
        }
    }

    private void UpdateCounts()
    {
        lastTriangleCount = 0;
        lastVertexCount = 0;
        lastLODGroupCount = 0;
        lastLODLevelCount = 0;
        lastLODTriangleCount = 0;
        lastActiveLODLevel = -1;
        lastIsCulled = false;
        
        // Check if we should count selection or entire scene
        if (Selection.activeGameObject != null)
        {
            CountGameObjectRecursive(Selection.activeGameObject);
        }
        else
        {
            CountAllObjectsInScene();
        }

        // Update UI
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (triangleLabel != null)
        {
            triangleLabel.text = $"Triangles: {lastTriangleCount:N0}";
            triangleLabel.style.color = GetColorForTriangleCount(lastTriangleCount);
        }

        bool hasSelection = Selection.activeGameObject != null;
        
        if (lodLabel != null)
        {
            if (hasSelection)
            {
                if (lastLODGroupCount > 0)
                {
                    lodLabel.text = $"LOD: {lastLODLevelCount} levels (0-{lastLODLevelCount - 1})";
                    lodLabel.style.color = Color.green;
                }
                else
                {
                    lodLabel.text = "LOD: None";
                    lodLabel.style.color = Color.gray;
                }
            }
            else
            {
                lodLabel.text = $"LOD Groups: {lastLODGroupCount}";
                lodLabel.style.color = lastLODGroupCount > 0 ? Color.green : Color.gray;
            }
        }
        
        if (modeLabel != null)
        {
            modeLabel.text = hasSelection ? 
                $"📌 SELECTED: {Selection.activeGameObject.name}" : 
                "🌍 TOTAL SCENE";
            modeLabel.style.color = hasSelection ? Color.cyan : Color.white;
        }

        if (perfLabel != null)
        {
            string perfIndicator = GetPerformanceIndicator(lastTriangleCount, hasSelection);
            perfLabel.text = perfIndicator;
            perfLabel.style.color = GetColorForPerformance(lastTriangleCount);
        }

        // Update LOD triangle section
        if (lodTriangleLabel != null)
        {
            lodTriangleLabel.text = $"LOD Triangles: {lastLODTriangleCount:N0}";
            lodTriangleLabel.style.color = GetColorForTriangleCount(lastLODTriangleCount);
        }

        if (lodModeLabel != null)
        {
            if (hasSelection)
            {
                if (lastLODGroupCount > 0)
                {
                    if (lastIsCulled)
                    {
                        lodModeLabel.text = "🚫 CULLED";
                        lodModeLabel.style.color = Color.red;
                    }
                    else
                    {
                        lodModeLabel.text = $"🎯 LOD{lastActiveLODLevel} ACTIVE";
                        lodModeLabel.style.color = GetColorForLODLevel(lastActiveLODLevel);
                    }
                }
                else
                {
                    lodModeLabel.text = "🎯 NO LOD";
                    lodModeLabel.style.color = Color.gray;
                }
            }
            else
            {
                lodModeLabel.text = "🎥 CAMERA VIEW";
                lodModeLabel.style.color = Color.white;
            }
        }
    }
    
    private void CountGameObjectRecursive(GameObject obj)
    {
        if (!obj.activeInHierarchy) return;
        
        // Check if this object has LOD group first
        LODGroup lodGroup = obj.GetComponent<LODGroup>();
        if (lodGroup != null)
        {
            lastLODGroupCount++;
            LOD[] lods = lodGroup.GetLODs();
            lastLODLevelCount = lods.Length;
            
            // For Regular Triangles: Count LOD0 (highest detail)
            if (lods.Length > 0)
            {
                LOD lod0 = lods[0]; // Highest detail level
                foreach (Renderer renderer in lod0.renderers)
                {
                    if (renderer != null && renderer.gameObject.activeInHierarchy)
                    {
                        if (renderer is MeshRenderer)
                        {
                            MeshFilter meshFilter = renderer.GetComponent<MeshFilter>();
                            if (meshFilter != null && meshFilter.sharedMesh != null)
                            {
                                Mesh mesh = meshFilter.sharedMesh;
                                lastTriangleCount += mesh.triangles.Length / 3;
                                lastVertexCount += mesh.vertexCount;
                            }
                        }
                        else if (renderer is SkinnedMeshRenderer skinnedMeshRenderer)
                        {
                            if (skinnedMeshRenderer.sharedMesh != null)
                            {
                                Mesh mesh = skinnedMeshRenderer.sharedMesh;
                                lastTriangleCount += mesh.triangles.Length / 3;
                                lastVertexCount += mesh.vertexCount;
                            }
                        }
                    }
                }
            }
            
            // For LOD Triangles: Count currently active LOD level based on camera distance
            Vector3 cameraPos = GetSceneViewCameraPosition();
            int activeLODLevel = GetActiveLODLevel(lodGroup, cameraPos);
            lastActiveLODLevel = activeLODLevel;
            
            // Check if object is culled (too far away)
            lastIsCulled = (activeLODLevel < 0);
            
            if (activeLODLevel >= 0 && activeLODLevel < lods.Length)
            {
                LOD activeLOD = lods[activeLODLevel];
                foreach (Renderer renderer in activeLOD.renderers)
                {
                    if (renderer != null && renderer.gameObject.activeInHierarchy)
                    {
                        if (renderer is MeshRenderer)
                        {
                            MeshFilter meshFilter = renderer.GetComponent<MeshFilter>();
                            if (meshFilter != null && meshFilter.sharedMesh != null)
                            {
                                Mesh mesh = meshFilter.sharedMesh;
                                lastLODTriangleCount += mesh.triangles.Length / 3;
                            }
                        }
                        else if (renderer is SkinnedMeshRenderer skinnedMeshRenderer)
                        {
                            if (skinnedMeshRenderer.sharedMesh != null)
                            {
                                Mesh mesh = skinnedMeshRenderer.sharedMesh;
                                lastLODTriangleCount += mesh.triangles.Length / 3;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            // No LOD group - count regular mesh renderers
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    Mesh mesh = meshFilter.sharedMesh;
                    lastTriangleCount += mesh.triangles.Length / 3;
                    lastVertexCount += mesh.vertexCount;
                    // For non-LOD objects, LOD triangles = regular triangles
                    lastLODTriangleCount += mesh.triangles.Length / 3;
                }
            }
            
            // Count skinned mesh renderers
            SkinnedMeshRenderer skinnedRenderer = obj.GetComponent<SkinnedMeshRenderer>();
            if (skinnedRenderer != null && skinnedRenderer.sharedMesh != null)
            {
                Mesh mesh = skinnedRenderer.sharedMesh;
                lastTriangleCount += mesh.triangles.Length / 3;
                lastVertexCount += mesh.vertexCount;
                // For non-LOD objects, LOD triangles = regular triangles
                lastLODTriangleCount += mesh.triangles.Length / 3;
            }
        }
        
        // Recursively count children
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            CountGameObjectRecursive(obj.transform.GetChild(i).gameObject);
        }
    }
    
    private void CountAllObjectsInScene()
    {
        // Get all mesh renderers in the scene
        MeshRenderer[] meshRenderers = Object.FindObjectsOfType<MeshRenderer>();
        
        foreach (MeshRenderer renderer in meshRenderers)
        {
            if (renderer.gameObject.activeInHierarchy)
            {
                MeshFilter meshFilter = renderer.GetComponent<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    Mesh mesh = meshFilter.sharedMesh;
                    lastTriangleCount += mesh.triangles.Length / 3;
                    lastVertexCount += mesh.vertexCount;
                }
            }
        }
        
        // Also count skinned mesh renderers
        SkinnedMeshRenderer[] skinnedRenderers = Object.FindObjectsOfType<SkinnedMeshRenderer>();
        
        foreach (SkinnedMeshRenderer renderer in skinnedRenderers)
        {
            if (renderer.gameObject.activeInHierarchy && renderer.sharedMesh != null)
            {
                Mesh mesh = renderer.sharedMesh;
                lastTriangleCount += mesh.triangles.Length / 3;
                lastVertexCount += mesh.vertexCount;
            }
        }
        
        // Count all LOD groups in the scene and calculate LOD triangles
        LODGroup[] lodGroups = Object.FindObjectsOfType<LODGroup>();
        lastLODGroupCount = lodGroups.Length;
        
        // Get camera position for LOD calculations
        Vector3 cameraPos = GetSceneViewCameraPosition();
        
        // Count LOD triangles for all LOD groups based on camera distance
        foreach (LODGroup lodGroup in lodGroups)
        {
            if (lodGroup.gameObject.activeInHierarchy)
            {
                int activeLODLevel = GetActiveLODLevel(lodGroup, cameraPos);
                LOD[] lods = lodGroup.GetLODs();
                
                if (activeLODLevel >= 0 && activeLODLevel < lods.Length)
                {
                    LOD activeLOD = lods[activeLODLevel];
                    foreach (Renderer renderer in activeLOD.renderers)
                    {
                        if (renderer != null && renderer.gameObject.activeInHierarchy)
                        {
                            if (renderer is MeshRenderer)
                            {
                                MeshFilter meshFilter = renderer.GetComponent<MeshFilter>();
                                if (meshFilter != null && meshFilter.sharedMesh != null)
                                {
                                    Mesh mesh = meshFilter.sharedMesh;
                                    lastLODTriangleCount += mesh.triangles.Length / 3;
                                }
                            }
                            else if (renderer is SkinnedMeshRenderer skinnedMeshRenderer)
                            {
                                if (skinnedMeshRenderer.sharedMesh != null)
                                {
                                    Mesh mesh = skinnedMeshRenderer.sharedMesh;
                                    lastLODTriangleCount += mesh.triangles.Length / 3;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        // Also count non-LOD objects for LOD triangle count
        foreach (MeshRenderer renderer in meshRenderers)
        {
            // Only count if it's not part of a LOD group
            if (renderer.gameObject.activeInHierarchy && renderer.GetComponentInParent<LODGroup>() == null)
            {
                MeshFilter meshFilter = renderer.GetComponent<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    Mesh mesh = meshFilter.sharedMesh;
                    lastLODTriangleCount += mesh.triangles.Length / 3;
                }
            }
        }
        
        foreach (SkinnedMeshRenderer renderer in skinnedRenderers)
        {
            // Only count if it's not part of a LOD group
            if (renderer.gameObject.activeInHierarchy && renderer.sharedMesh != null && 
                renderer.GetComponentInParent<LODGroup>() == null)
            {
                Mesh mesh = renderer.sharedMesh;
                lastLODTriangleCount += mesh.triangles.Length / 3;
            }
        }
        
        // For scene total, show total number of LOD groups (not levels)
        lastLODLevelCount = 0; // Reset for scene view
    }

    private Color GetColorForTriangleCount(int triangleCount)
    {
        if (triangleCount < 50000) return Color.green;
        if (triangleCount < 100000) return Color.yellow;
        if (triangleCount < 200000) return Color.red;
        return Color.magenta;
    }
    
    private Color GetColorForPerformance(int triangleCount)
    {
        if (triangleCount < 50000) return Color.green;
        if (triangleCount < 100000) return Color.yellow;
        return Color.red;
    }

    private Color GetColorForLODLevel(int lodLevel)
    {
        switch (lodLevel)
        {
            case 0: return Color.green;     // LOD0 - Highest detail
            case 1: return Color.yellow;    // LOD1 - High detail
            case 2: return new Color(1f, 0.5f, 0f); // LOD2 - Medium detail (orange)
            case 3: return Color.red;       // LOD3 - Low detail
            default: return Color.gray;     // Unknown or no LOD
        }
    }

    private string GetPerformanceIndicator(int triangleCount, bool isSelection)
    {
        if (isSelection)
        {
            if (triangleCount < 1000) return "🟢 Low Detail";
            if (triangleCount < 5000) return "🟡 Medium Detail";
            if (triangleCount < 15000) return "🟠 High Detail";
            return "🔴 Very High Detail";
        }
        else
        {
            if (triangleCount < 25000) return "🟢 Excellent";
            if (triangleCount < 50000) return "🟡 Good";
            if (triangleCount < 100000) return "🟠 Moderate";
            if (triangleCount < 200000) return "🔴 High";
            return "🟣 Very High";
        }
    }

    /// <summary>
    /// Calculate which LOD level should be active based on camera distance
    /// </summary>
    private int GetActiveLODLevel(LODGroup lodGroup, Vector3 cameraPosition)
    {
        if (lodGroup == null) return -1;

        // Get the LOD group's position and size
        Vector3 lodPosition = lodGroup.transform.position;
        float lodSize = lodGroup.size;
        
        // Calculate distance from camera to LOD group
        float distance = Vector3.Distance(cameraPosition, lodPosition);
        
        // Calculate relative distance (Unity's LOD system uses this)
        float relativeDistance = distance / lodSize;
        
        // Get LOD levels and find which one should be active
        LOD[] lods = lodGroup.GetLODs();
        
        for (int i = 0; i < lods.Length; i++)
        {
            // LOD transition values go from 1.0 (closest) to 0.0 (farthest)
            if (relativeDistance <= (1.0f / lods[i].screenRelativeTransitionHeight))
            {
                return i;
            }
        }
        
        // If we're too far away, object is culled (return -1 to indicate culling)
        return -1;
    }

    /// <summary>
    /// Get the current Scene View camera position
    /// </summary>
    private Vector3 GetSceneViewCameraPosition()
    {
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView != null && sceneView.camera != null)
        {
            return sceneView.camera.transform.position;
        }
        
        // Fallback to main camera if Scene View not available
        if (Camera.main != null)
        {
            return Camera.main.transform.position;
        }
        
        return Vector3.zero;
    }

    public override void OnWillBeDestroyed()
    {
        EditorApplication.update -= UpdateLoop;
        base.OnWillBeDestroyed();
    }
}

/// <summary>
/// Utility class for managing Polygon Counter from other scripts
/// </summary>
public static class PolygonCounterManager
{
    public static void ToggleOverlay()
    {
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView != null)
        {
            Debug.Log($"[Debug] Found Scene View: {sceneView.name}");
            Debug.Log($"[Debug] Overlay count: {sceneView.overlayCanvas.overlays.Count()}");
            
            var overlay = sceneView.overlayCanvas.overlays.FirstOrDefault(o => o is PolygonCounterOverlay);
            if (overlay != null)
            {
                bool wasDisplayed = overlay.displayed;
                overlay.displayed = !overlay.displayed;
                sceneView.Repaint();
                
                Debug.Log($"[Polygon Counter] Overlay {(overlay.displayed ? "ENABLED" : "DISABLED")} (was: {wasDisplayed})");
                Debug.Log($"[Debug] Overlay position: {overlay.layout}");
                
                // Force Scene View focus and refresh
                sceneView.Focus();
                SceneView.RepaintAll();
            }
            else
            {
                Debug.LogWarning("[Polygon Counter] Overlay not found! Available overlays:");
                foreach (var o in sceneView.overlayCanvas.overlays)
                {
                    Debug.Log($"  - {o.GetType().Name}: {o.displayName}");
                }
            }
        }
        else
        {
            Debug.LogWarning("[Polygon Counter] No active Scene View found");
        }
    }
    
    public static bool IsOverlayVisible()
    {
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView != null)
        {
            var overlay = sceneView.overlayCanvas.overlays.FirstOrDefault(o => o is PolygonCounterOverlay);
            return overlay != null && overlay.displayed;
        }
        return false;
    }
    
    public static void ForceShowOverlay()
    {
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView != null)
        {
            var overlay = sceneView.overlayCanvas.overlays.FirstOrDefault(o => o is PolygonCounterOverlay);
            if (overlay != null)
            {
                overlay.displayed = true;
                sceneView.Repaint();
                sceneView.Focus();
                SceneView.RepaintAll();
                Debug.Log("[Polygon Counter] FORCE ENABLED overlay");
            }
            else
            {
                Debug.LogError("[Polygon Counter] Cannot force show - overlay doesn't exist!");
                Debug.Log("Try refreshing the Scene View or recompiling scripts.");
            }
        }
        else
        {
            Debug.LogError("[Polygon Counter] No Scene View found!");
        }
    }
} 