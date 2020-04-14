using System;
using System.Collections.Generic;
using System.Text;

namespace EindWerkProg3
{
    [Serializable]
    class Provincie
    {
        public string ProvincieNaam { get; set; }
        public int Id { get; set; }
        public List<Gemeente> gemeentes = new List<Gemeente>();
        public Provincie(string naam, int id)
        {
            ProvincieNaam = naam;
            Id = id;
        }
    }
}
