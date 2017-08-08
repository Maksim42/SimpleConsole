using System.Collections.Generic;

namespace SimpleConsole
{
    public sealed class VerticalStack : ConsoleContainer
    {
        private ConsoleContainer innerSpace;
        private List<ConsoleContainer> innerContainers;

        public VerticalStack(int width, int height)
            : this(0, 0, width, height)
        {

        }

        public VerticalStack(int left, int top, int width, int height)
            : base(left, top, width, height)
        {
            innerContainers = new List<ConsoleContainer>();

            innerSpace = new ConsoleContainer(0, 0, width, height);
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
