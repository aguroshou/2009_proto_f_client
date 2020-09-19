using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hpDirector : MonoBehaviour
{ 
    [SerializeField] Slider slider;

    private int _hp;
    public int HP
    {
        set
        {
            _hp = value;
        }
    }
    void Start()
    {
        
    }

    public void DecreaseHp(int damage)
    {
        _hp -= damage;
        slider.value = _hp;
    }
}
