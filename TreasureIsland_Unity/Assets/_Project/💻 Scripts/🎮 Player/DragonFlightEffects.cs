using UnityEngine;

namespace PlasmaDragon.Player
{
    [RequireComponent(typeof(DragonFlightController))]
    public class DragonFlightEffects : MonoBehaviour
    {
        [Header("Trail Effects")]
        [SerializeField] private TrailRenderer[] wingTrails;
        [SerializeField] private float minTrailTime = 0.1f;
        [SerializeField] private float maxTrailTime = 1f;
        [SerializeField] private Gradient normalTrailGradient;
        [SerializeField] private Gradient boostTrailGradient;
        
        [Header("Particle Effects")]
        [SerializeField] private ParticleSystem boostParticles;
        [SerializeField] private ParticleSystem speedLinesParticles;
        [SerializeField] private float speedLinesThreshold = 0.5f;
        
        [Header("Audio")]
        [SerializeField] private AudioSource flightSound;
        [SerializeField] private AudioSource boostSound;
        [SerializeField] private float minPitch = 0.8f;
        [SerializeField] private float maxPitch = 1.3f;
        
        [Header("Camera Effects")]
        [SerializeField] private float speedFOVMultiplier = 1.2f;
        [SerializeField] private float fovChangeSpeed = 2f;
        
        private DragonFlightController flightController;
        private Camera mainCamera;
        private float originalFOV;
        
        private void Start()
        {
            flightController = GetComponent<DragonFlightController>();
            mainCamera = Camera.main;
            
            if (mainCamera != null)
            {
                originalFOV = mainCamera.fieldOfView;
            }
            
            InitializeEffects();
        }
        
        private void InitializeEffects()
        {
            if (wingTrails == null || wingTrails.Length == 0)
            {
                CreateDefaultWingTrails();
            }
            
            if (boostParticles == null)
            {
                CreateBoostParticles();
            }
            
            if (speedLinesParticles == null)
            {
                CreateSpeedLines();
            }
        }
        
        private void CreateDefaultWingTrails()
        {
            wingTrails = new TrailRenderer[2];
            
            for (int i = 0; i < 2; i++)
            {
                GameObject trailObj = new GameObject($"WingTrail_{i}");
                trailObj.transform.SetParent(transform);
                trailObj.transform.localPosition = new Vector3(i == 0 ? -2f : 2f, 0, -1f);
                
                TrailRenderer trail = trailObj.AddComponent<TrailRenderer>();
                trail.time = 0.5f;
                trail.startWidth = 0.5f;
                trail.endWidth = 0f;
                trail.material = new Material(Shader.Find("Sprites/Default"));
                
                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { 
                        new GradientColorKey(new Color(0.5f, 0.8f, 1f, 1f), 0.0f), 
                        new GradientColorKey(new Color(0.2f, 0.4f, 1f, 0f), 1.0f) 
                    },
                    new GradientAlphaKey[] { 
                        new GradientAlphaKey(1.0f, 0.0f), 
                        new GradientAlphaKey(0.0f, 1.0f) 
                    }
                );
                trail.colorGradient = gradient;
                
                wingTrails[i] = trail;
            }
        }
        
        private void CreateBoostParticles()
        {
            GameObject boostObj = new GameObject("BoostParticles");
            boostObj.transform.SetParent(transform);
            boostObj.transform.localPosition = new Vector3(0, 0, -2f);
            
            boostParticles = boostObj.AddComponent<ParticleSystem>();
            var main = boostParticles.main;
            main.startLifetime = 0.5f;
            main.startSpeed = 5f;
            main.startSize = 0.3f;
            main.startColor = new Color(1f, 0.5f, 0f, 0.8f);
            main.maxParticles = 100;
            
            var emission = boostParticles.emission;
            emission.rateOverTime = 50;
            
            var shape = boostParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 25f;
            shape.radius = 0.5f;
            
            boostParticles.Stop();
        }
        
