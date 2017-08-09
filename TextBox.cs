using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleConsole
{
    public sealed class TextBox : ConsoleContainer
    {
        private StringBuilder text;

        public TextBox(int width, int height)
            : this(0, 0, width, height)
        {

        }

        public TextBox(int left, int top, int width, int height)
            : base(left, top, width, height)
        {
            text = new StringBuilder();
        }

        public static TextBox GetTextBox(int width, int maxHeight, string text)
        {
            int neededHeight = text.Length / width +
                ((text.Length % width > 0) ? 1 : 0);

            if (neededHeight > maxHeight)
            {
                neededHeight = maxHeight;
            } 

            TextBox resultBox = new TextBox(width, neededHeight);
            resultBox.Text = text;

            return resultBox;
        }

        public string Text
        {
            get
            {
                return text.ToString();
            }

            set
            {
                text.Clear();
                text.Append(value);
            }
        }

        public override void Update()
        {
            int curentHeight = 0;

            Clear();
            ChangeColor();

            foreach (string line in Lines())
            {
                WriteInContainer(0, curentHeight, line);

                if (curentHeight == height - 1)
                {
                    break;
                }

                curentHeight++;
            }

            RemoveColor();

            DefultCursorPosition();
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

        public void Append(string text)
        {
            this.text.Append(text);
        }

        private IEnumerable<string> Lines()
        {
            int curentLine = 0;
            int lineLength = width;
            bool nextLine = true;

            while (nextLine)
            {
                if (curentLine + lineLength > text.Length)
                {
                    lineLength = text.Length - curentLine;
                    nextLine = false;
                }

                yield return text.ToString(curentLine, lineLength);

                curentLine += width;
            }

            yield break;
        }
    }
}
