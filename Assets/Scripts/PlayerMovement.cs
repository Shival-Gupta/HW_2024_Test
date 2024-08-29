using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameConfiguration GameConfiguration;
    private PlayerInputs playerInputs;

    void Start()
    {
        GameConfiguration = FindObjectOfType<GameConfiguration>();

        if (GameConfiguration == null)
        {
            Debug.LogError("GameConfiguration not found in the scene.");
            return;
        }

        playerInputs = new PlayerInputs();
        playerInputs.Enable();
    }

    void OnDestroy()
    {
        playerInputs.Disable();
    }

    void Update()
    {
        if (GameConfiguration == null) return;

        Vector2 moveInput = playerInputs.Player.Move.ReadValue<Vector2>();
        Debug.Log(moveInput);

        float moveX = moveInput.x * GameConfiguration.PlayerData.speed * Time.deltaTime;
        float moveZ = moveInput.y * GameConfiguration.PlayerData.speed * Time.deltaTime;

        transform.Translate(moveX, 0, moveZ);
    }
}