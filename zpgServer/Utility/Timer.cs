using System;
using System.Collections.Generic;
using System.Text;

namespace zpgServer
{
    public class Timer
    {
        bool _isActive = false;
        bool _isRepeatable = false;
        float _time;
        float _maxTime;

        public Timer(float time, bool isRepeatable)
        {
            _time = time;
            _maxTime = time;
            _isActive = true;
            _isRepeatable = isRepeatable;
        }
        public bool Tick() { return Tick(Time.deltaTime); }
        public bool Tick(float forceDelta)
        {
            if (_isActive)
            {
                _time -= forceDelta;
                if (_time < 0f)
                {
                    _time = 0f;
                    if (_isRepeatable) { _time = _maxTime; }
                    else { _isActive = false; }
                    return true;
                }
            }
            return false;
        }
        public void Reset()
        {
            _time = _maxTime;
        }
        public float delta { get { return _maxTime; } }
    }
}
