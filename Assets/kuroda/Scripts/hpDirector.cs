using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hpDirector : MonoBehaviour
{
    [SerializeField] EnemyController ec;
    Slider _slider;
    int _hp;
    void Start()
    {
        _slider = GameObject.Find("Slider").GetComponent<Slider>();
    }


    private void FixedUpdate()
    {
        _slider.value = _hp;
        _hp = ec.hp;
    }
}
