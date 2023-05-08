using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinata : MonoBehaviour
{

    [SerializeField]
    private Rigidbody _pinataRB;
    [SerializeField]
    private float _hitMultiplayer;


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

    private void OnHit(Vector3 hit)
    {
        _pinataRB.AddForce(hit);
    }
}
