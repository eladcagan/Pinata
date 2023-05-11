using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinata : MonoBehaviour
{

    [SerializeField]
    private Rigidbody _pinataRB;
    [SerializeField]
    private float _hitMultiplayer;
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
        _pinataRB.AddForce(point);
        var randomForce = Random.Range(0, 2);
        Debug.LogError(randomForce);
        if(randomForce < 0.5)
        {
            _pinataRB.AddForce(Vector3.right * _hitMultiplayer);
        } 
        else
        {
            _pinataRB.AddForce(Vector3.left * _hitMultiplayer);
        }
        _hitCount++;
        if(_hitCount > 5)
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
    }
}
