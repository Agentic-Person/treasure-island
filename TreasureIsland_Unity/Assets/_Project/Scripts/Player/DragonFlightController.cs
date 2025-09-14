using UnityEngine;

namespace PlasmaDragon.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class DragonFlightController : MonoBehaviour
    {
        [Header("Flight Parameters")]
        [SerializeField] private float baseSpeed = 7f; // ~15 mph constant forward speed
        [SerializeField] private float boostSpeed = 20f; // Additional speed with W
        [SerializeField] private float maxSpeed = 60f;
        [SerializeField] private float yawSpeed = 90f;
        [SerializeField] private float pitchSpeed = 60f;
        [SerializeField] private float rollSpeed = 90f;
        
        [Header("Physics")]
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 5f;
        [SerializeField] private float gravity = 9.8f;
        [SerializeField] private float minAltitude = 5f;
        [SerializeField] private float maxAltitude = 200f;
        
        [Header("Mouse Settings")]
        [SerializeField] private float mouseSensitivityX = 2f;
        [SerializeField] private float mouseSensitivityY = 2f;
        [SerializeField] private bool invertPitch = false;
        [SerializeField] private bool invertRoll = false;
        [SerializeField] private bool useMouseConstraint = true;
        [SerializeField] private float mouseConstraintSize = 300f; // Pixels from center
        
        [Header("Input Smoothing")]
        [SerializeField] private float inputSmoothing = 0.15f;
        
        private Rigidbody rb;
        private float currentSpeed;
        private float currentPitch;
        private float currentRoll;
        private float currentYaw;
        private Transform modelTransform;
        
        // Input values
        private float throttleInput;
        private float yawInput;
        private float pitchInput;
        private float rollInput;
        private float verticalInput;
        
        // Mouse constraint
        private Vector2 screenCenter;
        private float constraintRadius;
        
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            
            modelTransform = transform.Find("Dragon_Model");
            if (modelTransform == null)
            {
                GameObject model = new GameObject("Dragon_Model");
                model.transform.SetParent(transform);
                model.transform.localPosition = Vector3.zero;
                modelTransform = model.transform;
            }
            
            // Initialize speed to base speed
            currentSpeed = baseSpeed;
            
            // Setup mouse constraint
            screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            // Use screen height to make a square constraint
            constraintRadius = Mathf.Min(Screen.height / 2f, mouseConstraintSize);
            
            // Lock and hide cursor
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true; // Keep visible for now so you can see the constraint
        }
        
        private void Update()
        {
            HandleInput();
            HandleRotation();
            UpdateModelVisuals();
            
            // Toggle cursor visibility with Tab
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Cursor.visible = !Cursor.visible;
            }
            
            // Escape to unlock mouse completely
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = Cursor.lockState == CursorLockMode.None ? 
                    CursorLockMode.Confined : CursorLockMode.None;
            }
        }
        
        private void FixedUpdate()
        {
            ApplyMovement();
            ClampAltitude();
        }
        
        private void HandleInput()
        {
            // Keyboard inputs
            throttleInput = Input.GetKey(KeyCode.W) ? 1f : 0f;
            yawInput = Input.GetAxis("Horizontal"); // A/D keys
            verticalInput = 0f;
            if (Input.GetKey(KeyCode.Space)) verticalInput = 1f;
            if (Input.GetKey(KeyCode.LeftControl)) verticalInput = -1f;
            
            // Get mouse position relative to screen center
            Vector2 mousePos = Input.mousePosition;
            Vector2 mouseOffset = mousePos - screenCenter;
            
            // Constrain mouse to square area if enabled
            if (useMouseConstraint)
            {
                // Clamp to square boundary
                mouseOffset.x = Mathf.Clamp(mouseOffset.x, -constraintRadius, constraintRadius);
                mouseOffset.y = Mathf.Clamp(mouseOffset.y, -constraintRadius, constraintRadius);
                
                // Optional: Actually constrain the cursor position
                Vector2 constrainedPos = screenCenter + mouseOffset;
                // Note: Unity doesn't allow direct cursor position setting easily
                // We'll use the offset for control instead
            }
            
            // Normalize mouse offset to -1 to 1 range
            float normalizedX = mouseOffset.x / constraintRadius;
            float normalizedY = mouseOffset.y / constraintRadius;
            
            // Apply sensitivity and inversion
            normalizedX *= mouseSensitivityX;
            normalizedY *= mouseSensitivityY;
            
            if (invertPitch) normalizedY = -normalizedY;
            if (invertRoll) normalizedX = -normalizedX;
            
            // Smooth the inputs
            pitchInput = Mathf.Lerp(pitchInput, normalizedY, inputSmoothing);
            rollInput = Mathf.Lerp(rollInput, normalizedX, inputSmoothing);
        }
        
        private void HandleRotation()
        {
            // Yaw (turning left/right with A/D)
            currentYaw += yawInput * yawSpeed * Time.deltaTime;
            
            // Pitch based on mouse Y position (not delta)
            float targetPitch = -pitchInput * 45f; // Max 45 degrees up/down
            currentPitch = Mathf.Lerp(currentPitch, targetPitch, Time.deltaTime * 3f);
            currentPitch = Mathf.Clamp(currentPitch, -60f, 60f);
            
            // Roll based on mouse X position
            float targetRoll = -rollInput * 45f; // Max 45 degrees roll
            
            // Additional banking when turning with A/D
            targetRoll += -yawInput * 30f;
            
            currentRoll = Mathf.Lerp(currentRoll, targetRoll, Time.deltaTime * 3f);
            currentRoll = Mathf.Clamp(currentRoll, -60f, 60f);
            
            // Apply the rotation to the transform
            transform.rotation = Quaternion.Euler(currentPitch, currentYaw, currentRoll);
        }
        
        private void UpdateModelVisuals()
        {
            if (modelTransform == null) return;
            
            // Keep the model's initial rotation (90 degrees on X for horizontal capsule)
            // Don't reset it - let it maintain its scene setup
            // modelTransform.localRotation = Quaternion.identity; // Removed to preserve scene rotation
        }
        
        private void ApplyMovement()
        {
            // Always have base speed, W adds boost
            float targetSpeed = baseSpeed + (throttleInput * boostSpeed);
            
            // Smooth speed changes
            if (targetSpeed > currentSpeed)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
            }
            else
            {
                currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, deceleration * Time.fixedDeltaTime);
            }
            
            // Clamp to max speed
            currentSpeed = Mathf.Clamp(currentSpeed, baseSpeed, maxSpeed);
            
            // Calculate velocity - always moving forward
            Vector3 forwardVelocity = transform.forward * currentSpeed;
            
            // Add vertical movement (Space/Ctrl)
            Vector3 verticalVelocity = Vector3.up * verticalInput * 15f;
            
            // Apply gravity when not actively ascending/descending
            if (verticalInput == 0 && transform.position.y > minAltitude)
            {
                verticalVelocity.y -= gravity * Time.fixedDeltaTime;
            }
            
            // Pitch affects vertical movement when moving
            float pitchInfluence = Mathf.Sin(currentPitch * Mathf.Deg2Rad) * currentSpeed * 0.5f;
            verticalVelocity.y += pitchInfluence;
            
            // Combine velocities
            Vector3 finalVelocity = forwardVelocity + verticalVelocity;
            rb.linearVelocity = finalVelocity;
        }
        
        private void ClampAltitude()
        {
            Vector3 position = transform.position;
            
            if (position.y < minAltitude)
            {
                position.y = minAltitude;
                transform.position = position;
                
                if (rb.linearVelocity.y < 0)
                {
                    Vector3 velocity = rb.linearVelocity;
                    velocity.y = Mathf.Max(0, velocity.y);
                    rb.linearVelocity = velocity;
                }
            }
            else if (position.y > maxAltitude)
            {
                position.y = maxAltitude;
                transform.position = position;
                
                if (rb.linearVelocity.y > 0)
                {
                    Vector3 velocity = rb.linearVelocity;
                    velocity.y = 0;
                    rb.linearVelocity = velocity;
                }
            }
        }
        
        private void OnGUI()
        {
            if (useMouseConstraint && Cursor.visible)
            {
                // Draw constraint boundary visualization
                float left = screenCenter.x - constraintRadius;
                float right = screenCenter.x + constraintRadius;
                float top = screenCenter.y - constraintRadius;
                float bottom = screenCenter.y + constraintRadius;
                
                // Convert to GUI coordinates (Y is inverted)
                top = Screen.height - top;
                bottom = Screen.height - bottom;
                
                // Draw border lines
                GUI.color = new Color(0, 1, 0, 0.3f);
                GUI.DrawTexture(new Rect(left - 2, top, 4, bottom - top), Texture2D.whiteTexture);
                GUI.DrawTexture(new Rect(right - 2, top, 4, bottom - top), Texture2D.whiteTexture);
                GUI.DrawTexture(new Rect(left, top - 2, right - left, 4), Texture2D.whiteTexture);
                GUI.DrawTexture(new Rect(left, bottom - 2, right - left, 4), Texture2D.whiteTexture);
                
                // Draw center crosshair
                GUI.color = new Color(1, 1, 0, 0.5f);
                GUI.DrawTexture(new Rect(screenCenter.x - 20, Screen.height - screenCenter.y - 1, 40, 2), Texture2D.whiteTexture);
                GUI.DrawTexture(new Rect(screenCenter.x - 1, Screen.height - screenCenter.y - 20, 2, 40), Texture2D.whiteTexture);
                
                GUI.color = Color.white;
            }
        }
        
        // Public methods for other scripts
        public float GetCurrentSpeed() => currentSpeed;
        public float GetMaxSpeed() => maxSpeed;
        public float GetSpeedPercentage() => currentSpeed / maxSpeed;
        public float GetAltitude() => transform.position.y;
        public float GetNormalizedAltitude() => (transform.position.y - minAltitude) / (maxAltitude - minAltitude);
        public bool IsBoosting() => throttleInput > 0.5f;
        public float GetBaseSpeed() => baseSpeed;
        
        // Method to update parameters from Debug GUI
        public void SetFlightParameters(
            float _baseSpeed, float _boostSpeed, float _maxSpeed, 
            float _yawSpeed, float _pitchSpeed, float _rollSpeed,
            float _acceleration, float _deceleration, float _gravity,
            float _mouseSensX, float _mouseSensY, bool _invertPitch, bool _invertRoll,
            float _minAlt, float _maxAlt, bool _useConstraint, float _constraintSize)
        {
            baseSpeed = _baseSpeed;
            boostSpeed = _boostSpeed;
            maxSpeed = _maxSpeed;
            yawSpeed = _yawSpeed;
            pitchSpeed = _pitchSpeed;
            rollSpeed = _rollSpeed;
            acceleration = _acceleration;
            deceleration = _deceleration;
            gravity = _gravity;
            mouseSensitivityX = _mouseSensX;
            mouseSensitivityY = _mouseSensY;
            invertPitch = _invertPitch;
            invertRoll = _invertRoll;
            minAltitude = _minAlt;
            maxAltitude = _maxAlt;
            useMouseConstraint = _useConstraint;
            mouseConstraintSize = _constraintSize;
            
            // Update constraint radius
            constraintRadius = Mathf.Min(Screen.height / 2f, mouseConstraintSize);
        }
    }
}