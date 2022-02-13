using UnityEngine;

// A reward billboard that floats up and disappears
[RequireComponent(typeof(Billboard))]
public class RewardBillboard : MonoBehaviour
{
    // Velocity of the billboard
    public Vector3 velocity = Vector3.up * 1.5f;
    
    // Duration (seconds) that the billboard will display
    public float duration = 2f;

    private float _endTime;
    
    private void Start()
    {
        _endTime = Time.time + duration;
    }

    private void Update()
    {
        // Move
        transform.position += velocity * Time.deltaTime;
        
        // Destroy after Duration
        if (Time.time > _endTime)
        {
            Destroy(gameObject);
        }
    }
}