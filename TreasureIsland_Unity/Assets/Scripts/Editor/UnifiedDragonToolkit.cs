using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Linq;

public class UnifiedDragonToolkit : EditorWindow
{
    // Force recompile - Vegetation sliders fixed
    private Vector2 scrollPosition;
    private const string PROJECT_NAME = "TreasureDragon";
    private const string MAIN_OBJECT_NAME = "Dragon";
    
    // Vegetation slider values
    private static float grassPercentage = 20f;
    private static float plantPercentage = 20f;
    private static float treePercentage = 20f;
    private static float palmTreePercentage = 20f;
    private static float vinePercentage = 20f;
    private static float smallRockPercentage = 20f;
    private static float mediumRockPercentage = 20f;
    private static float largeRockPercentage = 20f;
    private static float bushPercentage = 20f;
    private static float flowerPercentage = 20f;
    
    // Custom vegetation types
    [System.Serializable]
    public class CustomVegetationType
    {
        public string name = "New Type";
        public string icon = "🌿";
        public string searchTerm = "";
        public float percentage = 20f;
    }
    
    private static List<CustomVegetationType> customTypes = new List<CustomVegetationType>();
    private static bool showAddCustomType = false;
    private static string newTypeName = "";
    private static string newTypeIcon = "🌿";
    private static string newTypeSearchTerm = "";
    
    private static readonly Dictionary<string, string> ASSET_PATHS = new Dictionary<string, string>
    {
        ["MainTexture"] = "Assets/Textures/DragonTexture.png",
        ["MaterialsFolder"] = "Assets/Materials/",
        ["URPAssetPath"] = "Assets/Settings/PC_RPAsset.asset",
        ["RendererDataPath"] = "Assets/Settings/PC_Renderer.asset"
    };

    // === MAIN MENU STRUCTURE ===
    [MenuItem("Tools/Dragon Toolkit")]
    public static void ShowWindow()
    {
        var window = GetWindow<UnifiedDragonToolkit>("🐉 Dragon Toolkit v3.0");
        window.minSize = new Vector2(700, 400);
        LoadCustomTypes();
    }

    [MenuItem("Tools/🚨 EMERGENCY - Open Dragon Toolkit")]
    public static void EmergencyOpen()
    {
        // Close any existing window first
        var existingWindow = GetWindow<UnifiedDragonToolkit>(false, "🐉 Dragon Toolkit v3.0", false);
        if (existingWindow != null)
        {
            existingWindow.Close();
        }
        
        // Open fresh window
        var window = GetWindow<UnifiedDragonToolkit>("🐉 Dragon Toolkit v3.0");
        window.minSize = new Vector2(700, 400);
        LoadCustomTypes();
        window.Show();
        window.Focus();
        Debug.Log("🐉 Dragon Toolkit opened with vegetation sliders!");
    }

    void OnGUI()
    {
        GUILayout.Label("🐉 TreasureDragon Unified Toolkit v3.0", EditorStyles.boldLabel);
        GUILayout.Label("All development tools in one place", EditorStyles.helpBox);
        GUILayout.Space(10);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // === EMERGENCY SECTION ===
        DrawEmergencySection();
        
        GUILayout.Space(10);
        
        // === URP FIXES SECTION ===
        DrawURPFixesSection();
        
        GUILayout.Space(10);
        
        // === VEGETATION OPTIMIZER SECTION ===
        DrawVegetationSection();
        
        GUILayout.Space(10);
        
        // === POLYGON COUNTER SECTION ===
        DrawPolygonCounterSection();
        
        GUILayout.Space(10);
        
        // === DIAGNOSTICS SECTION ===
        DrawDiagnosticsSection();
        
        GUILayout.Space(10);
        
        // === ORGANIZATION TOOLS SECTION ===
        DrawOrganizationSection();
        
        GUILayout.Space(10);
        
        // === TERRAIN TOOLS SECTION ===
        DrawTerrainToolsSection();

        EditorGUILayout.EndScrollView();
    }

