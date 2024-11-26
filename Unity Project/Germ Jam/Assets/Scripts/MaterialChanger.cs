using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    private Renderer _rend;

    [SerializeField] private Material noTexture;
    [SerializeField] private Material lambertMaterial;
    [SerializeField] private Material specularMaterial;
    [SerializeField] private Material toonMaterial;
    private Material _originalMaterial;

    private bool _useTexture;
    private bool _usingOriginalMaterial;
    
    void Start()
    {
        _rend = GetComponent<Renderer>();
        
        _originalMaterial = _rend.material;

        _useTexture = true;
        _usingOriginalMaterial = true;
    }

    void Update()
    {
        HandleLighting();
        HandleTexture();
    }

    private void HandleLighting()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _usingOriginalMaterial = true;
            _rend.material = _originalMaterial;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _usingOriginalMaterial = false;
            _rend.material = lambertMaterial;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _usingOriginalMaterial = false;
            _rend.material = specularMaterial;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _usingOriginalMaterial = false;
            _rend.material = toonMaterial;
        }
    }

    private void HandleTexture()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _useTexture = !_useTexture;
        }
        
        if (_useTexture && _usingOriginalMaterial)
            _rend.material = _originalMaterial;
        else if (!_useTexture && _usingOriginalMaterial)
            _rend.material = noTexture;
        
        _rend.material.SetFloat("_UseTexture", _useTexture ? 1f : 0f);
    }
}
