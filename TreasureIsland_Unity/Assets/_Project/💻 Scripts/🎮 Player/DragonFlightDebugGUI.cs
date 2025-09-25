using UnityEngine;

namespace PlasmaDragon.Player
{
    [RequireComponent(typeof(DragonFlightController))]
    public class DragonFlightDebugGUI : MonoBehaviour
    {
        private DragonFlightController flightController;
        private bool showGUI = true;
        private Rect windowRect = new Rect(10, 10, 400, 600);
        
        [Header("GUI Settings")]
        public KeyCode toggleKey = KeyCode.F1;
        
        // Flight parameters to tune
        private float baseSpeed = 7f; // Constant forward speed
        private float boostSpeed = 20f; // Additional speed with W
        private float maxSpeed = 60f;
        private float yawSpeed = 90f;
        private float pitchSpeed = 60f;
        private float rollSpeed = 90f;
        private float acceleration = 10f;
        private float deceleration = 5f;
        
        // Mouse settings
        private float mouseSensitivityX = 2f;
        private float mouseSensitivityY = 2f;
        private bool invertPitch = false;
        private bool invertRoll = false;
        private bool useMouseConstraint = true;
        private float mouseConstraintSize = 300f;
        
        // Physics settings
        private float gravity = 9.8f;
        private float drag = 2f;
        private float angularDrag = 5f;
        
        // Altitude settings
        private float minAltitude = 5f;
        private float maxAltitude = 200f;
        
        // Display values
        private float currentSpeed = 0f;
        private float currentAltitude = 0f;
        private float currentPitch = 0f;
        private float currentRoll = 0f;
        private float currentYaw = 0f;
        
        private void Start()
        {
            flightController = GetComponent<DragonFlightController>();
            LoadSettings();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                showGUI = !showGUI;
            }
            
            // Update display values
            if (flightController != null)
            {
                currentSpeed = flightController.GetCurrentSpeed();
                currentAltitude = flightController.GetAltitude();
                
                Vector3 euler = transform.eulerAngles;
                currentPitch = NormalizeAngle(euler.x);
                currentRoll = NormalizeAngle(euler.z);
                currentYaw = euler.y;
            }
        }
        
        private void OnGUI()
        {
            if (!showGUI) return;
            
            GUI.skin.window.fontSize = 12;
            GUI.skin.label.fontSize = 11;
            GUI.skin.button.fontSize = 11;
            
            windowRect = GUI.Window(0, windowRect, DrawWindow, "Dragon Flight Debug (F1 to toggle)");
        }
        
