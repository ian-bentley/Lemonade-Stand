using System;
using UnityEngine;

public enum State { Stopped, Moving }

public class CustomerController : MonoBehaviour {
    [SerializeField] private float speed = 5f; // TODO settle on speed and make constant

    [SerializeField] private Rigidbody2D Rb;

    public event Action ReachedStand;
    // public static event Action LeftScreen; // TODO see if event is needed or cut it

    private Vector2 MoveDir => (TargetPos - Rb.position).normalized;
    private Vector2 TargetPos => (Vector2)Target.position;

    public Transform Target { get; set; }

    private void FixedUpdate() => Move();

    private void OnBecameInvisible() {
        Target = null;
        gameObject.SetActive(false);
    }

    public void Activate() {
        gameObject.SetActive(true);
        Rb.position = (Vector2)transform.parent.position;
    }

    private void Move() {
        if (Target != null || Rb.position != TargetPos) Rb.MovePosition(Rb.position + MoveDir * speed * Time.fixedDeltaTime);
    }
}
