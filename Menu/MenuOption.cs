using System;

namespace SimpleConsole
{
    internal sealed class MenuOption
    {
        public TextBox textBox;
        public int id;
        public Action callback;

        public MenuOption(Menu menu, string text, int id, Action callback)
        {
            textBox = TextBox.GetTextBox(menu.Width, menu.Height, text);
            this.id = id;
            this.callback = callback;
        }
    }
}
