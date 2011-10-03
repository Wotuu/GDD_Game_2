using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MainGame.Cards;
using Microsoft.Xna.Framework.Graphics;

namespace MainGame.Managers
{
    public class CardManager
    {

        #region Singleton logic
        private static CardManager instance;

        public static CardManager GetInstance()
        {
            if (instance == null) instance = new CardManager();
            return instance;
        }

        private CardManager() { }
        #endregion

        public LinkedList<WinningCard> drawnCards = new LinkedList<WinningCard>();
        public LinkedList<WinningCard> awardedCards = new LinkedList<WinningCard>();

        public void Update()
        {
            foreach (WinningCard card in this.drawnCards)
            {
                card.Update();
            }
        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < this.drawnCards.Count; i++)
            {
                WinningCard card = this.drawnCards.ElementAt(i);
                if (card.shouldBeRemoved)
                {
                    this.drawnCards.Remove(card);
                    i--;
                }
                else card.Draw(sb);
            }
        }

        /// <summary>
        /// Checks if this card is already awarded.
        /// </summary>
        /// <param name="card">The card you want to add.</param>
        /// <returns>Yes no no.</returns>
        public Boolean IsAwarded(WinningCard card)
        {
            foreach (WinningCard value in this.awardedCards)
            {
                if (value.color == card.color) return true;
            }
            return false;
        }
    }
}
