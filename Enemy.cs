using System;

public abstract class Enemy
{
    public string Name { get; set; }
    public int Hp { get; set; }
    public int MaxHp { get; set; }
    public int AttackPower { get; set; }
    public int Defense { get; set; }
    public bool IsStunned { get; private set; }
    public int StunTurnsRemaining { get; private set; }
    public bool IsFrozen {get; private set;}
    public int FreezeTurnRemaining {get; private set;}
    private int OriginalDefense;
    private int DamagePerTurn {get; set;}
    private int DOTTurnsRemaining {get; set;}
    private int ReducedAttackPower {get; set;}
    private int ReducedAttackTurnsRemaining {get; set;}
    private int OriginalAttackPower {get; set;}
    private int ImmuneChance {get; set;}
    private int ImmuneTurnsRemaining {get; set;}
    public int ExpValue {get; set;}
    public Enemy(string name, int hp, int attackPower, int defense)
    {
        Name = name;
        MaxHp = Hp = hp;
        AttackPower = attackPower;
        Defense = defense;
        IsStunned = false;
        StunTurnsRemaining = 0;
        IsFrozen = false;
        FreezeTurnRemaining = 0;
        DamagePerTurn = 0;
        DOTTurnsRemaining = 0;
        ReducedAttackPower = 0;
        ReducedAttackTurnsRemaining = 0;
        OriginalAttackPower = attackPower;
        ImmuneChance = 0;
        ImmuneTurnsRemaining = 0;
    }

    public abstract void UseSkill(Player player);
    public virtual void DisplayStatus()
    {
        Console.WriteLine($"[Enemy] {Name}: HP {Hp}/{MaxHp}, Attack Power {AttackPower}, Defense {Defense}, Stunned: {IsStunned}, Frozen : {IsFrozen}");

        if (IsStunned)
        {
            Console.WriteLine($"{Name} is stunned and will not act this turn.");
        }

        if (IsFrozen)
        {
            Console.WriteLine($"{Name} is frozen and cannot act this turn. Defene is reduced to 0.");
        }
    }

