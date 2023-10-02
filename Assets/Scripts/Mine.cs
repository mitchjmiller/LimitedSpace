using UnityEngine;

public class Mine : MonoBehaviour {
  [SerializeField] private GameObject explosionPrefab;
  private Rigidbody rb;

  private void Awake() {
    rb = GetComponent<Rigidbody>();
  }

  private void Start() {
    rb.AddRelativeTorque(Random.onUnitSphere, ForceMode.VelocityChange);
  }

  private void OnCollisionEnter(Collision collision) {
    if (collision.collider.TryGetComponent<ShipController>(out ShipController ship)) {
      // Instant kill
      ship.AdjustHealth(-ship.GetMaxHealth());
    }
    Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    Destroy(gameObject);
  }
}
