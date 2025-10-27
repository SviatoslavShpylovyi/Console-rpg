using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public interface IObserverPotion
    {
        void OnEndEffect(int id);
    }
    public abstract class Potion : Iitem
    {
        protected IObserverPotion _observer;
        protected int Duration;
        public int Pot_ID;
        public bool IsUsable => true;

        public string Name { get; protected set; }

        public string Description { get; protected set; }
        public void Attach_observer(IObserverPotion observer)
        {
            _observer = observer;
        }
        public void OnStep(Player p)
        {
            if (Duration <= 0)
                return;
            applyEffect(p);
            Duration--;
            if (Duration == 0)
            {
                EndEffect(p);
                _observer?.OnEndEffect(Pot_ID);
            }
        }
        protected abstract void applyEffect(Player p);
        protected abstract void EndEffect(Player p);

        public void Equip(int hand, Player p)
        { return; }

        public void Unequip(int hand, Player p)
        {
            return;
        }
        public void _Pickup(Player p)
        {
            p.Add_item_to_PotionBag(this);
        }
        public Potion(string name, string description, int duration)
        {
            Name = name;
            Description = description;
            Duration = duration;
            Random rand = new Random();
            Pot_ID = rand.Next(1, 156);
        }
        public int GetDuration()
        {
            return Duration;
        }
        public abstract void Start(Player p);

    }

    public class HealthPotion : Potion
    {
        public HealthPotion(int duration) : base("Lucky_potion", " a small flask to increase the luck", duration) { }
        protected override void EndEffect(Player p)
        {
            p.Luck = 5;
        }
        protected override void applyEffect(Player p)
        {
            p.Luck *= Duration;
        }
        public override void Start(Player p)
        {
            Attach_observer(p);
            p.Luck += 5;
        }
    }
    public class StrenghtPotion : Potion
    {

        public StrenghtPotion(int duration) : base("StrenghtPotion", " Dramatically increases the health making you immortal", duration) { }

        protected override void applyEffect(Player p)
        {
            return;
        }
        public override void Start(Player p)
        {
            Attach_observer(p);
            p.Health += 5000;
        }
        protected override void EndEffect(Player p)
        {
            p.Health = 25;
        }
        public class WisdomPotion : Potion
        {
            public WisdomPotion(int duration) : base("Wisdom_Potion", " Increases Wisdom Permanently", duration) { }

            public override void Start(Player p)
            {
                p.Wisdom += 15;
            }

            protected override void applyEffect(Player p)
            {
                return;
            }

            protected override void EndEffect(Player p)
            {
                return;
            }
        }

    }
}
