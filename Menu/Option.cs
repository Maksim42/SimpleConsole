using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleConsole
{
    internal sealed class Option
    {
        public TextBox textBox;
        public int id;
        public Action callback;

        public Option(Menu menu, string text, int id, Action callback)
        {
            textBox = TextBox.GetTextBox(menu.Width, menu.Height, text);
            this.id = id;
            this.callback = callback;
        }
    }
}
