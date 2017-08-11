namespace SimpleConsole
{
    sealed class WindowPage : ConsoleContainer
    {
        public WindowPage(int width, int height, int left, int top)
            : base(width, height, left, top)
        {
            
        }

        public override void Update()
        {
            base.Update();

            DefultCursorPosition();
        }

        protected override void DefultCursorPosition()
        {
            SetCursorPosition(0, 0);

            SetCursorPosition(0, height - 1);
        }
    }
}
