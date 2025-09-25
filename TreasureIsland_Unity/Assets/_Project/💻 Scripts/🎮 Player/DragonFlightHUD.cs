using UnityEngine;
using UnityEngine.UI;

namespace PlasmaDragon.Player
{
    public class DragonFlightHUD : MonoBehaviour
    {
        [Header("HUD Elements")]
        [SerializeField] private Text speedText;
        [SerializeField] private Text altitudeText;
        [SerializeField] private Text boostText;
        [SerializeField] private Slider speedSlider;
        [SerializeField] private Slider altitudeSlider;
        [SerializeField] private Image boostIndicator;
        
        [Header("Colors")]
        [SerializeField] private Color normalSpeedColor = Color.white;
        [SerializeField] private Color boostSpeedColor = Color.yellow;
        [SerializeField] private Color warningColor = Color.red;
        
        private DragonFlightController flightController;
        private Canvas hudCanvas;
        
        private void Start()
        {
            flightController = FindObjectOfType<DragonFlightController>();
            
            if (hudCanvas == null)
            {
                CreateDefaultHUD();
            }
        }
        
        private void CreateDefaultHUD()
        {
            GameObject canvasObj = new GameObject("Dragon HUD Canvas");
            hudCanvas = canvasObj.AddComponent<Canvas>();
            hudCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            
            CreateSpeedDisplay();
            CreateAltitudeDisplay();
            CreateBoostIndicator();
            CreateControlsHelp();
        }
        
        private void CreateSpeedDisplay()
        {
            GameObject speedPanel = new GameObject("Speed Panel");
            speedPanel.transform.SetParent(hudCanvas.transform);
            
            RectTransform rect = speedPanel.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 0.5f);
            rect.anchorMax = new Vector2(0, 0.5f);
            rect.pivot = new Vector2(0, 0.5f);
            rect.anchoredPosition = new Vector2(20, 0);
            rect.sizeDelta = new Vector2(200, 100);
            
            GameObject textObj = new GameObject("Speed Text");
            textObj.transform.SetParent(speedPanel.transform);
            
            speedText = textObj.AddComponent<Text>();
            speedText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            speedText.fontSize = 20;
            speedText.color = normalSpeedColor;
            speedText.text = "Speed: 0";
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
        }
        
        private void CreateAltitudeDisplay()
        {
            GameObject altPanel = new GameObject("Altitude Panel");
            altPanel.transform.SetParent(hudCanvas.transform);
            
            RectTransform rect = altPanel.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 0.5f);
            rect.anchorMax = new Vector2(0, 0.5f);
            rect.pivot = new Vector2(0, 0.5f);
            rect.anchoredPosition = new Vector2(20, -40);
            rect.sizeDelta = new Vector2(200, 100);
            
            GameObject textObj = new GameObject("Altitude Text");
            textObj.transform.SetParent(altPanel.transform);
            
            altitudeText = textObj.AddComponent<Text>();
            altitudeText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            altitudeText.fontSize = 20;
            altitudeText.color = normalSpeedColor;
            altitudeText.text = "Altitude: 0m";
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
        }
        
        private void CreateBoostIndicator()
        {
            GameObject boostPanel = new GameObject("Boost Indicator");
            boostPanel.transform.SetParent(hudCanvas.transform);
            
            RectTransform rect = boostPanel.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0);
            rect.anchorMax = new Vector2(0.5f, 0);
            rect.pivot = new Vector2(0.5f, 0);
            rect.anchoredPosition = new Vector2(0, 50);
            rect.sizeDelta = new Vector2(300, 30);
            
            boostIndicator = boostPanel.AddComponent<Image>();
            boostIndicator.color = new Color(1, 1, 0, 0.3f);
            
            GameObject textObj = new GameObject("Boost Text");
            textObj.transform.SetParent(boostPanel.transform);
            
            boostText = textObj.AddComponent<Text>();
            boostText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            boostText.fontSize = 18;
            boostText.color = Color.white;
            boostText.text = "BOOST (Hold Shift)";
            boostText.alignment = TextAnchor.MiddleCenter;
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
        }
        
        private void CreateControlsHelp()
        {
            GameObject helpPanel = new GameObject("Controls Help");
            helpPanel.transform.SetParent(hudCanvas.transform);
            
            RectTransform rect = helpPanel.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(1, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(1, 1);
            rect.anchoredPosition = new Vector2(-20, -20);
            rect.sizeDelta = new Vector2(250, 150);
            
            GameObject bgObj = new GameObject("Background");
            bgObj.transform.SetParent(helpPanel.transform);
            Image bg = bgObj.AddComponent<Image>();
            bg.color = new Color(0, 0, 0, 0.5f);
            
            RectTransform bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;
            
            GameObject textObj = new GameObject("Help Text");
            textObj.transform.SetParent(helpPanel.transform);
            
            Text helpText = textObj.AddComponent<Text>();
            helpText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            helpText.fontSize = 14;
            helpText.color = Color.white;
            helpText.text = "DRAGON CONTROLS\n" +
                           "WASD - Move\n" +
                           "Space - Ascend\n" +
                           "Ctrl - Descend\n" +
                           "Shift - Boost\n" +
                           "Mouse - Look Around";
            helpText.alignment = TextAnchor.UpperLeft;
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(10, 10);
            textRect.offsetMax = new Vector2(-10, -10);
        }
        
        private void Update()
        {
            if (flightController == null)
            {
                flightController = FindObjectOfType<DragonFlightController>();
                return;
            }
            
            UpdateSpeedDisplay();
            UpdateAltitudeDisplay();
            UpdateBoostIndicator();
        }
        
        private void UpdateSpeedDisplay()
        {
            if (speedText == null) return;
            
            float speed = flightController.GetCurrentSpeed();
            bool isBoosting = flightController.IsBoosting();
            
            speedText.text = $"Speed: {speed:F1} m/s";
            speedText.color = isBoosting ? boostSpeedColor : normalSpeedColor;
            
            if (speedSlider != null)
            {
                speedSlider.value = flightController.GetSpeedPercentage();
            }
        }
        
        private void UpdateAltitudeDisplay()
        {
            if (altitudeText == null) return;
            
            float altitude = flightController.GetAltitude();
            altitudeText.text = $"Altitude: {altitude:F0}m";
            
            if (altitude < 10f)
            {
                altitudeText.color = warningColor;
            }
            else
            {
                altitudeText.color = normalSpeedColor;
            }
            
            if (altitudeSlider != null)
            {
                altitudeSlider.value = flightController.GetNormalizedAltitude();
            }
        }
        
        private void UpdateBoostIndicator()
        {
            if (boostIndicator == null) return;
            
            bool isBoosting = flightController.IsBoosting();
            
            if (isBoosting)
            {
                boostIndicator.color = new Color(1, 1, 0, 0.8f);
                if (boostText != null)
                {
                    boostText.text = "BOOSTING!";
                    boostText.color = Color.yellow;
                }
            }
            else
            {
                boostIndicator.color = new Color(1, 1, 0, 0.3f);
                if (boostText != null)
                {
                    boostText.text = "BOOST (Hold Shift)";
                    boostText.color = Color.white;
                }
            }
        }
    }
}