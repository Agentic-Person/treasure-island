using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FixTowerMaterials : EditorWindow
{
    private static Material taiAtlasMaterial;
    private static Shader toonShader;
    private static Shader urpLitShader;
    private static List<GameObject> affectedTowers = new List<GameObject>();
    
    [MenuItem("Tools/Fix Tower Materials")]
    public static void ShowWindow()
    {
        GetWindow<FixTowerMaterials>("Fix Tower Materials");
    }
    
    void OnGUI()
    {
        EditorGUILayout.LabelField("Tower Material Fix Tool", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox("This tool will fix gray tower materials by reassigning shaders.", MessageType.Info);
        EditorGUILayout.Space();
        
        if (GUILayout.Button("1. Analyze Tower Materials", GUILayout.Height(30)))
        {
            AnalyzeTowerMaterials();
        }
        
        EditorGUILayout.Space();
        
        if (affectedTowers.Count > 0)
        {
            EditorGUILayout.LabelField($"Found {affectedTowers.Count} towers with material issues:", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            foreach (var tower in affectedTowers)
            {
                if (tower != null)
                {
                    if (GUILayout.Button(tower.name))
                    {
                        Selection.activeGameObject = tower;
                        SceneView.FrameLastActiveSceneView();
                    }
                }
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("2. Fix with TAI Toon Shader", GUILayout.Height(30)))
            {
                FixWithToonShader();
            }
            
            if (GUILayout.Button("3. Fix with URP Lit Shader (Fallback)", GUILayout.Height(30)))
            {
                FixWithURPLitShader();
            }
        }
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Force Reimport TAI Shaders", GUILayout.Height(25)))
        {
            ReimportTAIShaders();
        }
    }
    
    static void AnalyzeTowerMaterials()
    {
        affectedTowers.Clear();
        
        // Load the TAI Atlas material
        taiAtlasMaterial = AssetDatabase.LoadAssetAtPath<Material>(
            "Assets/_Project/ImportedAssets/AssetStore_Islands/Toon Adventure Island/Models/Materials/TAI_Atlas_1A.mat"
        );
        
        // Load shaders
        toonShader = Shader.Find("Toon/TAI_CustomToon");
        urpLitShader = Shader.Find("Universal Render Pipeline/Lit");
        
        Debug.Log($"TAI Material found: {taiAtlasMaterial != null}");
        Debug.Log($"Toon Shader found: {toonShader != null}");
        Debug.Log($"URP Lit Shader found: {urpLitShader != null}");
        
        // Find all tower objects in the scene
        GameObject towersParent = GameObject.Find("Towers");
        if (towersParent == null)
        {
            Debug.LogError("Could not find 'Towers' GameObject in scene!");
            return;
        }
        
        // Get all renderers in tower children
        MeshRenderer[] renderers = towersParent.GetComponentsInChildren<MeshRenderer>(true);
        
        foreach (var renderer in renderers)
        {
            bool hasIssue = false;
            
            // Check if any material is using the TAI_Atlas_1A material but appears gray
            foreach (var mat in renderer.sharedMaterials)
            {
                if (mat != null && mat.name.Contains("TAI_Atlas_1A"))
                {
                    // Check if shader is missing or fallen back to default
                    if (mat.shader == null || mat.shader.name.Contains("Hidden/InternalErrorShader") || 
                        mat.shader.name.Contains("Sprites/Default") || 
                        !mat.shader.name.Contains("TAI"))
                    {
                        hasIssue = true;
                        Debug.Log($"Tower {renderer.gameObject.name} has shader issue: {mat.shader?.name ?? "null"}");
                    }
                }
            }
            
            if (hasIssue && !affectedTowers.Contains(renderer.gameObject))
            {
                affectedTowers.Add(renderer.gameObject);
            }
        }
        
        if (affectedTowers.Count == 0)
        {
            // Check for any gray materials even if not named TAI_Atlas_1A
            foreach (var renderer in renderers)
            {
                foreach (var mat in renderer.sharedMaterials)
                {
                    if (mat != null && (mat.color == Color.gray || mat.shader == null || 
                        mat.shader.name.Contains("Hidden/InternalErrorShader")))
                    {
                        if (!affectedTowers.Contains(renderer.gameObject))
                        {
                            affectedTowers.Add(renderer.gameObject);
                            Debug.Log($"Tower {renderer.gameObject.name} appears gray");
                        }
                    }
                }
            }
        }
        
        Debug.Log($"Found {affectedTowers.Count} towers with material issues");
    }
    
    static void FixWithToonShader()
    {
        if (taiAtlasMaterial == null)
        {
            Debug.LogError("TAI Atlas material not found!");
            return;
        }
        
        if (toonShader == null)
        {
            Debug.LogError("Toon shader not found! It may need to be recompiled.");
            return;
        }
        
        // Fix the material itself
        Undo.RecordObject(taiAtlasMaterial, "Fix TAI Material Shader");
        taiAtlasMaterial.shader = toonShader;
        EditorUtility.SetDirty(taiAtlasMaterial);
        
        // Force refresh on all affected towers
        foreach (var tower in affectedTowers)
        {
            if (tower != null)
            {
                MeshRenderer renderer = tower.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    // Force material refresh
                    Material[] mats = renderer.sharedMaterials;
                    renderer.sharedMaterials = mats;
                    EditorUtility.SetDirty(renderer);
                }
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"Applied Toon shader to {affectedTowers.Count} towers");
        EditorUtility.DisplayDialog("Success", $"Fixed {affectedTowers.Count} towers with Toon shader", "OK");
        
        // Re-analyze to update the list
        AnalyzeTowerMaterials();
    }
    
    static void FixWithURPLitShader()
    {
        if (taiAtlasMaterial == null)
        {
            Debug.LogError("TAI Atlas material not found!");
            return;
        }
        
        if (urpLitShader == null)
        {
            Debug.LogError("URP Lit shader not found!");
            return;
        }
        
        // Create a new material with URP Lit shader as fallback
        Material urpMaterial = new Material(urpLitShader);
        
        // Copy texture from TAI material
        if (taiAtlasMaterial.HasProperty("_TextureSample"))
        {
            Texture tex = taiAtlasMaterial.GetTexture("_TextureSample");
            if (tex != null)
            {
                urpMaterial.SetTexture("_BaseMap", tex);
                urpMaterial.SetTexture("_MainTex", tex);
            }
        }
        else if (taiAtlasMaterial.HasProperty("_MainTex"))
        {
            Texture tex = taiAtlasMaterial.GetTexture("_MainTex");
            if (tex != null)
            {
                urpMaterial.SetTexture("_BaseMap", tex);
                urpMaterial.SetTexture("_MainTex", tex);
            }
        }
        
        // Apply to all affected towers
        foreach (var tower in affectedTowers)
        {
            if (tower != null)
            {
                MeshRenderer renderer = tower.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    Undo.RecordObject(renderer, "Fix Tower Material");
                    
                    Material[] mats = renderer.sharedMaterials;
                    for (int i = 0; i < mats.Length; i++)
                    {
                        if (mats[i] != null && (mats[i].name.Contains("TAI_Atlas_1A") || 
                            mats[i].shader == null || 
                            mats[i].shader.name.Contains("Hidden/InternalErrorShader")))
                        {
                            mats[i] = urpMaterial;
                        }
                    }
                    renderer.sharedMaterials = mats;
                    EditorUtility.SetDirty(renderer);
                }
            }
        }
        
        // Save the new material
        AssetDatabase.CreateAsset(urpMaterial, "Assets/_Project/ImportedAssets/AssetStore_Islands/Toon Adventure Island/Models/Materials/TAI_Atlas_1A_URPFallback.mat");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"Applied URP Lit shader to {affectedTowers.Count} towers");
        EditorUtility.DisplayDialog("Success", $"Fixed {affectedTowers.Count} towers with URP Lit shader", "OK");
        
        // Re-analyze to update the list
        AnalyzeTowerMaterials();
    }
    
    static void ReimportTAIShaders()
    {
        string shaderPath = "Assets/_Project/ImportedAssets/AssetStore_Islands/Toon Adventure Island/Shaders";
        AssetDatabase.ImportAsset(shaderPath, ImportAssetOptions.ImportRecursive | ImportAssetOptions.ForceSynchronousImport);
        
        Debug.Log("Reimported TAI shaders");
        EditorUtility.DisplayDialog("Shaders Reimported", "TAI shaders have been reimported. Try the fix buttons again.", "OK");
    }
}