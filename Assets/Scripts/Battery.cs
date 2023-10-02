using UnityEngine;

public class Battery : MonoBehaviour {
  [SerializeField] private float powerValue = 20f;
  [SerializeField] private float lifetime = 120f;
  private Rigidbody rb;

  private void Awake() {
    rb = GetComponent<Rigidbody>();
  }

  public void Start() {
    rb.AddRelativeTorque(Random.onUnitSphere, ForceMode.VelocityChange);
  }

  public void Update() {
    lifetime -= Time.deltaTime;
    if (lifetime <= 0) {
      Destroy(gameObject);
    }
  }

  private void OnTriggerEnter(Collider other) {
    if (other.TryGetComponent<ShipController>(out ShipController ship)) {
      ship.AdjustPower(powerValue);
      Destroy(gameObject);
    }
  }
}
