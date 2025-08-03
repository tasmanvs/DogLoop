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
    }

    private void ProcessPlayerInput()
    {
        char keyPressed = '\0';
        if (Keyboard.current.upArrowKey.wasPressedThisFrame) keyPressed = 'U';
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame) keyPressed = 'D';
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame) keyPressed = 'L';
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame) keyPressed = 'R';

        if (keyPressed == '\0') return;

        if (_playerInput.Length < obstacleWord.Length && obstacleWord[_playerInput.Length] == keyPressed)
        {
            _obstacleDisplay.OnCorrectInput(_playerInput.Length);
            _playerInput += keyPressed;

            if (_playerInput.Length == obstacleWord.Length)
            {
                SucceedObstacle();
            }
        }
        else
        {
            FailObstacle(5, "Incorrect key! Returned to previous position.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        // log the trigger event
        Debug.Log($"Obstacle triggered by: {other.name}");


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
        StartCoroutine(DelayedReset());
    }

    private void SucceedObstacle()
    {
        _playerCollider.transform.position = _targetDoor.transform.position;

        // Measure the direction from the start to the end door
        Vector2 direction = (_targetDoor.transform.position - _enteredDoor.transform.position).normalized;
        player player_instance = _playerCollider.GetComponent<player>();
        player_instance.SetLookDirection(direction);

        game_manager.instance.IncrementNextObstacleIndex();
        Debug.Log("Correct! Teleported to the other door.");
        StartCoroutine(DelayedReset());
    }

    private System.Collections.IEnumerator DelayedReset()
    {
        yield return new WaitForEndOfFrame();
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
