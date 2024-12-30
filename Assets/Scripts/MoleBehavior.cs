using UnityEngine;
using UnityEngine.InputSystem;

public class MoleBehavior : MonoBehaviour
{
    private GameManager gameManager;
    private InputSystem_Actions inputSysActions;
    private Camera mainCamera;

    private void Awake()
    {
        // Initialize the Input Actions
        inputSysActions = new InputSystem_Actions();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        // Enable the input actions and bind the click action
        inputSysActions.Gameplay.Click.performed += OnClick;
        inputSysActions.Gameplay.Enable();
    }

    private void OnDisable()
    {
        // Unbind the click action and disable the input actions
        inputSysActions.Gameplay.Click.performed -= OnClick;
        inputSysActions.Gameplay.Disable();
    }

    private void Start()
    {
        // Reference to the GameManager
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Vector2 inputPosition;

        // Determine if the input is a mouse click or a touch
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            inputPosition = Mouse.current.position.ReadValue();
        }
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            inputPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else
        {
            return;
        }

        // Cast a ray from the input position to check for hits on the mole
        Ray ray = mainCamera.ScreenPointToRay(inputPosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        // Call the GameManager to handle mole hit
        gameManager.MoleHit(gameObject);
    }
}
