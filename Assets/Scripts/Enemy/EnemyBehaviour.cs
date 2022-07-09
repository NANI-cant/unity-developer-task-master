using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyAvatar))]
public class EnemyBehaviour : MonoBehaviour, IFinishable {
    [Header("Metrics")]
    [SerializeField] private float _ragdollDelay;
    [SerializeField] private float _ragdollDuration;

    [Header("Dev")]
    [SerializeField] private Transform _body;

    public event System.Action<IFinishable> Finished;

    private EnemyAvatar _avatar;

    private void Awake() {
        _avatar = GetComponent<EnemyAvatar>();
    }

    public void Finish(MonoBehaviour killer) {
        Finished?.Invoke(this);
        Vector3 direction = (transform.position - killer.transform.position).normalized;
        _body.forward = direction;
        StartCoroutine(Die());
    }

    private IEnumerator Die() {
        yield return new WaitForSeconds(_ragdollDelay);

        _avatar.Fall();
        yield return new WaitForSeconds(_ragdollDuration);

        _avatar.StandUp();
        _body.Rotate(Vector3.up, UnityEngine.Random.Range(30f, 180f), Space.World);
        transform.position = new Vector3(Random.Range(-10f, 10f), transform.position.y, Random.Range(-10f, 10f));
    }
}