using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private GameConfiguration gameConfiguration;
    private GameManager gameManager;
    private PlayerInputs playerInputs;
    


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

        Vector2 moveInput = playerInputs.Player.Move.ReadValue<Vector2>();

        float moveX = moveInput.x * gameConfiguration.PlayerData.speed * Time.deltaTime;
        float moveZ = moveInput.y * gameConfiguration.PlayerData.speed * Time.deltaTime;

        transform.Translate(moveX, 0, moveZ);
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