using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinata : MonoBehaviour
{

    [SerializeField]
    private Rigidbody _pinataRB;
    [SerializeField]
    private ParticleSystem _pinataPS;
    [SerializeField]
    private List<Vector3> _rotations;
    [SerializeField]
    private Transform _pinataTransform;
    [SerializeField]
    private float _hitRotationMultiplayer;
    [SerializeField]
    private float _hitDirectionMultiplayer;
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
    // Start is called before the first frame update
    void Start()
    {
    }

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
            //MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();

            var point = hit.point;
            var origPoint = hit.point;
            point += -hit.normal * _hitRotationMultiplayer;
            //deformer.AddDeformingForce(point, force);
            OnHit(point, origPoint);
        }
    }

    private void OnHit(Vector3 point, Vector3 origPoint)
    {
        Instantiate(_pinataPS, origPoint, Quaternion.identity);
        _randomRotation = Random.Range(0, _rotations.Count);
        var randomForceDirection = Random.Range(0, 2);
        if (!_isScaling)
        {
            var randomDirectrion = randomForceDirection > 0.5 ? Vector3.left : Vector3.right;
            _pinataRB.AddForceAtPosition(Vector3.up * _hitDirectionMultiplayer, point);
            _pinataRB.AddForceAtPosition(randomDirectrion * _hitDirectionMultiplayer    , point);

            StartCoroutine(HitScaleDown());
        }

    /*    if (_hitCount > _maxHits/3)
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
        }*/
        _hitCount++;
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
