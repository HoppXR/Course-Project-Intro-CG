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
    
    [Header("Audio")]
    [SerializeField] private AudioClip shakeSound;
    [SerializeField] private AudioClip dropSound;
    
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
            GameManager.instance.PlaySound(shakeSound, transform, 0.75f);
            
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

    public void PlayDropSound()
    {
        GameManager.instance.PlaySound(dropSound, transform, 1f);
    }
}
