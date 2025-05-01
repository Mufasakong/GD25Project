using UnityEngine;

public class Painting : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] paintingStates;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdatePainting();
        GameHandler.OnGhostsCaptured += UpdatePainting;
    }

    private void OnDisable()
    {
        GameHandler.OnGhostsCaptured -= UpdatePainting;
    }

    public void UpdatePainting()
    {
        int ghostsRemaining = GameHandler.Instance.ghostsRemaining;

        // Clamp index to prevent out-of-bounds access
        int index = Mathf.Clamp(5 - ghostsRemaining, 0, paintingStates.Length - 1);

        spriteRenderer.sprite = paintingStates[index];
    }

    public void Restore()
    {
        GameHandler.Instance.OnGhostCaptured();
    }
}
