using GameDevTV.Utils;
using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Stats { 

public class BaseStats : MonoBehaviour

{
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticaleEffect;
        [SerializeField] bool shouldUseModifiers = true;

        LazyValue<int> currentLevel;
        public event Action onLevelUp;
        Experience experience;
        private void UpdateLevel()
        {
            
            int newLevel = CalcuateLevel();
            if (newLevel > currentLevel.value) 
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
            }
            
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticaleEffect, transform);
            onLevelUp();
        }


        private void Awake()
        {
         experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalcuateLevel);
        }

        private void Start()
        {
            currentLevel.ForceInit();
            

        }
        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }
        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }


        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifer(stat)/100);
        }

        private float GetPercentageModifer(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetPercentageModifiers(stat))
                {
                    total += modifiers;
                }

            }
            return total;
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel()) ;
        }

        public int GetLevel() 
        { 
            return currentLevel.value; 
        }
        private int CalcuateLevel() 
        {

            Experience experience = GetComponent<Experience>();

            if (experience == null) return startingLevel;

            float currentXP = experience.GetExperiencePoints();
            int penultimaeLevel = progression.GetLevels(Stat.ExperienceToLevelup, characterClass);
            for (int level = 1; level <= penultimaeLevel; level++) 
            {

                float XPToLevlUp = progression.GetStat(Stat.ExperienceToLevelup, characterClass,level);
                if (XPToLevlUp > currentXP) 
                {
                return level;
                }
                
            }
            return penultimaeLevel + 1;
        }      
        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifiers in provider.GetAdditiveModifiers(stat)) 
                { 
                total += modifiers;
                }
                
            }
            return total;
            
        }
        
    }
}
