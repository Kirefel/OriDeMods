using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Randomiser
{
    public class RandomiserAction : ISerializable
    {
        public string action;
        public string[] parameters;

        public RandomiserAction(string action, string[] parameters)
        {
            this.action = action;
            this.parameters = parameters;

            if (!handlers.ContainsKey(action))
                Randomiser.Message("Unknown action: " + action);
        }

        public void Serialize(Archive ar)
        {
            ar.Serialize(ref action);
            parameters = ar.Serialize(parameters);
        }

        public delegate RandomiserActionResult RandomiserHandlerDelegate();
        static readonly Dictionary<string, RandomiserHandlerDelegate> handlers;
        static RandomiserAction()
        {
            handlers = new Dictionary<string, RandomiserHandlerDelegate>();

            var methods = typeof(RandomiserAction).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttributes(typeof(RandomiserActionHandlerAttribute), false).FirstOrDefault() as RandomiserActionHandlerAttribute;
                if (attribute == null)
                    continue;

                handlers[attribute.ActionID] = (RandomiserHandlerDelegate)Delegate.CreateDelegate(typeof(RandomiserAction), method);
            }
        }

        private static string Wrap(string str, char wrapChar) => $"{wrapChar}{str}{wrapChar}";

        public void Execute()
        {
            if (!handlers.ContainsKey(action))
            {
                Randomiser.Message("Unknown action: " + action);
                return;
            }

            RandomiserActionResult result = handlers[action]();

            string message = result.decoration.HasValue ? Wrap(result.text, result.decoration.Value) : result.text;
            Randomiser.Message(message);
        }

        [RandomiserActionHandler("SK")]
        private RandomiserActionResult HandleSkill()
        {
            var skill = (AbilityType)Enum.Parse(typeof(AbilityType), parameters[0]);
            Characters.Sein.PlayerAbilities.SetAbility(skill, true);

            return new RandomiserActionResult(skill.ToString(), '#');
        }

        [RandomiserActionHandler("EC")]
        private RandomiserActionResult HandleEC()
        {
            var sein = Characters.Sein;
            if (sein.Energy.Max == 0f)
                sein.SoulFlame.FillSoulFlameBar();

            sein.Energy.Max += 1f;
            if (Characters.Sein.Energy.Current < Characters.Sein.Energy.Max)
                sein.Energy.Current = sein.Energy.Max;

            UI.SeinUI.ShakeEnergyOrbBar();

            return new RandomiserActionResult("Energy Cell");
        }

        [RandomiserActionHandler("EX")]
        private RandomiserActionResult HandleSpiritLight()
        {
            int amount = int.Parse(parameters[0]);

            if (Randomiser.Seed.HasFlag(RandomiserFlags.ZeroXP))
                amount = 0;

            Characters.Sein.Level.GainExperience(amount); // TODO multiplier

            return new RandomiserActionResult($"{amount} experience");
        }

        [RandomiserActionHandler("HC")]
        private RandomiserActionResult HandleHC()
        {
            Characters.Sein.Mortality.Health.GainMaxHeartContainer();
            UI.SeinUI.ShakeHealthbar();

            return new RandomiserActionResult("Health Cell");
        }

        [RandomiserActionHandler("AC")]
        private RandomiserActionResult HandleAC()
        {
            Characters.Sein.Level.GainSkillPoint();
            Characters.Sein.Inventory.SkillPointsCollected++;
            UI.SeinUI.ShakeExperienceBar();

            return new RandomiserActionResult("Ability Cell");
        }

        [RandomiserActionHandler("KS")]
        private RandomiserActionResult HandleKS()
        {
            Characters.Sein.Inventory.CollectKeystones(1);
            UI.SeinUI.ShakeKeystones();

            return new RandomiserActionResult("Keystone");
        }

        [RandomiserActionHandler("MS")]
        private RandomiserActionResult HandleMS()
        {
            Characters.Sein.Inventory.MapStones++;
            UI.SeinUI.ShakeMapstones();

            return new RandomiserActionResult("Mapstone Fragment");
        }
    }
}