    public void TakeDamage(int damage)
    {
        HandleImmune();
        if (IsImmune())
        {
            Console.WriteLine($"{Name} avoids damage due to immunity!");
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

    public void Attack(Player player)
    {
        HandleDamageOverTime();
        HandleReducedAttackPower();

        if (Hp <= 0)
        {
            return;
        }

        if (IsStunned)
        {
            Console.WriteLine($"{Name} is stunned and cannot attack this turn!");
            StunTurnsRemaining--;
            if (StunTurnsRemaining <= 0)
            {
                IsStunned = false;
                Console.WriteLine($"{Name} is no longer stunned.");
            }
            return;
        }

        if (IsFrozen)
        {
            Console.WriteLine($"{Name} is frozen and cannot act this turn!");
            FreezeTurnRemaining--;
            if (FreezeTurnRemaining <= 0)
            {
                IsFrozen = false;
                Defense = OriginalDefense;
                Console.WriteLine($"{Name} is no longer frozen, defense is restored.");
            }
            return;
        }

        Random random = new Random();
        int action = random.Next(1, 101);

        if (action <= 50)
        {
            Console.WriteLine($"{Name} decides to use a skill!");
            UseSkill(player);
        }
        else
        {
            Console.WriteLine($"{Name} attacks {player.Name}!");
            int damage = Math.Max(AttackPower - player.Defense, 0);
            player.TakeDamage(damage);
            Console.WriteLine($"{Name} attacks {player.Name} for {damage} damage!");
        }
    }


    public void Stun(int turns)
    {
        IsStunned = true;
        StunTurnsRemaining = turns;
        Console.WriteLine($"{Name} is stunned for {turns} turn(s).");
    }
    public void Freeze(int turns)
    {
        IsFrozen = true;
        FreezeTurnRemaining = turns;
        OriginalDefense = Defense;
        Defense = 0;
        Console.WriteLine($"{Name} is frozen for {turns} turn(s)! Defense is reduced to 0.");

    }

    public void ApplyDamageOverTime (int damage, int turns)
    {
        DamagePerTurn = damage;
        DOTTurnsRemaining = turns;
        Console.WriteLine($"{Name} is afflicted with damage overtime: {damage} per turn for {turns} turn(s)");
    }

    private void HandleDamageOverTime()
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
            int roll = random.Next(0,101);
            if (roll < ImmuneChance)
            {
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

}

class PyroAbyssMage : Enemy
{
    public PyroAbyssMage() : base("Pyro Abyss Mage", 120, 30, 5) { }

    public override void DisplayStatus()
    {
        base.DisplayStatus();
    }

    public void Fireball(Player player)
    {
        Console.WriteLine($"{Name} casts Fireball!");

        int damage = 2 * AttackPower;
        player.TakeDamage(damage);
        Console.WriteLine($"Fireball hits {player.Name} for {damage} damage!");

        player.ApplyDamageOverTime(5, 3);
        Console.WriteLine($"{player.Name} is burned, taking 5 damage per turn for 3 turn(s)!");
    }

    public void PyroShield()
    {
        Console.WriteLine($"{Name} casts Pyro Shield!");
        Defense += 40;
        Console.WriteLine($"{Name}'s defense is increase to {Defense}!");
    }

    public override void UseSkill(Player player)
    {
        Random random = new Random();
        int skillChoice = random.Next(1, 3);

        if (skillChoice == 1)
        {
            Fireball(player);
        }
        else if (skillChoice == 2)
        {
            PyroShield();
        }
    }
}

class HydroAbyssMage : Enemy
{
    public HydroAbyssMage() : base("Hydro Abyss Mage", 120, 30, 5) { }

    public override void DisplayStatus()
    {
        base.DisplayStatus();

    }

    public void HealingRain()
    {
        Console.WriteLine($"{Name} casts Healing Rain!");
        Hp += 50;
        Console.WriteLine($"{Name} is healed!");
    }

    public void AquaProjectile(Player player)
    {
        Console.WriteLine($"{Name} casts Aqua Projectile!");
        int damage = 2 * AttackPower;
        player.TakeDamage(damage);
        Console.WriteLine($"Aqua Projectile hits {player.Name} for {damage} damage!");
    }

     public override void UseSkill(Player player)
    {
        Random random = new Random();
        int skillChoice = random.Next(1, 3);

        if (skillChoice == 1)
        {
            HealingRain();
        }
        else if (skillChoice == 2)
        {
            AquaProjectile(player);
        }
    }
}

class Rifthound : Enemy
{
    public Rifthound() : base("Rifthound", 200, 50, 15) { }

    public override void DisplayStatus()
    {
        base.DisplayStatus();
    }

    public void DimensionalStalk(Player player)
    {
        Console.WriteLine($"{Name} casts Dimensional Stalk!");
        int damage = 2 * AttackPower;
        player.TakeDamage(damage);
        Console.WriteLine($"Dimensional Stalk hits player for {damage} damage!");
    }

    public void DinnerTime()
    {
        Console.WriteLine($"{Name} casts Dinner Time!");
        AttackPower += 2 * AttackPower;
        Console.WriteLine($"{Name}'s Attack Power is increased to {AttackPower}");
    }

    public override void UseSkill(Player player)
    {
        Random random = new Random();
        int skillChoice = random.Next(1, 3);

        if (skillChoice == 1)
        {
            DimensionalStalk(player);
        }
        else if (skillChoice == 2)
        {
            DinnerTime();
        }
    }
}

class AbyssLector : Enemy
{
    public AbyssLector() : base("Abyss Lector", 500, 100, 15) { }

    public override void DisplayStatus()
    {
        base.DisplayStatus();
    }

    public void AbyssalCurse(Player player)
    {
        Console.WriteLine($"{Name} casts Abyssal Curse!");
        int damage = (int)(1.5 * AttackPower);
        player.TakeDamage(damage);

        player.ApplyDamageOverTime(10, 3);
        player.AppyReducedAttackPower(50, 3);
    }

    public void AbyssalDevourer(Player player)
    {
        Console.WriteLine($"{Name} casts Abyssal Devourer!");
        int damage = (int)(0.05 * player.Hp);
        player.TakeDamage(damage);
        Hp += damage;
        Console.WriteLine($"{Name} devour {player.Name}'s HP and healed itself for {damage} HP!");
    }

    public override void UseSkill(Player player)
    {
        Random random = new Random();
        int skillChoice = random.Next(1, 3);

        if (skillChoice == 1)
        {
            AbyssalCurse(player);
        }
        else if (skillChoice == 2)
        {
            AbyssalDevourer(player);
        }
    }
}