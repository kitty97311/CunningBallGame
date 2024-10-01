using UnityEngine;
using UnityEngine.UI; // or TMPro if using TextMeshPro
using System.Collections;

public class Toast : MonoBehaviour
{
    public GameObject toastPanel; // Assign your Panel here
    public Text toastText; // Assign your Text here (or TextMeshProUGUI)
    private float padding = 70f; // Padding around the text
    public static Toast Instance { get; private set; }
    private void Awake()
    {
        // Ensure this instance is the only one  
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed when loading new scenes  
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances  
        }
    }

    void Start()
    {
        // Initially hide the toast panel
        toastPanel.SetActive(false);
    }

    public void ShowToast(string message, float duration = 2f)
    {
        toastText.text = message; // Set the message
        toastPanel.SetActive(true); // Show the toast panel

        // Adjust the size of the panel based on the text
        AdjustToastSize();

        StartCoroutine(HideToastAfterDelay(duration)); // Hide after a delay
    }

    private void AdjustToastSize()
    {
        // Calculate preferred size for the text
        RectTransform textRectTransform = toastText.GetComponent<RectTransform>();
        RectTransform panelRectTransform = toastPanel.GetComponent<RectTransform>();

        // Set the size of the panel
        panelRectTransform.sizeDelta = new Vector2(toastText.preferredWidth + padding, 70);
        textRectTransform.sizeDelta = new Vector2(toastText.preferredWidth, 70);

    }

    private IEnumerator HideToastAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        toastPanel.SetActive(false); // Hide the toast panel
    }
}
