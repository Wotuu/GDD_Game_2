using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAInterfaceComponents.AbstractComponents;
using MainGame.UI;
using Microsoft.Xna.Framework.Graphics;

namespace MainGame.Managers
{
    public class MenuManager
    {

        #region Singleton logic
        private static MenuManager instance;

        public static MenuManager GetInstance()
        {
            if (instance == null) instance = new MenuManager();
            return instance;
        }

        private MenuManager() { }
        #endregion

        public static Texture2D PANEL_BACKGROUND { get; set; }
        public static Texture2D BUTTON_BACKGROUND { get; set; }
        public static Texture2D BUTTON_MOUSEOVER_BACKGROUND { get; set; }
        public static SpriteFont MAIN_MENU_BUTTON_FONT { get; set; }

        static MenuManager()
        {
            PANEL_BACKGROUND = Game1.GetInstance().Content.Load<Texture2D>("UI/Interface/testMenuBackground");
            BUTTON_BACKGROUND = Game1.GetInstance().Content.Load<Texture2D>("UI/Interface/testButtonBackground");
            BUTTON_MOUSEOVER_BACKGROUND = Game1.GetInstance().Content.Load<Texture2D>("UI/Interface/testMouseoverBackground");

            MAIN_MENU_BUTTON_FONT = Game1.GetInstance().Content.Load<SpriteFont>("Fonts/MainMenu");
        }

        private ParentComponent currentMenu { get; set; }

        public enum Menu
        {
            NoMenu,

            MainMenu,
            IngameMenu
        }

        /// <summary>
        /// Shows a certain menu.
        /// </summary>
        /// <param name="menu"></param>
        public void ShowMenu(Menu menu)
        {

            switch (menu)
            {
                case Menu.NoMenu:
                    {
                        if (this.GetCurrentMenu() != null)
                        {
                            this.GetCurrentMenu().Unload();
                            this.currentMenu = null;
                        }
                        break;
                    }
                case Menu.MainMenu:
                    {
                        this.currentMenu = new MainMenu();

                        break;
                    }
                case Menu.IngameMenu:
                    {
                        this.currentMenu = new IngameMenu();

                        break;
                    }

            }
        }

        /// <summary>
        /// Gets the current menu that may or may not be displaying.
        /// </summary>
        /// <returns>The menu, or null if no menu is displaying.</returns>
        public ParentComponent GetCurrentMenu()
        {
            return this.currentMenu;
        }


    }
}
