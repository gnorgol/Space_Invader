
using System;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Bunker : MonoBehaviour
{
    public Texture2D splat;
    private Texture2D originalTexture;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            gameObject.SetActive(false);
        }
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        originalTexture = spriteRenderer.sprite.texture;

        ResetBunker();
    }

    private void ResetBunker()
    {
        CopyTexture(originalTexture);

        gameObject.SetActive(true);
    }
    private void CopyTexture(Texture2D source)
    {
        Texture2D copy = new Texture2D(source.width, source.height, source.format, false)
        {
            filterMode = source.filterMode,
            anisoLevel = source.anisoLevel,
            wrapMode = source.wrapMode
        };

        copy.SetPixels32(source.GetPixels32());
        copy.Apply();

        Sprite sprite = Sprite.Create(copy, spriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f), spriteRenderer.sprite.pixelsPerUnit);
        spriteRenderer.sprite = sprite;
    }
    public bool CheckCollision(BoxCollider2D other, Vector3 hitPoint)
    {
        Vector2 offset = other.size / 2;

        // Check the four corners of the box collider
        return Splat(hitPoint) ||
               Splat(hitPoint + (Vector3.down * offset.y)) ||
               Splat(hitPoint + (Vector3.up * offset.y)) ||
               Splat(hitPoint + (Vector3.left * offset.x)) ||
               Splat(hitPoint + (Vector3.right * offset.x));
    }
    public bool Splat(Vector3 hitPoint)
    {
        if (!CheckPoint(hitPoint, out int px, out int py))
        {
            return false;
        }

        Texture2D texture = spriteRenderer.sprite.texture;

        px -= splat.width / 2;
        py -= splat.height / 2;

        int startX = px;

        // Loop through each pixel in the splat texture
        for (int y = 0; y < splat.height; y++)
        {
            px = startX;

            for (int x = 0; x < splat.width; x++)
            {
                // Get the current pixel from the bunker texture
                Color pixel = texture.GetPixel(px, py);
                // Modify the alpha value based on the splat texture's alpha
                pixel.a *= splat.GetPixel(x, y).a;
                // Set the modified pixel back to the bunker texture
                texture.SetPixel(px, py, pixel);
                px++;
            }

            py++;
        }

        texture.Apply();
        return true;
    }

    // This method checks if a given hit point is within the bounds of the texture
    // and returns the corresponding pixel coordinates (px, py) in the texture.
    private bool CheckPoint(Vector3 hitPoint, out int px, out int py)
    {
        Vector3 localPoint = transform.InverseTransformPoint(hitPoint);

        // Adjust the local point to be relative to the bottom-left corner of the collider
        localPoint.x += boxCollider.size.x / 2;
        localPoint.y += boxCollider.size.y / 2;

        Texture2D texture = spriteRenderer.sprite.texture;

        // Convert the local point to texture coordinates
        px = (int)(localPoint.x / boxCollider.size.x * texture.width);
        py = (int)(localPoint.y / boxCollider.size.y * texture.height);

        // Check if the pixel at the calculated coordinates is not fully transparent
        return texture.GetPixel(px, py).a != 0f;
    }
}

