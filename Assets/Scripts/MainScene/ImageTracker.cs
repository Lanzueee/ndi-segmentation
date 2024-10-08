using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTracker : MonoBehaviour
{
    public Image imageUI;
    public void CaptureImage()
    {
        // Check if the camera is busy
        if (NativeCamera.IsCameraBusy())
        {
            Debug.Log("Camera is currently busy.");
            return;
        }

        // Request permission to access the camera for taking pictures
        NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {
            if (path != null)
            {
                // Load the image as a Texture2D
                Texture2D texture = NativeCamera.LoadImageAtPath(path, -1);

                if (texture != null)
                {
                    // Save the texture as a JPEG image to the gallery
                    NativeGallery.SaveImageToGallery(texture, "MyGallery", "CapturedImage.jpg", (success, msg) =>
                    {
                        Debug.Log(success ? "Image saved to gallery!" : "Failed to save image to gallery: " + msg);
                    });

                    // Clean up the texture to free memory
                    Destroy(texture);
                }
                else
                {
                    Debug.Log("Failed to load image from path: " + path);
                }
            }
            else
            {
                Debug.Log("User cancelled the camera operation.");
            }
        });

        // Log the permission result
        Debug.Log("Camera permission result: " + permission);
    }
    public void UploadImage()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                // Load the selected image into a Texture2D and make sure it is readable
                Texture2D texture = NativeGallery.LoadImageAtPath(path, -1, false, false); // "false" ensures the texture is readable and writable

                if (texture != null)
                {
                    // Update the source of the Image UI component
                    imageUI.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                    // Save the selected image to the gallery (if needed)
                    NativeGallery.SaveImageToGallery(texture, "MyGallery", "SelectedImage.jpg", (success, msg) =>
                    {
                        Debug.Log(success ? "Image saved to gallery!" : "Failed to save image to gallery: " + msg);
                    });

                    // Optionally destroy the texture after it’s no longer needed
                    // Destroy(texture);
                }
                else
                {
                    Debug.Log("Failed to load texture from: " + path);
                }
            }
            else
            {
                Debug.Log("User canceled the gallery operation.");
            }
        });

        // Log the permission result
        Debug.Log("Gallery permission result: " + permission);
    }

}
