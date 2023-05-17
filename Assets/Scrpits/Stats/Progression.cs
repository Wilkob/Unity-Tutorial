
using RPG.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats { 
[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression",order = 0)]
public class Progression : ScriptableObject 
{
    [SerializeField] ProgressionCharacterClass[] CharacterClasses = null;

    
        Dictionary<CharacterClass, Dictionary<Stat,float[]>> lookuptable = null;
        public float GetStat(Stat stat,CharacterClass characterClass, int level) 
        {
            BuildLookip();

            float[] levels = lookuptable[characterClass][stat];
            
            if (levels.Length < level) 
            {
                return levels.Length;
            }

            return levels[level-1];
        }
        public int GetLevels(Stat stat, CharacterClass characterClass) 
        {
            BuildLookip();
            float[] levels = lookuptable[characterClass][stat];
            return levels.Length;
        }
        
        private void BuildLookip() 
        {

            if (lookuptable != null) return;
            
            lookuptable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            
            foreach (ProgressionCharacterClass progressionClass in CharacterClasses) 
            {
                Dictionary<Stat, float[]> statlookupTable = new Dictionary<Stat, float[]>();
                foreach (ProgressionStat progressionStat in progressionClass.stats) 
                {
                    statlookupTable[progressionStat.stat] = progressionStat.Levels;
                }
                lookuptable[progressionClass.characterClass] = statlookupTable;
            }
        }

        [System.Serializable]
    class ProgressionCharacterClass
    {
        public CharacterClass characterClass;
        public ProgressionStat[] stats;
            

        }
    [System.Serializable]
    class ProgressionStat 
        {
            public Stat stat;
            public float[] Levels;
        }
}
}