        private void DrawWindow(int windowID)
        {
            GUILayout.BeginVertical();
            
            // Current status display
            GUI.color = Color.cyan;
            GUILayout.Label("=== CURRENT STATUS ===");
            GUI.color = Color.white;
            GUILayout.Label($"Speed: {currentSpeed:F1} m/s");
            GUILayout.Label($"Altitude: {currentAltitude:F1} m");
            GUILayout.Label($"Pitch: {currentPitch:F1}°");
            GUILayout.Label($"Roll: {currentRoll:F1}°");
            GUILayout.Label($"Yaw: {currentYaw:F1}°");
            
            GUILayout.Space(10);
            
            // Movement Settings
            GUI.color = Color.yellow;
            GUILayout.Label("=== MOVEMENT SETTINGS ===");
            GUI.color = Color.white;
            
            GUILayout.Label($"Base Speed (Constant): {baseSpeed:F1}");
            baseSpeed = GUILayout.HorizontalSlider(baseSpeed, 0f, 30f);
            
            GUILayout.Label($"Boost Speed (W key): {boostSpeed:F1}");
            boostSpeed = GUILayout.HorizontalSlider(boostSpeed, 10f, 50f);
            
            GUILayout.Label($"Max Speed: {maxSpeed:F1}");
            maxSpeed = GUILayout.HorizontalSlider(maxSpeed, 20f, 120f);
            
            GUILayout.Label($"Yaw Speed (A/D): {yawSpeed:F1}");
            yawSpeed = GUILayout.HorizontalSlider(yawSpeed, 30f, 200f);
            
            GUILayout.Label($"Pitch Speed (Mouse Y): {pitchSpeed:F1}");
            pitchSpeed = GUILayout.HorizontalSlider(pitchSpeed, 20f, 150f);
            
            GUILayout.Label($"Roll Speed (Mouse X): {rollSpeed:F1}");
            rollSpeed = GUILayout.HorizontalSlider(rollSpeed, 30f, 180f);
            
            GUILayout.Label($"Acceleration: {acceleration:F1}");
            acceleration = GUILayout.HorizontalSlider(acceleration, 1f, 30f);
            
            GUILayout.Label($"Deceleration: {deceleration:F1}");
            deceleration = GUILayout.HorizontalSlider(deceleration, 1f, 20f);
            
            GUILayout.Space(10);
            
            // Mouse Settings
            GUI.color = Color.green;
            GUILayout.Label("=== MOUSE SETTINGS ===");
            GUI.color = Color.white;
            
            GUILayout.Label($"Mouse Sensitivity X: {mouseSensitivityX:F2}");
            mouseSensitivityX = GUILayout.HorizontalSlider(mouseSensitivityX, 0.1f, 5f);
            
            GUILayout.Label($"Mouse Sensitivity Y: {mouseSensitivityY:F2}");
            mouseSensitivityY = GUILayout.HorizontalSlider(mouseSensitivityY, 0.1f, 5f);
            
            invertPitch = GUILayout.Toggle(invertPitch, "Invert Pitch (Mouse Y)");
            invertRoll = GUILayout.Toggle(invertRoll, "Invert Roll (Mouse X)");
            
            useMouseConstraint = GUILayout.Toggle(useMouseConstraint, "Use Mouse Constraint Box");
            
            GUILayout.Label($"Constraint Size: {mouseConstraintSize:F0} pixels");
            mouseConstraintSize = GUILayout.HorizontalSlider(mouseConstraintSize, 100f, 500f);
            
            GUILayout.Space(10);
            
            // Physics Settings
            GUI.color = Color.magenta;
            GUILayout.Label("=== PHYSICS SETTINGS ===");
            GUI.color = Color.white;
            
            GUILayout.Label($"Gravity: {gravity:F1}");
            gravity = GUILayout.HorizontalSlider(gravity, 0f, 20f);
            
            GUILayout.Label($"Drag: {drag:F1}");
            drag = GUILayout.HorizontalSlider(drag, 0f, 10f);
            
            GUILayout.Label($"Angular Drag: {angularDrag:F1}");
            angularDrag = GUILayout.HorizontalSlider(angularDrag, 0f, 10f);
            
            GUILayout.Space(10);
            
            // Altitude Settings
            GUI.color = Color.red;
            GUILayout.Label("=== ALTITUDE LIMITS ===");
            GUI.color = Color.white;
            
            GUILayout.Label($"Min Altitude: {minAltitude:F1}");
            minAltitude = GUILayout.HorizontalSlider(minAltitude, 0f, 50f);
            
            GUILayout.Label($"Max Altitude: {maxAltitude:F1}");
            maxAltitude = GUILayout.HorizontalSlider(maxAltitude, 100f, 500f);
            
            GUILayout.Space(10);
            
            // Buttons
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply Settings"))
            {
                ApplySettings();
            }
            if (GUILayout.Button("Reset Defaults"))
            {
                ResetDefaults();
            }
            if (GUILayout.Button("Save Preset"))
            {
                SaveSettings();
            }
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
            
            GUI.DragWindow();
        }
        
