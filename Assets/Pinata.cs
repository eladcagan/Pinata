using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinata : MonoBehaviour
{

    [SerializeField]
    private Rigidbody _pinataRB;
    [SerializeField]
    private List<Vector3> _rotations;
    [SerializeField]
    private Transform _pinataTransform;
    [SerializeField]
    private float _hitMultiplayer;
    [SerializeField]
    private float _scaleDuration;
    [SerializeField]
    private Vector3 _targetSize;
    [SerializeField]
    private float _maxHits;
    [SerializeField]
    private GameObject _hole1;
    [SerializeField]
    private GameObject _hole2;
    [SerializeField]
    private GameObject _hole3;
    [SerializeField]
    private List<AudioClip> _hitSfx;
    [SerializeField]
    private List<AudioClip> _pinataSfx;
    [SerializeField]
    private AudioClip _intro;
    [SerializeField]
    private AudioSource _source;



    private int _hitCount;
    private bool _isScaling;
    private int _randomRotation;
    private Vector3 _initialSize;
    private Vector3 _currentSize;
    // Start is called before the first frame update
    void Start()
    {
        _initialSize = _pinataTransform.localScale;
        _currentSize = _initialSize;
    }

    // Update is called once per frame
    void Update()
    {
        var currentAngle = _pinataTransform.rotation.eulerAngles;
        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, _rotations[_randomRotation].x, Time.deltaTime * _hitMultiplayer),
            Mathf.LerpAngle(currentAngle.y, _rotations[_randomRotation].y, Time.deltaTime * _hitMultiplayer),
            Mathf.LerpAngle(currentAngle.z, _rotations[_randomRotation].z, Time.deltaTime * _hitMultiplayer));

        transform.eulerAngles = currentAngle;
    }

    private void OnMouseDown()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            //MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();

            Vector3 point = hit.point;
            point += -hit.normal * _hitMultiplayer;
            //deformer.AddDeformingForce(point, force);
            OnHit(point);
        }
    }

    private void OnHit(Vector3 point)
    {
        _randomRotation = Random.Range(0, _rotations.Count);

        if (!_isScaling)
        {
            StartCoroutine(HitScaleDown());
        }

        if (_hitCount > _maxHits/3)
        {
            _hole1.SetActive(true);
        }
        if (_hitCount > 2*_maxHits/3)
        {
            _hole2.SetActive(true);
        }
        if (_hitCount == _maxHits)
        {
            _hole3.SetActive(true);
        }
        _hitCount++;
    }
    private bool CompareVectors(Vector3 lhs, Vector3 rhs)
    {
        return Vector3.SqrMagnitude(lhs - rhs) < 9.99999944E-11f;
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
    }

    private IEnumerator HitScaleUp()
    {
            yield return null;

    }
}
