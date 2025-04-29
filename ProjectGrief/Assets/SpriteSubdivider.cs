using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSubdivider : MonoBehaviour
{
    [Range(1, 100)]
    public int horizontalSubdivisions = 10;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr.sprite != null)
        {
            SubdivideSprite();
        }
    }

    void SubdivideSprite()
    {
        Sprite sprite = sr.sprite;
        Rect rect = sprite.textureRect;
        float width = rect.width;
        float height = rect.height;

        // Create vertices
        Vector2[] vertices = new Vector2[(horizontalSubdivisions + 1) * 2];
        for (int i = 0; i <= horizontalSubdivisions; i++)
        {
            float t = (float)i / horizontalSubdivisions;
            float x = Mathf.Lerp(0, width, t);

            vertices[i] = new Vector2(x, 0); // Bottom row
            vertices[i + horizontalSubdivisions + 1] = new Vector2(x, height); // Top row
        }

        // Create triangles
        ushort[] triangles = new ushort[horizontalSubdivisions * 6];
        for (int i = 0; i < horizontalSubdivisions; i++)
        {
            int bottomLeft = i;
            int bottomRight = i + 1;
            int topLeft = i + horizontalSubdivisions + 1;
            int topRight = i + horizontalSubdivisions + 2;

            int triIndex = i * 6;
            triangles[triIndex + 0] = (ushort)bottomLeft;
            triangles[triIndex + 1] = (ushort)topLeft;
            triangles[triIndex + 2] = (ushort)topRight;

            triangles[triIndex + 3] = (ushort)bottomLeft;
            triangles[triIndex + 4] = (ushort)topRight;
            triangles[triIndex + 5] = (ushort)bottomRight;
        }

        // Apply to sprite
        sprite.OverrideGeometry(vertices, triangles);
    }
}
