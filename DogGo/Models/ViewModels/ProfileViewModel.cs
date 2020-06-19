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
        
    }
}
