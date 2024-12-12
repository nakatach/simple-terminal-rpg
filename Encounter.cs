using System;

class Encounter
{

    private static Enemy ChooseRandomEnemy()
    {
        Random rand = new Random();
        int randomChoice = rand.Next(1,7);

        switch (randomChoice)
        {
            case 1:
                return new PyroAbyssMage();
            case 2:
                return new HydroAbyssMage();
            case 3:
                return new Rifthound();
            case 4:
                return new AbyssLector();
            case 5:
                return new Asmodeus();
            case 6:
                return new Istaroth();
            default:
                return new PyroAbyssMage();
        }
    }

    public static bool EnterBossRoom(Player hero)
    {
        Console.Clear();
        Console.WriteLine("You found a door to a room... It seems like this is a boss room!");
        Console.WriteLine("Choose an action:");
        Console.WriteLine("1. Fight boss");
        Console.WriteLine("2. Run");

        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            Console.WriteLine("You must solve the boss room puzzle to fight the boss...");
            return SolveBossPuzzle(hero);
        }
        else if (choice == 2)
        {
            return false;
        }
        return false;
    }

    private static bool SolveBossPuzzle(Player hero)
    {
        Random random = new Random();
        int password = random.Next(100, 1000);
        int[] passwordDigits = new int[3];
        passwordDigits[0] = password / 100;
        passwordDigits[1] = (password / 10) % 10;
        passwordDigits[2] = password % 10;
        int guess1 = -1;
        int guess2 = -1;
        int guess3 = -1;
        int attempts = 0;

        Console.WriteLine("The password has 3 digits. Guess the hundreds, tens, and ones!");

        bool correctFirstDigit = false;
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine("Guess the first digit: ");
            guess1 = int.Parse(Console.ReadLine());

            if (guess1 == passwordDigits[0])
            {
                correctFirstDigit = true;
                break;
            }
            else if (guess1 < passwordDigits[0])
            {
                Console.WriteLine("Too low!");
            }
            else if (guess1 > passwordDigits[0])
            {
                Console.WriteLine("Too high!");
            }
        }
        if (!correctFirstDigit)
        {
            Console.WriteLine("You failed to guess the first digit...");
            return false;
        }

        bool correctSecondDigit = false;
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine("Guess the second digit: ");
            guess2 = int.Parse(Console.ReadLine());

            if (guess2 == passwordDigits[1])
            {
                correctSecondDigit = true;
                break;
            }
            else if (guess2 < passwordDigits[1])
            {
                Console.WriteLine("Too low!");
            }
            else if (guess2 > passwordDigits[1])
            {
                Console.WriteLine("Too high!");
            }
        }
        if (!correctSecondDigit)
        {
            Console.WriteLine("You failed to guess the first digit correctly...");
            return false;
        }

        bool correctThirdDigit = false;
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine("Guess the third digit: ");
            guess3 = int.Parse(Console.ReadLine());

            if (guess3 == passwordDigits[2])
            {
                correctThirdDigit = true;
                break;
            }
            else if (guess3 < passwordDigits[2])
            {
                Console.WriteLine("Too low!");
            }
            else if (guess3 > passwordDigits[2])
            {
                Console.WriteLine("Too high!");
            }
        }
        if (!correctThirdDigit)
        {
            Console.WriteLine("You failed to guess the third digit correctly...");
            return false;
        }

        Console.WriteLine("You successfully solves the puzzle!");
        Console.WriteLine("Press Enter to fight the boss!");
        Console.ReadLine();

        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
        Enemy boss = ChooseRandomBoss();
        BattleWithBoss(hero, boss);
        return true;
        
    }

    private static Enemy ChooseRandomBoss()
    {
        Random bossChoice = new Random();
        if (bossChoice.Next(1, 3) == 1)
        {
            return new Asmodeus();
        }
        else
        {
            return new Istaroth();
        }
    }
    public static void Start(Player hero)
    {
        while (hero.Hp > 0)
        {
            Enemy randomEnemy = ChooseRandomEnemy();

            if (randomEnemy is Asmodeus || randomEnemy is Istaroth)
            {
                bool canFight = EnterBossRoom(hero);

                if (canFight)
                {
                        BattleWithBoss(hero, randomEnemy);
                }
                else
                {
                    Console.WriteLine("Keep grinding?");
                    Console.WriteLine("1. Keep grinding");
                    Console.WriteLine("2. Go back to town");
                    string fleeChoice = Console.ReadLine();

                    if (fleeChoice == "2")
                    {
                        Console.WriteLine("Teleporting back to town...");
                        Environment.Exit(0);
                    }
                    else if (fleeChoice == "1")
                    {
                        randomEnemy = ChooseRandomEnemy();
                    }
                }
            }
            else
            {
                while (hero.Hp > 0 && randomEnemy.Hp > 0)
                {
                Console.Clear();
                hero.HandleDamageOverTime();
                hero.DisplayStatus();
                randomEnemy.DisplayStatus();

                Console.WriteLine("Choose an action:");
                Console.WriteLine("1. Attack");
                Console.WriteLine("2. Use skill");
                Console.WriteLine("3. Run");
                int action = int.Parse(Console.ReadLine());

                if (action == 1)
                {
                    hero.Attack(randomEnemy);
                }
                
                if (action == 2)
                {
                    hero.UseSkill(randomEnemy);
                }

                if (action == 3)
                {
                    bool escaped = TryToRun(hero, randomEnemy);
            
                    if (escaped)
                    {
                        Console.WriteLine("You ran from the battle...");
                        Console.WriteLine("Keep grinding?");
                        Console.WriteLine("1. Keep grinding");
                        Console.WriteLine("2. Go back to town");
                        string fleeChoice = Console.ReadLine();

                        if (fleeChoice == "2")
                        {
                            Console.WriteLine("Teleporting back to town...");
                            Environment.Exit(0);
                        }
                        else if (fleeChoice == "1")
                        {
                            randomEnemy = ChooseRandomEnemy();
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("You failed to run! The battle continues...");
                    }

                }
                if (randomEnemy.Hp > 0)
                {
                    randomEnemy.Attack(hero);
                }

                if (randomEnemy.Hp <= 0)
                {
                    Console.WriteLine("Enemy has been defeated!");
                    Console.WriteLine($"You gain 10 EXP!");
                    hero.GainExp(10);
                    Console.WriteLine("Keep grinding?");
                    Console.WriteLine("1. Keep grinding");
                    Console.WriteLine("2. Go back to town");
                    string choice = Console.ReadLine();
                    

                    if (choice == "2")
                    {
                        Console.WriteLine("Teleporting back to town...");
                        Environment.Exit(0);
                    }
                    else if (choice == "1")
                    {
                        randomEnemy = ChooseRandomEnemy();
                        break;
                    }
                }

                if (hero.Hp < 0)
                {
                    Console.WriteLine("You has been defeated!");
                    Environment.Exit(0);
                }

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                }
            }
        }
        
    }

    public static void BattleWithBoss(Player hero, Enemy boss)
    {

        while (hero.Hp > 0 && boss.Hp > 0)
        {
            Console.Clear();
            hero.HandleDamageOverTime();
            hero.DisplayStatus();
            boss.DisplayStatus();

            if (hero.IsStunned)
            {
                Console.WriteLine($"{hero.Name} is stunned!");
            }
            else
            {
            Console.WriteLine("Choose an action:");
            Console.WriteLine("1. Attack");
            Console.WriteLine("2. Use skill");
            Console.WriteLine("3. Run");
            int action = int.Parse(Console.ReadLine());

            if (action == 1)
            {
                hero.Attack(boss);
            }
            if (action == 2)
            {
                hero.UseSkill(boss);
            }
            if (action == 3)
            {
                bool escaped = TryToRun(hero, boss);

                if (escaped)
                {
                    Console.WriteLine("You ran from the battle...");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
                }
                else
                {
                    Console.WriteLine("You failed to run! The battle continues...");
                }
            }    
            }

            
            
            if (boss.Hp > 0)
            {
                boss.Attack(hero);
            }
            if (boss.Hp <= 0)
            {
                Console.WriteLine($"Congratulations! You defeated {boss.Name}!");
                hero.GainExp(500);
            }
            if (hero.Hp <= 0)
            {
                Console.WriteLine("You have been defeated!");
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }

    private static bool TryToRun(Player hero, Enemy enemy)
    {
        Random rand = new Random();
        int escapeChance = 70;

        if (hero.Hp < hero.MaxHp / 2)
        {
            escapeChance -= 20;
        }

        int chance = rand.Next(1,101);
        return chance <= escapeChance;
        
    }
}