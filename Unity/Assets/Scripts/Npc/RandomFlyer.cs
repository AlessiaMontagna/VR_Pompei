using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(OcclusionInteract))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]

public class RandomFlyer : MonoBehaviour
{
    [SerializeField] Transform _homeTarget, _flyingTarget;
    [SerializeField] public bool returnToBase = false;
    [SerializeField] public float randomBaseOffset = 5f, delayStart = 0f;

    private Animator _animator;
    private Rigidbody _body;
    private OcclusionInteract _fmodAudioSource;
    private LayerMask _mask;

    private float _idleSpeed = 10f, _turnSpeed = 80f, _switchSeconds = 3f, _idleRatio = 0.3f, changeTarget = 0f, changeAnim = 0f, timeSinceTarget = 0f, timeSinceAnim = 0f, prevAnim, currentAnim = 0f;
    private float prevSpeed, speed, zturn, prevz, _turnSpeedBackup, distanceFromBase, distanceFromTarget;
    private Vector2 _animSpeedMinMax = new Vector2(0.5f, 2f), _moveSpeedMinMax = new Vector2(10f, 20f), _changeAnimEveryFromTo = new Vector2(2f, 4f), _changeTargetEveryFromTo = new Vector2(3f, 8f), _radiusMinMax = new Vector2(20, 60), _yMinMax = new Vector2(10, 40);
    private Vector3 _rotateTarget, _position, _direction, _velocity, _randomizedBase;
    private Quaternion _lookRotation;

    // Start is called before the first frame update
    void Start()
    {   
        _fmodAudioSource = gameObject.AddComponent(typeof(OcclusionInteract)) as OcclusionInteract;
        _fmodAudioSource = GetComponent<OcclusionInteract>();
        _fmodAudioSource.enabled = false;
        _fmodAudioSource.PlayerOcclusionWidening = 0.15f;
        _fmodAudioSource.SoundOcclusionWidening = 1.2f;
        _mask = LayerMask.GetMask("ProjectCameraLayer");
        _fmodAudioSource.OcclusionLayer = _mask;
        _fmodAudioSource.SelectAudio = "event:/Props/Environment_seagull";
        int delay = Random.Range(0, 7);
        _animator = GetComponent<Animator>();
        _body = GetComponent<Rigidbody>();
        _turnSpeedBackup = _turnSpeed;
        _direction = Quaternion.Euler(transform.eulerAngles) * (Vector3.forward);
        if(delayStart < 0f) _body.velocity = _idleSpeed * _direction;
        if(_homeTarget == null) _homeTarget = gameObject.transform.parent;
        if(_flyingTarget == null) _flyingTarget = _homeTarget;
        _fmodAudioSource.enabled = true;
        //startSound(delay);
    }

    protected virtual IEnumerator startSound(int _delay) {
        int delay = _delay;
        if(delay>0) yield return new WaitForSeconds(_delay);
        _fmodAudioSource.enabled = true;
        
    }

