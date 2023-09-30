using UnityEngine;

public class Battery : MonoBehaviour {
  [SerializeField] private float powerValue = 15f;
  [SerializeField] private float lifetime = 30f;
  private Rigidbody rb;

  private void Awake() {
    rb = GetComponent<Rigidbody>();
  }

  public void Start() {
    rb.AddRelativeTorque(Random.onUnitSphere * 15f);
  }

  public void Update() {
    lifetime -= Time.deltaTime;
    if (lifetime <= 0) {
      Destroy(gameObject);
    }
  }

  private void OnTriggerEnter(Collider other) {
    if (other.TryGetComponent<ShipController>(out ShipController ship)) {
      ship.AddPower(powerValue);
      Destroy(gameObject);
    }
  }
}
