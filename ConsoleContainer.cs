using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleConsole
{
    public class ConsoleContainer
    {
        protected ConsoleContainer parentContainer;
        protected List<ConsoleContainer> childrenContainers;
        protected int width;
        protected int height;
        protected int left;
        protected int top;
        protected string clearLine = "";
        protected bool parentBackgroundColor;
        protected bool parentForegroundColor;
        protected ConsoleColor backgroundColor;
        protected ConsoleColor foregroundColor;
        private static Window window;

        public ConsoleContainer(int width, int height)
            : this(0, 0, width, height)
        {

        }

        public ConsoleContainer(int left, int top, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.left = left;
            this.top = top;
            parentBackgroundColor = true;
            parentForegroundColor = true;

            childrenContainers = new List<ConsoleContainer>();
        }

        public static void InitializeWindow()
        {
            InitializeWindow(Console.WindowWidth, Console.WindowHeight);
        }

        public static void InitializeWindow(int width, int height)
        {
            if (window == null)
            {
                window = new Window(width, height);
            }
        }

        public static void WindowUpdate()
        {
            Window.Update();
        }

        public static void WindowClear()
        {
            Window.Clear();
        }

        public static void WindowColors(ConsoleColor background, ConsoleColor foreground)
        {
            Window.backgroundColor = background;
            Window.foregroundColor = foreground;
        }

        public static ConsoleContainer GetWindowPage(int top)
        {
            InitializeWindow();

            return window.GetPage(top);
        }

        #region Propertys
        public static ConsoleContainer Window
        {
            get
            {
                InitializeWindow();

                return window;
            }
        }

        public virtual int Height
        {
            get
            {
                return height;
            }
        }

        public virtual int Width
        {
            get
            {
                return width;
            }
        }

        public ConsoleColor BackgroundColor
        {
            get
            {
                if (parentBackgroundColor)
                {
                    return parentContainer.BackgroundColor;
                }
                else
                {
                    return backgroundColor;
                }
            }

            set
            {
                parentBackgroundColor = false;
                backgroundColor = value;
            }
        }

        public ConsoleColor ForegroundColor
        {
            get
            {
                if (parentForegroundColor)
                {
                    return parentContainer.ForegroundColor;
                }
                else
                {
                    return foregroundColor;
                }
            }

            set
            {
                parentForegroundColor = false;
                foregroundColor = value;
            }
        }
        #endregion

        public virtual void Update()
        {
            Clear();

            foreach (var children in childrenContainers)
            {
                children.Update();
            }

            DefultCursorPosition();
        }

        public virtual void Clear()
        {
            ChangeColor();

            for (int i = 0; i < height; i++)
            {
                WriteInContainer(0, i, GetClearLine());
            }

            RemoveColor();
        }

        public virtual bool Attach(ConsoleContainer child)
        {
            bool result = false;

            if (child == this)
            {
                return false;
            }

            if (CheckPlace(child))
            {
                childrenContainers.Add(child);
                child.parentContainer = this;
                result = true;
            }

            return result;
        }

        public virtual bool Detach(ConsoleContainer child)
        {
            bool result = childrenContainers.Remove(child);

            if (result)
            {
                child.parentContainer = null;
            }

            return result;
        }

        public virtual void DetachAll()
        {
            while (childrenContainers.Count > 0)
            {
                Detach(childrenContainers[0]);
            }
        }

        public bool SetPosition(int left, int top)
        {
            if (parentContainer == null)
            {
                this.left = left;
                this.top = top;

                return true;
            }
            else
            {
                bool moveResult = parentContainer.Move(this, left, top);

                if (moveResult)
                {
                    this.left = left;
                    this.top = top;
                }

                return moveResult;
            }
        }

        // TODO: finish
        protected void WriteInContainer(int left, int top, string text)
        {
            SetCursorPosition(left, top);

            Console.Write(text);
        }

        protected virtual bool SetCursorPosition(int left, int top)
        {
            if (parentContainer == null)
            {
                return false;
            }

            if (!PositionIsInContainer(left, top))
            {
                return false;
            }

            return parentContainer.ChildSetCursorPosition(this, left, top);
        }

        protected bool ChildSetCursorPosition(ConsoleContainer child, int left, int top)
        {
            if (!childrenContainers.Contains(child))
            {
                return false;
            }

            return SetCursorPosition(child.left + left, child.top + top);
        }

        protected virtual void DefultCursorPosition()
        {
            parentContainer.DefultCursorPosition();
        }

        // TODO: finish
        protected virtual bool Move(ConsoleContainer child, int left, int top)
        {
            return false;
        }

        protected void ChangeColor()
        {
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = ForegroundColor;
        }

        protected void RemoveColor()
        {
            if (parentContainer != null)
            {
                parentContainer.RemoveColor();
            }
            else
            {
                ChangeColor();
            }
        }

        protected string GetClearLine()
        {
            if (clearLine.Length != width)
            {
                StringBuilder line = new StringBuilder();
                line.Append(' ', width);
                clearLine = line.ToString();
            }

            return clearLine;
        }

        protected bool CheckPlace(ConsoleContainer newChild)
        {
            bool result = true;

            if (!ContainerIsInContainer(newChild))
            {
                return false;
            }

            foreach (ConsoleContainer child in childrenContainers)
            {
                if (child == newChild)
                {
                    continue;
                }

                bool collision = ((child.left <= newChild.left && newChild.left <= child.left + child.width - 1) ||
                                 (newChild.left <= child.left && child.left <= newChild.left + newChild.width - 1)) &&
                                 ((child.top <= newChild.top && newChild.top <= child.top + child.height - 1) ||
                                 (newChild.top <= child.top && child.top <= newChild.top + newChild.height - 1));
                if (collision)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        protected bool ContainerIsInContainer(ConsoleContainer child)
        {
            if (PositionIsInContainer(child.left, child.top) &&
                PositionIsInContainer(child.left + child.width - 1,
                                      child.top + child.height - 1))
            {
                return true;
            }

            return false;
        }

        protected bool PositionIsInContainer(int leftPosition, int topPosition)
        {
            if (leftPosition < 0 || leftPosition > width - 1)
            {
                return false;
            }

            if (topPosition < 0 || topPosition > height - 1)
            {
                return false;
            }

            return true;
        }
    }
}
