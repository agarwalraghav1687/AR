using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    // "Warp" a texture by squashing its pixels to one side.
    // This involves sampling the image at non-integer pixel
    // positions to ensure a smooth effect.

    // Source image.
    public Texture2D sourceTex;

    // Amount of "warping".
    public float warpFactor = 1.0f;

    Texture2D destTex;
    Color[] destPix;


    void Start()
    {
        // Set up a new texture with the same dimensions as the original.
        destTex = new Texture2D(sourceTex.width, sourceTex.height);
        destPix = new Color[destTex.width * destTex.height];

        // For each pixel in the destination texture...
        for (var y = 0; y < destTex.height; y++)
        {
            for (var x = 0; x < destTex.width; x++)
            {
                // Calculate the fraction of the way across the image
                // that this pixel positon corresponds to.
                float xFrac = x * 1.0f / (destTex.width - 1);
                float yFrac = y * 1.0f / (destTex.height - 1);

                // Take the fractions (0..1)and raise them to a power to apply
                // the distortion.
                float warpXFrac = Mathf.Pow(xFrac, warpFactor);
                float warpYFrac = Mathf.Pow(yFrac, warpFactor);

                // Get the non-integer pixel positions using GetPixelBilinear.
                destPix[y * destTex.width + x] = sourceTex.GetPixelBilinear(warpXFrac, warpYFrac);
            }
        }

        // Copy the pixel data to the destination texture and apply the change.
        destTex.SetPixels(destPix);
        destTex.Apply();

        // Set our object's texture to the newly warped image.
        GetComponent<Renderer>().material.mainTexture = destTex;
    }
}
