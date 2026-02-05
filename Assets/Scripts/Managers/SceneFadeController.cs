using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFadeController : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Image fadePanel;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float startAlpha = 0f;
    [SerializeField] private float endAlpha = 1f;
    
    [Header("Spotlight Settings")]
    [Tooltip("The custom light script component (drag the spotlight GameObject here)")]
    [SerializeField] private MonoBehaviour lightScript;
    [Tooltip("Name of the volumetric intensity property/field in the light script")]
    [SerializeField] private string volumetricIntensityPropertyName = "volumetricIntensity";
    
    [Header("UI Objects to Disable")]
    [SerializeField] private GameObject uiObject1;
    [SerializeField] private GameObject uiObject2;
    
    [Header("Preview Settings")]
    [SerializeField] private GameObject previewObject;
    [SerializeField] private float previewDuration = 8f;
    
    [Header("Scene Settings")]
    [SerializeField] private int sceneBuildIndex = 1;
    
    private float initialVolumetricIntensity;
    private PropertyInfo volumetricIntensityProperty;
    private FieldInfo volumetricIntensityField;
    private bool isFading = false;

    private void Awake()
    {
        if (lightScript != null)
        {
            System.Type scriptType = lightScript.GetType();
            
            // Get volumetric intensity property/field
            volumetricIntensityProperty = scriptType.GetProperty(volumetricIntensityPropertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            volumetricIntensityField = scriptType.GetField(volumetricIntensityPropertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (volumetricIntensityProperty != null && volumetricIntensityProperty.CanRead)
            {
                initialVolumetricIntensity = (float)volumetricIntensityProperty.GetValue(lightScript);
            }
            else if (volumetricIntensityField != null)
            {
                initialVolumetricIntensity = (float)volumetricIntensityField.GetValue(lightScript);
            }
        }
    }

    private void Start()
    {
        // Ensure the fade panel starts with black color and starting alpha
        if (fadePanel != null)
        {
            Color panelColor = Color.black;
            panelColor.a = startAlpha;
            fadePanel.color = panelColor;
        }
    }

    // Call this when the start button is pressed
    public void StartFadeTransition()
    {
        if (!isFading)
        {
            // Disable UI objects
            if (uiObject1 != null)
            {
                uiObject1.SetActive(false);
            }
            if (uiObject2 != null)
            {
                uiObject2.SetActive(false);
            }
            
            StartCoroutine(FadeTransition());
        }
    }

    // Handles the fade effect for both panel and volumetric intensity
    private IEnumerator FadeTransition()
    {
        isFading = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;

            // Smooth fade using easing function
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            // Fade panel alpha (opacity) while keeping it black
            if (fadePanel != null)
            {
                Color panelColor = Color.black;
                panelColor.a = Mathf.Lerp(startAlpha, endAlpha, smoothT);
                fadePanel.color = panelColor;
            }

            // Fade volumetric intensity in the light script
            if (lightScript != null)
            {
                // Fade volumetric intensity
                float newVolumetricIntensity = Mathf.Lerp(initialVolumetricIntensity, 0f, smoothT);
                if (volumetricIntensityProperty != null && volumetricIntensityProperty.CanWrite)
                {
                    volumetricIntensityProperty.SetValue(lightScript, newVolumetricIntensity);
                }
                else if (volumetricIntensityField != null)
                {
                    volumetricIntensityField.SetValue(lightScript, newVolumetricIntensity);
                }
            }

            yield return null;
        }

        // Ensure final values are set
        if (fadePanel != null)
        {
            Color panelColor = Color.black;
            panelColor.a = endAlpha;
            fadePanel.color = panelColor;
        }
        
        if (lightScript != null)
        {
            // Set volumetric intensity to 0
            if (volumetricIntensityProperty != null && volumetricIntensityProperty.CanWrite)
            {
                volumetricIntensityProperty.SetValue(lightScript, 0f);
            }
            else if (volumetricIntensityField != null)
            {
                volumetricIntensityField.SetValue(lightScript, 0f);
            }
        }

        // Show preview and load the next scene
        StartCoroutine(ShowPreviewAndLoadScene());
    }

    // Show preview GameObject for set duration, then load scene
    private IEnumerator ShowPreviewAndLoadScene()
    {
        // Show preview object if assigned
        if (previewObject != null)
        {
            previewObject.SetActive(true);
            yield return new WaitForSeconds(previewDuration);
            previewObject.SetActive(false);
        }
        
        // Load the next scene
        LoadNextScene();
    }

    // Load scene by build index
    private void LoadNextScene()
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }

    // Start fade with a specific scene build index
    public void StartFadeTransitionToScene(int buildIndex)
    {
        sceneBuildIndex = buildIndex;
        StartFadeTransition();
    }
}
