using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstorijosTestas
{
    class StoryAndPointer
    {
        private string story;       // istorijos gabalelis/santrupa, kuri rodom dabar ekrane (output)
        private string nextStage;
        private int pointer;        // istorijos option numeris "story" kintamajame. Kuriam option'ui priklauso isotrija
        public bool nextGetOptions;
        public StoryAndPointer()
        {
            story = "";
            pointer = 0;
            nextStage = "";
            nextGetOptions = false;
        }
        public void setStory(string s) { story = s; }
        public string getStory() { return story; }
        public void setPointer(int i) { pointer = i; }
        public int getPointer() { return pointer; }
        public void setNextStage(string n) { nextStage = n; }
        public string getNextStage() { return nextStage; }
    }
}
