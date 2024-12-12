using System;

class Program
{
    static void Main()
    {
        Music music = new Music();
        music.PlayMusic("background_music.mp3");  

        Console.Clear();
        Story story = new Story();
        story.Display(music);  

        music.StopMusic();
        Console.WriteLine("Enter your nickname (Don't use real life name for safety): ");
        string nick = Console.ReadLine();
        
        music.PlayMusic("battle_music.mp3");

        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
        Player hero = new Novice(nick);
        Console.WriteLine("Press Enter to go grinding...");
        Console.ReadLine();

        // Proses permainan dimulai
        while (hero.experience < 50)
        {
            Console.WriteLine("You are fighting monsters...");
            hero.GainExp(10);
            Console.WriteLine($"Current EXP: {hero.experience}/{hero.expToNextLevel}");
            Thread.Sleep(3000);
        }

        if (hero.experience >= 50)
        {
            Console.WriteLine("Congratulations! You've earned enough EXP to choose a sub-class!");
            Console.WriteLine("1. Swordman");
            Console.WriteLine("2. Archer");
            Console.WriteLine("3. Mage");
            Console.WriteLine("4. Assassin");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.WriteLine("You have chosen Swordman!");
                    hero = new Swordman(hero.Name);
                    break;
                case 2:
                    Console.WriteLine("You have chosen Archer!");
                    hero = new Archer(hero.Name);
                    break;
                case 3:
                    Console.WriteLine("You have chosen Mage!");
                    hero = new Mage(hero.Name);
                    break;
                case 4:
                    Console.WriteLine("You have chosen Assassin!");
                    hero = new Assassin(hero.Name);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Remaining as Novice...");
                    break;
            }
        }


        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
        Encounter.Start(hero);
        
        music.StopMusic();
    }
}
