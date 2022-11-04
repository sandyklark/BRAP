using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Header("Config")]
    public int Health = 1;
    [Header("References")]
    public GameObject main;
    public GameObject insides;

    private SpriteRenderer _sprite;
    private Collider2D _collider;
    private List<Rigidbody2D> _innerRigidbodies;
    private int _currentHealth;

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _currentHealth = Health;
        _innerRigidbodies = insides.GetComponentsInChildren<Rigidbody2D>().ToList();

        insides.SetActive(false);
    }

    public void Break(Vector2 force)
    {
        _currentHealth--;

        if (_sprite != null) _sprite.color = Color.Lerp(Color.black, Color.white, (float)_currentHealth / Health);

        if(_currentHealth > 0) return;

        main.SetActive(false);
        insides.transform.parent = transform;
        insides.SetActive(true);

        foreach (var innerRigidbody in _innerRigidbodies)
        {
            innerRigidbody.AddForce(force, ForceMode2D.Impulse);
            innerRigidbody.AddTorque(Random.Range(-1f, 1f) * force.magnitude / 5f, ForceMode2D.Impulse);
        }
    }
}
