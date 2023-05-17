using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinataSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _pinataPrefab;
    [SerializeField]
    private Transform _pinataParent;
    [SerializeField]
    private GameObject _playAgainButton;


    private Pinata _pinataInstance;
    private GameObject _pinata;
    // Start is called before the first frame update
    void Awake()
    {
        CreatePinata();
    }

    public void PlayAgain()
    {
        _pinataInstance.gameObject.SetActive(false);
        DestroyPinata();
        CreatePinata();
    }

    private void CreatePinata()
    {
        _pinata = Instantiate(_pinataPrefab, _pinataParent);
        _pinataInstance = _pinata.GetComponentInChildren<Pinata>();
        _pinataInstance.PinataExploaded += OnPinataExploaded;
    }

    private void OnPinataExploaded()
    {
       
        _playAgainButton.SetActive(true);
    }

    private void DestroyPinata()
    {
        _pinataInstance.PinataExploaded -= OnPinataExploaded;
        DestroyImmediate(_pinata);
    }
}
