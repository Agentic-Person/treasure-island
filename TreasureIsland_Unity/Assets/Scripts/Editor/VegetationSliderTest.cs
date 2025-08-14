using UnityEngine;
using UnityEditor;

public class VegetationSliderTest : EditorWindow
{
    private float testSlider1 = 20f;
    private float testSlider2 = 30f;
    private float testSlider3 = 50f;
    
    [MenuItem("Tools/Test Vegetation Sliders")]
    public static void ShowWindow()
    {
        var window = GetWindow<VegetationSliderTest>("Slider Test");
        window.minSize = new Vector2(600, 300);
    }
    
    void OnGUI()
    {
        GUILayout.Label("VEGETATION SLIDER TEST", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        // Test Slider 1
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Test Grass", GUILayout.Width(100));
        testSlider1 = EditorGUILayout.Slider(testSlider1, 0f, 100f);
        testSlider1 = Mathf.Round(testSlider1 / 10f) * 10f;
        GUILayout.Label($"{testSlider1:F0}%", GUILayout.Width(40));
        if (GUILayout.Button("Remove", GUILayout.Width(60)))
        {
            Debug.Log($"Would remove {testSlider1}% of grass");
        }
        EditorGUILayout.EndHorizontal();
        
        // Test Slider 2
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Test Trees", GUILayout.Width(100));
        testSlider2 = EditorGUILayout.Slider(testSlider2, 0f, 100f);
        testSlider2 = Mathf.Round(testSlider2 / 10f) * 10f;
        GUILayout.Label($"{testSlider2:F0}%", GUILayout.Width(40));
        if (GUILayout.Button("Remove", GUILayout.Width(60)))
        {
            Debug.Log($"Would remove {testSlider2}% of trees");
        }
        EditorGUILayout.EndHorizontal();
        
        // Test Slider 3
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Test Rocks", GUILayout.Width(100));
        testSlider3 = EditorGUILayout.Slider(testSlider3, 0f, 100f);
        testSlider3 = Mathf.Round(testSlider3 / 10f) * 10f;
        GUILayout.Label($"{testSlider3:F0}%", GUILayout.Width(40));
        if (GUILayout.Button("Remove", GUILayout.Width(60)))
        {
            Debug.Log($"Would remove {testSlider3}% of rocks");
        }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("If you see sliders above, they work!"))
        {
            Debug.Log("Sliders are working!");
        }
    }
}