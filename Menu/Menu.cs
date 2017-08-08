﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleConsole
{
    public sealed class Menu : ConsoleContainer
    {
        public ConsoleColor selectedBackground;
        public ConsoleColor selectedForeground;
        private List<MenuPage> pages;
        private int optionId = 1;

        internal sealed class Option
        {
            public TextBox textBox;
            public int id;

            public Option(string text, Menu menu, int id)
            {
                textBox = TextBox.GetTextBox(menu.width, menu.height, text);
                this.id = id;
            }
        }

        public Menu(int width, int height)
            : this(0, 0, width, height)
        {

        }

        public Menu(int left, int top, int width, int height)
            : base(left, top, width, height)
        {
            selectedBackground = ConsoleColor.White;
            selectedForeground = ConsoleColor.Black;

            pages = new List<MenuPage>();
            var firstPage = new MenuPage(this);
            pages.Add(firstPage);
            base.Attach(firstPage);
        }

        public int CurentPage
        {
            get;
            private set;
        }

        public int PagesCount => pages.Count;

        private int OptionId
        {
            get
            {
                int id = optionId;
                optionId++;
                return id;
            }
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

        public int AddOption(string text)
        {
            bool addOption = false;

            int curentOptionId = OptionId;
            Option option = new Option(text, this, curentOptionId);

            foreach (var page in pages)
            {
                addOption = page.AddOption(option);

                if (addOption)
                {
                    break;
                }
            }

            if (!addOption)
            {
                var newPage = new MenuPage(this);

                addOption = newPage.AddOption(option);

                if (addOption)
                {
                    pages.Add(newPage);
                }
            }

            pages[CurentPage].Update();

            return (addOption) ? curentOptionId : 0;
        }

        public int Input()
        {
            bool confirm = false;

            while (!confirm)
            {
                ConsoleKey key =  Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        PageShift(-1);
                        break;
                    case ConsoleKey.RightArrow:
                        PageShift(1);
                        break;
                    case ConsoleKey.Enter:
                        confirm = true;
                        break;
                    default:
                        pages[CurentPage].Input(key);
                        break;
                }

                DefultCursorPosition();
            }

            return pages[CurentPage].GetSelectionId();
        }

        public void PageShift(int step)
        {
            if (pages.Count == 0)
            {
                return;
            }

            var selection = pages[CurentPage];
            base.Detach(selection);

            CurentPage += step;

            if (CurentPage > pages.Count - 1)
            {
                CurentPage = 0;
            }

            if (CurentPage < 0)
            {
                CurentPage = pages.Count - 1;
            }

            selection = pages[CurentPage];
            base.Attach(selection);
            selection.Update();
        }
    }
}