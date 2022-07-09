using UnityEngine;

public class FinishingMessage : MonoBehaviour {
    [Header("References")]
    [SerializeField] private PlayerBehaviour _player;

    private void Awake() {
        _player.FinishTargetFound += Show;
        _player.FinishTargetLost += Hide;
        Hide(null);
    }

    private void OnDestroy() {
        _player.FinishTargetFound -= Show;
        _player.FinishTargetLost -= Hide;
    }

    private void Show(IFinishable obj) => gameObject.SetActive(true);
    private void Hide(IFinishable obj) => gameObject.SetActive(false);
}
