using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    [Header("Dev")]
    [SerializeField] private LayerMask _groundLayer;

    private Camera _camera;

    public event Action Finishing;

    private Vector3 _localMoveDirection;
    private Vector3 _lookDirection;
    private bool _isEnable;

    public Vector3 MoveDirection => _camera.transform.TransformVector(_localMoveDirection).ToXZPlane().normalized;
    public Vector3 LookDirection => _lookDirection;

    private void Awake() {
        _camera = Camera.main;
        Enable();
    }

    private void Update() {
        HandleInput();
    }

    public void Enable() => _isEnable = true;
    public void Disable() => _isEnable = false;

    private void HandleInput() {
        if (!_isEnable) {
            _localMoveDirection = Vector3.zero;
            _lookDirection = Vector3.zero;
            return;
        }

        _localMoveDirection.x = Input.GetAxisRaw("Horizontal");
        _localMoveDirection.y = Input.GetAxisRaw("Vertical");
        _localMoveDirection.Normalize();

        Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, float.MaxValue, _groundLayer);
        _lookDirection = (hitInfo.point - transform.position).ToXZPlane().normalized;

        if (Input.GetKeyDown(KeyCode.Space)) {
            Finishing?.Invoke();
        }
    }
}
