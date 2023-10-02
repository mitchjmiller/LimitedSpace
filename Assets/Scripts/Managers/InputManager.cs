using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour {
  public static InputManager Instance { get; private set; }
  private PlayerInputActions actions;

  public UnityEvent OnBoostPerformed = new UnityEvent();
  public UnityEvent OnBoostCancelled = new UnityEvent();

  private void Awake() {
    if (Instance != null) {
      Destroy(this.gameObject);
      return;
    }

    Instance = this;
    actions = new PlayerInputActions();
  }

  void Start() {
    actions.Player.Enable();
    actions.Player.Boost.performed += Boost_performed;
    actions.Player.Boost.canceled += Boost_canceled;
  }

  void OnDestroy() {
    actions.Player.Disable();
    actions.Player.Boost.performed -= Boost_performed;
    actions.Player.Boost.canceled -= Boost_canceled;
  }

  private void Boost_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    OnBoostPerformed.Invoke();
  }

  private void Boost_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    OnBoostCancelled.Invoke();
  }

  public Vector2 GetMovementVector() {
    Vector2 movementVector = actions.Player.Movement.ReadValue<Vector2>();
    movementVector = movementVector.magnitude > 1 ? movementVector.normalized : movementVector;
    return movementVector;
  }

  public float GetRoll() => actions.Player.Roll.ReadValue<float>();
  public bool GetAccelerate() => actions.Player.Accelerate.ReadValue<float>() > 0.5f;
  public bool GetBoost() => actions.Player.Boost.ReadValue<float>() > 0.5f;
  public bool GetShoot() => actions.Player.Shoot.ReadValue<float>() > 0.5f;
}
