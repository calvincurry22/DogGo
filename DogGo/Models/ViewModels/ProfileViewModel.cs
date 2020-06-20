using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class ProfileViewModel
    {
        public Owner Owner { get; set; }
        public Walker Walker { get; set; }
        public List<Walker> Walkers { get; set; }
        public List<Dog> Dogs { get; set; }
        public List<Walk> Walks { get; set; }
        public Neighborhood Neighborhood { get; set; }

        public string ConvertedDurationToHrsMins(List<Walk> walks)
        {
            decimal durationSum = 0;
            foreach (Walk walk in walks)
            {
                durationSum += walk.Duration;
            }
            
            decimal totalHours = (int)Math.Floor(durationSum/3600);
            decimal totalMinutes = (int)Math.Floor((durationSum % 3600) / 60);

            return $"{totalHours}hr {totalMinutes}min";
        }
    }
}
