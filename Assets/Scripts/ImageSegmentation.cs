using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Make sure to include this namespace

public class ImageSegmentation : MonoBehaviour
{
    public Image inputImage;  // UI Image object
    public NNModel segmentationModel;
    private IWorker worker;
    public TextMeshProUGUI distanceText;  // UI TextMeshProUGUI object to display distance
    public GameObject getMainScene, main, resultsPanel;

    void Start()
    {
        // Load the Barracuda model and prepare the worker
        var model = ModelLoader.Load(segmentationModel);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Compute, model);

    }
    public void ApplyMask()
    {
        // Get the sprite's texture directly
        Sprite sprite = inputImage.sprite;
        if (sprite == null)
        {
            Debug.LogError("Sprite is null.");
            return;
        }

        Texture2D inputTexture = sprite.texture; // Directly use the sprite's texture
        Debug.Log($"Input Texture Size: {inputTexture.width} x {inputTexture.height}");

        // Run the segmentation model on the image texture
        var input = new Tensor(inputTexture, 3); // Assuming 3 channels (RGB)
        worker.Execute(input);

        // Get the output tensor from the model
        Tensor outputTensor = worker.PeekOutput(); // Adjust based on model specifics

        // Create the mask from the segmentation output
        CreateRedMask(outputTensor, inputTexture);

        // Dispose the input tensor
        input.Dispose();
    }

    void CreateRedMask(Tensor outputTensor, Texture2D inputTexture)
    {
        // Log the shape of the output tensor for debugging
        Debug.Log($"Output Tensor Shape: {outputTensor.shape}");

        // Create a new Texture2D for the masked output
        Texture2D maskedTexture = new Texture2D(inputTexture.width, inputTexture.height);

        // Get the dimensions of the output tensor
        int height = outputTensor.height;
        int width = outputTensor.width;

        // Loop through the output tensor and set pixels for the mask
        Color[] pixels = new Color[maskedTexture.width * maskedTexture.height];
        int topY = -1; // To track the top of the person's mask
        int bottomY = -1; // To track the bottom of the person's mask

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Access the output tensor correctly with inverted y-coordinate for masking
                float maskValue = outputTensor[0, height - y - 1, x, 0]; // Inverted y-coordinate only for mask

                if (maskValue > 0.5f) // Threshold to determine if pixel is part of the human
                {
                    pixels[y * maskedTexture.width + x] = Color.red; // Apply red color
                    if (topY == -1) topY = y; // Set top if not already set
                    bottomY = y; // Update the bottom every time a mask pixel is found
                }
                else
                {
                    pixels[y * maskedTexture.width + x] = inputTexture.GetPixel(x, y); // Keep original
                }
            }
        }

        // Apply the pixels to the new masked texture
        maskedTexture.SetPixels(pixels);
        maskedTexture.Apply();

        // Assign the masked texture to the Image component
        inputImage.sprite = Sprite.Create(maskedTexture, new Rect(0, 0, maskedTexture.width, maskedTexture.height), new Vector2(0.5f, 0.5f));
        // Removed inputImage.SetNativeSize(); 

        // Calculate the height in pixels
        if (topY != -1 && bottomY != -1)
        {
            int personHeightInPixels = bottomY - topY + 1; // Calculate height in pixels
            Debug.Log($"Top Y: {topY}, Bottom Y: {bottomY}, Person Height in Pixels: {personHeightInPixels}");

            // Update this to reflect the correct known height
            float knownHeightInPixels = 206f; // The height of the texture; adjust as needed
            float realHeightFeet = 5.25f; // Your actual height (5'3")

            // Calculate the height of the person in feet based on the detected height in pixels
            float personHeightInFeet = (personHeightInPixels / knownHeightInPixels) * realHeightFeet;

            // Display the calculated height
            distanceText.text = $"Estimated Height: {personHeightInFeet:F2} ft";
            resultsPanel.SetActive(true);
            main.SetActive(false);

            // Check if the person is approximately 5 feet away
            if (personHeightInFeet >= 5.0f)
            {
                distanceText.text += "\nThe person is approximately 5 feet away.";
            }
            else
            {
                distanceText.text += "\nThe person is not approximately 5 feet away.";
            }
        }
        else
        {
            distanceText.text = "No person detected.";
            Debug.Log("No person detected in the mask.");
        }
    }
    public void GoBack() 
    {
        resultsPanel.SetActive(false);
        main.SetActive(true);

    }
    private void OnDestroy()
    {
        worker.Dispose(); // Clean up the worker
    }
}
