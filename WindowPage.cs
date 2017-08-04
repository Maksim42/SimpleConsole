namespace SimpleConsole
{
    sealed class WindowPage : ConsoleContainer
    {
        public WindowPage(int left, int top, int width, int height)
            : base(left, top, width, height)
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
