﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace libraryClubEF
{
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StamNummer { get; set; } //niet automatisch te genereren
        public string TeamNaam { get; set; }
        public string TeamBijnaam { get; set; }
        public string Trainer { get; set; }//enkel naam
        public ICollection<Speler> spelers { get; set; } = new List<Speler>();

        //simpele constructor voor EF
        public Team(int stamNummer, string teamNaam, string teamBijnaam, string trainer)
        {
            StamNummer = stamNummer;
            TeamNaam = teamNaam;
            TeamBijnaam = teamBijnaam;
            Trainer = trainer;
        }

        public Team(int stamNummer, string teamNaam, string teamBijnaam, string trainer, ICollection<Speler> spelers) : this(stamNummer, teamNaam, teamBijnaam, trainer)
        {
            this.spelers = spelers;
        }

        public override bool Equals(object obj)
        {
            return obj is Team team &&
                   StamNummer == team.StamNummer;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StamNummer);
        }
    }
}
