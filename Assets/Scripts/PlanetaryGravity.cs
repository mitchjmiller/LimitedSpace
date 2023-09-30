using UnityEngine;

public class PlanetaryGravity : MonoBehaviour {
  [SerializeField] private Transform planet;
  [SerializeField] private float gravityForce = 9.81f;
  private Rigidbody rb;

  private void Awake() {
    rb = GetComponent<Rigidbody>();
    rb.useGravity = false;
  }

  void Update() {
    Vector3 gDir = (planet.transform.position - transform.position).normalized;
    rb.AddForce(gDir * gravityForce);
  }
}
