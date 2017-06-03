using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstorijosTestas
{
    class Option
    {
        private string name;
        private string[] option;
        private int end;
        private int pointer;
        private string[] choose;
        private TextUnit[] consequences;
        private int numberOfChoices;
        public int myMana, myHeart, JoshMana, JoshHeart, JoannaMana, JoannaHeart, robotFinalDefeated;
        private int aggressiveness;
        private string activatesTrigger;
        private string needsTrigger;
        public string alternativeEnding;
        public Option()
        {
            option = new string[50];
            end = 0;
            pointer = 0;
            choose = new string[10];
            numberOfChoices = 0;
            name = "";
            consequences = new TextUnit[10];

            for(int i = 0; i < 10; i++)
            {
                consequences[i] = new TextUnit();
            }

            for (int i = 0; i < 50; i++)
            { option[i] = ""; }

            myHeart = 0; myMana = 0; JoshHeart = 0; JoshMana = 0; JoannaHeart = 0; JoannaMana = 0;
            aggressiveness = 0;
            activatesTrigger = "none";
            needsTrigger = "";
            alternativeEnding = "";
        }
        public void addText(string text) { option[end++] = text; }
        public void addChoice(string choice, string whatsNext)
        {
            choose[numberOfChoices] = choice;
            consequences[numberOfChoices++].setText(whatsNext);
        }
        public void addAlternative(int i, string alt)
        { consequences[numberOfChoices - 1].setAlternative(i, alt); }
        public bool hasAlternative(int i) { return consequences[i].hasAlternative(); }
        public int getNumbreOfAlternatives(int i) { return consequences[i].getNumberOfAlternatives(); }
        public string[] getChoices() { return choose; }
        public void setTrigger(string t) { activatesTrigger = t; }
        public string getTrigger() { return activatesTrigger; }
        public TextUnit getConsequence(int i) { return consequences[i]; }
        public void setNeedsTrigger(string t) { needsTrigger = t; }
        public string getNeedsTrigger() { return needsTrigger; }
        public int getNumberOfChoices() { return numberOfChoices;  }
        public void setNextStage(string next) { consequences[0].setText(next); numberOfChoices++;  }
        public void setName(string n) { name = n; }
        public string getName() { return name; }
        public void setAggressiveness(int a) { aggressiveness = a; }
        public int getAggressiveness() { return aggressiveness; }
        public bool nextIsEnd() { return (pointer + 1 == end); }
        public string[] getText()
        {
            string[] rez = new string[2];
            rez[0] = option[pointer++];
            rez[1] = "n";
            if(pointer == end)
            {
                rez[1] = "y";
                pointer = 0;
            }
            return rez;
        }
    }
}
