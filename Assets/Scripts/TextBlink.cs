using System.Collections; // Needed for IEnumerator
using UnityEngine;
using UnityEngine.UI;

public class TextBlink : MonoBehaviour
{
    public Image imageToBlink; // Assign the Image component in the Inspector
    public float blinkInterval = 0.5f; // Time between blinks in seconds

    private bool isBlinking = true;

    void Start()
    {
        if (imageToBlink == null)
        {
            Debug.LogError("ImageToBlink is not assigned. Please assign an Image component.");
            return;
        }

        // Start the blinking effect
        StartCoroutine(BlinkImage());
    }

    IEnumerator BlinkImage()
    {
        while (isBlinking)
        {
            imageToBlink.enabled = !imageToBlink.enabled; // Toggle visibility
            yield return new WaitForSeconds(blinkInterval); // Wait for the specified time
        }
    }

    public void StopBlinking()
    {
        isBlinking = false;
        imageToBlink.enabled = true; // Ensure the image is visible when blinking stops
    }
}
