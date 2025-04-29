using UnityEngine;
using UnityEngine.UI;

public class SliderOverlay : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;

    [SerializeField, Range(0f, 0.5f)] private float centerThreshold = 0.05f;
    [SerializeField, Range(0f, 0.5f)] private float maxThreshold = 0.3f;

    private void Start()
    {
        ApplyGradient();
    }

    private void ApplyGradient()
    {
        int width = 256;
        Texture2D texture = new Texture2D(width, 1);
        texture.wrapMode = TextureWrapMode.Clamp;

        for (int x = 0; x < width; x++)
        {
            float t = x / (float)(width - 1);
            float distFromCenter = Mathf.Abs(t - 0.5f);

            Color color;

            if (distFromCenter <= centerThreshold)
                color = Color.green;
            else if (distFromCenter <= maxThreshold)
                color = Color.yellow;
            else
                color = Color.red;

            texture.SetPixel(x, 0, color);
        }

        texture.Apply();

        Sprite gradientSprite = Sprite.Create(texture, new Rect(0, 0, width, 1), new Vector2(0.5f, 0.5f));
        fillImage.sprite = gradientSprite;
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Horizontal;
        fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
    }
}
