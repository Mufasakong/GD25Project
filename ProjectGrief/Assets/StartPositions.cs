using UnityEngine;

public class StartPositions : MonoBehaviour
{
    // A variable to store the initial local position of the object relative to its parent
    private Vector3 initialLocalPosition;
    private bool initialPositionSaved;

    // OnEnable is called when the object is enabled
    void OnEnable()
    {
        // Save the initial local position of this object when it is activated
        if (!initialPositionSaved)
        initialLocalPosition = transform.localPosition;
    }

    void OnDisable()
    {
        initialPositionSaved = true;
    }

    // Method to apply the initial local position back to the object
    public void ApplyStartPosition()
    {
        // Reset this object’s local position to its initial local position relative to the parent
        transform.localPosition = initialLocalPosition;
    }
}
