using System;
using TMPro;
using UnityEngine;

public enum State { Stopped, Moving }

public class CustomerController : MonoBehaviour {
    [SerializeField] private float speed = 5f; // TODO settle on speed and make constant

    [SerializeField] private Rigidbody2D Rb;
    [SerializeField] private BoxCollider2D Collider;
    [SerializeField] private Animator Animator;
    [SerializeField] private TextMeshProUGUI PosText;
    [SerializeField] private TextMeshProUGUI NodePosText;

    public event Action ReachedStand;

    private Vector2 MoveDir => new Vector2(Mathf.Clamp((TargetPos - Rb.position).x, -1f, 1f), Mathf.Clamp((TargetPos - Rb.position).y, -1f, 1f));
    private Vector2 TargetPos => (Vector2)Target.position;

    private Transform _target;

    public Transform Target {
        get => _target;
        set {
            _target = value;
        }
    }

    public Transform ExitNode { get; set; }
    public Transform LeaveNode { get; set; }

    private void FixedUpdate() => Move();

    private void OnTriggerEnter2D(Collider2D collision) {
        ReachedStand?.Invoke();
    }

    public void OnCustomerLeave() => DisableCollision();

    public void DisableCollision() => Collider.enabled = false;
    public void EnableCollision() => Collider.enabled = true;

    public void Activate() {
        gameObject.SetActive(true);
        Rb.position = transform.parent.position;
    }

    public void Deactivate() {
        gameObject.SetActive(false);
        Target = null;
    }

    public void Exit() => Target = ExitNode;

    public void Leave() => Target = LeaveNode;

    private void Move() {
        if (Target == null) return;

        Vector2 DistanceVector = MoveDir.normalized * speed * Time.deltaTime;
        Vector2 DistanceToTarget = TargetPos - Rb.position;
        bool TargetTooClose = DistanceToTarget.sqrMagnitude <= DistanceVector.sqrMagnitude;

        if (TargetTooClose) {
            Animator.SetInteger("MoveX", 0);
            Rb.MovePosition(TargetPos);
            PosText.text = $"{(Vector2)transform.position}";
            NodePosText.color = Color.red;

            if (Target == LeaveNode || Target == ExitNode) Deactivate();

            Target = null;
            return;
        }

        Animator.SetInteger("MoveX", (int)MoveDir.x);
        Rb.MovePosition(Rb.position + DistanceVector);
        PosText.text = $"{(Vector2)transform.position}";
        NodePosText.color = Color.green;
        NodePosText.text = $"{TargetPos}";
    }
}
