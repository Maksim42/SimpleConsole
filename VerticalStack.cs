using System.Collections.Generic;

namespace SimpleConsole
{
    public sealed class VerticalStack : ConsoleContainer
    {
        private int freeHeight;
        private ConsoleContainer innerSpace;
        private List<ConsoleContainer> innerContainers;

        public VerticalStack(int width, int height)
            : this(0, 0, width, height)
        {

        }

        public VerticalStack(int left, int top, int width, int height)
            : base(left, top, width, height)
        {
            freeHeight = height;
            innerContainers = new List<ConsoleContainer>();

            innerSpace = new ConsoleContainer(0, 0, width, height);
            base.Attach(innerSpace);
        }

        public override void Update()
        {
            int topPosition = freeHeight / 2;

            Clear();
            innerSpace.DetachAll();

            foreach (var child in innerContainers)
            {
                child.SetPosition(0, topPosition);
                innerSpace.Attach(child);
                // TODO: if topPosition > height throw error
                topPosition += child.Height;
            }

            innerSpace.Update();
        }

        public override bool Attach(ConsoleContainer child)
        {
            if (freeHeight - child.Height >= 0 && child.Width <= width)
            {
                freeHeight -= child.Height;

                innerContainers.Add(child);

                return true;
            }

            return false;
        }

        public override bool Detach(ConsoleContainer child)
        {
            bool result =  innerContainers.Remove(child);

            if (result)
            {
                innerSpace.Detach(child);
                // TODO: calculate real height
                freeHeight += child.Height;
            }

            return result;
        }

        public override void DetachAll()
        {
            innerSpace.DetachAll();
            innerContainers.Clear();

            freeHeight = height;
        }

        public override void Clear()
        {
            innerSpace.Clear();
        }
    }
}
