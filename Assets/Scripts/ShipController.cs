using UnityEngine;
using UnityEngine.Events;

public class ShipController : MonoBehaviour {
  [SerializeField] private Transform[] guns;
  [SerializeField] private GameObject pewPrefab;
  [SerializeField] private float maxVelocity = 5f;
  [SerializeField] private float maxBoostVelocity = 10f;
  [SerializeField] private float accelerateForce = 1f;
  [SerializeField] private float boostForce = 2f;
  [SerializeField] private float boostPowerCost = 1f;
  [SerializeField] private float torqueMultiplier = 0.1f;
  [SerializeField] private float shootCooldownTime = 0.1f;
  [SerializeField] private float shootPowerCost = 0.5f;
  [SerializeField] private float maxPower = 100f;

  public UnityEvent OnPowerChange = new UnityEvent();

  private Rigidbody rb;
  private Vector2 movementVector;
  private bool accelerate, boost, shoot, altShoot;
  private float collisionTimer = 0f, shootTimer = 0f;
  private float _currentPower;
  private float CurrentPower {
    get { return _currentPower; }
    set { _currentPower = Mathf.Clamp(value, 0f, maxPower); OnPowerChange.Invoke(); }
  }

  private void Awake() {
    rb = GetComponent<Rigidbody>();
  }

  private void Start() {
    rb.AddForce(transform.forward * 1f, ForceMode.VelocityChange);
    CurrentPower = maxPower;
  }

  void Update() {
    movementVector = InputManager.Instance.GetMovementVector();
    accelerate = InputManager.Instance.GetAccelerate();
    boost = InputManager.Instance.GetBoost();
    shoot = InputManager.Instance.GetShoot();
    // Debug.Log($"Movement: {movementVector}, Accelerate: {accelerate}, Boost: {boost}");

    if (shoot) Shoot();
  }

  private void FixedUpdate() {
    ReduceTimers();
    UpdateTorque();
    UpdateVelocity();
  }
  private void ReduceTimers() {
    collisionTimer = Mathf.Clamp(collisionTimer - Time.deltaTime, 0, 100);
    shootTimer = Mathf.Clamp(shootTimer - Time.deltaTime, 0, 100);
  }

  private void UpdateTorque() {
    Vector3 torqueVector = new Vector3(movementVector.y, movementVector.x, -movementVector.x * 2f).normalized;
    rb.AddRelativeTorque(torqueVector * torqueMultiplier);
  }

  private void UpdateVelocity() {
    if (boost && CurrentPower > 0) {
      CurrentPower -= boostPowerCost * Time.fixedDeltaTime;
      rb.AddForce(transform.forward * boostForce, ForceMode.Acceleration);
      rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxBoostVelocity);
    }
    else {
      rb.AddForce(transform.forward * accelerateForce, ForceMode.Acceleration);
      rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }
  }

  private void Shoot() {
    if (shootTimer > 0 || CurrentPower < shootPowerCost) return;

    CurrentPower -= shootPowerCost;
    shootTimer = shootCooldownTime;
    Transform gun = altShoot ? guns[0] : guns[1];
    altShoot = !altShoot;
    Instantiate(pewPrefab, gun.position, transform.rotation);
  }

  public void HandleCollision(Vector3 hitNormal) {
    if (collisionTimer > 0) return;

    rb.velocity = Vector3.Reflect(rb.velocity, hitNormal);
    transform.LookAt(-hitNormal, Vector3.up);
    collisionTimer = 1f;
  }

  public void AddPower(float power) => CurrentPower += power;
  public float GetCurrentPower() => CurrentPower;
  public float GetMaxPower() => maxPower;
}