    void FixedUpdate()
    {
        // Wait if start should be delayed (useful to add small differences in large flocks)
        if(delayStart > 0f)
        {
            delayStart -= Time.fixedDeltaTime;
            return;
        }

        // Calculate distances
        distanceFromBase = Vector3.Magnitude(_randomizedBase - _body.position);
        distanceFromTarget = Vector3.Magnitude(_flyingTarget.position - _body.position);

        // Allow drastic turns close to base to ensure target can be reached
        if(returnToBase && distanceFromBase< 10f)
        {
            if(_turnSpeed != 300f && _body.velocity.magnitude != 0f)
            {
                _turnSpeedBackup = _turnSpeed;
                _turnSpeed = 300f;
            }
            else if(distanceFromBase <= 1f)
            {
                _body.velocity = Vector3.zero;
                _turnSpeed = _turnSpeedBackup;
                return;
            }
        }

        // Time for a new animation speed
        if(changeAnim < 0f)
        {
            prevAnim = currentAnim;
            currentAnim = ChangeAnim(currentAnim);
            changeAnim = Random.Range(_changeAnimEveryFromTo.x, _changeAnimEveryFromTo.y);
            timeSinceAnim = 0f;
            prevSpeed = speed;
            if(currentAnim == 0) speed = _idleSpeed;
            else speed = Mathf.Lerp(_moveSpeedMinMax.x, _moveSpeedMinMax.y, (currentAnim - _animSpeedMinMax.x)/(_animSpeedMinMax.y - _animSpeedMinMax.x));
        }

        // Time for a new target position
        if(changeTarget < 0f)
        {
            _rotateTarget = ChangeDirection(_body.transform.position);
            if(returnToBase) changeTarget = 0.2f; else changeTarget = Random.Range(_changeTargetEveryFromTo.x, _changeTargetEveryFromTo.y);
            timeSinceTarget = 0f;
        }

        // Turn when approaching height limits
        // TODO: Adjust limit and "exit direction" by object's direction and velocity, instead of the 10f and 1f - this works in my current scenario/scale
        if(_body.transform.position.y < _yMinMax.x + 10f || _body.transform.position.y > _yMinMax.y - 10f)
        {
            if(_body.transform.position.y < _yMinMax.x + 10f) _rotateTarget.y = 1f; else _rotateTarget.y = -1f;
        }

        // _body.transform.Rotate(0f, 0f, -prevz, Space.Self); // if required to make Quaternion.LookRotation work correctly, but it seems to be fine
        zturn = Mathf.Clamp(Vector3.SignedAngle(_rotateTarget, _direction, Vector3.up), -45f, -45f);

        // Update times
        changeAnim -= Time.fixedDeltaTime;
        changeTarget -= Time.fixedDeltaTime;
        timeSinceTarget += Time.fixedDeltaTime;
        timeSinceAnim += Time.fixedDeltaTime;

        // Rotate towards target
        if(_rotateTarget != Vector3.zero) _lookRotation = Quaternion.LookRotation(_rotateTarget, Vector3.up);
        Vector3 rotation = Quaternion.RotateTowards(_body.transform.rotation, _lookRotation, _turnSpeed * Time.fixedDeltaTime).eulerAngles;
        _body.transform.eulerAngles = rotation;

        // Rotate on z-axis to tilt body towards turn direction
        float temp = prevz;
        if(prevz < zturn) prevz += Mathf.Min(_turnSpeed * Time.fixedDeltaTime, zturn - prevz);
        else prevz -= Mathf.Min(_turnSpeed * Time.fixedDeltaTime, prevz - zturn);

        // Min and max rotation on z-axis - can also be parametrized
        prevz = Mathf.Clamp(prevz, -45f, 45f);

        // Remove temp if transform is rotated back earlier in FixedUpdate
        _body.transform.Rotate(0f, 0f, prevz - temp, Space.Self);

        // Move flyer
        _direction = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;
        if(returnToBase && distanceFromBase < _idleSpeed)
        {
            _body.velocity = Mathf.Min(_idleSpeed, distanceFromBase) * _direction;
        }
        else {}
        _body.velocity = Mathf.Lerp(prevSpeed, speed, Mathf.Clamp((timeSinceAnim / _switchSeconds), 0f, 1f)) * _direction;

        // hard-limit the height, in case the limit is breached despite of the turnaround attempt
        if(_body.transform.position.y < _yMinMax.x || _body.transform.position.y > _yMinMax.y)
        {
            _position = _body.transform.position;
            _position.y = Mathf.Clamp(_position.y, _yMinMax.x, _yMinMax.y);
            _body.transform.position = _position;
        }
    }

    // Select a new animation speed randomly
    private float ChangeAnim(float currentAnim)
    {
        float newState;
        if(Random.Range(0f, 1f) < _idleRatio) newState = 0f; else
        {
            newState = Random.Range(_animSpeedMinMax.x, _animSpeedMinMax.y);
        }
        if(newState != currentAnim)
        {
            _animator.SetFloat("flySpeed", newState);
            if(newState == 0) _animator.speed = 1f; else _animator.speed = newState;
        }
        return newState;
    }

    // Select a new direction to fly in randomly
    private Vector3 ChangeDirection(Vector3 currentPosition)
    {
        Vector3 newDir;
        if(returnToBase)
        {
            _randomizedBase = _homeTarget.position;
            _randomizedBase.y += Random.Range(-randomBaseOffset, randomBaseOffset);
            newDir = _randomizedBase - currentPosition;
        }
        else if(distanceFromTarget > _radiusMinMax.y)
        {
            newDir = _flyingTarget.position - currentPosition;
        }
        else if(distanceFromTarget < _radiusMinMax.x)
        {
            newDir = currentPosition - _flyingTarget.position;
        }
        else
        {
            // 360-degree freedom of choice on the horizontal plane5
            float angleXZ = Random.Range(-Mathf.PI, Mathf.PI);
            // Limited max steepnees of ascent/descent in the vertical directions
            float angleY = Random.Range(-Mathf.PI/48f, Mathf.PI/48f);
            // Calculate direction
            newDir = Mathf.Sin(angleXZ) * Vector3.forward + Mathf.Cos(angleXZ) * Vector3.right + Mathf.Sin(angleY) * Vector3.up;
        }

        return newDir.normalized;
    }

    public void SetFlyingTarget(GameObject target){_flyingTarget = target.transform;}
}
