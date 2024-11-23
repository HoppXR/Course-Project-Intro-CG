using Unity.Netcode;
using UnityEngine;

public class FallingObject : NetworkBehaviour
{
    private Animator _animator;
    
    [Header("Falling Object Properties")]
    [SerializeField] private float fallCooldown = 3;
    private bool _isFalling;
    [SerializeField] private float nextFallTime;
    private float _timer;
    
    [Header("Particles")]
    [SerializeField] private ParticleSystem dropParticle;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _timer += Time.deltaTime;
        
        HandleFall();
    }

    private void HandleFall()
    {
        if (_timer >= nextFallTime)
        {
            _animator.SetTrigger("Fall");
            nextFallTime = _timer + fallCooldown;
            
            if (fallCooldown <= 5)
                fallCooldown = 5;
            else
                fallCooldown -= 3;
        }
    }

    public void SpawnParticles()
    {
        dropParticle.Play();
    }
}
