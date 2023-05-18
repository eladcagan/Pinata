using UnityEngine;
using UnityEngine.SceneManagement;


public class Pinata2DSceneManager : MonoBehaviour
{

    private const int _mainMenuScene = 0;

    [SerializeField]
    private GameObject _pinataPrefab;
    [SerializeField]
    private Transform _pinataParent;
    [SerializeField]
    private GameObject _playAgainButton;

    private Pinata2D _pinataInstance;
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

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync(_mainMenuScene);
    }

    private void CreatePinata()
    {
        _pinata = Instantiate(_pinataPrefab, _pinataParent);
        _pinataInstance = _pinata.GetComponentInChildren<Pinata2D>();
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
