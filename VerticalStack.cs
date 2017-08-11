using System.Collections.Generic;

namespace SimpleConsole
{
    public sealed class VerticalStack : ConsoleContainer
    {
        private ConsoleContainer innerSpace;
        private List<ConsoleContainer> innerContainers;

        public VerticalStack(int width, int height)
            : this(width, height, 0, 0)
        {

        }

        public VerticalStack(int width, int height, int left, int top)
            : base(width, height, left, top)
        {
            innerContainers = new List<ConsoleContainer>();

            innerSpace = new ConsoleContainer(width, height, 0, 0);
            base.Attach(innerSpace);
        }

        public int FreeHeight
        {
            get
            {
                int sumChildHeight = 0;

                foreach (var child in innerContainers)
                {
                    sumChildHeight += child.Height;
                }

                int result = height - sumChildHeight;

                return (result > 0) ? result : 0;
            }
        }

        public override void Update()
        {
            int topPosition = FreeHeight / 2;

            Clear();
            innerSpace.DetachAll();

            foreach (var child in innerContainers)
            {
                child.SetPosition(0, topPosition);
                innerSpace.Attach(child);
                topPosition += child.Height;
            }

            innerSpace.Update();

            DefultCursorPosition();
        }

        public override bool Attach(ConsoleContainer child)
        {
            if (FreeHeight >= child.Height && child.Width <= width)
            {
                innerContainers.Add(child);

                return true;
            }

            return false;
        }

        public override bool Detach(ConsoleContainer child)
        {
            bool result =  innerContainers.Remove(child);
            innerSpace.Detach(child);

            return result;
        }

        public override void DetachAll()
        {
            innerSpace.DetachAll();
            innerContainers.Clear();
        }

        public override void Clear()
        {
            innerSpace.Clear();
        }
    }
}
