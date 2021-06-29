using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SingleSnowBallController : MonoBehaviour, ISnowBallController {

	protected bool _isMoving = true;
	protected Vector3 _initialVelocity;
    protected BattleGameManager _battleGameManager;
    protected Transform _transform;
    protected bool _shouldDespawn;

    protected float Voy;
    protected float Vox;
    protected float alpha;
    protected Vector3 fw;

    private void Awake()
    {
        _battleGameManager = BattleGameManager.instance;
        _shouldDespawn = true;
        _transform = GetComponent<Transform>();
        _isMoving = true;
    }

    private void OnEnable()
    {
        StartCoroutine(DespawnTimer());
    }

    private void FixedUpdate() {
	    if (_isMoving) {
		    float t = Time.deltaTime;
		    float g = _battleGameManager.Gravity;

		    Vector3 currentX = new Vector3(_transform.position.x, 0.0f, _transform.position.z);
		    Vector3 fwX = new Vector3(fw.x, 0.0f, fw.z).normalized;
		    Vector3 X = currentX + (fwX * Vox * t);

		    Vector3 currentY = new Vector3(0.0f, _transform.position.y, 0.0f);
		    var Vy = Voy + g * t;
		    Vector3 Y = currentY + (Vector3.up * Vy * t) + (Vector3.up * 0.5f * g * t * t);

		    _transform.position = X + Y;

		    Voy = Vy;
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        _initialVelocity = velocity;
        Voy = (float) (Math.Sin(alpha) * velocity.magnitude);
        Vox = (float) (Math.Cos(alpha) * velocity.magnitude);
    }

    public void DontDespawn()
    {
        _shouldDespawn = false;
    }

    public bool IsMine() {
	    return true;
    }

    public void SetShootAngle(float angle) {
	    alpha = angle;
    }

    public void SetForward(Vector3 forward) {
	    fw = forward;
    }

    private void OnCollisionEnter(Collision other) {
        Despawn();
    }

    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(_battleGameManager.TimeToSpawnSnowBall);
        Despawn();
    }

    private void Despawn()
    {
	    _isMoving = false;
        if (_shouldDespawn)
        {
            //Destroy(gameObject);
        }
    }
}