        private void CreateSpeedLines()
        {
            GameObject speedObj = new GameObject("SpeedLines");
            speedObj.transform.SetParent(transform);
            speedObj.transform.localPosition = new Vector3(0, 0, 5f);
            
            speedLinesParticles = speedObj.AddComponent<ParticleSystem>();
            var main = speedLinesParticles.main;
            main.startLifetime = 0.2f;
            main.startSpeed = 50f;
            main.startSize = 0.1f;
            main.startColor = new Color(1f, 1f, 1f, 0.3f);
            main.maxParticles = 200;
            
            var emission = speedLinesParticles.emission;
            emission.rateOverTime = 100;
            
            var shape = speedLinesParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(10f, 10f, 0.1f);
            
            var velocityOverLifetime = speedLinesParticles.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
            velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(-50f);
            
            speedLinesParticles.Stop();
        }
        
        private void Update()
        {
            if (flightController == null) return;
            
            float speedPercent = flightController.GetSpeedPercentage();
            bool isBoosting = flightController.IsBoosting();
            
            UpdateTrailEffects(speedPercent, isBoosting);
            UpdateParticleEffects(speedPercent, isBoosting);
            UpdateAudioEffects(speedPercent, isBoosting);
            UpdateCameraEffects(speedPercent, isBoosting);
        }
        
        private void UpdateTrailEffects(float speedPercent, bool isBoosting)
        {
            if (wingTrails == null) return;
            
            foreach (var trail in wingTrails)
            {
                if (trail != null)
                {
                    trail.time = Mathf.Lerp(minTrailTime, maxTrailTime, speedPercent);
                    
                    if (isBoosting && boostTrailGradient != null)
                    {
                        trail.colorGradient = boostTrailGradient;
                    }
                    else if (normalTrailGradient != null)
                    {
                        trail.colorGradient = normalTrailGradient;
                    }
                }
            }
        }
        
        private void UpdateParticleEffects(float speedPercent, bool isBoosting)
        {
            if (boostParticles != null)
            {
                if (isBoosting && !boostParticles.isPlaying)
                {
                    boostParticles.Play();
                }
                else if (!isBoosting && boostParticles.isPlaying)
                {
                    boostParticles.Stop();
                }
            }
            
            if (speedLinesParticles != null)
            {
                if (speedPercent > speedLinesThreshold && !speedLinesParticles.isPlaying)
                {
                    speedLinesParticles.Play();
                }
                else if (speedPercent <= speedLinesThreshold && speedLinesParticles.isPlaying)
                {
                    speedLinesParticles.Stop();
                }
                
                if (speedLinesParticles.isPlaying)
                {
                    var emission = speedLinesParticles.emission;
                    emission.rateOverTime = Mathf.Lerp(50, 200, speedPercent);
                }
            }
        }
        
        private void UpdateAudioEffects(float speedPercent, bool isBoosting)
        {
            if (flightSound != null)
            {
                flightSound.pitch = Mathf.Lerp(minPitch, maxPitch, speedPercent);
                flightSound.volume = Mathf.Lerp(0.3f, 1f, speedPercent);
                
                if (!flightSound.isPlaying)
                {
                    flightSound.Play();
                }
            }
            
            if (boostSound != null)
            {
                if (isBoosting && !boostSound.isPlaying)
                {
                    boostSound.Play();
                }
                else if (!isBoosting && boostSound.isPlaying)
                {
                    boostSound.Stop();
                }
            }
        }
        
        private void UpdateCameraEffects(float speedPercent, bool isBoosting)
        {
            if (mainCamera == null) return;
            
            float targetFOV = originalFOV;
            if (isBoosting)
            {
                targetFOV = originalFOV * speedFOVMultiplier;
            }
            
            mainCamera.fieldOfView = Mathf.Lerp(
                mainCamera.fieldOfView, 
                targetFOV, 
                fovChangeSpeed * Time.deltaTime
            );
        }
    }
}