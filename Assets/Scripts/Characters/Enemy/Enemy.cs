using UnityEngine;

public class Enemy : MonoBehaviour, ICharacter
{
    public int _maxHP=3;
    private int _currentHP;

    private void Start()
    {
        _currentHP = _maxHP;
    }

    private void Death()
    {
        Debug.Log("I " + gameObject.name + (" have died."));
        Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        if (_currentHP < 0)
        {
            Death();
        }
    }

    public void Heal(int heal)
    {
        _currentHP += heal;
    }
}
