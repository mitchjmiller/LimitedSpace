using UnityEngine;

public class Satellite : MonoBehaviour {

  private Rigidbody rb;

  private void Awake() {
    rb = GetComponent<Rigidbody>();
    transform.LookAt(Vector3.zero, Random.onUnitSphere);
  }

  private void Start() {
    rb.AddRelativeTorque(new Vector3(0, 0, Random.Range(-1, 1)), ForceMode.VelocityChange);
  }
}
