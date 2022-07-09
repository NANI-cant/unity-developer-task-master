using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAvatar))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerFighter))]
[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerWeaponContainer))]
public class PlayerBehaviour : MonoBehaviour {
    [Header("Metrics")]
    [Header("Moving")]
    [SerializeField][Min(0f)] private float _speed;

    [Header("Fighting")]
    [SerializeField] private FinishingOptions _finishing;

    public event Action<IFinishable> FinishTargetFound;
    public event Action<IFinishable> FinishTargetLost;

    private PlayerAvatar _avatar;
    private PlayerInput _input;
    private PlayerFighter _fighter;
    private PlayerMover _mover;
    private PlayerWeaponContainer _weaponContainer;

    private void Awake() {
        _avatar = GetComponent<PlayerAvatar>();
        _input = GetComponent<PlayerInput>();
        _fighter = GetComponent<PlayerFighter>();
        _mover = GetComponent<PlayerMover>();
        _weaponContainer = GetComponent<PlayerWeaponContainer>();

        _mover.Initialize(_speed);
        _fighter.Initialize(_finishing);
        _weaponContainer.SetRiffle();
    }

    private void OnEnable() {
        _fighter.FinishTargetFound += OnFinishTargetFound;
        _fighter.FinishTargetLost += OnFinishTargetLost;
        _input.Finishing += OnFinishing;
    }

    private void OnFinishing() {
        if (_fighter.HandleFinishing(out MonoBehaviour finishableBehaviour)) {
            _input.Disable();
            _avatar.Disable();
            _weaponContainer.SetSword();
            _avatar.PlayFinishing(finishableBehaviour);

            this.Invoke(() => {
                _input.Enable();
                _avatar.Enable();
                _weaponContainer.SetRiffle();
            },
            _finishing.Duration);
        }
    }

    private void OnDisable() {
        _fighter.FinishTargetFound -= OnFinishTargetFound;
        _fighter.FinishTargetLost -= OnFinishTargetLost;
        _input.Finishing -= OnFinishing;
    }

    private void Update() {
        _mover.MoveTo(_input.MoveDirection);
    }

    private void LateUpdate() {
        _avatar.VisualiseMoving(_input.MoveDirection, _input.LookDirection);
    }

    private void OnFinishTargetFound(IFinishable finishable) => FinishTargetFound?.Invoke(finishable);
    private void OnFinishTargetLost(IFinishable finishable) => FinishTargetLost?.Invoke(finishable);
}
