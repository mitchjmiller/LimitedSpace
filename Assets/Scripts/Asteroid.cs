using UnityEngine;

public class Asteroid : MonoBehaviour {
  [SerializeField] private float collisionDamage = 10f;
  private Rigidbody rb;

  private void Awake() {
    rb = GetComponent<Rigidbody>();
  }

  private void Start() {
    rb.AddRelativeTorque(Random.onUnitSphere, ForceMode.VelocityChange);
  }

  private void OnCollisionEnter(Collision collision) {
    if (collision.collider.TryGetComponent<ShipController>(out ShipController ship)) {
      ship.AdjustHealth(-collisionDamage);
    }
  }
}
