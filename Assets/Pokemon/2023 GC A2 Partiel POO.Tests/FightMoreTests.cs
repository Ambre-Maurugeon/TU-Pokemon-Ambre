using _2023_GC_A2_Partiel_POO.Level_2;
using NUnit.Framework;

using System;

namespace _2023_GC_A2_Partiel_POO.Tests.Level_2
{
    public class FightMoreTests
    {
        // Tu as probablement remarqué qu'il y a encore beaucoup de code qui n'a pas été testé ...
        // À présent c'est à toi de créer des features et les TU sur le reste du projet

        // Ce que tu peux ajouter:
        // - Ajouter davantage de sécurité sur les tests apportés
        // - un heal ne régénère pas plus que les HP Max
        // - si on abaisse les HPMax les HP courant doivent suivre si c'est au dessus de la nouvelle valeur
        // - ajouter un equipement qui rend les attaques prioritaires puis l'enlever et voir que l'attaque n'est plus prioritaire etc)
        // - Le support des status (sleep et burn) qui font des effets à la fin du tour et/ou empeche le pkmn d'agir
        // - Gérer la notion de force/faiblesse avec les différentes attaques à disposition (skills.cs)
        // - Cumuler les force/faiblesses en ajoutant un type pour l'équipement qui rendrait plus sensible/résistant à un type
        // - L'utilisation d'objets : Potion, SuperPotion, Vitess+, Attack+ etc.
        // - Gérer les PP (limite du nombre d'utilisation) d'une attaque,
        // si on selectionne une attack qui a 0 PP on inflige 0

        // Choisis ce que tu veux ajouter comme feature et fait en au max.
        // Les nouveaux TU doivent être dans ce fichier.
        // Modifiant des features il est possible que certaines valeurs
        // des TU précédentes ne matchent plus, tu as le droit de réadapter les valeurs
        // de ces anciens TU pour ta nouvelle situation.

        [Test] 
        public void HealMoreThanMaxHealth() // - un heal ne régénère pas plus que les HP Max
        {
            var pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            pikachu.Heal(120);
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(100));

            var punch = new Punch();
            pikachu.ReceiveAttack(punch);
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(60)); // 100 - (70 -30)
            pikachu.Heal(20);
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(80)); // 60 + 20
            pikachu.Heal(40);
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(100));

            var shield = new Equipment(40, 0, 0, 0);
            pikachu.Equip(shield);
            pikachu.ReceiveAttack(punch);
            pikachu.Heal(120);
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(100));
            Assert.That(pikachu.MaxHealth, Is.EqualTo(140));
        }


        //[Test]
        //public void ShieldAndHealthAreIndependent() 
        //{
        //    var pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
        //    var shield = new Equipment(40, 0, 0, 0);

        //    pikachu.Equip(shield);
        //    pikachu.Heal(20, true);
        //    Assert.That(pikachu.MaxHealth, Is.EqualTo(160)); // 100 + 40 + 20
        //    pikachu.Heal(10, true); 
        //    Assert.That(pikachu.CurrentEquipment.BonusHealth, Is.EqualTo(60)); // 40 + 20 + 10

        //}

        [Test]
        public void CurrentHPDoesNotExceedHPMax()
        {
            var pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            pikachu.DecreaseMaxHealth(20);
            Assert.That(pikachu.MaxHealth, Is.EqualTo(20));
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(pikachu.MaxHealth)); // 100 -20 => 80
        }

        [Test]
        public void PriorityOfAttacks()
        {
            Character pikachu = new Character(10, 50, 30, 20, TYPE.NORMAL);
            Character mewtwo = new Character(30, 70, 10, 200, TYPE.NORMAL);

            Fight f = new Fight(pikachu, mewtwo);
            MegaPunch p = new MegaPunch();

            Equipment speed = new Equipment(0, 0, 0, 500);
            pikachu.Equip(speed);
            Assert.That(pikachu.Speed, Is.EqualTo(520)); // 500 + 20

            //pikachu attacks first, one shot mextxo
            f.ExecuteTurn(p, p);

            Assert.That(pikachu.IsAlive, Is.EqualTo(true));
            Assert.That(mewtwo.IsAlive, Is.EqualTo(false));
            Assert.That(f.IsFightFinished, Is.EqualTo(true));

            pikachu.Unequip();

            pikachu.Heal(pikachu.MaxHealth);
            mewtwo.Heal(mewtwo.MaxHealth);

            //mewtwo attacks first, oneshot pikachu
            f.ExecuteTurn(p, p);

            Assert.That(mewtwo.IsAlive, Is.EqualTo(true));
            Assert.That(pikachu.IsAlive, Is.EqualTo(false));
            Assert.That(f.IsFightFinished, Is.EqualTo(true));
        }

        [Test]
        public void AttackNumberPerSkill()
        {
            Character pikachu = new Character(10, 50, 30, 20, TYPE.NORMAL);
            Character mewtwo = new Character(30, 70, 10, 200, TYPE.NORMAL);

            Fight f = new Fight(pikachu, mewtwo);
            MegaPunch mp = new MegaPunch();
            Punch p = new Punch();

            f.ExecuteTurn(p, p);

            Assert.That(mewtwo.IsAlive, Is.EqualTo(true));
            Assert.That(pikachu.IsAlive, Is.EqualTo(false));
            Assert.That(f.IsFightFinished, Is.EqualTo(true));

            pikachu.Heal(pikachu.MaxHealth);
            mewtwo.Heal(mewtwo.MaxHealth);

            Assert.Throws<ArgumentNullException>(() =>
            {
                f.ExecuteTurn(p, p);
            });


        }

    }
}
