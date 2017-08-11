using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleConsole
{
    internal class MenuPage : ConsoleContainer
    {
        public ConsoleColor selectedBackground;
        public ConsoleColor selectedForeground;
        private VerticalStack innerSpace;
        private List<MenuOption> options;
        private int selectedOption;

        public MenuPage(Menu parentMenu)
            : this(parentMenu.Width, parentMenu.Height, 0, 0)
        {
            selectedBackground = parentMenu.selectedBackground;
            selectedForeground = parentMenu.selectedForeground;
        }

        private MenuPage(int width, int height, int left, int top)
            : base(width, height, left, top)
        {
            options = new List<MenuOption>();
            innerSpace = new VerticalStack(width, height);
            base.Attach(innerSpace);
        }

        public override void Update()
        {
            if (selectedOption != 0 && selectedOption < options.Count)
            {
                var selection = options[selectedOption].textBox;
                selection.BackgroundColor = BackgroundColor;
                selection.ForegroundColor = ForegroundColor;
            }

            selectedOption = 0;

            innerSpace.Update();

            if (options.Count > 0)
            {
                UpdateTextColor(options[selectedOption].textBox, selectedBackground, selectedForeground);
            }
        }

        #region Stabs
        public override bool Attach(ConsoleContainer child)
        {
            return false;
        }

        public override bool Detach(ConsoleContainer child)
        {
            return false;
        }
        #endregion Stabs

        public bool AddOption(MenuOption option)
        {
            if (innerSpace.Attach(option.textBox))
            {
                options.Add(option);
                return true;
            }

            return false;
        }

        public void RemoveOption(MenuOption option)
        {
            options.Remove(option);
            innerSpace.Detach(option.textBox);
            selectedOption = 0;
        }

        public void Input(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    SelectionNext(-1);
                    break;
                case ConsoleKey.DownArrow:
                    SelectionNext(1);
                    break;
            }
        }

        public int SelectOption()
        {
            if (options.Count == 0)
            {
                return 0;
            }

            var selected = options[selectedOption];

            if (selected.callback != null)
            {
                selected.callback();
            }

            return selected.id;
        }

        private void SelectionNext(int step)
        {
            if (options.Count == 0)
            {
                return;
            }

            var selection = options[selectedOption].textBox;
            UpdateTextColor(selection, BackgroundColor, ForegroundColor);

            selectedOption += step;

            if (selectedOption > options.Count - 1)
            {
                selectedOption = 0;
            }

            if (selectedOption < 0)
            {
                selectedOption = options.Count - 1;
            }

            selection = options[selectedOption].textBox;
            UpdateTextColor(selection, selectedBackground, selectedForeground);
        }

        private void UpdateTextColor(TextBox textBox, ConsoleColor background, ConsoleColor foreground)
        {
            textBox.BackgroundColor = background;
            textBox.ForegroundColor = foreground;
            textBox.Update();
        }
    }
}
