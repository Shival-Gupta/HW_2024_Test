using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameConfig gameConfig;

    void Start()
    {
        gameConfig = FindObjectOfType<GameConfig>();

        if (gameConfig == null)
        {
            Debug.LogError("GameConfig not found in the scene.");
            return;
        }
    }

    void Update()
    {
        if (gameConfig == null) return;

        float moveX = Input.GetAxis("Horizontal") * gameConfig.PlayerData.speed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * gameConfig.PlayerData.speed * Time.deltaTime;

        transform.Translate(moveX, 0, moveZ);
    }
}
