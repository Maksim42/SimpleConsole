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
        private List<Menu.Option> options;
        private int selectedOption;

        public MenuPage(Menu parentMenu)
            : this(0, 0, parentMenu.Width, parentMenu.Height)
        {
            selectedBackground = parentMenu.selectedBackground;
            selectedForeground = parentMenu.selectedForeground;
        }

        private MenuPage(int left, int top, int width, int height)
            : base(left, top, width, height)
        {
            options = new List<Menu.Option>();
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

        public bool AddOption(Menu.Option option)
        {
            if (innerSpace.Attach(option.textBox))
            {
                options.Add(option);
                return true;
            }

            return false;
        }

        public void RemoveOption(Menu.Option option)
        {
            options.Remove(option);
            innerSpace.Detach(option.textBox);
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

        public int GetSelectionId()
        {
            if (options.Count == 0)
            {
                return 0;
            }

            return options[selectedOption].id;
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
