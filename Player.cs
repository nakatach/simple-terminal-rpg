using System;
using System.Security.Cryptography.X509Certificates;

public abstract class Player
{
    public string Name { get; set; }
    public int Hp { get; set; }
    public int MaxHp { get; set; }
    public int Mana { get; set; }
    public int MaxMana { get; set; }
    public int AttackPower { get; set; }
    public int Defense { get; set; }
    private int ImmuneChance {get; set;}
    private int ImmuneTurnsRemaining {get; set;}
    public int experience {get; set;}
    public int expToNextLevel {get; set;}
    public int level {get; set;}
    private int DamagePerTurn {get; set;}
    private int DOTTurnsRemaining {get; set;}
    private int ReducedAttackPower {get; set;}
    private int ReducedAttackTurnsRemaining {get; set;}
    private int OriginalAttackPower {get; set;}
    public bool IsStunned {get; private set;}
    public int StunTurnsRemaining {get; private set;}
    public Player(string name, int hp, int mana, int attackPower, int defense)
    {
        Name = name;
        MaxHp = Hp = hp;
        MaxMana = Mana = mana;
        AttackPower = attackPower;
        Defense = defense;
        ImmuneChance = 0;
        ImmuneTurnsRemaining = 0;
        ReducedAttackPower = 0;
        ReducedAttackTurnsRemaining = 0;
        OriginalAttackPower = attackPower;
        IsStunned = false;
        StunTurnsRemaining = 0;
        experience = 0;
        expToNextLevel = 100;
        level = 1;
    }

    public override string ToString()
    {
        return $"{GetType().Name}: HP {Hp}/{MaxHp}, Mana {Mana}/{MaxMana}, Attack Power {AttackPower}, Defense {Defense}, Immune: {ImmuneTurnsRemaining}, EXP {experience}/{expToNextLevel}";
    }

    public void ApplyDamageOverTime (int damage, int turns)
    {
        DamagePerTurn = damage;
        DOTTurnsRemaining = turns;
        Console.WriteLine($"{Name} is afflicted with damage overtime: {damage} per turn for {turns} turn(s)");
    }

    public void HandleDamageOverTime()
    {
        if (DOTTurnsRemaining > 0)
        {
            Hp -= DamagePerTurn;
            Console.WriteLine($"{Name} takes {DamagePerTurn} damage from damage overtime! HP: {Hp}/{MaxHp} ");
            DOTTurnsRemaining --;

            if (Hp <= 0)
            {
                Hp = 0;
                Console.WriteLine($"{Name} has been defeated!");
            }
        }
    }

    public void AppyReducedAttackPower (int reductionAmmount, int turns)
    {
        if (ReducedAttackTurnsRemaining > 0)
        {
            Console.WriteLine($"{Name} is affected by weaken!");
            return;
        }

        ReducedAttackPower = reductionAmmount;
        ReducedAttackTurnsRemaining = turns;
        AttackPower -= reductionAmmount;

        if (AttackPower < 0) AttackPower = 0;
        Console.WriteLine($"{Name}'s attack power is reduced by {reductionAmmount} for {turns} turn(s). New attack power: {AttackPower}");
    }

    private void HandleReducedAttackPower()
    {
        if (ReducedAttackTurnsRemaining > 0)
        {
            ReducedAttackTurnsRemaining--;
            if (ReducedAttackTurnsRemaining <= 0)
            {
                AttackPower += ReducedAttackPower;
                ReducedAttackPower = 0;
                Console.WriteLine($"{Name}'s attack power has returned to normal.");
            };
        }
    }
    public void ApplyImmune(int chance, int turns)
    {
        if (ImmuneTurnsRemaining > 0)
        {
            Console.WriteLine($"{Name} gains immunity!");
            return;
        }

        ImmuneChance = chance;
        ImmuneTurnsRemaining = turns;
        Console.WriteLine($"{Name} gains immunity with a {chance}% chance for {turns} turns(s).");
    }

