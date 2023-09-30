using UnityEngine;

public class ParticlesController : MonoBehaviour {
  private ParticleSystem ps;

  private void Awake() {
    ps = GetComponent<ParticleSystem>();
  }

  public void SetStartSpeed(float speed) {
    var main = ps.main;
    main.startSpeed = speed;
  }
}
