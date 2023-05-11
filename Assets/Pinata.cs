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
    private float _maxRotation;
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
    private bool _pinataDroped;
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
        //_pinataRB.AddForceAtPosition(point);
        var randomRotation = Random.Range(0, _rotations.Count);
        _pinataTransform.eulerAngles = _rotations[randomRotation];
        var currentRotation = _pinataTransform.rotation.eulerAngles.y;
        //_pinataRB.AddForce(Vector3.up *_hitMultiplayer);
       /* if (Mathf.Abs(currentRotation) < _maxRotation)
        {
            _pinataTransform.Rotate(Vector3.up, randomRotation);
        }
        else if (currentRotation > _maxRotation)
        {
            _pinataTransform.Rotate(Vector3.up, -_maxRotation/2);
        } 
        else
        {
            _pinataTransform.Rotate(Vector3.up, _maxRotation / 2);
        }*/


        if (_hitCount > 5)
        {
            _hole1.SetActive(true);
        }
        if (_hitCount > 10)
        {
            _hole2.SetActive(true);
        }
        if (_hitCount > 15)
        {
            _hole3.SetActive(true);
        }
        _hitCount++;

    }
}
