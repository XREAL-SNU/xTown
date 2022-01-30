using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialProgress : MonoBehaviour
{
    private bool _done;
    public bool done { get { return _done; } }
    private float _speed = 100;
    private bool _activated;
    [SerializeField]
    private Image _loadingBar;
    private float _currentValue;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _done = false;
        _activated = false;
        _currentValue = 0f;
        _loadingBar.fillAmount = _currentValue;
    }

    public void Activate()
    {
        Initialize();

        _activated = true;
    }

    public void Deactivate()
    {
        _loadingBar.fillAmount = 0;

        _activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_activated)
        {
            if (_currentValue < 100)
            {
                _currentValue += _speed * Time.deltaTime;
            }
            else
            {
                _currentValue = 100;
                _done = true;
            }

            _loadingBar.fillAmount = _currentValue / 100;
        }
    }
}
