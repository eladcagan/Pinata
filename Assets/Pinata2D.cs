using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pinata2D : MonoBehaviour
{

    [Header("Variables")]
    [SerializeField]
    private float _hitDirectionMultiplayer;
    [SerializeField]
    private float _maxHits;
    [SerializeField]
    private float _scaleDuration;
    [SerializeField]
    private Vector3 _targetSize;
    [Header("References")]
    [SerializeField]
    private Rigidbody2D _pinataRB;



    public event Action PinataExploaded;


    private int _hitCount;
    private bool _isScaling;
    private int _randomRotation;
    private GameObject _hitPS;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseDown()
    {
        Debug.LogError("OnMouseDown");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
        if (hit.collider != null || hit.collider.transform == this.transform)
        {
            //MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();

            var point = hit.point;
            var origPoint = hit.point;
            //deformer.AddDeformingForce(point, force);
            OnHit(point, origPoint);
        }
    }
    private void OnHit(Vector3 point, Vector3 origPoint)
    {
        Debug.LogError("OnHit");
        origPoint.z = origPoint.z - .001f;
        var randomForceDirection = UnityEngine.Random.Range(0, 2);
        if (!_isScaling)
        {
            var randomDirectrion = randomForceDirection > 0.5 ? Vector3.left : Vector3.right;
            _pinataRB.AddForceAtPosition(Vector3.up * _hitDirectionMultiplayer, point);
            _pinataRB.AddForceAtPosition(randomDirectrion * _hitDirectionMultiplayer, point);

            StartCoroutine(HitScaleDown());
        }

        if (_hitCount > _maxHits / 3)
        {
        }
        if (_hitCount > 2 * _maxHits / 3)
        {
        }
        if (_hitCount == _maxHits)
        {
           /* _pinataMeshRendrer.enabled = false;
            _pinataFragmentsParent.SetActive(true);
            _pinataExplosion.SetActive(true);
            foreach (Rigidbody rb in _pinataFragments)
            {
                rb.AddExplosionForce(_explosionForce, _pinataTransform.position, _explosionRadius);
            }
            PinataExploaded.Invoke();*/

        }
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
        Destroy(_hitPS);
    }
}
