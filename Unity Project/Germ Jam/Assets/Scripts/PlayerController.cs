using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    #region References
    
    private Rigidbody _rb;
    //private Animator _animator;
    
    [Header("Player Index")]
    [SerializeField] private int playerIndex;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    private Vector3 _input;
    
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Audio")] 
    [SerializeField] private AudioClip squishSound;
    
    [Header("Particles")]
    [SerializeField] private ParticleSystem squishParticles;
    
    #endregion
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        //_animator = GetComponent<Animator>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) Destroy(this);
    }

    private void Update()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        _rb.AddForce(_input.normalized * (moveSpeed * 10f), ForceMode.Force);
        _rb.linearVelocity = Vector3.ClampMagnitude(_rb.linearVelocity, maxSpeed);
    }

    private void HandleRotation()
    {
        if (_input != Vector3.zero)
            transform.forward = Vector3.Slerp(transform.forward, _input.normalized, Time.deltaTime * rotationSpeed);
    }

    private void Die()
    {
        GameManager.instance.PlaySound(squishSound, transform, 0.75f);
        
        squishParticles.Play();
        
        transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
        
        GameManager.instance.PlayerDie(playerIndex);
        
        _rb.linearVelocity = Vector3.zero;
        
        Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FallingObject"))
        {
            Die();
        }
    }
}
