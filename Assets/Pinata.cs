using System;
using System.Collections;
using System.Collections.Generic;
using MarchingBytes;
using UnityEngine;


public class Pinata : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField]
    private float _explosionForce;
    [SerializeField]
    private float _explosionRadius;
    [SerializeField]
    private float _hitRotationMultiplayer;
    [SerializeField]
    private float _hitDirectionMultiplayer;
    [SerializeField]
    private float _scaleDuration;
    [SerializeField]
    private Vector3 _targetSize;
    [SerializeField]
    private int _maxHits;
    [Header("References")]
    [SerializeField]
    private Rigidbody _pinataRB;
    [SerializeField]
    private MeshRenderer _pinataMeshRendrer;
    [SerializeField]
    private GameObject _pinataFragmentsParent;
    [SerializeField]
    private List<Rigidbody> _pinataFragments;
    [SerializeField]
    private GameObject _pinataHitPool;
    [SerializeField]
    private GameObject _pinataExplosion;
    [SerializeField]
    private List<Vector3> _rotations;
    [SerializeField]
    private Transform _pinataTransform;
    [SerializeField]
    private List<AudioClip> _hitSfx;
    [SerializeField]
    private List<AudioClip> _pinataSfx;
    [SerializeField]
    private List<AudioClip> _pinataFinishedSfx;
    [SerializeField]
    private AudioClip _intro;
    [SerializeField]
    private AudioSource _source;


    public event Action PinataExploaded;


    private int _hitCount;
    private bool _isScaling;
    private int _randomRotation;
    private GameObject _hitPS;


    // Update is called once per frame
    void Update()
    {
        var currentAngle = _pinataTransform.rotation.eulerAngles;
        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, _rotations[_randomRotation].x, Time.deltaTime * _hitRotationMultiplayer),
            Mathf.LerpAngle(currentAngle.y, _rotations[_randomRotation].y, Time.deltaTime * _hitRotationMultiplayer),
            Mathf.LerpAngle(currentAngle.z, _rotations[_randomRotation].z, Time.deltaTime * _hitRotationMultiplayer));

        transform.eulerAngles = currentAngle;
    }

    private void OnMouseDown()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            var point = hit.point;
            var origPoint = hit.point;
            point += -hit.normal * _hitRotationMultiplayer;
            OnHit(point, origPoint);
        }
    }

    private void OnHit(Vector3 point, Vector3 origPoint)
    {
        PlayRandomHitSound();
        origPoint.z = origPoint.z - .001f;
        _hitPS = EasyObjectPool.instance.GetObjectFromPool("PinataHitPool", origPoint, Quaternion.identity);
        _randomRotation = UnityEngine.Random.Range(0, _rotations.Count);
        var randomForceDirection = UnityEngine.Random.Range(0, 2);
        if (!_isScaling)
        {
            var randomDirectrion = randomForceDirection > 0.5 ? Vector3.left : Vector3.right;
            _pinataRB.AddForceAtPosition(Vector3.up * _hitDirectionMultiplayer, point);
            _pinataRB.AddForceAtPosition(randomDirectrion * _hitDirectionMultiplayer, point);

            StartCoroutine(HitScaleDown());
        }

        if ((int)_hitCount == _maxHits / 3)
        {
            PlayRandomPinataSound();
        }
        if ((int)_hitCount == 2 * _maxHits / 3)
        {
            PlayRandomPinataSound();
        }
        if ((int)_hitCount == _maxHits)
        {
            PlayPinataFinishedSound();
            _pinataMeshRendrer.enabled = false;
            _pinataFragmentsParent.SetActive(true);
            _pinataExplosion.SetActive(true);
            foreach (Rigidbody rb in _pinataFragments)
            {
                rb.AddExplosionForce(_explosionForce, _pinataTransform.position, _explosionRadius);
            }
            StartCoroutine(OnPinataExploded(3));
        }
        _hitCount++;
    }

    private void PlayRandomHitSound()
    {
        var RandomHitSound = UnityEngine.Random.Range(0, _hitSfx.Count);
        _source.clip = _hitSfx[RandomHitSound];
        _source.Play();
    }

    private void PlayRandomPinataSound()
    {
        var RandomHitSound = UnityEngine.Random.Range(0, _pinataSfx.Count);
        _source.clip = _pinataSfx[RandomHitSound];
        _source.Play();
    }


    private void PlayPinataFinishedSound()
    {
        var RandomHitSound = UnityEngine.Random.Range(0, _pinataFinishedSfx.Count);
        _source.clip = _pinataFinishedSfx[RandomHitSound];
        _source.Play();
    }

    private IEnumerator HitScaleDown()
    {
        _isScaling = true;
        var initialScale = transform.localScale;

        for (float time = 0; time < _scaleDuration * 2; time += Time.deltaTime)
        {
            float progress = Mathf.PingPong(time, _scaleDuration) / _scaleDuration;
            transform.localScale = Vector3.Lerp(initialScale, _targetSize, progress);
            yield return null;
        }
        transform.localScale = initialScale;
        _isScaling = false;
        Destroy(_hitPS);
    }

    private IEnumerator OnPinataExploded(float delayInSecond)
    {
        PinataExploaded.Invoke();
        yield return new WaitForSeconds(delayInSecond);
    }
}