    public bool IsImmune()
    {
        if (ImmuneTurnsRemaining > 0)
        {
            Random random = new Random();
            int roll = random.Next(0,100);
            if (roll < ImmuneChance)
            {
                Console.WriteLine($"{Name} avoids attack due to immunity!");
                return true;
            }
        }
        return false;
    }

    private void HandleImmune()
    {
        if (ImmuneTurnsRemaining > 0)
        {
            ImmuneTurnsRemaining--;
            if (ImmuneTurnsRemaining <= 0)
            {
                ImmuneChance = 0;
                Console.WriteLine($"{Name}'s immunity has worn off.");
            }
        }
    }


    public void TakeDamage(int damage)
    {
        if (IsImmune())
        {
            return;
        }

        if (damage < Defense)
        {
            Defense -= damage;
            if (Defense < 0)
            {
                Defense = 0;
            }
            Console.WriteLine($"{Name}'s defense has been reduced by {damage}. New defense: {Defense}");
            damage = 0;
        }
        else
        {
            int actualDamage = Math.Max(damage - Defense, 0);
            Hp -= actualDamage;
            Console.WriteLine($"{Name} takes {actualDamage} damage! HP: {Hp}/{MaxHp}");
        }

        if (Hp <= 0)
        {
            Hp = 0;
            Console.WriteLine($"{Name} has been defeated!");
        }
    }

    public void Stun(int turns)
    {
        StunTurnsRemaining = turns;
        IsStunned = true;
        Console.WriteLine($"{Name} is stunned for {turns} turn(s).");
    }
    public void Attack(Enemy enemy)
    {
        Console.WriteLine($"{Name} attacks {enemy.Name}!");

        if (AttackPower < enemy.Defense)
        {
            enemy.Defense -= AttackPower;
            if (enemy.Defense < 0)
            {
                enemy.Defense = 0;
            }

            Console.WriteLine($"{Name}'s attack is too weak! {enemy.Name}'s defense is reduced by {AttackPower}. New defense: {enemy.Defense}");
        }
        else
        {
            int actualDamage = Math.Max(AttackPower - enemy.Defense, 0);
            enemy.Hp -= actualDamage;
            Console.WriteLine($"{enemy.Name} takes {actualDamage} damage! {enemy.Name}'s HP: {enemy.Hp}/{enemy.MaxHp}");
        }
        Mana += 10;
        Hp += 10;
    }

    public virtual void DisplayStatus()
    {
        Console.WriteLine($"[Player] {Name}: HP {Hp}/{MaxHp}, Mana {Mana}/{MaxMana}, Attack Power: {AttackPower}, Defense: {Defense}, Immune: {ImmuneChance}, EXP: {experience}, EXP To Level Up: {expToNextLevel}");
    }

    public void GainExp(int exp)
    {
        experience += exp;
        Console.WriteLine($"{Name} gained {exp} EXP!");
        
        if (experience >= expToNextLevel)
        {
            LevelUp();
        }

    }
    private void LevelUp()
    {
        level++;
        experience -= expToNextLevel;
        expToNextLevel = level * 50;
        Console.WriteLine($"Congratulations! You've reached level {level}!");
    }

    public abstract void UseSkill(Enemy enemy);
}

class Novice : Player
{
    public Novice(string name) : base(name, 100, 100, 10, 10)
    {
        Console.WriteLine("You are a Novice! Train hard to become stronger!");
    }

    public override void UseSkill(Enemy enemy)
    {
        Console.WriteLine("Novice does not have any skill..");
    }
}

class Swordman : Player
{
    public Swordman(string name) : base(name, 1000, 100, 100, 50) { }