    void DrawEmergencySection()
    {
        GUILayout.Label("🚨 Emergency Fixes", EditorStyles.boldLabel);
        
        if (GUILayout.Button("🚨 EMERGENCY - Fix Everything", GUILayout.Height(40)))
        {
            EmergencyFixEverything();
        }
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("🔍 Diagnose All Issues"))
        {
            DiagnoseAllIssues();
        }
        if (GUILayout.Button("🛠️ Quick URP Fix"))
        {
            FixURPSetup();
        }
        GUILayout.EndHorizontal();
    }

    void DrawURPFixesSection()
    {
        GUILayout.Label("🔧 URP & Rendering Fixes", EditorStyles.boldLabel);
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Fix URP Setup"))
        {
            FixURPSetup();
        }
        if (GUILayout.Button("Fix Pink Textures"))
        {
            FixAllMaterials();
        }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Fix Lighting & Camera"))
        {
            FixLightingAndCamera();
        }
        if (GUILayout.Button("Fix Scene Visibility"))
        {
            FixSceneVisibility();
        }
        GUILayout.EndHorizontal();
    }

    void DrawVegetationSection()
    {
        GUILayout.Label("🌿 Vegetation Optimizer - WITH SLIDERS", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("Use sliders to set removal percentage (0-100% in 10% intervals)", EditorStyles.miniLabel);
        GUILayout.Label("Click 'Remove' to delete objects | Use +10/-10 for quick adjustments", EditorStyles.miniLabel);
        EditorGUILayout.EndVertical();
        
        GUILayout.Space(5);
        
        // Vegetation items
        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("🌿 Vegetation Objects", EditorStyles.boldLabel);
        GUILayout.Space(5);
        ShowVegetationSlider("🌱 Grass", "grass", ref grassPercentage);
        ShowVegetationSlider("🌿 Plants", "plant", ref plantPercentage);  
        ShowVegetationSlider("🌳 Trees", "TAI_Tree_", ref treePercentage);
        ShowVegetationSlider("🌴 Palm Trees", "palm", ref palmTreePercentage);
        ShowVegetationSlider("🍃 Vines", "vine", ref vinePercentage);
        ShowVegetationSlider("🌺 Flowers", "flower", ref flowerPercentage);
        ShowVegetationSlider("🌳 Bushes", "bush", ref bushPercentage);
        EditorGUILayout.EndVertical();
        
        GUILayout.Space(10);
        
        // Rock items
        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("🪨 Rock Objects", EditorStyles.boldLabel);
        GUILayout.Space(5);
        ShowVegetationSlider("🪨 Small Rocks", "TAI_Small_Rock", ref smallRockPercentage);
        ShowVegetationSlider("🗿 Medium Rocks", "TAI_Rock", ref mediumRockPercentage);
        ShowVegetationSlider("🏔️ Large Rocks", "TAI_Big_Rock", ref largeRockPercentage);
        EditorGUILayout.EndVertical();
        
        GUILayout.Space(10);
        
        // Custom types section
        if (customTypes.Count > 0 || showAddCustomType)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("🎯 Custom Object Types", EditorStyles.boldLabel);
            GUILayout.Space(5);
            
            // Show existing custom types
            for (int i = 0; i < customTypes.Count; i++)
            {
                var customType = customTypes[i];
                ShowCustomVegetationSlider(customType, i);
            }
            
            if (customTypes.Count == 0 && !showAddCustomType)
            {
                EditorGUILayout.HelpBox("No custom types defined. Click '+ Add Custom Object Type' to create one.", MessageType.Info);
            }
            
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
        }
        
        // Add new custom type UI
        if (showAddCustomType)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Add Custom Object Type", EditorStyles.boldLabel);
            
            newTypeName = EditorGUILayout.TextField("Name:", newTypeName);
            newTypeIcon = EditorGUILayout.TextField("Icon:", newTypeIcon);
            newTypeSearchTerm = EditorGUILayout.TextField("Search Term:", newTypeSearchTerm);
            
            EditorGUILayout.HelpBox("Search Term: The text to search for in object names (case-insensitive)", MessageType.Info);
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Type", GUILayout.Height(25)))
            {
                if (!string.IsNullOrEmpty(newTypeName) && !string.IsNullOrEmpty(newTypeSearchTerm))
                {
                    customTypes.Add(new CustomVegetationType 
                    { 
                        name = newTypeName,
                        icon = newTypeIcon,
                        searchTerm = newTypeSearchTerm,
                        percentage = 20f
                    });
                    
                    SaveCustomTypes();
                    GenerateCustomTypeMenuItems();
                    
                    // Reset fields
                    newTypeName = "";
                    newTypeIcon = "🌿";
                    newTypeSearchTerm = "";
                    showAddCustomType = false;
                    
                    Debug.Log($"✅ Added custom type: {newTypeName}");
                }
                else
                {
                    EditorUtility.DisplayDialog("Missing Information", "Please fill in Name and Search Term", "OK");
                }
            }
            if (GUILayout.Button("Cancel", GUILayout.Height(25)))
            {
                showAddCustomType = false;
                newTypeName = "";
                newTypeIcon = "🌿";
                newTypeSearchTerm = "";
            }
            GUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
        }
        else
        {
            if (GUILayout.Button("➕ Add Custom Object Type", GUILayout.Height(30)))
            {
                showAddCustomType = true;
                newTypeName = "";
                newTypeIcon = "🌿";
                newTypeSearchTerm = "";
            }
        }
        
        GUILayout.Space(10);
        
        // Quick action buttons
        GUILayout.Label("Quick Actions:", EditorStyles.miniBoldLabel);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Remove 10% ALL", GUILayout.Height(25)))
        {
            if (EditorUtility.DisplayDialog("Confirm Removal", 
                "Remove 10% of ALL vegetation and rocks?", 
                "Remove", "Cancel"))
            {
                RemoveAllVegetation(0.1f);
            }
        }
        if (GUILayout.Button("Remove 30% ALL", GUILayout.Height(25)))
        {
            if (EditorUtility.DisplayDialog("Confirm Removal", 
                "Remove 30% of ALL vegetation and rocks?\nThis CANNOT be undone!", 
                "Remove", "Cancel"))
            {
                RemoveAllVegetation(0.3f);
            }
        }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Optimize for Mobile", GUILayout.Height(25)))
        {
            if (EditorUtility.DisplayDialog("Mobile Optimization", 
                "This will remove:\n- 50% Grass\n- 30% Plants\n- 20% Trees\n- 40% Small Rocks\n\nContinue?", 
                "Optimize", "Cancel"))
            {
                OptimizeForMobile();
            }
        }
        if (GUILayout.Button("Reset Sliders", GUILayout.Height(25)))
        {
            ResetVegetationSliders();
        }
        GUILayout.EndHorizontal();
    }

    void DrawPolygonCounterSection()
    {
        GUILayout.Label("🔺 Polygon Counter Control", EditorStyles.boldLabel);
        GUILayout.Space(5);
        
        bool isOverlayVisible = PolygonCounterManager.IsOverlayVisible();
        
        GUILayout.BeginHorizontal();
        
        // Status indicator
        GUIStyle statusStyle = new GUIStyle(GUI.skin.label);
        statusStyle.normal.textColor = isOverlayVisible ? Color.green : Color.red;
        GUILayout.Label(isOverlayVisible ? "🟢 VISIBLE" : "🔴 HIDDEN", statusStyle);
        
        GUILayout.FlexibleSpace();
        
        // Toggle button
        string buttonText = isOverlayVisible ? "🔺 Hide Counter" : "🔺 Show Counter";
        if (GUILayout.Button(buttonText, GUILayout.Height(30), GUILayout.Width(120)))
        {
            PolygonCounterManager.ToggleOverlay();
        }
        
        GUILayout.EndHorizontal();
        
        // Force Show button (emergency backup)
        if (GUILayout.Button("🚨 FORCE SHOW", GUILayout.Height(25)))
        {
            PolygonCounterManager.ForceShowOverlay();
        }
        
        GUILayout.Space(5);
        
        // Info text
        EditorGUILayout.HelpBox(
            "Shows real-time triangle count in Scene View.\n" +
            "• No selection = Total scene count\n" +
            "• Object selected = Just that object's count\n" +
            "Perfect for monitoring file size during optimization!", 
            MessageType.Info);
    }

    void DrawDiagnosticsSection()
    {
        GUILayout.Label("🔍 Diagnostics", EditorStyles.boldLabel);
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Diagnose Render Pipeline"))
        {
            DiagnoseRenderPipeline();
        }
        if (GUILayout.Button("Diagnose Camera"))
        {
            DiagnoseCamera();
        }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Diagnose Lighting"))
        {
            DiagnoseLighting();
        }
        if (GUILayout.Button("Diagnose Materials"))
        {
            DiagnoseMaterials();
        }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Diagnose Main Object"))
        {
            DiagnoseMainObject();
        }
        if (GUILayout.Button("Diagnose Grass Objects"))
        {
            DiagnoseGrassObjects();
        }
        GUILayout.EndHorizontal();
    }

    void DrawOrganizationSection()
    {
        GUILayout.Label("📁 Project Organization", EditorStyles.boldLabel);
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Combat Structure"))
        {
            CreateCombatStructure();
        }
        if (GUILayout.Button("Create Architecture Structure"))
        {
            CreateArchitectureStructure();
        }
        GUILayout.EndHorizontal();
        
        if (GUILayout.Button("Refresh Project Colors"))
        {
            RefreshProjectColors();
        }
    }

    void DrawTerrainToolsSection()
    {
        GUILayout.Label("🏔️ Terrain Tools", EditorStyles.boldLabel);
        
        GUI.enabled = Selection.gameObjects.Length > 0;
        if (GUILayout.Button($"Snap {Selection.gameObjects.Length} Selected Objects to Terrain"))
        {
            QuickSnapSelectedToTerrain();
        }
        GUI.enabled = true;
        
        if (GUILayout.Button("Open Advanced Terrain Snapper"))
        {
            TreasureDragon.Tools.TerrainSnapper.Init();
        }
        
        if (Selection.gameObjects.Length == 0)
        {
            EditorGUILayout.HelpBox("Select objects in scene to enable terrain snapping.", MessageType.Info);
        }
    }

    // === FIX METHODS ===
    [MenuItem("Tools/Dragon Toolkit/Fixes/Fix Everything")]
    static void EmergencyFixEverything()
    {
        Debug.Log($"🚨 EMERGENCY FIX EVERYTHING FOR {PROJECT_NAME}...");
        FixURPSetup();
        FixLightingAndCamera();
        FixSceneVisibility();
        FixAllMaterials();
        CustomProjectFixes();
        Debug.Log("🚨 Emergency fixes complete - check console for results!");
    }

    // Moved to Diagnostics submenu
    static void DirectDiagnoseAllIssues()
    {
        DiagnoseAllIssues();
    }

    // === SPECIFIC FIX MENU ITEMS ===
    [MenuItem("Tools/Dragon Toolkit/Fixes/Fix URP Setup")]
    static void MenuFixURPSetup()
    {
        FixURPSetup();
    }

    [MenuItem("Tools/Dragon Toolkit/Fixes/Fix Lighting and Camera")]
    static void MenuFixLightingAndCamera()
    {
        FixLightingAndCamera();
    }

    [MenuItem("Tools/Dragon Toolkit/Fixes/Fix Scene Visibility")]
    static void MenuFixSceneVisibility()
    {
        FixSceneVisibility();
    }

    [MenuItem("Tools/Dragon Toolkit/Fixes/Fix All Materials")]
    static void MenuFixAllMaterials()
    {
        FixAllMaterials();
    }

    [MenuItem("Tools/Dragon Toolkit/Vegetation/DESTROY 30% Random Grass")]
    static void RemoveRandomGrass30()
    {
        if (EditorUtility.DisplayDialog("Confirm Grass Deletion", 
            "This will PERMANENTLY DELETE 30% of grass objects to reduce prefab file size.\n\nThis CANNOT be undone!\n\nSave a backup first?", 
            "DELETE Grass", "Cancel"))
        {
            RemoveRandomGrass(0.3f);
        }
    }
    
    // === Removed redundant quick access - all items are in proper submenus now ===

    // === URP FIXES ===
    static void FixURPSetup()
    {
        Debug.Log("🔧 Fixing URP setup...");
        
        // Set URP as the active render pipeline
        var urpAsset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(ASSET_PATHS["URPAssetPath"]);
        if (urpAsset == null)
        {
            // Find any URP asset in the project
            string[] guids = AssetDatabase.FindAssets("t:UniversalRenderPipelineAsset");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                urpAsset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(path);
                Debug.Log($"Found URP asset at: {path}");
            }
        }
        
        if (urpAsset != null)
        {
            GraphicsSettings.defaultRenderPipeline = urpAsset;
            QualitySettings.renderPipeline = urpAsset;
            Debug.Log("✅ URP setup fixed!");
        }
        else
        {
            Debug.LogWarning("⚠️ URP asset not found - check if URP is installed");
        }
    }

    static void FixLightingAndCamera()
    {
        Debug.Log("🔧 Fixing lighting and camera...");
        
        // Fix main camera
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject cameraGO = new GameObject("Main Camera");
            mainCamera = cameraGO.AddComponent<Camera>();
            cameraGO.tag = "MainCamera";
            Debug.Log("✅ Created new Main Camera");
        }
        
        mainCamera.transform.position = new Vector3(0, 1, -10);
        mainCamera.transform.rotation = Quaternion.identity;
        mainCamera.fieldOfView = 60f;
        mainCamera.backgroundColor = Color.black;
        
        // Add directional light if none exists
        Light[] lights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        bool hasDirectionalLight = false;
        foreach (Light light in lights)
        {
            if (light.type == LightType.Directional)
            {
                hasDirectionalLight = true;
                break;
            }
        }
        
        if (!hasDirectionalLight)
        {
            GameObject lightGO = new GameObject("Directional Light");
            Light light = lightGO.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.0f;
            lightGO.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            Debug.Log("✅ Created Directional Light");
        }
        
        Debug.Log("✅ Lighting and camera fixed!");
    }

    static void FixSceneVisibility()
    {
        Debug.Log("🔧 Fixing scene visibility...");
        
        // Ensure scene view camera is positioned correctly
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView != null)
        {
            sceneView.pivot = Vector3.zero;
            sceneView.rotation = Quaternion.LookRotation(new Vector3(1, -1, 1));
            sceneView.size = 10f;
            sceneView.Repaint();
        }
        
        // Find and position main object if it exists
        GameObject mainObject = GameObject.Find(MAIN_OBJECT_NAME);
        if (mainObject != null && mainObject.transform.position.y < -100)
        {
            mainObject.transform.position = Vector3.zero;
            Debug.Log($"✅ {MAIN_OBJECT_NAME} repositioned to origin");
        }
        
        Debug.Log("✅ Scene visibility fixed!");
    }

    static void FixAllMaterials()
    {
        Debug.Log("🔧 Fixing all materials for URP...");
        
        Material[] materials = Resources.FindObjectsOfTypeAll<Material>();
        int fixedCount = 0;
        
        foreach (Material material in materials)
        {
            if (material.shader.name.Contains("Standard") || 
                material.shader.name.Contains("Legacy") ||
                material.color == Color.magenta)
            {
                material.shader = Shader.Find("Universal Render Pipeline/Lit");
                fixedCount++;
            }
        }
        
        Debug.Log($"✅ Fixed {fixedCount} materials for URP compatibility!");
    }

    // === VEGETATION METHODS ===
    // === GRASS REMOVAL ===
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Grass/Remove 10% Grass")]
    static void RemoveGrass10()
    {
        RemoveVegetationType("grass", 0.1f);
    }
    
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Grass/Remove 20% Grass")]
    static void RemoveGrass20()
    {
        RemoveVegetationType("grass", 0.2f);
    }
    
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Grass/Remove 30% Grass")]
    static void RemoveGrass30()
    {
        RemoveVegetationType("grass", 0.3f);
    }
    
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Grass/Remove 50% Grass")]
    static void RemoveGrass50()
    {
        RemoveVegetationType("grass", 0.5f);
    }
    
    // === TREE REMOVAL ===
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Trees/Remove 10% Trees")]
    static void RemoveTrees10()
    {
        RemoveVegetationType("TAI_Tree_", 0.1f);
    }
    
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Trees/Remove 30% Trees")]
    static void RemoveTrees30()
    {
        RemoveVegetationType("TAI_Tree_", 0.3f);
    }
    
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Trees/Remove 50% Trees")]
    static void RemoveTrees50()
    {
        RemoveVegetationType("TAI_Tree_", 0.5f);
    }
    
    // === ROCK REMOVAL ===
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Rocks/Remove 10% Small Rocks")]
    static void RemoveSmallRocks10()
    {
        RemoveVegetationType("TAI_Small_Rock", 0.1f);
    }
    
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Rocks/Remove 30% Small Rocks")]
    static void RemoveSmallRocks30()
    {
        RemoveVegetationType("TAI_Small_Rock", 0.3f);
    }
    
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Rocks/Remove 50% Small Rocks")]
    static void RemoveSmallRocks50()
    {
        RemoveVegetationType("TAI_Small_Rock", 0.5f);
    }
    
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Rocks/Remove 10% Medium Rocks")]
    static void RemoveMediumRocks10()
    {
        RemoveVegetationType("TAI_Rock", 0.1f);
    }
    
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Rocks/Remove 30% Medium Rocks")]
    static void RemoveMediumRocks30()
    {
        RemoveVegetationType("TAI_Rock", 0.3f);
    }
    
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Rocks/Remove 10% Large Rocks")]
    static void RemoveLargeRocks10()
    {
        RemoveVegetationType("TAI_Big_Rock", 0.1f);
    }
    
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Rocks/Remove 30% Large Rocks")]
    static void RemoveLargeRocks30()
    {
        RemoveVegetationType("TAI_Big_Rock", 0.3f);
    }
    
    // === PLANT REMOVAL ===
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Plants/Remove 10% Plants")]
    static void RemovePlants10()
    {
        RemoveVegetationType("plant", 0.1f);
    }
    
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Plants/Remove 30% Plants")]
    static void RemovePlants30()
    {
        RemoveVegetationType("plant", 0.3f);
    }
    
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Plants/Remove 50% Plants")]
    static void RemovePlants50()
    {
        RemoveVegetationType("plant", 0.5f);
    }
    
    // === PALM TREE REMOVAL ===
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Palms/Remove 10% Palm Trees")]
    static void RemovePalms10()
    {
        RemoveVegetationType("palm", 0.1f);
    }
    
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Palms/Remove 30% Palm Trees")]
    static void RemovePalms30()
    {
        RemoveVegetationType("palm", 0.3f);
    }
    
    // === VINE REMOVAL ===
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Vines/Remove 10% Vines")]
    static void RemoveVines10()
    {
        RemoveVegetationType("vine", 0.1f);
    }
    
    [MenuItem("Tools/Dragon Toolkit/Vegetation/Vines/Remove 30% Vines")]
    static void RemoveVines30()
    {
        RemoveVegetationType("vine", 0.3f);
    }
    
    void ShowCustomVegetationSlider(CustomVegetationType customType, int index)
    {
        // Count current objects
        var objects = FindObjectsByType<GameObject>(FindObjectsSortMode.None)
            .Where(o => o.name.ToLower().Contains(customType.searchTerm.ToLower())).ToArray();
        
        EditorGUILayout.BeginHorizontal();
        
        // Label with count
        GUILayout.Label($"{customType.icon} {customType.name}", GUILayout.Width(140));
        GUILayout.Label($"({objects.Length})", GUILayout.Width(50));
        
        // Slider with 10% intervals
        float newPercentage = EditorGUILayout.Slider(customType.percentage, 0f, 100f, GUILayout.MinWidth(200));
        
        // Snap to 10% intervals
        newPercentage = Mathf.Round(newPercentage / 10f) * 10f;
        customType.percentage = newPercentage;
        
        // Percentage display
        GUILayout.Label($"{customType.percentage:F0}%", GUILayout.Width(40));
        
        // Quick action buttons
        if (GUILayout.Button("-10", GUILayout.Width(35)))
        {
            customType.percentage = Mathf.Max(0, customType.percentage - 10);
        }
        if (GUILayout.Button("+10", GUILayout.Width(35)))
        {
            customType.percentage = Mathf.Min(100, customType.percentage + 10);
        }
        
        // Remove button
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Remove", GUILayout.Width(60)))
        {
            if (customType.percentage > 0)
            {
                if (EditorUtility.DisplayDialog("Confirm Removal", 
                    $"Remove {customType.percentage:F0}% of {objects.Length} {customType.name} objects?\nThis cannot be undone!", 
                    "Remove", "Cancel"))
                {
                    RemoveVegetationType(customType.searchTerm, customType.percentage / 100f);
                }
            }
        }
        GUI.backgroundColor = Color.white;
        
        // Delete custom type button
        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("X", GUILayout.Width(20)))
        {
            if (EditorUtility.DisplayDialog("Delete Custom Type", 
                $"Delete the custom type '{customType.name}'?", 
                "Delete", "Cancel"))
            {
                customTypes.RemoveAt(index);
                SaveCustomTypes();
            }
        }
        GUI.backgroundColor = Color.white;
        
        EditorGUILayout.EndHorizontal();
    }
    
    void ShowVegetationSlider(string label, string searchTerm, ref float percentage)
    {
        // Count current objects
        var objects = FindObjectsByType<GameObject>(FindObjectsSortMode.None)
            .Where(o => o.name.ToLower().Contains(searchTerm.ToLower())).ToArray();
        
        EditorGUILayout.BeginHorizontal();
        
        // Label with count - wider for better visibility
        GUILayout.Label($"{label}", GUILayout.Width(140));
        GUILayout.Label($"({objects.Length})", GUILayout.Width(50));
        
        // Slider with 10% intervals
        float newPercentage = EditorGUILayout.Slider(percentage, 0f, 100f, GUILayout.MinWidth(200));
        
        // Snap to 10% intervals
        newPercentage = Mathf.Round(newPercentage / 10f) * 10f;
        percentage = newPercentage;
        
        // Percentage display
        GUILayout.Label($"{percentage:F0}%", GUILayout.Width(40));
        
        // Quick action buttons
        if (GUILayout.Button("-10", GUILayout.Width(35)))
        {
            percentage = Mathf.Max(0, percentage - 10);
        }
        if (GUILayout.Button("+10", GUILayout.Width(35)))
        {
            percentage = Mathf.Min(100, percentage + 10);
        }
        
        // Remove button
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Remove", GUILayout.Width(70)))
        {
            if (percentage > 0)
            {
                if (EditorUtility.DisplayDialog("Confirm Removal", 
                    $"Remove {percentage:F0}% of {objects.Length} {label} objects?\nThis cannot be undone!", 
                    "Remove", "Cancel"))
                {
                    RemoveVegetationType(searchTerm, percentage / 100f);
                }
            }
            else
            {
                EditorUtility.DisplayDialog("No Percentage Selected", 
                    "Please set a percentage above 0% to remove objects.", "OK");
            }
        }
        GUI.backgroundColor = Color.white;
        
        EditorGUILayout.EndHorizontal();
    }
    
    public static void RemoveVegetationType(string searchTerm, float percentage)
    {
        var objects = FindObjectsByType<GameObject>(FindObjectsSortMode.None)
            .Where(o => o.name.ToLower().Contains(searchTerm)).ToArray();
        
        if (objects.Length == 0)
        {
            Debug.Log($"No {searchTerm} objects found in scene!");
            return;
        }
        
        int toRemove = Mathf.RoundToInt(objects.Length * percentage);
        
        if (toRemove == 0)
        {
            Debug.Log($"Percentage too low - no {searchTerm} objects to remove");
            return;
        }
        
        Debug.Log($"🌿 Removing {toRemove} {searchTerm} objects ({percentage * 100:F0}% of {objects.Length})");
        
        // Randomize selection
        var randomized = objects.OrderBy(x => System.Guid.NewGuid()).ToArray();
        
        for (int i = 0; i < toRemove; i++)
        {
            if (randomized[i] != null)
            {
                Debug.Log($"Destroying {searchTerm}: {randomized[i].name}");
                DestroyImmediate(randomized[i]);
            }
        }
        
        Debug.Log($"✅ Removed {toRemove} {searchTerm} objects!");
        
        // Mark scene dirty
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }

    // === NEW VEGETATION HELPER METHODS ===
    static void RemoveAllVegetation(float percentage)
    {
        Debug.Log($"🌿 REMOVING {percentage * 100}% OF ALL VEGETATION AND ROCKS...");
        
        RemoveVegetationType("grass", percentage);
        RemoveVegetationType("plant", percentage);
        RemoveVegetationType("TAI_Tree_", percentage);
        RemoveVegetationType("palm", percentage);
        RemoveVegetationType("vine", percentage);
        RemoveVegetationType("flower", percentage);
        RemoveVegetationType("bush", percentage);
        RemoveVegetationType("TAI_Small_Rock", percentage);
        RemoveVegetationType("TAI_Rock", percentage);
        RemoveVegetationType("TAI_Big_Rock", percentage);
        
        // Also remove custom types
        foreach (var customType in customTypes)
        {
            RemoveVegetationType(customType.searchTerm, percentage);
        }
        
        Debug.Log($"✅ Removed {percentage * 100}% of all vegetation and rocks!");
    }
    
    static void OptimizeForMobile()
    {
        Debug.Log("📱 OPTIMIZING FOR MOBILE...");
        
        RemoveVegetationType("grass", 0.5f);        // Remove 50% grass
        RemoveVegetationType("plant", 0.3f);        // Remove 30% plants
        RemoveVegetationType("TAI_Tree_", 0.2f);    // Remove 20% trees
        RemoveVegetationType("TAI_Small_Rock", 0.4f); // Remove 40% small rocks
        RemoveVegetationType("flower", 0.4f);       // Remove 40% flowers
        RemoveVegetationType("vine", 0.3f);         // Remove 30% vines
        
        Debug.Log("✅ Mobile optimization complete!");
    }
    
    void ResetVegetationSliders()
    {
        grassPercentage = 20f;
        plantPercentage = 20f;
        treePercentage = 20f;
        palmTreePercentage = 20f;
        vinePercentage = 20f;
        smallRockPercentage = 20f;
        mediumRockPercentage = 20f;
        largeRockPercentage = 20f;
        bushPercentage = 20f;
        flowerPercentage = 20f;
        
        // Reset custom types
        foreach (var customType in customTypes)
        {
            customType.percentage = 20f;
        }
        
        Debug.Log("✅ Vegetation sliders reset to defaults!");
    }
    
    // === CUSTOM TYPE MANAGEMENT ===
    static void SaveCustomTypes()
    {
        // Save custom types to EditorPrefs as JSON
        string json = JsonUtility.ToJson(new SerializableList<CustomVegetationType> { items = customTypes });
        EditorPrefs.SetString("DragonToolkit_CustomVegetationTypes", json);
        Debug.Log($"💾 Saved {customTypes.Count} custom vegetation types");
    }
    
    static void LoadCustomTypes()
    {
        // Load custom types from EditorPrefs
        string json = EditorPrefs.GetString("DragonToolkit_CustomVegetationTypes", "");
        if (!string.IsNullOrEmpty(json))
        {
            try 
            {
                var loaded = JsonUtility.FromJson<SerializableList<CustomVegetationType>>(json);
                if (loaded != null && loaded.items != null)
                {
                    customTypes = loaded.items;
                    Debug.Log($"📂 Loaded {customTypes.Count} custom vegetation types");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Failed to load custom types: {e.Message}");
                customTypes = new List<CustomVegetationType>();
            }
        }
    }
    
    static void GenerateCustomTypeMenuItems()
    {
        // This would require code generation and recompilation
        // For now, we'll just log a message
        Debug.Log("🔄 Custom type menu items will be available after recompilation");
        Debug.Log("💡 TIP: Custom types are accessible through the Dragon Toolkit window");
        
        // Optional: Generate a script file with menu items for custom types
        GenerateCustomTypeScript();
    }
    
    static void GenerateCustomTypeScript()
    {
        string scriptPath = "Assets/Scripts/Editor/DragonToolkitCustomTypes.cs";
        string scriptContent = @"// Auto-generated custom vegetation type menu items
using UnityEngine;
using UnityEditor;

public static class DragonToolkitCustomTypes
{";
        
        foreach (var customType in customTypes)
        {
            string safeName = customType.name.Replace(" ", "");
            scriptContent += $@"
    [MenuItem(""Tools/Dragon Toolkit/Vegetation/Custom/{customType.name}/Remove 10%"")]
    static void Remove{safeName}10()
    {{
        UnifiedDragonToolkit.RemoveVegetationType(""{customType.searchTerm}"", 0.1f);
    }}
    
    [MenuItem(""Tools/Dragon Toolkit/Vegetation/Custom/{customType.name}/Remove 30%"")]
    static void Remove{safeName}30()
    {{
        UnifiedDragonToolkit.RemoveVegetationType(""{customType.searchTerm}"", 0.3f);
    }}
    
    [MenuItem(""Tools/Dragon Toolkit/Vegetation/Custom/{customType.name}/Remove 50%"")]
    static void Remove{safeName}50()
    {{
        UnifiedDragonToolkit.RemoveVegetationType(""{customType.searchTerm}"", 0.5f);
    }}";
        }
        
        scriptContent += @"
}";
        
        // Write the script file
        System.IO.File.WriteAllText(scriptPath, scriptContent);
        AssetDatabase.Refresh();
        
        Debug.Log($"✅ Generated custom type menu items at: {scriptPath}");
    }
    
    // Serializable wrapper for List
    [System.Serializable]
    private class SerializableList<T>
    {
        public List<T> items;
    }
    
    static void RemoveRandomGrass(float percentage)
    {
        Debug.Log($"🌱 PERMANENTLY REMOVING {percentage * 100}% OF GRASS OBJECTS FOR PREFAB OPTIMIZATION...");
        
        // Find all GameObjects with "grass" in their name (case insensitive)
        List<GameObject> grassObjects = new List<GameObject>();
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.ToLower().Contains("grass"))
            {
                grassObjects.Add(obj);
            }
        }
        
        if (grassObjects.Count == 0)
        {
            Debug.LogWarning("⚠️ No grass objects found in scene!");
            return;
        }
        
        Debug.Log($"Found {grassObjects.Count} grass objects in scene");
        
        // Calculate how many to remove
        int objectsToRemove = Mathf.RoundToInt(grassObjects.Count * percentage);
        Debug.Log($"⚠️ PERMANENTLY DESTROYING {objectsToRemove} grass objects ({percentage * 100}%) - THIS CANNOT BE UNDONE!");
        
        // Shuffle the list and remove the specified percentage
        System.Random random = new System.Random();
        var shuffledGrass = grassObjects.OrderBy(x => random.Next()).ToList();
        
        int removedCount = 0;
        long totalVertices = 0;
        
        for (int i = 0; i < objectsToRemove && i < shuffledGrass.Count; i++)
        {
            GameObject grassObj = shuffledGrass[i];
            if (grassObj != null)
            {
                // Count vertices for polygon reduction reporting
                MeshFilter[] meshFilters = grassObj.GetComponentsInChildren<MeshFilter>();
                foreach (MeshFilter mf in meshFilters)
                {
                    if (mf.sharedMesh != null)
                        totalVertices += mf.sharedMesh.vertexCount;
                }
                
                Debug.Log($"🗑️ DESTROYING grass object: {grassObj.name}");
                DestroyImmediate(grassObj);
                removedCount++;
            }
        }
        
        Debug.Log($"✅ Successfully DESTROYED {removedCount} grass objects!");
        Debug.Log($"📊 Reduced vertex count by approximately: {totalVertices}");
        Debug.Log($"📦 Prefab file size will be significantly reduced!");
        Debug.Log($"Remaining grass objects: {grassObjects.Count - removedCount}");
        
        // Mark scene as dirty for saving
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }

    // === DIAGNOSTIC METHODS ===
    [MenuItem("Tools/Dragon Toolkit/Diagnostics/Diagnose All Issues")]
    static void DiagnoseAllIssues()
    {
        Debug.Log($"🔍 DIAGNOSING ALL ISSUES FOR {PROJECT_NAME}...");
        DiagnoseRenderPipeline();
        DiagnoseCamera();
        DiagnoseLighting();
        DiagnoseMainObject();
        DiagnoseMaterials();
        DiagnoseGrassObjects();
        Debug.Log("🔍 Diagnostic complete - check console for results!");
    }

    [MenuItem("Tools/Dragon Toolkit/Diagnostics/Diagnose Render Pipeline")]
    static void DiagnoseRenderPipeline()
    {
        Debug.Log("🔍 RENDER PIPELINE DIAGNOSIS:");
        
        var currentPipeline = GraphicsSettings.defaultRenderPipeline;
        if (currentPipeline == null)
        {
            Debug.Log("❌ No render pipeline assigned - using Built-in");
        }
        else if (currentPipeline is UniversalRenderPipelineAsset)
        {
            Debug.Log("✅ URP is properly assigned");
            Debug.Log($"   URP Asset: {AssetDatabase.GetAssetPath(currentPipeline)}");
        }
        else
        {
            Debug.Log($"⚠️ Unknown pipeline: {currentPipeline.GetType().Name}");
        }
    }

    [MenuItem("Tools/Dragon Toolkit/Diagnostics/Diagnose Camera")]
    static void DiagnoseCamera()
    {
        Debug.Log("🔍 CAMERA DIAGNOSIS:");
        
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.Log("❌ No main camera found");
        }
        else
        {
            Debug.Log($"✅ Main camera found: {mainCamera.name}");
            Debug.Log($"   Position: {mainCamera.transform.position}");
            Debug.Log($"   FOV: {mainCamera.fieldOfView}");
            Debug.Log($"   Background: {mainCamera.backgroundColor}");
        }
    }

    [MenuItem("Tools/Dragon Toolkit/Diagnostics/Diagnose Lighting")]
    static void DiagnoseLighting()
    {
        Debug.Log("🔍 LIGHTING DIAGNOSIS:");
        
        Light[] lights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        Debug.Log($"Found {lights.Length} lights in scene");
        
        bool hasDirectionalLight = false;
        foreach (Light light in lights)
        {
            Debug.Log($"   {light.name}: {light.type}, Intensity: {light.intensity}");
            if (light.type == LightType.Directional) hasDirectionalLight = true;
        }
        
        if (!hasDirectionalLight)
        {
            Debug.Log("⚠️ No directional light found - scene may be dark");
        }
    }

    [MenuItem("Tools/Dragon Toolkit/Diagnostics/Diagnose Main Object")]
    static void DiagnoseMainObject()
    {
        Debug.Log($"🔍 {MAIN_OBJECT_NAME} DIAGNOSIS:");
        
        GameObject mainObject = GameObject.Find(MAIN_OBJECT_NAME);
        if (mainObject == null)
        {
            Debug.Log($"❌ {MAIN_OBJECT_NAME} not found in scene");
        }
        else
        {
            Debug.Log($"✅ {MAIN_OBJECT_NAME} found");
            Debug.Log($"   Position: {mainObject.transform.position}");
            Debug.Log($"   Active: {mainObject.activeInHierarchy}");
            
            // Check for components
            var components = mainObject.GetComponents<Component>();
            Debug.Log($"   Components: {string.Join(", ", components.Select(c => c.GetType().Name))}");
        }
    }

    [MenuItem("Tools/Dragon Toolkit/Diagnostics/Diagnose Materials")]
    static void DiagnoseMaterials()
    {
        Debug.Log("🔍 MATERIALS DIAGNOSIS:");
        
        Material[] materials = Resources.FindObjectsOfTypeAll<Material>();
        int urpCount = 0;
        int standardCount = 0;
        int magentaCount = 0;
        
        foreach (Material material in materials)
        {
            if (material.shader.name.Contains("Universal"))
                urpCount++;
            else if (material.shader.name.Contains("Standard") || material.shader.name.Contains("Legacy"))
                standardCount++;
            
            if (material.color == Color.magenta)
                magentaCount++;
        }
        
        Debug.Log($"   URP Materials: {urpCount}");
        Debug.Log($"   Non-URP Materials: {standardCount}");
        Debug.Log($"   Magenta Materials: {magentaCount}");
        
        if (standardCount > 0)
        {
            Debug.Log("⚠️ Non-URP materials found - may appear pink/magenta");
        }
    }

    [MenuItem("Tools/Dragon Toolkit/Diagnostics/Diagnose Grass Objects")]
    static void DiagnoseGrassObjects()
    {
        Debug.Log("🔍 GRASS OBJECTS DIAGNOSIS:");
        
        // Find all grass objects in the scene
        List<GameObject> grassObjects = new List<GameObject>();
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        long totalVertices = 0;
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.ToLower().Contains("grass"))
            {
                grassObjects.Add(obj);
                
                // Count vertices for polygon analysis
                MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();
                foreach (MeshFilter mf in meshFilters)
                {
                    if (mf.sharedMesh != null)
                        totalVertices += mf.sharedMesh.vertexCount;
                }
            }
        }
        
        Debug.Log($"   Total grass objects: {grassObjects.Count}");
        Debug.Log($"   Total grass vertices: {totalVertices}");
        Debug.Log($"   Estimated polygon count: {totalVertices / 3}");
        
        if (grassObjects.Count > 100)
        {
            Debug.Log("💡 SUGGESTION: Consider removing 30-50% of grass for prefab optimization");
        }
    }

    // === ORGANIZATION METHODS ===
    [MenuItem("Tools/Dragon Toolkit/Organization/Create Combat Structure")]
    static void CreateCombatStructure()
    {
        string[] combatFolders = {
            "Assets/_Project/Scripts/Combat",
            "Assets/_Project/Prefabs/Combat",
            "Assets/_Project/Materials/Combat"
        };
        
        foreach (string folder in combatFolders)
        {
            if (!AssetDatabase.IsValidFolder(folder))
            {
                System.IO.Directory.CreateDirectory(folder);
                AssetDatabase.CreateFolder(System.IO.Path.GetDirectoryName(folder), System.IO.Path.GetFileName(folder));
            }
        }
        
        AssetDatabase.Refresh();
        Debug.Log("✅ Combat structure created!");
    }

    [MenuItem("Tools/Dragon Toolkit/Organization/Create Architecture Structure")]
    static void CreateArchitectureStructure()
    {
        string[] archFolders = {
            "Assets/_Project/Models/Architecture",
            "Assets/_Project/Prefabs/Architecture",
            "Assets/_Project/Materials/Architecture"
        };
        
        foreach (string folder in archFolders)
        {
            if (!AssetDatabase.IsValidFolder(folder))
            {
                System.IO.Directory.CreateDirectory(folder);
                AssetDatabase.CreateFolder(System.IO.Path.GetDirectoryName(folder), System.IO.Path.GetFileName(folder));
            }
        }
        
        AssetDatabase.Refresh();
        Debug.Log("✅ Architecture structure created!");
    }

    [MenuItem("Tools/Dragon Toolkit/Organization/Refresh Project Colors")]
    static void RefreshProjectColors()
    {
        // This would refresh any custom folder coloring - placeholder for now
        AssetDatabase.Refresh();
        Debug.Log("✅ Project colors refreshed!");
    }

    static void CustomProjectFixes()
    {
        Debug.Log($"🎯 Running {PROJECT_NAME}-specific fixes...");
        
        // TreasureDragon specific fixes can be added here
        // Example: Position specific objects, configure dragon controller, etc.
        
        Debug.Log("✅ Custom project fixes complete!");
    }
    
    // === POLYGON COUNTER MENU ITEMS ===
    [MenuItem("Tools/Dragon Toolkit/Polygon Counter/Toggle Counter Visibility")]
    static void TogglePolygonCounter()
    {
        PolygonCounterManager.ToggleOverlay();
    }
    
    [MenuItem("Tools/Dragon Toolkit/Polygon Counter/Force Show Counter")]
    static void ForceShowPolygonCounter()
    {
        PolygonCounterManager.ForceShowOverlay();
    }

    // === TERRAIN TOOLS ===
    [MenuItem("Tools/Dragon Toolkit/Terrain/Quick Snap Selected to Terrain")]
    static void QuickSnapSelectedToTerrain()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        
        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No objects selected for terrain snapping");
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
        
        Debug.Log($"🏔️ Snapped {snappedCount}/{selectedObjects.Length} objects to terrain!");
    }
}