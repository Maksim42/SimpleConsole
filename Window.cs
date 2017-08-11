using System;

namespace SimpleConsole
{
    sealed class Window : ConsoleContainer
    {
        private int windowWidth;
        private int windowHeight;

        internal Window(int width, int height)
            : base(Console.BufferWidth, Console.BufferHeight)
        {
            BackgroundColor = Console.BackgroundColor;
            ForegroundColor = Console.ForegroundColor;

            windowWidth = width;
            windowHeight = height;

            // TODO: checking size
        }

        public override int Width
        {
            get
            {
                return windowWidth;
            }
        }

        public override void Update()
        {
            MaximaizeWindow();

            base.Update();

            RemoveColor();
        }

        public override void Clear()
        {
            //foreach (var child in childrenContainers)
            //{
            //    child.Clear();
            //}
        }

        public ConsoleContainer CreatePage(int top)
        {
            var newPage = new WindowPage(windowWidth, windowHeight, 0, top);

            bool attachResult = Attach(newPage);

            return (attachResult) ? newPage : null;
        }

        protected override bool SetCursorPosition(int left, int top)
        {
            if (!PositionIsInContainer(left, top))
            {
                return false;
            }

            Console.SetCursorPosition(left, top);
            return true;
        }

        protected override void DefultCursorPosition()
        {
            return;
        }

        private void MaximaizeWindow()
        {
            Console.WindowWidth = windowWidth;
            Console.WindowHeight = windowHeight;
        }
    }
}
