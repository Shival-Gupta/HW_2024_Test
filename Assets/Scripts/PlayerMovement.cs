using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Speed at which the player moves.")]
    public float moveSpeed = 5f;

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(moveX, 0, moveZ);
    }
}
