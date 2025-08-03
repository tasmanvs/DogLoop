using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider2D))]
public class obstacle : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private BoxCollider2D door_a;
    [SerializeField] private BoxCollider2D door_b;
    [SerializeField] private string obstacleWord = "UDLR";

    private bool _isPlayerInside;
    private string _playerInput;
    private Collider2D _playerCollider;
    private BoxCollider2D _enteredDoor;
    private BoxCollider2D _targetDoor;
    private ObstacleDisplay _obstacleDisplay;

    private void Awake()
    {
        _obstacleDisplay = GetComponentInChildren<ObstacleDisplay>(true);
        if (_obstacleDisplay == null)
        {
            Debug.LogError("ObstacleDisplay child not found on obstacle: " + gameObject.name);
        }
    }

    private void Update()
    {
        if (!_isPlayerInside) return;

        HandleObstacleInput();
    }

    private void HandleObstacleInput()
    {
        if (game_manager.instance == null)
        {
            Debug.LogError("Game manager instance is null!");
            return;
        }

        if (!game_manager.instance.CheckCorrectNextObstacle(_enteredDoor.gameObject))
        {
            FailObstacle(10, "Incorrect obstacle! Returned to previous position.");
            return;
        }

        ProcessPlayerInput();

        if (_playerInput.Length > 0)
        {
            if (!obstacleWord.StartsWith(_playerInput))
            {
                FailObstacle(5, "Incorrect string! Returned to previous position.");
                return;
            }

            if (_playerInput.Length == obstacleWord.Length)
            {
                SucceedObstacle();
            }
        }
    }

    private void ProcessPlayerInput()
    {
        if (Keyboard.current.upArrowKey.wasPressedThisFrame) _playerInput += "U";
        if (Keyboard.current.downArrowKey.wasPressedThisFrame) _playerInput += "D";
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame) _playerInput += "L";
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame) _playerInput += "R";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Vector2 playerPos = other.transform.position;
        if (door_a.OverlapPoint(playerPos))
        {
            _enteredDoor = door_a;
            _targetDoor = door_b;
        }
        else if (door_b.OverlapPoint(playerPos))
        {
            _enteredDoor = door_b;
            _targetDoor = door_a;
        }
        else
        {
            return;
        }

        StartObstacle(other);
    }

    private void StartObstacle(Collider2D playerCollider)
    {
        _playerCollider = playerCollider;
        _isPlayerInside = true;
        _playerInput = "";
        player.isFrozen = true;

        _obstacleDisplay.SetCode(obstacleWord);
        _obstacleDisplay.ShowDisplay();
        Debug.Log("Enter the obstacle word using arrow keys: " + obstacleWord);
    }

    private void FailObstacle(int deduction, string reason)
    {
        Debug.Log(reason);
        _playerCollider.GetComponent<player>()?.ReverseLastMove();
        game_manager.instance.TakeDeduction(deduction);
        ResetObstacleState();
    }

    private void SucceedObstacle()
    {
        _playerCollider.transform.position = _targetDoor.transform.position;
        game_manager.instance.IncrementNextObstacleIndex();
        Debug.Log("Correct! Teleported to the other door.");
        ResetObstacleState();
    }

    private void ResetObstacleState()
    {
        _isPlayerInside = false;
        _playerInput = "";
        _playerCollider = null;
        _enteredDoor = null;
        _targetDoor = null;
        player.isFrozen = false;
        _obstacleDisplay.HideDisplay();
        Debug.Log("Obstacle state reset.");
    }
}
