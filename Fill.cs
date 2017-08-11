using System;
using System.Text;

namespace SimpleConsole
{
    public sealed class Fill : ConsoleContainer
    {
        private char fill = '*';
        private string fillLine = "";

        public Fill(int width, int height) 
            : this(width, height, 0, 0)
        {

        }

        public Fill(int width, int height, int left, int top) :
            base(width, height, left, top)
        {
            
        }

        public override void Update()
        {
            ChangeColor();

            for (int i = 0; i < height; i++)
            {
                WriteInContainer(0, i, GetFillLine());
            }

            RemoveColor();

            DefultCursorPosition();
        }

        public void SetFill(char fillSymbol)
        {
            fill = fillSymbol;
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

        private string GetFillLine()
        {
            if (fillLine.Length != width)
            {
                var lineBilder = new StringBuilder();
                lineBilder.Append(fill, width);

                fillLine = lineBilder.ToString();
            }

            return fillLine;
        }
    }
}
