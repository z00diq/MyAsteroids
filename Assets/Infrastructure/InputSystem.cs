using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Infrastructure
{
    public class InputSystem : IUpdatable
    {
        private const string Horizontal = nameof(Horizontal);
        private const string Vertical = nameof(Vertical);

        private const KeyCode fireButton = KeyCode.F;
        private const KeyCode altFireButton = KeyCode.G;

        private Vector2 _inputData = Vector2.zero;

        public event Action<Vector2> InputChanged;
        public event Action FireButtonPressed;
        public event Action AltFireButtonPressed;

        public void Update()
        {
            _inputData.x = Input.GetAxisRaw(Horizontal);
            _inputData.y = Input.GetAxisRaw(Vertical);

            if (_inputData.y > 0 || _inputData.x != 0)
                InputChanged?.Invoke(_inputData);

            if (Input.GetKey(fireButton))
                FireButtonPressed?.Invoke();

            if (Input.GetKey(altFireButton))
                AltFireButtonPressed?.Invoke();
        }
    }
}
