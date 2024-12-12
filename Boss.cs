class Asmodeus : Enemy
{
    private bool Evo = false;
    public Asmodeus() : base("Asmodeus", 2500, 300, 90) { }

    public override void DisplayStatus()
    {
        base.DisplayStatus();
    }

    public void NoWhereToRun(Player player)
    {
        Console.WriteLine($"{Name} casts No Where To Run!");
        int damage = 2 * AttackPower;
        player.TakeDamage(damage);

    }

    public void AbyssalBarrier()
    {
        
        if (IsImmune())
        {
            Console.WriteLine($"{Name} already has immunity. Healing 100 HP instead of applying new barrier!");
            Hp += 100;
            if (Hp > MaxHp) Hp = MaxHp;
            Console.WriteLine($"{Name} heals 100 HP. Current HP: {Hp}/{MaxHp}");
        }
        else
        {
            Console.WriteLine($"{Name} casts Abyssal Barrier and gain immunity to any skill!");
            ApplyImmune(100, 4);
        }
    }

    public void Zoltraak(Player player)
    {
        Console.WriteLine($"WATCH OUT! {Name} casts Zoltraak!!");
        int damage = (int)(0.33 * player.Hp);

        Random random = new Random();
        if (random.Next(1, 101) <= 5)
        {
            Console.WriteLine($"{Name} performs an instant kill with Zoltraak!");
            player.Hp = 0;
        }
        else
        {
            player.TakeDamage(damage);
        }
    }

    public void Evolve()
    {
        if (!Evo)
        {
            Console.WriteLine($"{Name} is evolving!");
            AttackPower += AttackPower / 2;
            Console.WriteLine($"{Name}'s Attack Power has increased to {AttackPower}!");
        }
    }

    public override void UseSkill(Player player)
    {
        if (Hp <= MaxHp / 2 && !Evo)
        {
            Evolve();
        }

        if (Evo)
        {
            Random rand = new Random();
            int skill = rand.Next(1, 4);

            if (skill == 1)
            {
                NoWhereToRun(player);
            }
            else if (skill == 2)
            {

                AbyssalBarrier();
            }
            else if(skill == 3)
            {
                Zoltraak(player);
            }
        
        }

        Random random = new Random();
        int skillChoice = random.Next(1, 3);

        if (skillChoice == 1)
        {
            NoWhereToRun(player);
        }
        else if (skillChoice == 2)
        {
            AbyssalBarrier();
        }
    }
    
}

class Istaroth : Enemy
{
    public Istaroth() : base("Istaroth", 1500, 150, 90) { }

    public override void DisplayStatus()
    {
        base.DisplayStatus();
    }

    public void CorruptedBlood(Player hero)
    {
        Console.WriteLine($"{Name} casts Corrupted Blood!");
        int damage = (int)(1.5 * AttackPower);
        hero.TakeDamage(damage);
        hero.ApplyDamageOverTime(50, 4);
    }

    public void ComeWithMe(Player player)
    {
        Console.WriteLine($"{Name} casts Come With Me!");
        int damage = player.AttackPower;
        player.TakeDamage(damage);
        AttackPower += damage;
        Console.WriteLine($"{Name} absorp {player.Name}'s Attack Power for {damage} Attack Power!");
    }

    public override void UseSkill(Player player)
    {
        Random random = new Random();
        int skillChoice = random.Next(1, 2);

        if (skillChoice == 1)
        {
            CorruptedBlood(player);
        }
        else if (skillChoice > 1)
        {
            ComeWithMe(player);
        }
    }
}