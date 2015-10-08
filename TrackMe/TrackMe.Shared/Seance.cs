using System;
using System.Collections.Generic;
using System.Text;

namespace TrackMe
{
  public class Seance
    {
        public int Id { get; set; }
        public DateTime DateSeance { get; set; }
        public double Vitesse { get; set; }
        public double Calories { get; set; }
        public double Distance { get; set; }
        public int Duree { get; set; }

        public Seance(){}
        public Seance(int Id, DateTime DateSeance, double Vitesse, double Calories, double Distance, int Duree) {
            this.Id = Id;
            this.DateSeance = DateSeance;
            this.Vitesse = Vitesse;
            this.Calories = Calories;
            this.Duree = Duree;
            this.Distance = Distance;
        }
        public Seance(DateTime DateSeance, double Vitesse, double Calories, double Distance, int Duree)
        {
            this.DateSeance = DateSeance;
            this.Vitesse = Vitesse;
            this.Calories = Calories;
            this.Duree = Duree;
            this.Distance = Distance;
        }
    }
}
