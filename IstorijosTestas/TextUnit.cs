using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstorijosTestas
{
    class TextUnit
    {
        private string[] text;
        private bool isChangable;
        private int numberOfAlternatives;
        public TextUnit()
        {
            text = new string[4];

            isChangable = false;
            numberOfAlternatives = 0;
        }
        public void setText(string t)
        {
            text[0] = t;
        }
        public string getText()
        {
            return text[0];
        }
        public string getAlternative(int i)
        {
            if (i < 4)
                return text[i + 1];
            return text[0];
        }
        public void setAlternative(int i, string alt)   // paskui: BUTINAI NURODYTI, KOKIE TRIGGERIAI SITA OPTION SUKELIA
        {   // PADIDINTI RIBA, NES VIENAME VARIANTE TRUKSTA
            if(i < 4)
            {
                isChangable = true;
                text[i] = alt;
                numberOfAlternatives++;
            }
        }
        public int getNumberOfAlternatives() { return numberOfAlternatives; }
        public void change() { isChangable = true; }
        public bool hasAlternative() { return isChangable; }
    }
}
