using UnityEngine;

public class PlayerMover : MonoBehaviour {
    private float _speed;

    public void Initialize(float speed) {
        _speed = Mathf.Max(speed, 0);
    }

    public void MoveTo(Vector3 direction) {
        transform.Translate(direction * _speed * Time.deltaTime, Space.World);
    }
}
