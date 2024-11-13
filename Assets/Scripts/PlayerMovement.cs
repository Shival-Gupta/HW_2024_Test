using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private GameConfiguration gameConfiguration;
    private GameManager gameManager;
    private PlayerInputs playerInputs;

    // New Variables
    private Vector3 movementDirection;
    public float rotationSpeed = 10f; // Speed at which the player rotates

    void Start()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Enable();

        gameManager = FindObjectOfType<GameManager>();
        gameConfiguration = FindObjectOfType<GameConfiguration>();

        if (gameManager == null)
        {
            Debug.LogError("[GameScene.Player] GameManager not found in the scene.");
            Debug.Log("Loading Start Scene...");
            SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
            return;
        }

        if (gameConfiguration == null)
        {
            Debug.LogError("[GameScene.Player] Game Configuration not found in the scene.");
            return;
        }
    }

    void OnDestroy()
    {
        playerInputs.Disable();
    }

    void Update()
    {
        if (gameConfiguration.isPlayerAlive)
        {
            CheckDead();
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        if (gameConfiguration == null) return;

        // Get player input
        Vector2 moveInput = playerInputs.Player.Move.ReadValue<Vector2>();

        // Calculate movement along X and Z axes
        float moveX = moveInput.x * gameConfiguration.PlayerData.speed * gameConfiguration.speedMuliplier * Time.deltaTime;
        float moveZ = moveInput.y * gameConfiguration.PlayerData.speed * gameConfiguration.speedMuliplier * Time.deltaTime;

        // Create the movement vector
        movementDirection = new Vector3(moveX, 0, moveZ);

        // Move the player
        transform.Translate(movementDirection, Space.World);

        // Rotate the player in the direction of movement
        RotatePlayer();
    }

    void RotatePlayer()
    {
        // Check if there is any movement input
        if (movementDirection.magnitude > 0.1f)
        {
            // Calculate the target rotation based on the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);

            // Smoothly rotate the player towards the target direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void CheckDead()
    {
        if (transform.position.y < -8f)
        {
            Debug.Log("[GameScene.Player] Player Died");
            gameConfiguration.isPlayerAlive = false;
            if (gameManager != null)
                gameManager.LoadGameOverScene();
        }
    }
}
