using System.Collections.Generic;
using UnityEngine;

using ProjectMGG.UI;

namespace ProjectMGG
{
    public class MenuInput
    {
        public int Count { get; private set; } = 0;
        public bool Hover { get; private set; } = false;

        private int _selectedMenuNumber = -1;
        private List<ButtonEvent> _buttons = new List<ButtonEvent>();

        public MenuInput()
        {

        }

        public MenuInput(IEnumerable<ButtonEvent> menuButtons)
        {
            _buttons.AddRange(menuButtons);
            Count = _buttons.Count;
        }

        public void AddMenuButton(ButtonEvent button)
        {
            _buttons.Add(button);
            Count = _buttons.Count;
        }

        public void SetButtonIgnoreEvent(bool ignore)
        {
            for (int i = 0; i < _buttons.Count; i++) _buttons[i].IgnoreEvent = ignore;
        }

        public void UpdateForKeyboardInput()
        {
            bool succeed = false;

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_selectedMenuNumber >= 0) _buttons[_selectedMenuNumber].OnPointerExit(null);

                _selectedMenuNumber++;
                if (_selectedMenuNumber >= Count) _selectedMenuNumber = 0;

                _buttons[_selectedMenuNumber].OnPointerEnter(null);
                succeed = true;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_selectedMenuNumber >= 0) _buttons[_selectedMenuNumber].OnPointerExit(null);

                if (_selectedMenuNumber == -1) _selectedMenuNumber = 1;
                _selectedMenuNumber--;
                if (_selectedMenuNumber < 0) _selectedMenuNumber = Count - 1;

                _buttons[_selectedMenuNumber].OnPointerEnter(null);
                succeed = true;
            }
            else if (Hover && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))) //Space / Enter
            {
                if (_selectedMenuNumber >= 0) _buttons[_selectedMenuNumber].OnPointerClick(null);
                succeed = true;
            }

            if (succeed) Input.ResetInputAxes(); //for preventing overlap [IngameManagerV2, MenuChoice] Input
        }

        public void OnMouseHover(int index)
        {
            if (_selectedMenuNumber >= 0 && _selectedMenuNumber != index) _buttons[_selectedMenuNumber].OnPointerExit(null);
            _selectedMenuNumber = index;
            Hover = true;
        }

        public void OnMouseExit(int index)
        {
            Hover = false;
        }

        public void OnMouseClick(int index = -1)
        {
            //inference
            int selectedIndex = index >= 0 && index < Count ? index : _selectedMenuNumber;
            bool isBound = selectedIndex >= 0 && selectedIndex < _buttons.Count;

            if (isBound) _buttons[selectedIndex].OnPointerExit(null); //comment this if something went wrong after click (I inserted this code because of after click)
            Reset();
        }

        public void Reset()
        {
            Hover = false;
            _selectedMenuNumber = -1;
        }
    }
}