        private void ApplySettings()
        {
            if (flightController != null)
            {
                flightController.SetFlightParameters(
                    baseSpeed, boostSpeed, maxSpeed,
                    yawSpeed, pitchSpeed, rollSpeed,
                    acceleration, deceleration, gravity,
                    mouseSensitivityX, mouseSensitivityY,
                    invertPitch, invertRoll,
                    minAltitude, maxAltitude,
                    useMouseConstraint, mouseConstraintSize
                );
                
                // Update Rigidbody
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearDamping = drag;
                    rb.angularDamping = angularDrag;
                }
            }
        }
        
        private void ResetDefaults()
        {
            baseSpeed = 7f;
            boostSpeed = 20f;
            maxSpeed = 60f;
            yawSpeed = 90f;
            pitchSpeed = 60f;
            rollSpeed = 90f;
            acceleration = 10f;
            deceleration = 5f;
            mouseSensitivityX = 1f;
            mouseSensitivityY = 1f;
            invertPitch = false;
            invertRoll = false;
            gravity = 9.8f;
            drag = 2f;
            angularDrag = 5f;
            minAltitude = 5f;
            maxAltitude = 200f;
            useMouseConstraint = true;
            mouseConstraintSize = 300f;
            
            ApplySettings();
        }
        
        private void SaveSettings()
        {
            PlayerPrefs.SetFloat("Dragon_BaseSpeed", baseSpeed);
            PlayerPrefs.SetFloat("Dragon_BoostSpeed", boostSpeed);
            PlayerPrefs.SetFloat("Dragon_MaxSpeed", maxSpeed);
            PlayerPrefs.SetFloat("Dragon_YawSpeed", yawSpeed);
            PlayerPrefs.SetFloat("Dragon_PitchSpeed", pitchSpeed);
            PlayerPrefs.SetFloat("Dragon_RollSpeed", rollSpeed);
            PlayerPrefs.SetFloat("Dragon_Acceleration", acceleration);
            PlayerPrefs.SetFloat("Dragon_Deceleration", deceleration);
            PlayerPrefs.SetFloat("Dragon_MouseSensX", mouseSensitivityX);
            PlayerPrefs.SetFloat("Dragon_MouseSensY", mouseSensitivityY);
            PlayerPrefs.SetInt("Dragon_InvertPitch", invertPitch ? 1 : 0);
            PlayerPrefs.SetInt("Dragon_InvertRoll", invertRoll ? 1 : 0);
            PlayerPrefs.SetFloat("Dragon_Gravity", gravity);
            PlayerPrefs.SetFloat("Dragon_Drag", drag);
            PlayerPrefs.SetFloat("Dragon_AngularDrag", angularDrag);
            PlayerPrefs.SetFloat("Dragon_MinAlt", minAltitude);
            PlayerPrefs.SetFloat("Dragon_MaxAlt", maxAltitude);
            PlayerPrefs.SetInt("Dragon_UseConstraint", useMouseConstraint ? 1 : 0);
            PlayerPrefs.SetFloat("Dragon_ConstraintSize", mouseConstraintSize);
            PlayerPrefs.Save();
            
            Debug.Log("Dragon flight settings saved!");
        }
        
        private void LoadSettings()
        {
            if (PlayerPrefs.HasKey("Dragon_BaseSpeed"))
            {
                baseSpeed = PlayerPrefs.GetFloat("Dragon_BaseSpeed");
                boostSpeed = PlayerPrefs.GetFloat("Dragon_BoostSpeed", 20f);
                maxSpeed = PlayerPrefs.GetFloat("Dragon_MaxSpeed");
                yawSpeed = PlayerPrefs.GetFloat("Dragon_YawSpeed");
                pitchSpeed = PlayerPrefs.GetFloat("Dragon_PitchSpeed");
                rollSpeed = PlayerPrefs.GetFloat("Dragon_RollSpeed");
                acceleration = PlayerPrefs.GetFloat("Dragon_Acceleration");
                deceleration = PlayerPrefs.GetFloat("Dragon_Deceleration");
                mouseSensitivityX = PlayerPrefs.GetFloat("Dragon_MouseSensX");
                mouseSensitivityY = PlayerPrefs.GetFloat("Dragon_MouseSensY");
                invertPitch = PlayerPrefs.GetInt("Dragon_InvertPitch") == 1;
                invertRoll = PlayerPrefs.GetInt("Dragon_InvertRoll") == 1;
                gravity = PlayerPrefs.GetFloat("Dragon_Gravity");
                drag = PlayerPrefs.GetFloat("Dragon_Drag");
                angularDrag = PlayerPrefs.GetFloat("Dragon_AngularDrag");
                minAltitude = PlayerPrefs.GetFloat("Dragon_MinAlt");
                maxAltitude = PlayerPrefs.GetFloat("Dragon_MaxAlt");
                useMouseConstraint = PlayerPrefs.GetInt("Dragon_UseConstraint", 1) == 1;
                mouseConstraintSize = PlayerPrefs.GetFloat("Dragon_ConstraintSize", 300f);
                
                ApplySettings();
                Debug.Log("Dragon flight settings loaded!");
            }
        }
        
        private float NormalizeAngle(float angle)
        {
            if (angle > 180f) angle -= 360f;
            return angle;
        }
    }
}