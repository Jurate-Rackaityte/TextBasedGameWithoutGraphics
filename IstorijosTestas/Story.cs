using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstorijosTestas
{
    class Story
    {
        public int myMana, JoshMana, JoannaMana, aggresiveness;
        public int myHealth, JoshHealth, JoannaHealth;
        private Option[] story;
        private int optionPointer;
        private string stage;               //dabartinio stage vardas???
        private string previousStage;       //praeito stage vardas
        bool telekinesisKnown, metJosh, rebelSide, robotsAreComing, haveJoanna, MessedToEmpty, MysteryGirlDead,
            MysteryGirlMet, hadSeriousFight, endFight, leakingGas, JoannaAlive, JoshAlive;
        public bool helpOtherCountryToWin, theEnd;
        private string heroesName;
        private int robotFinalCount;
        private const int numberOfOptions = 123;

        public Story(string playerName)
        {
            heroesName = playerName;
            myMana = 100;
            JoshMana = 100;
            JoannaMana = 100;
            myHealth = 100;
            JoshHealth = 100;
            JoannaHealth = 100;
            story = new Option[numberOfOptions];            //JEI KAS, KEISTI SI NUMERI
            stage = "main01";
            previousStage = "";
            telekinesisKnown = false; rebelSide = true; robotsAreComing = false; metJosh = false;
            haveJoanna = false; MessedToEmpty = false; MysteryGirlDead = false;
            MysteryGirlMet = false; hadSeriousFight = false; endFight = false;
            leakingGas = false; JoannaAlive = true; JoshAlive = true; theEnd = false;
            optionPointer = 0;
            robotFinalCount = 10;
            aggresiveness = 0;

            for(int i = 0; i < numberOfOptions; i++)
            {
                story[i] = new Option();
            }

            initialiseOptions();
        }

        public int showMyMana() { return myMana; }
        public int showMyHealth() { return myHealth; }
        public bool nextIsTheEndOfOption(StoryAndPointer sp) { return story[sp.getPointer()].nextIsEnd(); }
        public bool nextHasOptions(StoryAndPointer sp)
        {
            return (story[sp.getPointer()].nextIsEnd() && story[sp.getPointer()].getNumberOfChoices() > 1);
        }
        public bool thisIsTheEnd() { return theEnd; }
        private bool iCanDoThis(int i)
        {
            if (JoannaMana + story[i].JoannaMana < 0)
                return false;
            else if (JoshMana + story[i].JoshMana < 0)
                return false;
            else if (myMana + story[i].myMana < 0)
                return false;
            else if (!JoannaAlive && story[i].getName().Contains("Joanna"))
                return false;
            else if (!JoshAlive && story[i].getName().Contains("Josh"))
                return false;
            return true;
        }
        public StoryAndPointer lastTextWithoutOptions(StoryAndPointer sp)
        {
            int x = sp.getPointer();
            activateTriggers(x);
            int tempMyMana = story[x].myMana;
            int tempMyHeart = story[x].myHeart;
            JoshMana += story[x].JoshMana;
            JoshHealth += story[x].JoshHeart;
            JoannaHealth += story[x].JoannaHeart;
            JoannaMana += story[x].JoannaMana;
            if (tempMyMana != 100)
                myMana += tempMyMana;
            else
                myMana = 100;
            if (tempMyHeart != 100)
                myHealth += tempMyHeart;
            else
                myHealth = 100;
            aggresiveness += story[x].getAggressiveness();
            robotFinalCount -= story[x].robotFinalDefeated;

            StoryAndPointer rez = new StoryAndPointer();
            //jei istorija neturi sekancio stage (reikia eiti i anksciau buvusi)
            if(story[x].getNumberOfChoices() == 0)
            {
                rez.setNextStage(stage);
                rez.setPointer(getStoryIndex(stage));
                rez.setStory(story[x].getText()[0]);
            }
            //jei istorija turi sekanti stage
            else
            {
                string nextStage = story[x].getConsequence(0).getText();
                rez.setNextStage(nextStage);


                rez.setPointer(getStoryIndex(nextStage));
                rez.setStory(story[x].getText()[0]);
            }
            
            return rez;
        }

        //PRIES IMANT OUTPUT, PATIKRINTI, AR OPTION NAME NERA ""
        //paimame paskutini teksta ir jame esancius options. pakeiciame manas, gyvybes, agresyvuma, robotu skaiciu
        //VYKDOMA TIK TUOMET, KAI ISTORIJA TURI OPTIONS (tikrinti: .nextHasOptions() )
        public StoryAndPointer[] lastTextAndOptions(StoryAndPointer sp)
        {
            int x = sp.getPointer();
            string[] temp = story[x].getText();     //dabartinis tekstas. Tikrinam, ar tai tikrai paskutinis tekstas
            // jei tai tikrai paskutinis pasirinkimas
            if(temp[1].Equals("y"))
            {
                stage = story[x].getName();     //nustatome stage i dabartini. Jei sekantis pasirinkimas neturi sekancio stage, tai grisime i sita
                // paskutinei istorijos eilutei ir visiems pasirinkimams
                StoryAndPointer[] rez = new StoryAndPointer[story[x].getNumberOfChoices() + 1];

                for(int i = 0; i < story[x].getNumberOfChoices() + 1; i++)
                {
                    rez[i] = new StoryAndPointer();
                }

                //aktyvuojam trigerius, perziurim manas, gyvybes, agresyvuma
                activateTriggers(x);
                int tempMyMana = story[x].myMana;
                int tempMyHeart = story[x].myHeart;
                JoshMana += story[x].JoshMana;
                JoshHealth += story[x].JoshHeart;
                JoannaHealth += story[x].JoannaHeart;
                JoannaMana += story[x].JoannaMana;
                if (tempMyMana != 100)
                    myMana += tempMyMana;
                else
                    myMana = 100;
                if (tempMyHeart != 100)
                    myHealth += tempMyHeart;
                else
                    myHealth = 100;
                aggresiveness += story[x].getAggressiveness();
                robotFinalCount -= story[x].robotFinalDefeated;

                rez[0].setPointer(x);
                string addStory = "";                   //jei reikes prideti daugiau story
                //jei mirsta Joanna ar Josh, tai pridedama ju mirimo eilute. Jei herojus mirsta, pridedama jo mirimo eilute + einama i pabaigos ekrana
                if (JoannaHealth <= 0 && JoannaAlive)
                {
                    int index = getStoryIndex("Joanna dies");
                    addStory += story[index].getText();
                    activateTriggers(index);
                }
                if (JoshHealth <= 0 && JoshAlive)
                {
                    int index = getStoryIndex("Josh dies");
                    addStory += story[index].getText();
                    activateTriggers(index);
                }
                if (myHealth <= 0)
                {
                    int index = getStoryIndex("I die");
                    addStory += story[index].getText();
                    activateTriggers(index);
                    rez[0].setNextStage(story[index].getConsequence(0).getText());
                }
                //pridedamas galutinis tekstas
                rez[0].setStory(temp[0] + " " + addStory);



                string[] choices = story[x].getChoices();
                int numberOfChoices = story[x].getNumberOfChoices();

                // jei istorija turi alternative ending (t.y. per viena is dvieju robo kovu), tuomet paziurim, ar kova baigesi. Jei taip, tai einam i alternative ending.
                if (!story[x].alternativeEnding.Equals("") && robotFinalCount <= 0)
                {
                    rez[1].setStory("End the fight");
                    rez[1].setNextStage(story[x].alternativeEnding);
                    rez[1].setPointer(getStoryIndex(story[x].alternativeEnding));
                    robotFinalCount = 20;
                    return rez;
                }

                // einam po pasirinkimus
                int ij = 1;
                for(int i = 0; i < numberOfChoices; i++)
                {
                    rez[ij].setStory(choices[i]);            //pasakome, kaip vadinsis mygtukas (bet kokiu atveju, mygtukas vadinsis taip pat, tik pasekmes gali skirtis)
                    if(story[x].hasAlternative(i))
                    {
                        int numberOfAlternatives = story[x].getNumbreOfAlternatives(i);
                        bool alternativeFound = false;
                        for(int j = 0; j < numberOfAlternatives; j++)
                        {
                            // patikrinti triggerius ir pagal tai pasirinkti alternatyva
                            string nextStoryName = story[x].getConsequence(i).getAlternative(j);        // surandam alternatyvos varda
                            int storyIndex = getStoryIndex(nextStoryName);                              // surandam alternatyvos indeksa story masyve
                            if (storyIndex != -1)
                            {
                                bool chooseThisStory = checkIfTriggerIsOn(storyIndex);                      // patikrinam, ar reikalingas triggeris yra ijungtas
                                if (chooseThisStory == true && iCanDoThis(storyIndex))
                                {
                                    // jei reikia rodyti si pasirinkima --- nextStory nustatyti i sitai
                                    rez[ij].setNextStage(nextStoryName);
                                    rez[ij].setPointer(storyIndex);
                                    alternativeFound = true;
                                    j = numberOfAlternatives + 10;          // jei radome tinkama alternatyva, kitu jau nebeieskom
                                    ij++;
                                }
                            }
                            else
                            {
                                Exception ex = new Exception("Neradau \"" + nextStoryName + "\" story masyve" + "\n");
                            }
                        }
                        // jei alternatyva taip ir nebuvo surasta
                        if(!alternativeFound)
                        {
                            string nextStageName = story[x].getConsequence(i).getText();
                            int tempIndex = getStoryIndex(nextStageName);
                            if(iCanDoThis(tempIndex))
                            {
                                if (tempIndex != -1)
                                {
                                    rez[ij].setNextStage(nextStageName);
                                    rez[ij++].setPointer(tempIndex);
                                }
                                else
                                {
                                    Exception ex = new Exception("Neradau \"" + nextStageName + "\" story masyve" + "\n");
                                }
                            }
                        }
                    }
                    else
                    {
                        string nextStageName = story[x].getConsequence(i).getText();
                        int tempIndex = getStoryIndex(nextStageName);
                        if(iCanDoThis(tempIndex))
                        {
                            if(tempIndex != -1)
                            {
                                rez[ij].setNextStage(nextStageName);
                                rez[ij++].setPointer(tempIndex);
                            }
                            else
                            {
                                Exception ex = new Exception("Neradau \"" + nextStageName + "\" story masyve" + "\n");
                            }
                        }
                        
                    }
                }
                return rez;
            }
            else
            {
                throw new Exception("Sitai nera paskutinis tekstas, bet atejo prie paskutinio...");
            }
            
        }
        //public StoryAndPointer chooseOption(string option, string prevStage)
        //{
        //    optionPointer = getStoryIndex(option);
        //    stage = option;
        //    previousStage = prevStage;
        //}

        public StoryAndPointer nextStory(StoryAndPointer sp)
        {
            //if(story[sp.getPointer()].getName().Equals(stage))              // jei tas pats option?
            //{
            //    optionPointer = 0;                          //wait, what?
            //}
            //else                                            // jei naujas option?
            //{
            //    stage = story[sp.getPointer()].getName();
            //    previousStage
            //}
            string[] temp = story[sp.getPointer()].getText();
            StoryAndPointer rez = new StoryAndPointer();
            rez.setStory(temp[0]);
            rez.setPointer(sp.getPointer());
            return rez;
        }

        // VISAD PATIKRINTI, AR ATS NERA -1
        public int getStoryIndex(string name)
        {
            for(int i = 0; i < story.Length; i++)
            {
                if (story[i].getName().Equals(name))
                    return i;
            }
            Console.WriteLine("Neradau story pavadinimu: " + name + "\n");
            return -1;
        }

        private void activateTriggers(int i)
        {
            string trigger = story[i].getTrigger();
            switch(trigger)
            {
                case "telekinesis is known":
                    telekinesisKnown = true;
                    break;
                case "met Josh":
                    metJosh = true;
                    break;
                case "robots are coming":
                    robotsAreComing = true;
                    break;
                case "have Joanna":
                    haveJoanna = true;
                    robotsAreComing = false;
                    break;
                case "no robots are coming":
                    robotsAreComing = false;
                    break;
                case "robot side start":
                    rebelSide = false;
                    break;
                case "Messed to empty":
                    MessedToEmpty = true;
                    break;
                case "Crazy girl met":
                    MysteryGirlMet = true;
                    break;
                case "Crazy girl dead":
                    MysteryGirlDead = true;
                    break;
                case "had serious fight":
                    hadSeriousFight = true;
                    break;
                case "Leaking gas":
                    leakingGas = true;
                    break;
                case "THE END":
                    endFight = true;
                    robotFinalCount = 20;
                    break;
                case "help other country to win":
                    helpOtherCountryToWin = true;
                    break;
                case "stay silent":
                    helpOtherCountryToWin = false;
                    break;
                case "end of story":
                    theEnd = true;
                    break;
                case "Joanna dead":
                    JoannaAlive = false;
                    break;
                case "Josh dead":
                    JoshAlive = false;
                    break;
                default:
                    break;
            }
        }

        private bool checkIfTriggerIsOn(int i)
        {
            string trigger = story[i].getNeedsTrigger();
            switch (trigger)
            {
                case "telekinesis is known":
                    return telekinesisKnown;
                    break;
                case "met Josh":
                    return metJosh;
                    break;
                case "robots are coming":
                    return robotsAreComing;
                    break;
                case "have Joanna":
                    return haveJoanna;
                    break;
                case "no robots are coming":
                    return robotsAreComing;
                    break;
                case "robot side start":
                    return rebelSide;
                    break;
                case "Messed to empty":
                    return MessedToEmpty;
                    break;
                case "Crazy girl met":
                    return MysteryGirlMet;
                    break;
                case "Crazy girl dead":
                    return MysteryGirlDead;
                    break;
                case "had serious fight":
                    return hadSeriousFight;
                    break;
                case "Leaking gas":
                    return leakingGas;
                    break;
                case "THE END":
                    return endFight;
                    break;
                case "help other country to win":
                    return helpOtherCountryToWin;
                    break;
                case "stay silent":
                    return helpOtherCountryToWin;
                    break;
                case "end of story":
                    return theEnd;
                    break;
                default:
                    return false;
                    break;
            }
        }
        // JEI NERANDA OPTION (ISTORIJOS DALIES)
        // RASYTI, KAD "ATLEISKITE, BET SI ISTORIJOS DALIS DAR NERA PASIEKIAMA, TAD NUKREIPIAME JUS I SIUO METU GALIMA ISTORIJOS PUSE"

        //NEPAMIRSTI PATIKRINTI AGRESYVUMO TASKU
        //IR HEALTH IR MANA

        // IR JEI ATLIKUS VEIKSMA VEIKEJO GYVYBES YRA 0 ARBA MAZIAU, REIKIA NUVESTI I MIRTIES PABAIGA (ARBA ATSAKA - JEI MIRE NE PAGRINDINIS VEIKEJAS)

        // SAUGOTI ANKSTESNIO OPTION INDEKSA, KAD GALETUMEME PASIIMTI JU OPTIONS (TOKIEMS KAIP "EMPTY CELL" OPTION'AMS REIKIA

        // PATIKRINTI TRIGGERIUS JEI LEAKING GAS, TAI AKTYVUOTI PARTICLE SCENOJE

        // JEI TAI YRA endFight (FINAL BATTLE), TAI PRIEJUS OPTION X TEKSTO GALA PATIKRINTI, AR JOANNA AR/IR JOSH YRA GYVI. JEI MIRE, TAI 
        //NUVESTI I JU MIRIMO OPTION Y IR TUOMET PATEIKTI OPTION X PASIRINKIMUS (CHOICES) (ta pati padaryti su mana)

        ///////////////////// JEI YRA FINAL BATTLE (endFight) IR ROBOTU SKAICIUS (robotFinalCount) YRA 0 AR MAZESNIS, TAI EITI I "Final. The end" OPTION
        //////////////////// JEI NERA FINAL BATTLE, BET ISTORIJA YRA 113-119, VADINASI, YRA ROBOTFIGHT IR VISIENA REIKIA TIKRINTI robotFinalCount

        private void initialiseOptions()
        {
            story[0].setName("main01");
            story[0].addText("They‘ve come again.");
            story[0].addText("Dark rooms, filled with wretched whispers. Giant eye, silently watching me...");
            story[0].addText("They all wanted me to feel miserable, alone, powerless, out of control... And so I’ve");
            story[0].addChoice("started to run", "01");
            story[0].addChoice("tried to fight them", "02");
             
            story[1].setName("01");
            story[1].addText("I ran deeper into the darkness, desperately trying to escape the giant eye. However, it’s gaze did not leave me for a second, petrifying me more and more with every step I took. Shadowy figures squealed behind me, restlessly chasing me. ");
            story[1].addText("My legs started to feel heavy. My pace reluctantly slowed down. Heart raced so quickly, it seemed it will jump out any second. Shadowy figures reached me, tied me, lifted my head. The huge drill was already there, ready to pierce through my skull. ");
            story[1].addText("The giant eye looked to the instrument getting closer and closer to my head. Suddenly I’ve felt the tip of the drill touch the forehead and an immense pain shuddered my body. Eyes wide open I’ve scream into nowhere and... and.. and...");
            story[1].setNextStage("main02");

            story[2].setName("02");
            story[2].setAggressiveness(1);
            story[2].addText("It’s either me or them. I’ve clenched my fists and turned to face the shadowy figures, ready to fight my way out. ");
            story[2].addText("The first creature came, reaching my head with it’s long hands. I’ve ducked and tried to punch it in the stomach.");
            story[2].addText("But when my fist reached his body, it went right through it. I’ve gasped in surprise, not believing my eyes. Meanwhile the creature repositioned himself and put his hands on my forehead.");
            story[2].addText("A strong headache trembled my body, a terrified scream filled the scene. I’ve tried to push the creature away from me, but once again my hands went right through its body.");
            story[2].addText("Other creatures came, putting their hands over my neck, shoulders, stomach, legs. Trembling convulsingly I’ve tried to push them away, screaming the words, I’ve never heard before. ");
            story[2].addText("Hopelessly, I’ve turned my head to the ceiling and saw it again.");
            story[2].addText("A giant eye, blinklessly watched me. The last convulsion shuddered my body, when I... when I...");
            story[2].setNextStage("main02");

            story[3].setName("main02");
            story[3].addText("... woke up.");
            story[3].addText("My body still trembles. I gasp for air in my capsule. Few warm teardrops run through my cheeks. ");
            story[3].addText("“It’s just a dream. It’s just a dream” – I tell myself.");
            story[3].addText("But tears still fill my eyes, preventing me from seeing clear. Feeling defeated, I huddle and wrap my arms around myself.");
            story[3].addText("A message on my SmartHelm appears. Tears prevents me from seeing it, but I am sure it is from RoboFriend, SmartHelm’s AI. It’s probably asking if everything’s alright.");
            story[3].addText("For the past few weeks some warning signs of helmetheria has shown up.");
            story[3].addText("Hemletheria is a disease of our future. We all live our day-to-day lives with a SmartHelm on. It has become a vital part of our lives. However, wearing too much of it causes some people to suffer helmetheria.");
            story[3].addText("Some even call such sick people an undeveloped organisms, since they can't adapt to the future that we live in.");
            story[3].addText("After all, being seen naked isn't as big of a shame as being seen without a SmartHelm. But this is exactly what people with helmetheria needs to do to get well.");
            story[3].addText("To prevent shame and social discomfort, the government created a special clinic for those, suffering from helmetheria, a place where they are seen only by other in the same condition.");
            story[3].addText("I've hoped that I will not need to ever know what it means to be without SmartHelm, but the condition has became uncontrollable.");
            story[3].addText("I take a deep breath and say to AI of SmartHelm.\n- RoboFriend, call the doctor. I need to heal and get back to this civilization as soon as possible.");
            story[3].setNextStage("main03");

            story[4].setName("main03");
            story[4].addText("I slowly open my eyes. Where am I? It’s so bright... I look around me. I’m in a capsule. But not in mine. And why does moving my head feels so strange, so... light...");
            story[4].addText("I gasp and touch my head. My SmartHelm... It’s gone. It’s completely gone. What? When? How? I expected to start panicking, but instead just an irritation fills my head. This is so... unnatural." );
            story[4].addText("I’m about to say for a RoboFriend to open the capsule when suddenly I realise, that I can’t go outside like this. I’m helmetless. ");
            story[4].addText("Shame washes over me, once again raising panic is unnaturally shut down. I’m in the clinic. That’s the only logical explanation.");
            story[4].addText("And now I am here all alone, with only robots to trust on. No outside communication, no endless opportunities to learn and entertain yourself. ");
            story[4].addText("With only some psychos around you. Psychos, who will see you helmetless, who will see you naked of your pinnacle of civilization. ");
            story[4].addText("I want to cry, but the tears are sucked up before I know it. Is this natural for a helmetless person? I want to find out, but I am afraid to get out of the capsule like this...");
            story[4].addChoice("sleep", "sleep1");
            story[4].addChoice("get out", "get out");

            story[5].setName("sleep1");
            story[5].addText("No, this is too much for me. I don’t want to be seen by others. I don’t want to meet anyone.");
            story[5].addText("A billion of thoughts pour over me. What will I do now? What will my relatives think about me? How about my friends? I promised to do this and that... How will they respond when they won’t find me?");
            story[5].addText("Though they are probably informed by now. How will I continue my life with everyone knowing that my genes are not evolved enough, that I’m not ready for the technology age.");
            story[5].addText("I spend the whole day swinging back and forth, thinking about what, if’s and why’s. I can’t force myself to get out of the capsule. I don’t want to face the reality behind these doors.");
            story[5].addText("So I stay there until I get tired and go to sleep. The following day...");
            story[5].addChoice("sleep", "sleep2");
            story[5].addChoice("get out", "get out");

            story[6].setName("sleep2");
            story[6].addText("I’ve decided to stay in my capsule again. I don’t need to know what happens in the outside world.");
            story[6].addText("I get the food from the side of the capsule. More reasons why it is not necessary to get out in the first place.");
            story[6].addText("I’ll stay with my shameful helmetless head here, where only I can know about it.");
            story[6].addText("A lot of thoughts attacked me, tearing me into different sides. But now I could cry and I cry quite a lot. I still can’t accept what happened.");
            story[6].addText("I end up getting tired from the dialogues and quarrels inside my head. I close my eyes to get some rest and end up sleeping for an unknown amount of hours.");
            story[6].addChoice("sleep", "sleeping shutdown");
            story[6].addChoice("get out", "get out");

            story[7].setName("sleeping shutdown");
            story[7].addText("Once again I wake up, deciding not to go anywhere. My breakfast comes from the side of the capsule. I eat it and return to my thoughts, continue the never-ending monologue.");
            story[7].addText("However, after some time something goes wrong. The light behind the opaque glass shuts down. I sit silently, trying not to breath. Will this be my end? ");
            story[7].addText("Wait... I can’t get out of the capsule if there is no electricity, isn’t that right? ");
            story[7].addText("A panic takes over me. What’s happening? Will I stay here forever right now? Maybe a clinic is shutting down, or there is a fire and everyone’s evacuating?");
            story[7].addText("Maybe they’ve took all other humans, but forgot to take me? And now I’ll die here, where I’ve been for the few days... or months... or years... or life... I can’t tell by now, how long has it been.");
            story[7].addText("A smashing sound hits the glass in front of me. I scream out of terror. Another smashing sound comes in, destroying the glass completely.");
            story[7].addText("I see a robot’s face in front of me. So they did remember me! They’re here for me!");
            story[7].addText("But then I see a man behind the robot. He lifts up some kind of a framed picture, ready to smash the robot. I...");
            story[7].addChoice("save robot", "robots side");
            story[7].addChoice("join human", "rebels side");

            story[8].setName("get out");
            story[8].addText("The room was bright, with three cells. It had an electric grid around each cell. Each cell had a bookcase, a TV, a chair and a picture of the country’s leader. Nothing abnormal.");
            story[8].addText("Next to the cells there is a RoboDoc, waiting for any input. I came closer to it.\n-	RoboDoc, where am I?\nRobot turned to me.");
            story[8].addText("- Good day, " + heroesName + ". You’re in the clinic. You’ve got a helmetheria, so you were put into hospital.\n-	When will I get out? Is my condition bad?");
            story[8].addText("- You’ll get out when a symptoms of helmetheria will fade away. So far you’re doing good, though I’m not sure if it’s your progress, or the depressants we gave to you to make the first day calm.");
            story[8].addText("My eyes pops out. So that’s why I’ve felt so strange back there?\n-	Uhm... I guess I needed that. Thank you. What I should do now? Is there anything interesting to do here?");
            story[8].addText("- Of course! – RoboDoc said. -  There’s a TV and a bookcase for your entertainment. I know it will not be the same as with the SmartHelm, but it is quite a good substitute.");
            story[8].addText("Please don’t judge such old technology. And I hope you’ll have a nice stay here. The dinner will be ready in a few hours. For your convenience it will show up in your capsule.");
            story[8].addText("Okay. So I have to find myself how to kill this time while I’m here. What should I do now?");
            story[8].addChoice("Watch TV","watch tv");
            story[8].addChoice("Read books", "read books");
            story[8].addChoice("Talk to the robot", "talk to the robot");
            story[8].addChoice("look around", "look around");
            story[8].addAlternative(1, "talk to Josh");
            story[8].addAlternative(2, "practise telekinesis");

            story[9].setName("watch tv");
            story[9].addText("I turn the TV on and sit in the arm-chair in front of it. A sound of rumbling and  disapproving mumbling comes from my back.");
            story[9].addText("It shows a documentary about the leaders of the country, how technology came and improved our lives, how the war with the neighbouring country started and still goes on.");
            story[9].addText("Once again I’ve felt proud and happy I am to live in this country. Our leaders are great, our values are strong, our will unbroken. We are the good, which in the end spread to this world.");
            story[9].addText("We are the ones who will make this planet full of happiness, truth, love. We are the ones who fight for the good of the people.");
            story[9].addText("The moves shown were so interesting that I spend the whole day watching them. Another day...");
            story[9].addChoice("Watch TV", "watch tv");
            story[9].addChoice("Read books", "read books");
            story[9].addChoice("Talk to the robot", "talk to the robot");
            story[9].addChoice("look around", "look around");
            story[9].addAlternative(1, "talk to Josh");
            story[9].addAlternative(2, "practise telekinesis");

            story[10].setName("read books");
            story[10].addText("I turn towards the bookcase. There are quite a lot of books there. I grab the first that there is and sit into the arm-chair. I hear a snare of despise from my back.");
            story[10].addText("It turns out it’s a history book. I’ve always liked history and hearing all those stories of how bad it was in the past, how we were oppressed and used and how good it is now.");
            story[10].addText("Reading all of the stories of how we finally understood the truth, fought for it and won. How we struggled with all the inventions until the miraculous nowadays technology came in.");
            story[10].addText("It really makes me feel great. I feel like we, humanity, did so much, we improved so greatly. We are victorious and better than ever before.");
            story[10].addText("Even though it was hard reading a real book at first (wait till my friends hear about this! Real book! Wow...), after some time I got used to it.");
            story[10].addText("I’ve finished the book in what seemed like a couple of hours, but from the silence around me and tiredness it looks like it’s already past midnight.");
            story[10].addText("I put the book back into the bookcase and go to sleep. Next day...");
            story[10].addChoice("Watch TV", "watch tv");
            story[10].addChoice("Read books", "read books");
            story[10].addChoice("Talk to the robot", "talk to the robot");
            story[10].addChoice("look around", "look around");
            story[10].addAlternative(1, "talk to Josh");
            story[10].addAlternative(2, "practise telekinesis");

            story[11].setName("talk to the robot");
            story[11].addText("I miss the days I could freely browse through internet for some random facts and funny things. To fill the emptiness of this longing I turn to the RoboDoc.");
            story[11].addText("After a few requests he agrees to tell me anything I want to want, in this way acting like an internet itself.");
            story[11].addText("At first it tells me all the latest news. Than with my jumping mind and the possibilities of getting all the possible answers we talk about the best diet, the green movement, ...");
            story[11].addText("... how many polar bears there in Antarctica, 10 steps to become a better citizen, the pros and cons of staying single, when and where will my favourite music band will perform, and so on.");
            story[11].addText("I talk with him for hours when it suddenly tells me that it is recommended to get back to sleep if I want my skin to look pretty. I thank him and go to capsule. Next day...");
            story[11].addChoice("look around", "look around");
            story[11].addAlternative(1, "talk to Josh");
            story[11].addAlternative(2, "practise telekinesis");
            story[11].addChoice("Watch TV", "watch tv");
            story[11].addChoice("Read books", "read books");
            story[11].addChoice("Talk to the robot", "talk to the robot");

            story[12].setName("look around");
            story[12].addText("I look around myself. There is one neighbour in front of my capsule, sitting in his arm-chair. Neighbour in another cell still haven’t come out of his capsule.");
            story[12].addText("There is a painting of the country’s leader on the wall. I go look at his picture closer, when I hear the silent voice:");
            story[12].addText("- Psst, hey, new kid. What’s your name?");
            story[12].addText("I look towards the voice. The neighbour turned to me in his arm-chair, closely looking at my side.\n-	Um... I’m "+ heroesName +". And you are?...");
            story[12].addText("- Josh, - he whispers. He grins devilishly. – Saw you get into your new home yesterday. Must be tough living a new life.");
            story[12].addText("Suddenly a man jumps out of his chair and looks towards the exit, where the RoboDoc is. A man returns his gaze to me. His eyes still looks scared, but he quickly recompose himself and looks with the same confident grin.");
            story[12].addText("- So, how’s it going in your mind? You’re in asylum after all. One of the few places in this country where you can hear your own voice.");
            story[12].addText("The last words he excitedly whispered, and nervously looked towards the doors again. Hear my own voice ? What does he mean ? I could always hear my own voice.");
            story[12].addText("Oh, poor fellow. He probably was one of those people, who has spent all their days on the internet, believing everything they were told. And now, when he was left all alone with his thoughts he had to face a new reality, had to hear and form his own opinions.");
            story[12].addText("Of course life offline must have been hard for him.\n- I can see it, you know, - he silently whispers. – You judge me right now. Think I am some crazy man. I was just like you some time ago. But everything changed. They will change you.");
            story[12].addText("- What the hell are you talking about?\n- Oh, I’m talking about the truth.");
            story[12].addText("He jumps again and looks to a RoboDoc. But the robot seem to not care about him. Instead, it is closely examining the neighbour in the capsule. Neighbour turns his gaze towards me again an quickly whispers:");
            story[12].addText("- But after that some other time, I have to go now.\nJosh sits back into his arm-chair, turning his back on any outside world. He hugs his knees and starts rocking back and forth, whispering something to himself.");
            story[12].addText("I shrug my shoulders and go into my capsule and think more about the situation I am in. The new sensation of not wearing a helmet really feels tiring and boring. I close my eyes with the hope that the next day will be better.");
            story[12].addChoice("talk to Josh", "talk to Josh");
            story[12].addChoice("Watch TV", "watch tv");
            story[12].addChoice("Read books", "read books");
            story[12].addChoice("Talk to the robot", "talk to the robot");
            story[12].setTrigger("met Josh");

            story[13].setName("talk to Josh");
            story[13].addText("Today I’ve decided to go talk with Josh again. I get out of the capsule to find him curled up in his armchair. \n“Josh?” – I say. – “Do you have a moment?”");
            story[13].addText("Josh turns to me, a satisfied smile on his lips. “Oh, well, looks who’s back.Wait a minute.”");
            story[13].addText("He then turns to a robot and starts whispering something while wiggling his fingers. Then loudly exhales, stretching his fingers far apart, showing his palms to the robot.");
            story[13].addText("The robot turns from us into a picture of our country’s leader, seemingly ignoring Josh’s craziness. I turn to my neighbour again with one eyebrow raised.He notices me and says:");
            story[13].addText("“I’m not crazy. Trust me. This is the only way we can talk.”\nSomething in his voice seems different.Calmer, more relaxed, confident... sane.");
            story[13].addText("I quickly blinked my surprised eyes. “Why?” – I ask.");
            story[13].addText("“Because they’re watching, hearing, recording and analysing. Because if they knew what we know, they will force you out of here and torture you with their truths.”");
            story[13].addText("“You may have noticed it by now”, - he continues. – “But you work differently without a helmet. You think differently. You are a different person.”");
            story[13].addText("Josh sees my disbelief, so he adds: “Oh, come on, weren’t things different with a helmet? Whenever a bad thought comes in, it stays. It will not come back. You can stick inside your capsule for days,");
            story[13].addText("without doing nothing, but being depressed, crying more than one teardrop. And nothing calms you, no thought will come to your mind to get you back on your feet and tell you what to do.”");
            story[13].addText("I want to disagree with him, even though he describes exactly how I’ve felt every night before going to sleep. Something deep in the corner of my mind feels that everything he is talking is truth.");
            story[13].addText("This feeling scares me, so I just listen to him with my eyes popped out, mouth open from surprise. No, this can’t be truth... No... But what if...");
            story[13].addText("“You’re all by yourself,” – he continues, - “alone and vulnerable. Yet you have never been freer. In fact, this is probably the first time you’re actually tasting freedom.”");
            story[13].addText("No, it’s not true --- wait, this is a strange thought. My mind starts to buzz with different ideas and arguments. I put my hands on my forehead, trying to grasp them again into order.");
            story[13].addText("“Funny, isn’t it?” – Josh continues. – “You’re in the prison, where you can’t even get out of your little cell, yet this is the only freedom you have in this fucked up society.”");
            story[13].addText("“No, “– I whisper. My voice was shaking, but it gets louder and louder. – “No, no, no, no, NO!”");
            story[13].myMana = -100;
            story[13].addText("I quickly turn my head to a wall, which made a cracking sound somewhere in my brain. Eyesight gets dark for a second. A swooshing noise comes just next to my ear.");
            story[13].addText("“Ouch!”\nI lower my hands and turn to see Josh grasping his left arm.Next to his toes a book lays.\n“What did you do this time ?” – I ask.");
            story[13].addText("Josh laughed. “Me? Hahaha, no, dear, it was you. I just didn’t know you’ll be so strong... and rare”. He stares at me with great curiosity and amusement in his mind. I narrow my eyebrows. This whole situation just irritates me. I start to feel a mild pain in the back of my brain again.");
            story[13].addText("“Me? I’ve done nothing!” – I say, but then my eyes catch a movement near the ground. The laying book started floating and headed right to Josh’s jaw. I gasp and the book stops two centimetres to my neighbour’s face.");
            story[13].addText("“This...” – he whispers. – “Is very rare...”\nThe book drops to his feet.I switch my eyes from the book to the Josh’s eyes.");
            story[13].addText("“What... What was that?” – I ask. \n“Telekinesis, my dear neighbour” – Josh responds. – “A rare and valuable gift.If you learn to use it well, that is.”\n“Te-te-telekinesis?” – I gasp. Josh sees my shock and explains further:");
            story[13].addText("“You see, long ago, this all future technology, which nowadays is adored and praised, caused humans a different kind of mutations. Some has gotten a third eye. Some were left with four arms. Some began to change the magnetic fields of surrounding area to their will,");
            story[13].addText("meanwhile others could imply any wanted image, mirage to the other person’s mind. Some were able to read minds, while others’ touch caused a bodies to melt. Some could become invisible to their own will, and others,");
            story[13].addText("such as you, have gotten a power of telekinesis. Everyone had a different power. But someone didn’t like people having such powers. So the whole knowledge about such abilities were erased from the public.”");
            story[13].addText("I wrinkled my eyebrows. “This must be non-sense. No one could simply erase people’s memories and control such a huge crowd. After all, wouldn’t people re-discover their abilities again? ”");
            story[13].addText("Josh smiled. “I don’t know how it was erased either. Another mutant power? Or maybe the new technology? After all the all mighty and glorified smart-helm could be a good tool to keep people away from their true identities and gifts, don’t you think?”");
            story[13].addText("I blink. No, it can’t be...");
            story[13].addText("Strange images appear in my mind. College. Friends. Fireworks. Books flying all around me. Shadows running back and forth. A bucket with a red paint floating away in the air. A dark tunnel. The dreams I had before I got here.");
            story[13].addText("No, it can’t be... I feel my body get all tensed up. I grip my palm, my breath got quickier. No, it can’t be, but... Josh’s voice disturbs my thoughts.");
            story[13].addText("“Wow! Now this is a once in a lifetime view.” I look around me. Books from my, Josh’s and our neighbour’s cells floated around the room.");
            story[13].addText("“I think I’ve misjudged you the first time I saw you.” – Josh continued. – “But you have to tune it down a little. You don’t want any robot to see you do your tricks.”");
            story[13].addText("He nodded into the robots side. Surprisingly, it was still looking into the leader’s portrait. I try to relax my muscles a bit and all the books slowly floats down to the ground. I turn to Josh.");
            story[13].addText("“The robot doesn’t move. Your job?” Josh grins proudly.");
            story[13].addText("“Yeap. Got me some time to figure this out. But it seems I am able to change or create a magnetic field wherever I want and see. Once they caught me practising and put me into rehabilitation. But after a few days I’ve managed to trick them that I’m okay.");
            story[13].addText("However, I can’t do it for a long time. Gifts have their own drawbacks. It takes mental power. Many like to call it mana. Bloody gamers. However, it does make it sound like just a game and this pretending may save your life in this society.”");
            story[13].addText("“Mana could be refilled in two ways,” – Josh continues. – “Either by having a good night’s sleep or by meditating. But for now you might want to practice more.”");
            story[13].addText("I gulp. This is just so strange. Just yesterday I were a normal citizen, no different from anyone else. And now I am convinced that I can throw objects merely by my will, without moving a finger.");
            story[13].addText("It all seems scary, but something in the back of my mind says I must discover more. I want to know what happened. For how long do I have this “gift”? Why was it hidden from me? What more secrets hides in the corners of this asylum?");
            story[13].addText("I nod and our training begins. After a few hours of tossing books back and forth, trying to concentrate and channel my energy to objects for them to fly wherever I get tired.");
            story[13].addText("Thankfully, Josh also gets tired of holding the robot in one place without electricity, so we call it a day. I get back to my cell, eager to know what else can I do tomorrow with my powers. The following day I... ");
            story[13].addChoice("Watch TV", "watch tv");
            story[13].addChoice("Read books", "read books");
            story[13].addChoice("Talk to the robot", "talk to the robot");
            story[13].addChoice("Practise telekinesis", "practise telekinesis");
            story[13].setTrigger("telekinesis is known");
            story[13].setNeedsTrigger("met Josh");

            story[14].setName("practise telekinesis");
            story[14].addText("I decide it is time to practice my telekinesis powers again. For so long I haven’t even knew I had them and now, when I finally know, I want to learn as much as I can about them.");
            story[14].addText("Josh turned off the robot for me, saying it is necessary and that I do not want to know what happens, when they’ll see what I can.");
            story[14].addText("This time after hours of practising with books I’ve also tried heavier objects. I could manage to lift a bookshelf, but a capsule was too heavy. I’ve raised it only few centimetres up, but after a few seconds it went down.");
            story[14].addText("Still I was happy. It seems that with telekinesis I can lift objects, which would be hardly movable for me with my hands. And I also managed to lift and track few objects at a time.");
            story[14].addText("Few hours later I was too tired to do anything else. Josh complimented for my efforts, praising the rarity of my gift. I wonder, how many other people with powers he has encountered.");
            story[14].addText("The robot was turned on again and I were too tired to do anything. So I went back to my capsule.");
            story[14].setNextStage("main04");
            story[14].setNeedsTrigger("telekinesis is known");

            story[15].setName("main04");
            story[15].myMana = 100; 
            story[15].addText("Today I wake up, get out of the capsule, and sit in the armchair. I decide that I need some time to ponder on what happened to me.");
            story[15].addText("Just a week or so I was petrified of the idea of getting my smart-helm off and be in the clinics. Yet now I feel more confident. It is not that bad. I’ve got to know new things, spend my time doing multiple interesting things.");
            story[15].addText("I still miss my smart-helm. It’s ability to play whatever song I want. Browse the internet, get to know new facts, receipts, ideas. Socialise with friends and family.");
            story[15].addText("But in a way this state I am in now seems... better. What is it that I like about this place? I keep wondering about all the experiences I had here, when suddenly...");
            story[15].setNextStage("main05");

            story[16].setName("main05");
            story[16].addText("The light turns black.");
            story[16].addText("I blink few times. “Wait... what?” – I whisper into darkness. Few seconds later robot turns his eyes’ flashlights.");
            story[16].addText("“Everyone, calm do...” – the robot started to say, but then Josh realised what just happened and jumped on the robot, attacking him.");
            story[16].addText("We were free. Our cells were no longer a cell. Without electric grid, we were merely two people and a robot in one big room.");
            story[16].addText("I look into a started fight between the robot and Josh the grunting crazy-looking neighbour. I need to interfere or they will kill one another. And so I:");
            story[16].addChoice("Save the neighbour", "rebels side");
            story[16].addChoice("Save the robot", "robots side");

            story[17].setName("rebels side");
            story[17].addText("A man is more important than any robotic machine. And so I:");
            story[17].addChoice("throw a robot to the wall (-20 mana)", "rebels side 01");  // +2 agg
            story[17].addChoice("throw books to a robot (-10 mana)", "rebels side 02");     // +1 agg
            story[17].addChoice("Grab the leader’s portrait and hit robot’s head with it", "rebels side 03");   // +2 agg
            story[17].addChoice("Try to grab robot’s arms, making it unable to do more damage", "rebels side 04");

            story[18].setName("rebels side 01");
            story[18].setAggressiveness(2);
            story[18].addText("I focus my energy on the heavy robot’s body and imagine it being thrown into the nearby wall. A second later the image becomes a reality. Robot’s body lifts and swooshes to the nearby wall like a lifeless mannequin.");
            story[18].addText("A metallic crushing sound fills the room for a second, but then a silence follows. Josh gets up, still breathing heavily, and turns to me with narrowed eyebrows.");
            story[18].addText("“Why haven’t you used your powers to disable it? You could take this robot out in a few seconds yourself” – I ask. Josh grunts.");
            story[18].addText("“It takes too much energy” – he answers. – “And there’s quite a lot of robots on our way to freedom, kid. If you were a bit smarter, you wouldn’t have used your gifts so easily either.”");
            story[18].addText("I roll my eyes. Come on, is this is the only “thanks for saving me” he can give?");
            story[18].setNextStage("rebel main 01");

            story[19].setName("rebels side 02");
            story[19].setAggressiveness(1);
            story[19].myHeart = -5;
            story[19].addText("I quickly focus my energy on the laying books around us and use my newly found telekinesis power to lift them all up and throw them into the fighting robot, hoping it will be enough to disable it.");
            story[19].addText("Unfortunately, I miscalculate their trajectory and few books hits Josh too. He stumbles to the ground, while the robot, reaching his way out of the flying and punching books, gets up and starts crawling to my direction.");
            story[19].addText("I desperately keep throwing books back and forth to his head. But no matter what, the robot kept crawling to my side. Frightened, I step back. My breathing stops, I can’t think what to do.");
            story[19].addText("Suddenly a metal clang hits my ears and a robot falls forward hitting my stomach. I gasp from the pain as I see Josh getting back up and kicking the robot again to the ground. After a few kicks the robot stops moving.");
            story[19].addText("Josh turns to me: holding my stomach, still looking shocked. He grins. \n“Guess we’re equal now.” – he says.I turn to him.\n“Sorry for hitting you.” – I apologise.");
            story[19].addText("“No worries, kido.” – he says. – “Just try not to waste your gifts so much. There’s quite a lot of robots waiting for us ahead. We sure have little rest, but a lot of fun ahead of us.”");
            story[19].addText("I stare down to the determined smile on his face and my shoulders relaxes a bit. Maybe together we actually can do it.");
            story[19].setNextStage("rebel main 01");

            story[20].setName("rebels side 03");
            story[20].setAggressiveness(2);
            story[20].myHeart = -10;
            story[20].addText("I grab a country leader’s portrait hanging on my left and hit it right into the robots head. The picture goes right through the metallic head, but the strong wooden frame around it hits Josh. I watch in shock as he falls onto the floor.");
            story[20].addText("I should have thought this through a bit more. Robot turns his head towards me, ready to leap forwards. I pull the frame towards myself, so that it hits the stuck metallic head. The punch leads the robot onto the floor.");
            story[20].addText("I lift the frame and punch the robot several times to make sure that he’s dead. When metalic layers of the humanoid face started to go away and the camera-like eyes got broken, I finally put down the leader’s picture.");
            story[20].addText("I exhaled relieved and turn my head to Josh. With one hand on his head, he slowly gets up and unsteadily walks towards me.\n“Are you o..” – I start asking, when he swings a punch right into my face.");
            story[20].addText("“Watch. Where. The hell. You’re hitting” – he grunts.\nI lower my eyebrows. “Sorry.But at least I’ve saved our lives.”\n“I could save it myself!” – Josh shouts. – “But instead you’re wasting our health, hitting wherever you please.”");
            story[20].setNextStage("rebel main 01");

            story[21].setName("rebels side 04");
            story[21].myHeart = -15;
            story[21].addText("If only we could somehow safely turn the robot down or tie him up... But there’s no time for that in the middle of the fight. The robot is just about to punch Josh in the face again, when I grab its arms from the back.");
            story[21].addText("Taking the advantage of his opponent’s sudden stop, Josh punches the robot’s head. The metallic humanoid leans back, hitting my face. My nose gets pushed by the robot’s neck, its touch with something made of rubber makes a clicking sound. Forehead gets punched by the rest of the metallic head.");
            story[21].addText("I scream out of suddenness and pain. The humanoid in my arms collapses. \n“Oh, crap.” – Josh curses. – “You alright ?”");
            story[21].addText("I check my nose... but it seems fine. \n“Yeah, kind of...” – I answer, wondering.");
            story[21].setNextStage("rebel main 01");

            story[22].setName("rebel main 01");
            story[22].addText("I loudly exhale.\n“So... what do we do now ?” – I ask.\n„We run, of course“ – Josh says. I turn to the doors.");
            story[22].addChoice("Go through the doors in front of you", "go straight 01");
            story[22].addChoice("Go through the doors on your right", "turn right 01");

            story[23].setName("go straight 01");
            story[23].addText("We face a crossroad. It seems that there are some doors across me. To my left and to my right there is only a dark corridor.");
            story[23].addChoice("Go straight", "Elevator 01");
            story[23].addChoice("Turn left", "turn left 01");
            story[23].addChoice("Turn right", "turn right 02");

            story[24].setName("Elevator 01");
            story[24].addText("I go straight the corridor and see what is behind the door. However, the doors are stuck and do not open.");
            story[24].addChoice("Use telekinesis to open the door", "Open broken elevator");
            story[24].addChoice("Go back", "go back 01");
            story[24].addAlternative(1, "Straight 03");

            story[25].setName("Open broken elevator");
            story[25].addText("I focus my mental energy and use telekinesis to swing the door open. It is too dark to see, what is inside, so I go in to quickly reach the metallic wall.");
            story[25].addText("“Hm, that’s strange” – I say, while searching what else is here in this tiny room. Josh starts searching too. Shortly after that, I hear a clicking sound behind me. ");
            story[25].addText("“It’s an elevator” – Josh says. – “But it is not working without electricity. We should search for some stairs.”");
            story[25].setNextStage("go back 01");
            story[25].addAlternative(1, "Straight 03");

            story[26].setName("go back 01");
            story[26].addText("We get back and are met in the same crossroad. Suddenly Josh’s body tenses up.\n“Shhh” – he whispers.I listen closely.But my ear catch nothing just a silent buzz somewhere far away.");
            story[26].addText("“They’re coming” – he whispers. – “Quickly, come this way.”\nWe turn to the left.");
            story[26].setTrigger("robots are coming");
            story[26].setNextStage("turn right 02");

            story[27].setName("turn right 02");
            story[27].addText("The dark corridor turned quickly to the left, then to the right. We went through the darkness until we met two doors in the opposite directions. We could also continue go through the corridor.");
            story[27].addChoice("Go through the doors on the left", "Cell 01");
            story[27].addAlternative(1, "Messed room 02");
            story[27].addAlternative(2, "Empty cell"); 
            story[27].addChoice("Go through the doors on the right", "Humans");
            story[27].addAlternative(1, "Room with people");
            story[27].addAlternative(2, "Humans 02");
            story[27].addChoice("Continue going through corridor", "Go straight 02");

            story[28].setName("Go straight 02");
            story[28].addText("We ignore the doors near as and go further to the corridor. Unfortunately, we meet the similar situation. There are two doors on both our sides and one door in front of us. It seems that the corridor still goes further though...");
            story[28].addChoice("Go through the doors on the left", "Cell 01");
            story[28].addAlternative(1, "Empty cell");
            story[28].addAlternative(2, "Empty cell. With Joanna");
            story[28].addChoice("Go through the doors on the right", "Room with people");
            story[28].addAlternative(1, "Humans");
            story[28].addAlternative(2, "Humans 02");
            story[28].addChoice("Go through the doors in front of us", "Empty cell");
            story[28].addAlternative(1, "Run 01");
            story[28].addAlternative(2, "Empty cell. With Joanna");
            story[28].addChoice("Continue going through the corridor", "turn right 03");
            story[28].addAlternative(1, "Run 01");

            story[29].setName("Run 01");
            story[29].myMana = -35;
            story[29].setNeedsTrigger("robots are coming");
            story[29].addText("We run forward, hearing the noise of the robots coming from our backs.\n- Stop! – one robot shouts. A short lightning crosses the corridor, making a smashing sound on the doors of the room ahead. Another one passes just through my left.");
            story[29].addText("- Aaaaah!, - Josh screams. I look at him, holding his right arm. I turn with terror to the coming robots.Their eyes turned into a flashlights, they looked straight into us.How mush there were ? Three ? Four ? Five ? It seemed that they’ve kept getting back.");
            story[29].addText("- Surrender peacefully and you will not be harmed. – a robot said.\n- To hell with it! – Josh shouts, as he starts channelling his energy towards the army.");
            story[29].addText("The robot raises his arm again, getting ready to shoot. I quickly focus my energy to it and flip the robot upside down.\n- Damn it, -Josh curses again. – I’ve just finished disabling it.Target someone else, " + heroesName + ".");
            story[29].addText("Just then a door on our left swings open, hitting one of the robots. A woman with two swords jumps out, making growling sounds. She attacks the robots. Near her, a small ball of fire appears, making the corridor bright.");
            story[29].addText("I gasp with wonder, as a woman quickly destroys three of the robots. One robot is about to hit her in the head, when another fireball from the opened room appears and hits the attacking robot.");
            story[29].addText("I concentrate my energy and smash another robot to the wall. Another robot suddenly gets stunned. “Josh’s work” – I think to myself.");
            story[29].addText("In just few minutes, the robots were done and over. The woman turns toward us, and just then I’ve notice that she did not have any swords with her. Her arms were the swords themselves.");
            story[29].addText("- Who are you? – the woman speaks.\n- We were the prisoners.And since the power shut down... -I hesitate for a second.\n- We are runaways. – Josh ends my sentence. – Fellow humans.\n- Good.Come with me.");
            story[29].addText("She shows towards the door she just rushed in through and so we follow her.");
            story[29].setNextStage("resolution");

            story[30].setName("Cell 01");
            story[30].myMana = -15;
            story[30].setNeedsTrigger("robots are coming");
            story[30].addText("We quickly open the doors on our left and run into it. The room is so dark, we can’t see anything, though near the wall on my right I grasp a picture frame.");
            story[30].addText("The noise from the robot’s footsteps proves the Josh’s theorem to be true. It seemed that a whole troop came this cells’ sector. Josh and I quickly lean to the doors to hold them in case the troops will come here.");
            story[30].addText("The sound of opening doors and more robots footsteps are heard.  I and Josh look to each other. If they all will try to break the door, I doubt we could handle it. Moreover, to defeat them all.");
            story[30].addText("But then another opening of the door and an animal-like growling reaches our ears. Smashes, scratches, rattle. What is going on there? Suddenly a humanly scream echoes through the dark. There are humans!");
            story[30].addText("Before I know it, our doors flings open. A lightning struck my eyes. A woman with two swords turns to see who caused the noise and gets punched in the face by a robot. She lets a growl.");
            story[30].addText("I quickly use my powers to smash the robot to the wall. Another robot suddenly stops moving, proving the presence of Josh’s powers. The woman quickly regains herself and hits the stunned robot with the sword.");
            story[30].addText("The fireball from my right flies to the last robot from the troop. I turn to see who caused it and see a girl, not older than 15 years, looking through the doors. The woman turn to us.");
            story[30].addText("- Thank you. Haven’t expected more humans to come here. Follow me.\nShe comes to the doors, where the girl is waiting for her.Without a hesitation, we follow her.");
            story[30].setNextStage("resolution");

            story[31].setName("Room with people");
            story[31].setNeedsTrigger("robots are coming");
            story[31].addText("We rush in through the doors and find around 12 people there. In the back of the room, the fire near the bookshelf is burning. A woman in a green dress approaches us, her body showing confidence and strength.");
            story[31].addText("- Who are you? – she asks. – Where did you come from?\n- There’s no time for that, -Josh quickly interrupts. – The robots.They’re coming.\nPeople’s bodies stiffen.Some seems scared, others – fierce.");
            story[31].addText("- Then we must act quickly. – the woman in the green dress says. – Joanna, come here.\n- Gladly. – a girl, who seems not older than 15 years old, replies.");
            story[31].addText("- We can help. – I say. – We have mental powers which can help you out. Josh can stun robots for a second by causing some kind of chaos with the magnetic fields. And I can:");
            story[31].addChoice("Make them levitate and make no harm", "People.Fight.Stun");
            story[31].addChoice("Throw and smash them up in the wall", "People.Fight.Smash");   // +2 agg

            story[32].setName("People.Fight.Stun");
            story[32].myMana = -20;
            story[32].addText("- Okay, - the woman replies, - together we may defeat them. Let’s go.\nWe went through the door to meet 8 robots troop.");
            story[32].addText("- Oh my... - I whispered. A woman did not hesitate. She let out a grunt and started attacking the robots with two swords. Wait, swords? How did she get them? Just then one of the robots swings its arm, ready to punch me. But suddenly it stops.");
            story[32].addText("- Don’t just stand around! – Josh shouts. – Use your powers!");
            story[32].addText("I regain myself and concentrate. Three robots flies into the air. I throw them in the end of the corridor, so they would not interfere right now. In the meantime, a fireball swooshes past me and hits the metallic humanoid next to me. I turn to meet Joanna’s, the girl who went with us, smile.");
            story[32].addText("- Wow, - I whisper and turn back to the fighting scene. The three thrown robots came back, but by that time every other metallic humanoid were destroyed by the woman in the green dress. ");
            story[32].addText("She jumps to meet them too. One enemy suddenly stops moving. I throw the other one again back to the end of the corridor. Woman quickly finishes the third robot and the stunned one. A ball of fire hits the thrown robot. ");
            story[32].setNextStage("Room with people. End");

            story[33].setName("People.Fight.Smash");
            story[33].myMana = -25;
            story[33].setAggressiveness(2);
            story[33].addText("- Excellent. – the woman smiles. – Let’s go then. We went through the door to meet 8 robots troop.I quickly concentrate and smash the first one to the wall.The woman draws two swords and defeats the second robot.");
            story[33].addText("Without waiting anyone’s command, Josh stuns a third robot. A ball of fire hits the fourth robot. Few minutes later all robots are defeated.");
            story[33].setNextStage("Room with people. End");

            story[34].setName("Room with people. End");
            story[34].addText("As we return to the room I look at the woman’s swords and see that they were not held by the arms -- they were the only arms she had. I gasp.");
            story[34].setNextStage("resolution");

            story[35].setName("resolution");
            story[35].myMana = 100;
            story[35].myHeart = 100;
            story[35].addText("The woman notices me staring at her arms and grins. Then, in front of my own eyes, I see how those swords turn into the gentle looking human hands.\n- What in the world... -I start, but the woman interrupts, answering my question:");
            story[35].addText("- I can morph my body into any shape and material I want. I’m Dominic, by the way. My jaws drop.\n- Wait, isn’t that a man’s name ? – I ask.She laughs.\n- Well, yes, it is. As I said – morph into any shape. As I keep standing there, stunned by her words, Josh starts to laugh himself.");
            story[35].addText("- A sword to survive among robots and a woman’s body to fight your way amongst people. Pretty smart, I would say.\n- It’s not like that. – Dominic fires back with a wry smile. – Anyway, what’s your name and what are your plans ?");
            story[35].addText("- I’m Josh. And this is " + heroesName + ". We are planning to get out from this asylum. What else there is to do here?");
            story[35].addText("- Get out? – Dominic replies. – And then what? You do know that behind these walls lies a world, in which you without a helmet are no one worth mercy or life. And you with a helmet are no one, but a faceless robot in the crowd. A pleb, who have forgotten who he is, who follows orders and has no will.");
            story[35].addText("If you are seen without helmet you will either die bodily or psychologically. There is no escape. There is nowhere to run to, so there is nowhere to get out of.");
            story[35].addText("- So you’re planning to stay here? – Josh raises his voice. – You’re planning to wait for a bigger recruitment, for electricity to turn on and put you into prison again? There’s no other way. You either fight, or you die.");
            story[35].addText("- I’ve seen enough of death today. – Dominic cuts off Josh’s rant. – We’ve lost 4 good men in this battle already. If you want to go to your death, go. But we will battle for our lives here. Though you are welcome to rest here if you want. Someone as powerful as you would be in handy here.");
            story[35].addText("Josh starts murmuring to himself, but takes a seat near a fireplace. I sit next to him. A girl, not older than 15, approaches us.");
            story[35].addText("- I agree. – she whispers. – Dominic is scared. He has forgotten how the other world looks like. He would not be accepted there and probably would be sent to death. That’s the world that’s waiting for him outside these walls either way.");
            story[35].addText("But this is not how it might turn out for us. My name’s Joanna and I’m sick of these walls. I want to escape. Will you accept me to your crew? I can provide the fireballs as the light source in darkness or as a deadly weapon in battle.");
            story[35].addText("- Sure. – I reply, – But first let’s rest. There’s probably a big journey up ahead of us. And we don’t know when will be the next time to replenish our mana.\nGirl nods in agreement.We patch our injuries up, get a good nap and get out of the room.");
            story[35].setTrigger("have Joanna");
            story[35].addChoice("Turn left", "Left 04");
            story[35].addChoice("Turn right", "turn right 03");
            story[35].addChoice("Go to the cell in front of you", "Empty cell. With Joanna");

            story[36].setName("turn left 01");
            story[36].addText("We turn to our left and continue moving the corridor. Suddenly Josh grabs my arm and stops.\n- What is it? – I say.");
            story[36].addText("- Shhhh... - he whispers. He stands there for a few minutes and then panickily replies, - the robots are coming this way. We better run! We turn back and start to run.");
            story[36].setTrigger("robots are coming");
            story[36].setNextStage("turn right 02");

            story[37].setName("Empty cell. With Joanna");
            story[37].setNeedsTrigger("have Joanna");
            story[37].addText("We go into the room. The fireball, which always follows Joanna, lights up the empty cell. There is a broken robot lying on the floor.\n- Nothing to see here. – says Josh and we turn back.");

            story[38].setName("turn right 01");
            story[38].addText("We keep moving forward through the dark corridor. It turns to our left, but on our right, we see the door.");
            story[38].addChoice("Go through the doors", "Empty cell");
            story[38].addAlternative(1, "Empty cell. With Joanna");
            story[38].addChoice("Turn to the left", "Left 03");

            story[39].setName("Empty cell");
            story[39].addText("We go through the doors. We see nothing through the darkness, it seems that there is picture on the walls. Maybe it is another cell? Either way, it is not a way back, so we get back to the corridor.");
            story[39].setNeedsTrigger("Messed to empty");

            story[40].setName("Left 03");
            story[40].addText("We turn left and continue walking the corridor. Not long before we are met with another choice:");
            story[40].addChoice("Go through the doors on our right", "Empty cell");
            story[40].addAlternative(1, "Empty cell. With Joanna");
            story[40].addChoice("Go through the doors on our left", "Humans");
            story[40].addAlternative(1, "Humans 02");
            story[40].addChoice("Continue going through the corridor", "Left 04");

            story[41].setName("Left 04");
            story[41].addText("Another choice.");
            story[41].addChoice("Go through the doors on our left", "Humans");
            story[41].addAlternative(1, "Humans 02");
            story[41].addChoice("Go through the doors on our right", "Messed room 02");
            story[41].addAlternative(1, "Empty cell. With Joanna");
            story[41].addChoice("Continue going through the corridor", "Straight 03");

            story[42].setName("Straight 03");
            story[42].addText("We meet a crossroad. On our left and on our right the corridor ends with doors. In front of us the corridor makes a turn.");
            story[42].addChoice("Turn right", "Elevator 01");
            story[42].addChoice("Turn left", "Back to main");
            story[42].addChoice("Go straight", "Stage 3 start");
            story[42].setNeedsTrigger("have Joanna");

            story[43].setName("Back to main");
            story[43].addText("We go through the doors to see another cell. After looking around a bit we notice, that this is my and Josh’s original cell. We went back to it. We once again turn to the door.");
            story[43].addChoice("Go through the doors in front of us", "Straight 04");
            story[43].addChoice("Go through the doors on our right", "turn right 01");

            story[44].setName("Straight 04");
            story[44].addText("We meet the crossroad. There is a door in front of us and an unknown corridor in both of our sides.");
            story[44].addChoice("Go straight", "Elevator 01");
            story[44].addChoice("Turn right", "turn right 02");
            story[44].addChoice("Turn left", "Stage 3 start");
            story[44].setNeedsTrigger("have Joanna");

            story[45].setName("turn right 03");
            story[45].addText("We turn right and see a door in front of us. Going through the corridor on our right we would end up with another door.");
            story[45].addChoice("Go through the doors in front of us", "Empty cell");
            story[45].addAlternative(1, "Empty cell. With Joanna");
            story[45].addChoice("Go through the doors on our right", "Back to main");

            story[46].setName("Messed room 02");
            story[46].setNeedsTrigger("have Joanna");
            story[46].addText("We go through the doors to meet another cell. There is a standing robot in the corner of the room. He turn to us and begins attacking.");
            story[46].addChoice("Send Joanna's fireball to it.", "Messed 02 Joanna");       // +1 aggr  -15 JoannasMana     trigger: Messed to empty
            story[46].addChoice("Smash the robot to the wall", "Messed 02 Me");             // +2 aggr  -10 MyMana          trigger: Messed to empty
            story[46].addChoice("Stun the robot and leave", "Messed 02 Josh");              //          -15 JoshsMana

            story[47].setName("Messed 02 Joanna");
            story[47].addText("Joanna creates a fireball and sends it to the robot. It instantly collapses. We found nothing else in the room, so we went out.");
            story[47].JoannaMana = -15;
            story[47].setAggressiveness(1);
            story[47].setTrigger("Messed to empty");
            story[47].setNextStage("Left 04");

            story[48].setName("Messed 02 Me");
            story[48].addText("I concentrate my energy and smash the robot up to the wall. Without that robot, it’s just another empty cell, so we turn back to the corridor.");
            story[48].myMana = -10;
            story[48].setAggressiveness(2);
            story[48].setTrigger("Messed to empty");
            story[48].setNextStage("Left 04");

            story[49].setName("Messed 02 Josh");
            story[49].addText("Josh concentrates his powers and stuns the robot. We silently leave the room.");
            story[49].JoshMana = -15;
            story[49].setNextStage("Left 04");

            story[50].setName("Humans 02");
            story[50].setNeedsTrigger("have Joanna");
            story[50].addText("We go through the doors to see Dominic and his people in it.\n- This is not where we should be. – Joanna says and we turn back.");

            story[51].setName("Humans");
            story[51].addText("We go through the doors. To our surprise, a crowd of around 12 people was there. A fireplace in another corner of the room provided the light and the heat. A woman in a green dress approaches us, her body showing confidence and strength.");
            story[51].addText("But before she could say a thing, a boy runs into the room shouting: “Robots! They’re coming!” People’s bodies stiffen. Some seems scared, others – fierce.");
            story[51].addText("- Then we must act quickly. – the woman in the green dress says. – Joanna, come here.\n- Gladly. – a girl, who seems not older than 15 years old, replies.");
            story[51].addText("- We can help. – I say. – We have mental powers which can help you out. Josh can stun robots for a second by causing some kind of chaos with the magnetic fields. And I can:");
            story[51].addChoice("Make them levitate and make no harm", "People.Fight.Stun");
            story[51].addChoice("Throw and smash them up in the wall", "People.Fight.Smash");   // +2 agg

            story[52].setName("Stage 3 start");
            story[52].setNeedsTrigger("have Joanna");
            story[52].addText("We walk through the corridor, Joanna's fire ball lightning up our way. Finally we see something else: ");
            story[52].setNextStage("Stage3 1st crossroad");

            //---------------------------------------------------------------------------------------------------------------------------
            
            story[53].setName("Stage3 1st crossroad");
            story[53].addText("Two doors in the opposite directions and a crossroad. There seem to be some light  coming from the right side if the crossroad.");
            story[53].addChoice("Go through doors on the left", "Leaking gas room");        
            story[53].addChoice("Go through the doors on the right", "Dead body");       
            story[53].addChoice("Turn right", "Mystery room corridors. Girl alive");
            story[53].addAlternative(1, "Mystery room corridors. Girl met");               
            story[53].addChoice("Turn left", "Dirty rooms corridor");       
            story[53].addChoice("Go straight", "2doors 2");

            story[99].setName("Dirty rooms corridor");
            story[99].addText("We go through the corridors. Walls on the sides seems to be glass. However, they were all covered in dirt and blood. It seems like some horrible things happened there.");
            story[99].addText("The corridor turns to the right. As we turn with it, we see a door on our right. It must be leading to one of those dirty rooms. We could also continue going through the corridor.");
            story[99].addChoice("Go through the doors", "Dirty room");
            story[99].addChoice("Continue going through the corridor", "Continue through corridor");

            story[92].setName("Mystery room corridors. Girl met");
            story[92].addText("We've turned to the corridor. Glasses from the side showed us tidy rooms with chairs and people in it. One of the room was messy. It is where we've met the crazy woman.");
            story[92].addText("We continue going through the corridor...");
            story[92].setNextStage("Mystery crossroad 2");
            story[92].setNeedsTrigger("Crazy girl met");                

            story[93].setName("Mystery crossroad 2");
            story[93].addText("We've got past the corridor with the galss windows and turned left. There's a door on our left. We...");
            story[93].addChoice("Go through the door", "Dead body");                                            
            story[93].addChoice("Continue going through the corridor", "Continue through corridor");            
            story[93].addChoice("Get back", "Mystery room corridors. Girl alive");
            story[93].addAlternative(1, "Mystery room corridors. Girl met");

            story[94].setName("Continue through corridor");
            story[94].addText("We continue walking through the corridor, till we see doors on our right.");
            story[94].addChoice("Go through the doors", "RoboFight");
            story[94].addAlternative(1, "Cell after fight");
            story[94].addChoice("Continue going through the corridor", "Stage3 2nd crossroad");

            story[95].setName("Dead body");
            story[95].addText("We go through the doors and an unusual smell reaches our nostrils. We get past the speaker to see what has happened here and see an unknown gender person sitting in the chair.");
            story[95].addText("From the look of it, it seemed like the man has been tied to the chair for days, though he died just an hour ago. There is nothing how we can help him anymore, so we get back.");

            story[89].setName("Leaking gas room");
            story[89].addText("We go into the room. It was a total mess. It's walls were painted with blood. You could see two men's bodies and one broken robot on the floor.");
            story[89].addText("I was about to go back, when an idea popped up. \"Hey, if I can move objects with my thoughts, wouldn't it be great to have some object near me to serve me as a weapon?\"");
            story[89].addText("My companions nod in agreement and I look around the room. The chair might be too soft to hit the robot. The speaker seems too heavy");
            story[89].addText("And dragging a robot's body would be the same as throwing a working robot into the wall... But maybe I could take out one of the pipes? It would be heavy enough to hit the robot, yet light enough to frag around.");
            story[89].setNextStage("Leaking gas ouch");

            story[90].setName("Leaking gas ouch");
            story[90].myMana = -5;
            story[90].setTrigger("Leaking gas");
            story[90].myHeart = -5;
            story[90].JoshHeart = -5;
            story[90].JoannaHeart = -5;
            story[90].addText("Using my telekinectic powers, I pop out the middle pipe. A yellowish green thick gas or liquid started to pour out of the pipe.");
            story[90].addText("The air around changed. Something tingled my throat and nostrils, and so I've coughed. My teammates started to cough too. Quickly we ran out of the poluted room. Only when we've got back did I notice that I've forgot to take the pipe with me. But it was too late, we won't get back there.");
            story[90].setNextStage("Stage3 1st crossroad");

            story[54].setName("Mystery room corridors. Girl alive");
            story[54].addText("We turned into the corridor and I’ve got stunned for a minute. The walls on the side were glass like. Even though anywhere else the electricity were shut down, those rooms still had lightning.");
            story[54].addText("I could even see a playing TV through the glass. The sound of two big speakers did not reach the ears. But between those speakers people were seated. Every person had their own room, four in total.");
            story[54].addText("The most interesting room was the first one on our left. TV, speakers, a picture of a country’s leader – all were flying across the room like crazy. The light next to the seated person were twitching and, from time to time, a sparkles flied from it.");
            story[54].addText("- He’s still alive. – I whisper. Just then I turn to see Josh’s stiffened body and Joanna’s lowered eyebrows. – What’s the matter?");
            story[54].addText("- Nothing. – they both answered at the same time. Surprised, they’ve looked into each other.\n- Whatever.Don’t say me if you don’t want to.But we got to:");
            story[54].addChoice("Go and help that person out", "Crazy girl");
            story[54].addChoice("Continue moving", "Mystery crossroad 2");            
            story[54].addChoice("Go back", "Stage3 1st crossroad");

            story[55].setName("Crazy girl");
            story[55].addText("We go into the room and face a shaking big speaker in front of us. On our left I can see a flying TV, moving. Only few wires were still holding it near the wall. Few robots lied on the floor.");
            story[55].addText("We turn to the left and see another big shaking speaker. The light was flickering and the some sparkles flew from it. Under this light a one-handed woman sat, tied to the chair.");
            story[55].addText("She looked angrily at us. Her look shook me, but, trying to calm down, I speak:\n- Don’t be afraid.We’re here to help you.\nThe woman grunts.I go and tie her ropes:");
            story[55].addChoice("Using telekinesis (-5 mana)", "Release ropes. Telekinesis");
            story[55].setTrigger("Crazy girl met");
            story[55].addChoice("With my hands", "Release ropes. Hands"); 

            story[67].setName("Release ropes. Hands");
            story[67].setTrigger("had serious fight");
            story[67].addText("I rush to the girl and untie the ropes. A crazy look flashes on the woman face, she lets out an animalistic growl and hits me.");
            story[67].setNextStage("Me hit 5. Crazy girl");

            story[56].setName("Release ropes. Telekinesis");
            story[56].myMana = -5;
            story[56].setTrigger("had serious fight");
            story[56].addText("Standing few feet from her, I use my powers to release the ropes. The woman grunts, a crazy look in her eyes and starts attacking us! She’s just about to hit me.");
            story[56].addChoice("Hide behind Josh’s back", "Josh hit 5. Crazy girl");   
            story[56].addChoice("Hide behind Joanna's back", "Joanna hit 5. Crazy girl");
            story[56].addChoice("Stand still", "Me hit 5. Crazy girl");

            story[57].setName("Josh hit 5. Crazy girl");
            story[57].JoshHeart = -5;
            story[57].addText("Josh grunts. What's the next move?");
            story[57].addChoice("Josh hits her back", "Josh hits crazy girl");  
            story[57].addChoice("Joanna fires a fireball to woman", "Joanna, fire, crazy girl");    //-10 mana +3 aggr      
            story[57].addChoice("Throw a speaker on her (-10 mana)", "Crazy girl stunned by speaker");

            story[70].setName("Josh hits crazy girl");
            story[70].setAggressiveness(1);
            story[70].addText("Quickly reacting to the punch, Josh punches the woman back.");
            story[70].setNextStage("Crazy girl hit");

            story[72].setName("Joanna hits crazy girl");
            story[72].setAggressiveness(1);
            story[72].addText("Quickly reacting to the punch, Joanna punches the woman back.");
            story[72].setNextStage("Crazy girl hit");

            story[73].setName("Me hit crazy girl");
            story[73].setAggressiveness(1);
            story[73].addText("Quickly reacting to the punch, I punch the woman back.");
            story[73].setNextStage("Crazy girl hit");

            story[71].setName("Crazy girl hit");
            story[71].addText("She lets out a short scream. But then she gets back, looking with wide open eyes to us. A wicked grin appears on her face.");
            story[71].addText("Raising her arms, she lifts up TV and the speakers to the ceiling with her mental powers.");
            story[71].addChoice("Try to take over the floating furniture (-30 mana)", "Take back. Crazy girl");  // -30 mana
            story[71].addChoice("Do nothing", "Nothing. Crazy girl");

            story[75].setName("Take back. Crazy girl");
            story[75].myMana = -30;
            story[75].addText("I focus my telekinesis powers and try to take back the furniture, to lay it down safely. Immediately I feel how strong she is.");
            story[75].addText("While I try and fight her mentally, Josh runs to the woman and...");
            story[75].addChoice("Punch her", "Punch crazy girl");   // +1 agr                           
            story[75].addChoice("Tie her down", "Tie crazy girl. Telekinesis fight");                   

            story[76].setName("Punch crazy girl");
            story[76].setAggressiveness(1);
            story[76].addText("Josh punches the crazy woman. She looses her focus and tries to punch Josh back.");
            story[76].setNextStage("Crazy girl resolution. Floating things");

            story[77].setName("Crazy girl resolution. Floating things");
            story[77].addText("Before the woman could do anything, I throw a floating picture of the country's leader into her. She looses consciousness. We grab the rope from the chair and tie her down again.");
            story[77].setNextStage("Crazy girl resolution");

            story[78].setName("Tie crazy girl. Telekinesis fight");
            story[78].addText("Josh grabs the floating rope, with which the woman was tied down earlier. Jumping towards the woman he tries to tie her arms down.");
            story[78].addText("Seeing Josh getting his way to her, woman losses her focus. I can feel all the floating furniture going completely under my control.");
            story[78].setNextStage("Crazy girl resolution. Floating things");

            story[74].setName("Nothing. Crazy girl");
            story[74].myHeart = -15;
            story[74].JoannaHeart = -15;
            story[74].JoshHeart = -15;
            story[74].addText("We stand there stunned when the woman throws all the furniture at us. I loose consciousness. And everything goes black.");
            story[74].addText("I blink. What happened? My head hurts... I look around me. Joanna and Josh slowly gets up. Crazy woman is nowhere to be seen, so we get out of the room.");
            story[74].setNextStage("2doors 2");

            story[68].setName("Joanna, fire, crazy girl");
            story[68].setAggressiveness(4);
            story[68].JoannaMana = -10;
            story[68].setTrigger("Crazy girl dead");
            story[68].addText("Without any more hesitation Joanna throws one of her fireballs into a crazy woman. Unearthly screams fill the room. A shudder runs through my spine.");
            story[68].setNextStage("Crazy girl resolution");

            story[69].setName("Crazy girl resolution");
            story[69].addText("Wasting no more time, three of us gets out of the room.");
            story[69].setNextStage("2doors 2");

            story[58].setName("Joanna hit 5. Crazy girl");
            story[58].JoannaHeart = -5;
            story[58].addText("Joanna grunts. What's the next move?");
            story[58].addChoice("Joanna hits her back", "Joanna hits crazy girl");  // +1 aggr                              
            story[58].addChoice("Joanna fires a fireball to woman", "Joanna, fire, crazy girl");    //-10 mana +3 aggr      
            story[58].addChoice("Throw a speaker on her (-10 mana)", "Crazy girl stunned by speaker");

            story[59].setName("Me hit 5. Crazy girl");
            story[59].myHeart = -5;
            story[59].addText("I grunt from pain. What's the next move?");
            story[59].addChoice("I hit her back", "Me hit crazy girl"); // +1 aggr                                          
            story[59].addChoice("Joanna fires a fireball to woman", "Joanna, fire, crazy girl");    //-10 mana +3 aggr      
            story[59].addChoice("Throw a speaker on her, making her inable to move (-10 mana)", "Crazy girl stunned by speaker");   

            story[60].setName("Crazy girl stunned by speaker");
            story[60].myMana = -10;
            story[60].JoannaHeart = -15;
            story[60].addText("I channel my telekinectic powers and make the speaker fall right onto the girl. For the moment she's unable to move. The moving objects stops and drops on the floor.");
            story[60].addText("The falling TV hits Joanna's shoulder, she lets out a short scream.\nNow we need to...");
            story[60].addChoice("Kill this crazy woman", "Joanna, fire, crazy girl");   // +4aggr                                
            story[60].addChoice("Tie this woman up", "Spare the crazy girl");

            story[61].setName("Spare the crazy girl");
            story[61].addText("There's still a rope hanging next to the chair. We could tie the womn with it and leave safely... who grabs it?");
            story[61].addChoice("Josh grabs the rope", "Josh grabs rope");
            story[61].addChoice("Joanna grabs the rope", "Joanna grabs rope");                                              
            story[61].addChoice("I grab the rope", "I grab rope");                                                          
             
            story[79].setName("I grab rope");
            story[79].addText("I grab the rope, but as I turn back to tie the woman up, the speaker, which held her to the floor, flies right to me and hits me. I loose consciousness.");
            story[79].myHeart = -20;
            story[79].JoannaMana = -5;
            story[79].JoshHeart = -5;
            story[79].addText("As I open my eyes, I see Josh's and Joanna's worried eyes. They let out a deep breath. \"Oh, dear, you're alive.\" - Joanna says relieved.");
            story[79].addText("I look around. The body of the woman lays down on the floor. Joanna and Josh explains that they had no other choice but to kill her, or she would have greatly injured us.");
            story[79].setNextStage("Crazy girl resolution");

            story[62].setName("Josh grabs rope");
            story[62].JoshHeart = -20;
            story[62].addText("Josh jumps to the chair to grab the rope, but as he turns back to tie the woman up, the speaker, which held her to the floor, flies up and smash Josh's back.");
            story[62].addText("Josh looses a consciousness and fells to the floor. The woman starts standing up, but then:");
            story[62].addChoice("Joanna fires her fireball to a woman", "Joanna, fire, crazy girl");  // +3aggr       
            story[62].addChoice("I tie the woman up (-5 mana)", "Tie crazy girl. Josh unconscious");

            story[80].setName("Joanna grabs rope");
            story[80].JoannaHeart = -20;
            story[80].addText("Joanna jumps to the chair to grab the rope, but as she turns back to tie the woman up, the speaker, which held her to the floor, flies up and smash Joanna's back.");
            story[80].addText("Joanna looses a consciousness and fells to the floor. The woman starts standing up, but then I quickly use telekinesis to take the rope from Joanna's palms and tie womans hands with it.");
            story[80].addText("The woman tried to fight back when we were putting her back into her chair. We went to Joanna. She was still breathing. We started discussing how to get her conscious again, when she woke up herself.");
            story[80].setNextStage("Crazy girl resolution");

            story[122].setName("Tie crazy girl. Josh unconscious");
            story[122].addText("I quickly use telekinesis to take the rope from Josh's palms and tie womans hands with it. The woman tried to fight back when we were putting her back into her chair.");
            story[122].setNextStage("Crazy girl resolution. Josh unconscios");

            story[63].setName("Crazy girl resolution. Josh unconscios");
            story[63].addText("We went to Josh. He was still breathing. We started discussing how to get him conscious again, when he woke up himself.");
            story[63].addText("- Wha-- What happened? - he asks, looking around.\n- You've lost your consciousness for a moment. But it's okay. The girl is neutralised now.");
            story[63].addText("He looks to the girl.\n-Oh, - he whispers. Not wasting any more minute, we get out.");
            story[63].setNextStage("2doors 2");

            story[64].setName("2doors 2");
            story[64].addText("There's two doors on our sides. We could also continue going through the corridor.");
            story[64].addChoice("Go to the doors on our left", "Stuck doors");          
            story[64].addChoice("Go through the doors on our right", "Crazy girl");
            story[64].addAlternative(1, "Crazy girl dead");                     
            story[64].addAlternative(2, "Crazy girl met");                      
            story[64].addChoice("Go straight", "Stage3 2nd crossroad");
            story[64].addChoice("Go back", "Stage3 1st crossroad");

            story[81].setName("Stuck doors");
            story[81].addText("We tried opening the doors, but we couldn't. It seems they are stuck.");
            story[81].addChoice("Use powers to open the door (-10 mana)", "Stuck doors open");             
            story[81].addChoice("Leave it be", "2doors 2");

            story[85].setName("Stuck doors open");
            story[85].myMana = -10;
            story[85].addText("I focus my telekinectic powers to open the doors, but still nothing happened.");
            story[85].addChoice("Try again (-10 mana)", "Stuck doors open");
            story[85].addChoice("Leave it be", "2doors 2");

            story[82].setName("Crazy girl dead");
            story[82].addText("We go into the room. It is the same room where we were attacked by a crazy woman. We see the the ashes of her body. Finding nothing else to do here, we get out of the room.");
            story[82].setNextStage("2doors 2");
            story[82].setNeedsTrigger("Crazy girl dead");

            story[83].setName("Crazy girl met");
            story[83].addText("We go into a messed room. The same woman, who attacked us, is sitting in the chair, tied. Not wanting any more business with her, we get out of the room.");
            story[83].setNextStage("2doors 2");

            story[65].setName("Stage3 2nd crossroad");
            story[65].addText("Another crossroad.");
            story[65].addChoice("Turn left", "Cell towards dirty room");        
            story[65].addChoice("Turn right", "Cell towards mystery room");     
            story[65].addChoice("Go straight", "Elevator 02");              
            story[65].addAlternative(1, "End fight start");
            story[65].addChoice("Go back", "2doors 2");

            story[87].setName("Cell towards dirty room");
            story[87].addText("We go through the corridor, when we see a door on our right.");
            story[87].addChoice("Go through the doors", "RoboFight");                           
            story[87].addAlternative(1, "Cell after fight");                               
            story[87].addChoice("Continue through corridor", "Dirty crossroad");                

            story[88].setName("Cell towards mystery room");
            story[88].addText("We go through the corridor, when we see a door on our left.");
            story[88].addChoice("Go through the doors", "RoboFight");                           
            story[88].addAlternative(1, "Cell after fight");                                    
            story[88].addChoice("Continue through corridor", "Mystery crossroad");

            story[113].setName("RoboFight");
            story[113].setTrigger("had serious fight");
            story[113].addText("We go into the room to see 10 robots there and three dead human bodies. Robots turn to us, ready to punch us too.");
            story[113].addChoice("I throw all robots to the back of corridor (-35 mana)", "Throw back. Many");
            story[113].addChoice("Josh makes a robot immovable. I kick him till shutdown", "Josh stuns. I hit");
            story[113].addChoice("Joanna throws a fireball to one robot", "Joanna. 1");
            story[113].addChoice("Joanna throws a fireball to multiple robots", "Joanna. Many");
            story[113].addChoice("We try to kick their asses in the old-fashioned martial ways", "Melee");
            story[113].addChoice("I throw one robot to the wall so strong, it shutdowns instantely (-10 mana)", "Throw back. 1");
            story[113].alternativeEnding = "RoboFight end";

            story[114].setName("Throw back. Many");
            story[114].myMana = -35;
            story[114].robotFinalDefeated = 5;
            story[114].addText("I throw all robots back from us. They can't touch us, yet cruching to the wall and into each other shut some of them down. However, I feel tired. What's next?");
            story[114].addChoice("We try to kick their asses in the old-fashioned martial ways", "Melee");
            story[114].addChoice("Joanna throws a fireball to multiple robots", "Joanna. Many");
            story[114].addChoice("Joanna throws a fireball to one robot", "Final. Joanna. 1");
            story[114].addChoice("Josh makes a robot immovable. I kick him till shutdown", "Josh stuns. I hit");
            story[114].addChoice("I throw all robots to the back of corridor (-35 mana)", "Throw back. Many");
            story[114].addChoice("I throw one robot to the wall so strong, he shutdowns instantely (-10 mana)", "Throw back. 1");
            story[114].alternativeEnding = "RoboFight end";
            story[114].setAggressiveness(1);

            story[115].setName("Melee");
            story[115].robotFinalDefeated = 3;
            story[115].JoannaHeart = -25;
            story[115].JoshHeart = -15;
            story[115].myHeart = -20;
            story[115].addText("We rush to them and start kicking their metallic bodies. Robots punch as back, so we growl, but we still manage to shutdown few of them.");
            story[115].addChoice("We try to kick their asses in the old-fashioned martial ways", "Melee");
            story[115].addChoice("Joanna throws a fireball to multiple robots", "Joanna. Many");
            story[115].addChoice("Joanna throws a fireball to one robot", "Joanna. 1");
            story[115].addChoice("Josh makes a robot immovable. I kick him till shutdown", "Josh stuns. I hit");
            story[115].addChoice("I throw all robots to the back of corridor (-35 mana)", "Throw back. Many");
            story[115].addChoice("I throw one robot to the wall so strong, he shutdowns instantely (-10 mana)", "Throw back. 1");
            story[115].alternativeEnding = "RoboFight end";

            story[116].setName("Joanna. Many");
            story[116].addText("Joanna channels her energy to create 5 fireballs and throws them all into a different robot. 5 less robots to defeat, but Joanna is tired.");
            story[116].JoannaMana = -35;
            story[116].robotFinalDefeated = 5;
            story[116].addChoice("We try to kick their asses in the old-fashioned martial ways", "Melee");
            story[116].addChoice("Joanna throws a fireball to multiple robots", "Joanna. Many");
            story[116].addChoice("Joanna throws a fireball to one robot", "Joanna. 1");
            story[116].addChoice("Josh makes a robot immovable. I kick him till shutdown", "Josh stuns. I hit");
            story[116].addChoice("I throw all robots to the back of corridor (-35 mana)", "Throw back. Many");
            story[116].addChoice("I throw one robot to the wall so strong, he shutdowns instantely (-10 mana)", "Throw back. 1");
            story[116].alternativeEnding = "RoboFight end";
            story[116].setAggressiveness(1);

            story[117].setName("Joanna. 1");
            story[117].addText("Joanna lights up a new fireball and throws it to one of the robot. That one robot is no longer a threat, but while she was doing it, I and Josh got hit by another two robots.");
            story[117].JoshHeart = -10;
            story[117].myHeart = -10;
            story[117].JoannaMana = -5;
            story[117].robotFinalDefeated = 1;
            story[117].addChoice("We try to kick their asses in the old-fashioned martial ways", "Melee");
            story[117].addChoice("Joanna throws a fireball to multiple robots", "Joanna. Many");
            story[117].addChoice("Joanna throws a fireball to one robot", "Joanna. 1");
            story[117].addChoice("Josh makes a robot immovable. I kick him till shutdown", "Josh stuns. I hit");
            story[117].addChoice("I throw all robots to the back of corridor (-35 mana)", "Throw back. Many");
            story[117].addChoice("I throw one robot to the wall so strong, he shutdowns instantely (-10 mana)", "Throw back. 1");
            story[117].alternativeEnding = "RoboFight end";

            story[118].setName("Josh stuns. I hit");
            story[118].JoshMana = -10;
            story[118].JoannaHeart = -10;
            story[118].robotFinalDefeated = 1;
            story[118].addText("Josh channels his energy to stun the neariest robot. Seeing it, I quickly leap to the robot and kick him to a defeating shut down.");
            story[118].addText("While we were worried about that one robot, another robot hurts Joanna.");
            story[118].addChoice("We try to kick their asses in the old-fashioned martial ways", "Melee");
            story[118].addChoice("Joanna throws a fireball to multiple robots", "Joanna. Many");
            story[118].addChoice("Joanna throws a fireball to one robot", "Joanna. 1");
            story[118].addChoice("Josh makes a robot immovable. I kick him till shutdown", "Josh stuns. I hit");
            story[118].addChoice("I throw all robots to the back of corridor (-35 mana)", "Throw back. Many");
            story[118].addChoice("I throw one robot to the wall so strong, he shutdowns instantely (-10 mana)", "Throw back. 1");
            story[118].alternativeEnding = "RoboFight end";

            story[119].setName("Throw back. 1");
            story[119].JoannaHeart = -10;
            story[119].myMana = -10;
            story[119].robotFinalDefeated = 1;
            story[119].addText("I focus my energy on the closest enemy, lift him and throw him into the wall as strong as I can. The robot shutdown, but while I was focusing my energy on that one robot, another one hits Joanna.");
            story[119].addChoice("We try to kick their asses in the old-fashioned martial ways", "Melee");
            story[119].addChoice("Joanna throws a fireball to multiple robots", "Joanna. Many");
            story[119].addChoice("Joanna throws a fireball to one robot", "Joanna. 1");
            story[119].addChoice("Josh makes a robot immovable. I kick him till shutdown", "Josh stuns. I hit");
            story[119].addChoice("I throw all robots to the back of corridor (-35 mana)", "Throw back. Many");
            story[119].addChoice("I throw one robot to the wall so strong, he shutdowns instantely (-10 mana)", "Throw back. 1");
            story[119].alternativeEnding = "RoboFight end";

            story[120].setName("RoboFight end");
            story[120].addText("Finally the last robot falls down to the floor. The cell is empty and we are save. There\'s nothing else to do here in this empty cell, so we get back.");
            story[120].setNextStage("Continue through corridor");

            story[96].setName("Mystery crossroad");
            story[96].addText("There's a door on our right.");
            story[96].addChoice("Go through the doors", "Dead body");
            story[96].addChoice("Continue going through corridor", "Stage3 1st crossroad");

            story[97].setName("Dirty crossroad");
            story[97].addText("There's a door on our left.");
            story[97].addChoice("Go through the door", "Dirty room");                           
            story[97].addChoice("Continue going through corridor", "Stage3 1st crossroad");

            story[98].setName("Dirty room");
            story[98].addText("We go into the messy room. The furniture is throwed wherever, mainly to the other side of the room. There's several dead human bodies laying on the floor. Both floor and wallpapers are painted with blood.");
            story[98].addText("It was a horrible sight, yet nothing else to do here. So we go back.");

            story[91].setName("Cell after fight");
            story[91].addText("We go into the room. It's another empty cell with few dead people and robots laying on the floor. There's nothing to do here, so we get back.");
            story[91].setNeedsTrigger("had serious fight");

            story[84].setName("Elevator 02");
            story[84].addText("We go straight and see another closed door. Should we try to open it?");
            story[84].addChoice("Use powers to open the door (-10 mana)", "Elevator 02 open");
            story[84].addChoice("Go back", "Stage3 2nd crossroad");

            story[86].setName("Elevator 02 open");
            story[86].myMana = -10;
            story[86].addText("I channel my powers to open the stuck doors. Joanna lights up the room. It is a small room without any floors. As I gasp from how deep the hole in the ground seems, Josh says:");
            story[86].addText("\"It's probably another elevator. Damn it. If there is no other choice but to go through the elevator to the outside, we'll need to wait till electricity comes back.\"");
            story[86].addText("\"But when electricity comes back, will we be able to survive?\" - Joanna whispers.\n\"No one knows.\" - I say. We go back to the crossroad.");
            story[86].setNextStage("Stage3 2nd crossroad");

            story[66].setName("End fight start");                                       
            story[66].setTrigger("THE END");
            story[66].addText("We go straight when suddenly a light in the corridor turns on.\n- Electricity.. It got back!");
            story[66].addText("A humming sound is heard in front of us, warning that something is coming up. \"Get ready\" - Josh says.");
            story[66].addText("The door opens up and an army of robots shows up.\n\"Stop. You are not allowed to be here.\" - one of the robots says.\n\"Oh, hell, we are!\" - Josh screams.");
            story[66].addText("There's around 20 robots in here. What do we do first?");
            story[66].addChoice("I throw all robots to the back of corridor (-35 mana)", "Final. Throw back. Many");          
            story[66].addChoice("Josh makes a robot immovable. I kick him till shutdown", "Final. Josh stuns. I hit");  
            story[66].addChoice("Joanna throws a fireball to one robot", "Final. Joanna. 1");                           
            story[66].addChoice("Joanna throws a fireball to multiple robots", "Final. Joanna. Many");                  
            story[66].addChoice("We try to kick their asses in the old-fashioned martial ways", "Final. Melee");        
            story[66].addChoice("I throw one robot to the wall so strong, it shutdowns instantely (-10 mana)", "Final. Throw back. 1");
            story[66].setNeedsTrigger("had serious fight"); 

            story[100].setName("Final. Melee");
            story[100].robotFinalDefeated = 3;
            story[100].JoannaHeart = -25;
            story[100].JoshHeart = -15;
            story[100].myHeart = -20;
            story[100].addText("We rush to them and start kicking their metallic bodies. Robots punch as back, so we growl, but we still manage to shutdown few of them.");
            story[100].addChoice("We try to kick their asses in the old-fashioned martial ways", "Final. Melee");
            story[100].addChoice("Joanna throws a fireball to multiple robots", "Final. Joanna. Many");                 
            story[100].addChoice("Joanna throws a fireball to one robot", "Final. Joanna. 1");                          
            story[100].addChoice("Josh makes a robot immovable. I kick him till shutdown", "Final. Josh stuns. I hit"); 
            story[100].addChoice("I throw all robots to the back of corridor (-35 mana)", "Final. Throw back. Many");   
            story[100].addChoice("I throw one robot to the wall so strong, he shutdowns instantely (-10 mana)", "Final. Throw back. 1");
            story[100].alternativeEnding = "Final. The end";                        

            story[101].setName("Final. Throw back. Many");
            story[101].myMana = -35;
            story[101].robotFinalDefeated = 5;
            story[101].addText("I throw all robots back from us. They can't touch us, yet cruching to the wall and into each other shut some of them down. However, I feel tired. What's next?");
            story[101].addChoice("We try to kick their asses in the old-fashioned martial ways", "Final. Melee");
            story[101].addChoice("Joanna throws a fireball to multiple robots", "Final. Joanna. Many");
            story[101].addChoice("Joanna throws a fireball to one robot", "Final. Joanna. 1");                          
            story[101].addChoice("Josh makes a robot immovable. I kick him till shutdown", "Final. Josh stuns. I hit"); 
            story[101].addChoice("I throw all robots to the back of corridor (-35 mana)", "Final. Throw back. Many");   
            story[101].addChoice("I throw one robot to the wall so strong, he shutdowns instantely (-10 mana)", "Final. Throw back. 1");
            story[101].alternativeEnding = "Final. The end";                        
            story[101].setAggressiveness(1);

            story[102].setName("Final. Throw back. 1");
            story[102].JoannaHeart = -10;
            story[102].myMana = -10;
            story[102].robotFinalDefeated = 1;
            story[102].addText("I focus my energy on the closest enemy, lift him and throw him into the wall as strong as I can. The robot shutdown, but while I was focusing my energy on that one robot, another one hits Joanna.");
            story[102].addChoice("We try to kick their asses in the old-fashioned martial ways", "Final. Melee");
            story[102].addChoice("Joanna throws a fireball to multiple robots", "Final. Joanna. Many");
            story[102].addChoice("Joanna throws a fireball to one robot", "Final. Joanna. 1");                          
            story[102].addChoice("Josh makes a robot immovable. I kick him till shutdown", "Final. Josh stuns. I hit"); 
            story[102].addChoice("I throw all robots to the back of corridor (-35 mana)", "Final. Throw back. Many");
            story[102].addChoice("I throw one robot to the wall so strong, he shutdowns instantely (-10 mana)", "Final. Throw back. 1");
            story[102].alternativeEnding = "Final. The end";                        

            story[103].setName("Final. Joanna. 1");
            story[103].addText("Joanna lights up a new fireball and throws it to one of the robot. That one robot is no longer a threat, but while she was doing it, I and Josh got hit by another two robots.");
            story[103].JoshHeart = -10;
            story[103].myHeart = -15;
            story[103].robotFinalDefeated = 1;
            story[103].addChoice("We try to kick their asses in the old-fashioned martial ways", "Final. Melee");
            story[103].addChoice("Joanna throws a fireball to multiple robots", "Final. Joanna. Many");
            story[103].addChoice("Joanna throws a fireball to one robot", "Final. Joanna. 1");                          
            story[103].addChoice("Josh makes a robot immovable. I kick him till shutdown", "Final. Josh stuns. I hit"); 
            story[103].addChoice("I throw all robots to the back of corridor (-35 mana)", "Final. Throw back. Many");
            story[103].addChoice("I throw one robot to the wall so strong, he shutdowns instantely (-10 mana)", "Final. Throw back. 1");
            story[103].alternativeEnding = "Final. The end";                         

            story[104].setName("Final. Joanna. Many");
            story[104].addText("Joanna channels her energy to create 5 fireballs and throws them all into a different robot. 5 less robots to defeat, but Joanna is tired.");
            story[104].JoannaMana = -35;
            story[104].robotFinalDefeated = 5;
            story[104].addChoice("We try to kick their asses in the old-fashioned martial ways", "Final. Melee");
            story[104].addChoice("Joanna throws a fireball to multiple robots", "Final. Joanna. Many");
            story[104].addChoice("Joanna throws a fireball to one robot", "Final. Joanna. 1");
            story[104].addChoice("Josh makes a robot immovable. I kick him till shutdown", "Final. Josh stuns. I hit"); 
            story[104].addChoice("I throw all robots to the back of corridor (-35 mana)", "Final. Throw back. Many");
            story[104].addChoice("I throw one robot to the wall so strong, he shutdowns instantely (-10 mana)", "Final. Throw back. 1");
            story[104].alternativeEnding = "Final. The end";                        
            story[104].setAggressiveness(1);

            story[105].setName("Final. Josh stuns. I hit");
            story[105].JoshMana = -10;
            story[105].JoannaHeart = -10;
            story[105].robotFinalDefeated = 1;
            story[105].addText("Josh channels his energy to stun the neariest robot. Seeing it, I quickly leap to the robot and kick him to a defeating shut down.");
            story[105].addText("While we were worried about that one robot, another robot hurts Joanna.");
            story[105].addChoice("We try to kick their asses in the old-fashioned martial ways", "Final. Melee");
            story[105].addChoice("Joanna throws a fireball to multiple robots", "Final. Joanna. Many");
            story[105].addChoice("Joanna throws a fireball to one robot", "Final. Joanna. 1");
            story[105].addChoice("Josh makes a robot immovable. I kick him till shutdown", "Final. Josh stuns. I hit"); 
            story[105].addChoice("I throw all robots to the back of corridor (-35 mana)", "Final. Throw back. Many");
            story[105].addChoice("I throw one robot to the wall so strong, he shutdowns instantely (-10 mana)", "Final. Throw back. 1");
            story[105].alternativeEnding = "Final. The end";                        

            story[106].setName("Final. The end");
            story[106].addText("We finally defeated the last robot. We quickly get into the elevator. There probably are a lot of robots in the lower levels... So we're going to the roof. At the very least, no robot should look for us there");
            story[106].addText("As we get into the roof we see a helicopter. A man opens its doors and shouts to us: \n\"Come on, get inside!\"\nHaving no other choice, we listen to his command.");
            story[106].addText("A woman waits us there. She give us a blanket, food and water. \"Why are you doing this?\" - I ask. - \"Who are you?\"");
            story[106].addText("- We are what your country sees as enemies. - the woman replies. - You may not known, but your and our countries were one long time ago. But then a creation of SmartHelm divided our countries into two.");
            story[106].addText("We are still fighting that war. We tried to make peace with the leader several times, but he refuses to listen about it.");
            story[106].addText("We can\'t say for sure, but it seems that your country leader is breaking some serious human rights in your country. If i't is truth, we\'ll have more country as our allies and the your leader will fall.");
            story[106].addText("\"Can you collaborate with us and tell us all about it?\"\nThe woman looks hopefully into my eyes. Should I help her and tell her everything?");
            story[106].addChoice("Yes", "Yes");
            story[106].addChoice("No", "No");

            story[107].setName("Yes");
            story[107].addText("The leader did too many harm to my people. It is time to end it.");
            story[107].addText("....");
            story[107].addText("To be contnued... Or (hopefully) redone...");
            story[107].setTrigger("help other country to win");
            story[107].setNextStage("the end");

            story[108].setName("No");
            story[108].addText("I shall never betray my country, no matter what happens or what\'s to come!");
            story[108].setTrigger("stay silent");
            story[108].setNextStage("the end");

            story[111].setName("robots side");
            story[111].addText("This story is not yet finished. Sorry about that. You sure you did not want to save the neighbour? However, this is where the story goes...");
            story[111].setNextStage("rebels side");

            story[109].setName("Joanna dies");
            story[109].addText("The last hit was too much for Joanna. She screams in pain and falls to the floor. Poor fire girl died.");
            story[109].setNextStage("the end");
            story[109].setTrigger("Joanna dead");

            story[110].setName("Josh dies");
            story[110].addText("The last hit was too much for Josh. He screams in pain and falls to the floor, dying like a true soldier would have wanted to die.");
            story[110].setNextStage("the end");
            story[110].setTrigger("Josh dead");

            story[112].setName("I die");
            story[112].addText("The hit from the robot was the limit I could handle. I scream and fall to the floor. My eyes shuts and I fall into an eternal sleep...");
            story[112].setNextStage("the end");

            story[121].setName("the end");       //the end bus tas pats visiems, kad zinociau, kada pasibaige
            story[121].addText("THE END.");
            story[121].setTrigger("end of story");
            // 122
        }
    }
}