    public override void UseSkill(Enemy enemy)
    {
        Console.WriteLine("Choose a skill:");
        Console.WriteLine("1. Shield Bash (10 Mana)");
        Console.WriteLine("2. Rage (20 Mana)");
        Console.WriteLine("3. Raging Slash (10 Mana)");
        Console.WriteLine("4. Take This (40 Mana)");

        int choice = int.Parse(Console.ReadLine());
        switch (choice)
        {
            case 1:
                if (Mana >= 10)
                {
                    Console.WriteLine($"{Name} used Shield Bash!");
                    Mana -= 10;
                    enemy.TakeDamage(AttackPower);
                    enemy.Stun(2);
                    Console.WriteLine($"{enemy.Name} is stunned for 2 turns!");
                }
                else Console.WriteLine("Not enough mana.");
                break;

            case 2:
                if (Mana >= 20)
                {
                    Console.WriteLine($"{Name} used Rage!");
                    Mana -= 20;
                    AttackPower += (int)(AttackPower * 0.5);
                    Console.WriteLine($"{Name}'s Attack Power increased to {AttackPower} for 2 turns!");
                }
                else Console.WriteLine("Not enough mana.");
                break;

            case 3:
                if (Mana >= 10)
                {
                    Console.WriteLine($"{Name} used Raging Slash!");
                    Mana -= 10;
                    enemy.TakeDamage(AttackPower);
                }
                else Console.WriteLine("Not enough mana.");
                break;

            case 4:
                if (Mana >= 40)
                {
                    Console.WriteLine($"{Name} used Take This!");
                    Mana -= 40;
                    enemy.TakeDamage(AttackPower * 5);
                    if (new Random().Next(0, 100) < 20)
                        enemy.Stun(2);
                }
                else Console.WriteLine("Not enough mana.");
                break;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}

class Archer : Player
{
    public Archer(string name) : base(name, 800, 100, 150, 20) { }

    public override void UseSkill(Enemy enemy)
    {
        Console.WriteLine("Choose a skill:");
        Console.WriteLine("1. Arrow Rain (10 Mana)");
        Console.WriteLine("2. The Floor Is Poison (20 Mana)");
        Console.WriteLine("3. Bowmerang (15 Mana)");
        Console.WriteLine("4. Straight To Your Heart (40 Mana)");

        int choice = int.Parse(Console.ReadLine());
        switch (choice)
        {
            case 1:
                if (Mana >= 10)
                {
                    Console.WriteLine($"{Name} used Arrow Rain!");
                    Mana -= 10;
                    enemy.TakeDamage(2 * AttackPower);
                }
                else Console.WriteLine("Not enough mana.");
                break;

            case 2:
                if (Mana >= 20)
                {
                    Console.WriteLine($"{Name} used The Floor Is Poison!");
                    enemy.ApplyDamageOverTime((int)(0.05 * AttackPower), 3);
                    Mana -= 20;
                    
                }
                else Console.WriteLine("Not enough mana.");
                break;

            case 3:
                if (Mana >= 15)
                {
                    Console.WriteLine($"{Name} used Bowmerang!");
                    Mana -= 15;
                    enemy.TakeDamage(2 * AttackPower);
                    if (new Random().Next(0, 100) < 1)
                    {
                        enemy.Hp -= enemy.Hp;
                    }
                }
                else Console.WriteLine("Not enough mana.");
                break;

            case 4:
                if (Mana >= 40)
                {
                    Console.WriteLine($"{Name} used Straight To Your Heart!");
                    Mana -= 40;
                    enemy.TakeDamage(AttackPower * 4);
                    enemy.ApplyDamageOverTime((int)(0.08 * AttackPower), 3);
                }
                else Console.WriteLine("Not enough mana.");
                break;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}

class Mage : Player
{
    public Mage(string name) : base(name, 750, 200, 120, 20) { }

    public override void UseSkill(Enemy enemy)
    {
        Console.WriteLine("Choose a skill:");
        Console.WriteLine("1. Fireball (10 Mana)");
        Console.WriteLine("2. Breeze and Freeze (10 Mana)");
        Console.WriteLine("3. Astral Bind (15 Mana)");
        Console.WriteLine("4. Finale (40 Mana)");

        int choice = int.Parse(Console.ReadLine());
        switch (choice)
        {
            case 1:
                if (Mana >= 10)
                {
                    if (enemy.Name is "Hydro Abyss Mage")
                    {
                        Console.WriteLine($"{Name} used Fireball!");
                        Mana -= 10;
                        enemy.TakeDamage(AttackPower);

                        Console.WriteLine("Hydro Abyss Mage is immune to burning!");
                    }
                    else
                    {
                        Console.WriteLine($"{Name} used Fireball!");
                        Mana -= 10;
                        enemy.TakeDamage(AttackPower);

                        Console.WriteLine($"{enemy.Name} burned!");
                        enemy.ApplyDamageOverTime((int)(0.05 * enemy.Hp), 3);

                    }
                }
                else Console.WriteLine("Not enough mana.");
                break;

            case 2:
                if (Mana >= 10)
                {
                    Console.WriteLine($"{Name} used Breeze and Freeze!");
                    enemy.TakeDamage(2 * AttackPower);
                    enemy.Freeze(1);
                    Mana -= 10;
                    
                }
                else Console.WriteLine("Not enough mana.");
                break;

            case 3:
                if (Mana >= 15)
                {
                    Console.WriteLine($"{Name} used Astral Bind!");
                    enemy.AppyReducedAttackPower((int)(0.2 * enemy.AttackPower), 4);
                    Mana -= 15;

                }
                else Console.WriteLine("Not enough mana.");
                break;

            case 4:
                if (Mana >= 40)
                {
                    Console.WriteLine($"{Name} used Finale!");
                    Mana -= 40;
                    enemy.TakeDamage(AttackPower * 5);
                }
                else Console.WriteLine("Not enough mana.");
                break;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}

class Assassin : Player
{
    public Assassin(string name) : base(name, 500, 100, 300, 20) { }

    public override void UseSkill(Enemy enemy)
    {
        Console.WriteLine("Choose a skill:");
        Console.WriteLine("1. Deadly Dance (15 Mana)");
        Console.WriteLine("2. Rapid Shot (15 Mana)");
        Console.WriteLine("3. Shadow Step (20 Mana)");
        Console.WriteLine("4. Assassinate (40 Mana)");

        int choice = int.Parse(Console.ReadLine());
        switch (choice)
        {
            case 1:
                if (Mana >= 15)
                {
                    Console.WriteLine($"{Name} used Deadly Dance!");
                    Mana -= 15;
                    enemy.TakeDamage(3 * AttackPower);
                    
                    if (new Random().Next(0, 100) < 20)
                    {
                        enemy.ApplyDamageOverTime((int)(0.05 * enemy.Hp), 3);
                    }
                }
                else Console.WriteLine("Not enough mana.");
                break;

            case 2:
                if (Mana >= 15)
                {
                    Console.WriteLine($"{Name} used Rapid Shot!");
                    enemy.TakeDamage((int)(0.1 * enemy.Hp));
                    Mana -= 15;
                    
                }
                else Console.WriteLine("Not enough mana.");
                break;

            case 3:
                if (Mana >= 20)
                {
                    Console.WriteLine($"{Name} used Shadow Step!!");
                    Mana -= 20;
                    ApplyImmune(50, 4);
                }
                else Console.WriteLine("Not enough mana.");
                break;

            case 4:
                if (Mana >= 40)
                {
                    Console.WriteLine($"{Name} used Assassinate!");
                    Mana -= 40;
                    enemy.TakeDamage(AttackPower * 5);
                    if (new Random().Next(0, 100) < 20)
                    {
                        enemy.Hp -= enemy.Hp;
                    }
                }
                else Console.WriteLine("Not enough mana.");
                break;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}