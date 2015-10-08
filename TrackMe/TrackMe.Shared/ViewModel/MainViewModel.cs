using System;
using System.Collections.Generic;
using System.Text;

namespace TrackMe.ViewModel
{
    public class MainViewModel
    {

        public List<Seance> Seances { get; set; }

        public MainViewModel()
        {
            Seances = new List<Seance>();
        }
    }
}
