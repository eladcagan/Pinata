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
    // Start is called before the first frame update
    void Awake()
    {
        CreatePinata();
    }

    public void PlayAgain()
    {
        DestroyPinata();
        _pinataInstance.gameObject.SetActive(false);
        CreatePinata();
    }

    private void CreatePinata()
    {
        var pinata = Instantiate(_pinataPrefab, _pinataParent);
        _pinataInstance = pinata.GetComponentInChildren<Pinata>();
        _pinataInstance.PinataExploaded += OnPinataExploaded;
    }

    private void OnPinataExploaded()
    {
        _playAgainButton.SetActive(true);
    }

    private void DestroyPinata()
    {
        _pinataInstance.PinataExploaded -= OnPinataExploaded;
        Destroy(_pinataInstance);
    }
}
