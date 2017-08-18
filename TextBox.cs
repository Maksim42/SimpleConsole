using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleConsole
{
    public sealed class TextBox : ConsoleContainer
    {
        private StringBuilder text;

        public TextBox(int width, int height)
            : this(width, height, 0, 0)
        {

        }

        public TextBox(int width, int height, int left, int top)
            : base(width, height, left, top)
        {
            text = new StringBuilder();
        }

        public static TextBox GetTextBox(int width, int maxHeight, string text)
        {
            int neededHeight = 0;
            var tempBox = new TextBox(width, 1);
            tempBox.Text = text;

            foreach (var line in tempBox.Lines())
            {
                if (neededHeight >= maxHeight)
                {
                    neededHeight = maxHeight;
                    break;
                }

                neededHeight++;
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
            int lineStart = 0;
            int lineLength;
            bool nextLine = true;

            while (nextLine)
            {
                lineLength = width;

                if (lineStart + lineLength > text.Length)
                {
                    lineLength = text.Length - lineStart;
                }

                string resultLine = text.ToString(lineStart, lineLength);

                lineLength = LineProcesing(ref resultLine);

                yield return resultLine;

                lineStart += lineLength;

                if (lineStart == text.Length)
                {
                    nextLine = false;
                }
            }

            yield break;
        }

        private int LineProcesing(ref string resultLine)
        {
            int resultShift = resultLine.Length;
            string newLine = resultLine;

            int findIndex = newLine.IndexOf('\n');
            if (findIndex != -1)
            {
                newLine = newLine.Substring(0, findIndex);
                resultShift = findIndex + 1;
            }

            newLine = newLine.Replace('\t', ' ');

            resultLine = newLine;
            return resultShift;
        }
    }
}
