using UnityEngine;
using UnityEngine.Events;

public class ShipController : MonoBehaviour {
  [SerializeField] private Transform[] guns;
  [SerializeField] private GameObject pewPrefab;
  [SerializeField] private GameObject explosionPrefab;
  [SerializeField] private float maxVelocity = 5f;
  [SerializeField] private float maxBoostVelocity = 10f;
  [SerializeField] private float accelerateForce = 1f;
  [SerializeField] private float boostForce = 2f;
  [SerializeField] private float boostPowerCost = 1f;
  [SerializeField] private float torqueMultiplier = 20f;
  [SerializeField] private float rollTorqueMultiplier = 30f;
  [SerializeField] private float shootCooldownTime = 0.1f;
  [SerializeField] private float shootPowerCost = 0.5f;
  [SerializeField] private float maxHealth = 100f;
  [SerializeField] private float maxPower = 100f;
  [SerializeField] private float atmosphereDamage = 5f;

  public UnityEvent OnHealthChange = new UnityEvent();
  public UnityEvent OnPowerChange = new UnityEvent();
  public UnityEvent OnShoot = new UnityEvent();
  public UnityEvent OnChargeUp = new UnityEvent();
  public UnityEvent OnBoostStart = new UnityEvent();
  public UnityEvent OnBoostEnd = new UnityEvent();
  public UnityEvent OnForceFieldCollision = new UnityEvent();

  private Rigidbody rb;
  private Vector2 movementVector;
  private float roll;
  private bool accelerate, boost, shoot, altShoot, inAtmosphere;
  private float collisionTimer = 0f, shootTimer = 0f;

  private float _currentHealth;
  private float CurrentHealth {
    get { return _currentHealth; }
    set { _currentHealth = Mathf.Clamp(value, 0f, maxHealth); OnHealthChange.Invoke(); }
  }

  private float _currentPower;
  private float CurrentPower {
    get { return _currentPower; }
    set { _currentPower = Mathf.Clamp(value, 0f, maxPower); OnPowerChange.Invoke(); }
  }

  private GameObject pewParent;

  private void Awake() {
    rb = GetComponent<Rigidbody>();
    pewParent = new GameObject("Pews");
  }

  private void Start() {
    rb.AddForce(transform.forward * 1f, ForceMode.VelocityChange);
    boost = false;
    _currentHealth = maxHealth;
    _currentPower = maxPower;
  }


  void Update() {
    movementVector = InputManager.Instance.GetMovementVector();
    roll = InputManager.Instance.GetRoll();
    accelerate = InputManager.Instance.GetAccelerate();
    shoot = InputManager.Instance.GetShoot();
    // Debug.Log($"Movement: {movementVector}, Accelerate: {accelerate}, Boost: {boost}");

    if (shoot) Shoot();
    CheckBoost();
    CheckAtmosphere();
  }

  private void FixedUpdate() {
    ReduceTimers();
    UpdateTorque();
    UpdateVelocity();
  }

  private void OnTriggerEnter(Collider other) {
    if (other.CompareTag("DeadZone")) {
      AdjustHealth(-maxHealth);
    }
    if (other.CompareTag("Atmosphere")) {
      inAtmosphere = true;
    }
  }

  private void OnTriggerExit(Collider other) {
    if (other.CompareTag("Atmosphere")) {
      inAtmosphere = false;
    }
  }

  private void CheckBoost() {
    if (!boost && InputManager.Instance.GetBoost() && CurrentPower > 0) {
      boost = true;
      OnBoostStart.Invoke();
    }
    if (boost && (!InputManager.Instance.GetBoost() || CurrentPower <= 0)) {
      boost = false;
      OnBoostEnd.Invoke();
    }
  }

  private void CheckAtmosphere() {
    if (inAtmosphere) AdjustHealth(-Time.deltaTime * atmosphereDamage);
  }

  private void ReduceTimers() {
    collisionTimer = Mathf.Clamp(collisionTimer - Time.deltaTime, 0, 100);
    shootTimer = Mathf.Clamp(shootTimer - Time.deltaTime, 0, 100);
  }

  private void UpdateTorque() {
    Vector3 torqueVector = new Vector3(movementVector.y, movementVector.x, -movementVector.x * 2f).normalized;
    rb.AddRelativeTorque(torqueVector * torqueMultiplier);
    rb.AddRelativeTorque(new Vector3(0, 0, -roll * rollTorqueMultiplier));
  }

  private void UpdateVelocity() {
    if (boost) {
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
    Instantiate(pewPrefab, gun.position, transform.rotation, pewParent.transform);
    OnShoot.Invoke();
  }

  public void HandleCollision(Vector3 hitNormal) {
    if (collisionTimer > 0) return;

    AdjustHealth(-10f);
    rb.velocity = Vector3.Reflect(rb.velocity, hitNormal);
    transform.LookAt(-hitNormal, Vector3.up);
    collisionTimer = 1f;
    OnForceFieldCollision.Invoke();
  }

  public float GetCurrentHealth() => CurrentHealth;
  public float GetMaxHealth() => maxHealth;
  public void AdjustHealth(float health) {
    if (health > 0) OnChargeUp.Invoke();
    CurrentHealth += health;

    if (CurrentHealth <= 0) {
      Instantiate(explosionPrefab, transform.position, Quaternion.identity);
      GameManager.Instance.GameOver();
      gameObject.SetActive(false);
    }
  }

  public float GetCurrentPower() => CurrentPower;
  public float GetMaxPower() => maxPower;
  public void AdjustPower(float power) {
    if (power > 0) OnChargeUp.Invoke();
    CurrentPower += power;
  }
}
