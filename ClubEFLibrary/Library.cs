﻿using libraryClubEF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClubEFLibrary
{
    public class Library
    {
        ClubContext context = new ClubContext();
        public void VoegSpelerToe(Speler speler)
        {
            Console.WriteLine("Adding speler...");
            context.Add(speler);
            context.SaveChanges();
            Console.WriteLine($"speler: {speler.SpelerNaam} added.");
        }
        public void VoegTeamToe(Team team)
        {
            Console.WriteLine("Adding Team...");
            context.Add(team);
            context.SaveChanges();
            Console.WriteLine($"Team: {team.TeamNaam} added.");
        }
        public void VoegTransferToe(Transfer transfer)
        {
            Console.WriteLine("Adding transfer...");
            context.Add(transfer);
            context.SaveChanges();
            Console.WriteLine(transfer + " added.");
        }
        public void UpdateSpeler(Speler speler)
        {
            //object opvragen aanpassen en SaveChanges oproeopen
            using (context)
            {
                #region update
                //speler object opvragen
                /*Speler spelerItem = context.Spelers.SingleOrDefault(spelerDB => spelerDB.SpelerId == speler.SpelerId);
                //
                if (spelerItem != null)
                {
                    //setvalues spelerITem
                    //SaveChanges
                }*/
                #endregion
                context.Update(speler);
                context.SaveChanges();
            }
        }
        public void UpdateTeam(Team team)
        {

        }
        #region Selectie
        /*public Speler SelecteerSpeler(int spelerID) 
        {

        }
        public Team SelecteerTeam(int stamnummer) { }
        public Transfer SelecteerTransfer(int transferID) { }*/
        #endregion
        public void InitialiseerDatabank(string path)
        {
            List<Team> teams = GetTeamsFromFile(path);
            using (context) 
            {
            context.Teams.AddRange(teams);
            context.SaveChanges();
            }
        }
        /// <summary>
        /// Leest lijn per lijn en geeft lijst van lijnen terug opgesplitst door delimeter
        /// </summary>
        /// <param name="path"></param>
        /// <param name="delimeter">indien niets standaard ;</param>
        /// <returns></returns>
        private List<string[]> FileReader(string path, char delimeter = ';')
        {
            List<string[]> lines = new List<string[]>();
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                Console.WriteLine("Loading : ");
                int teller = 0;
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    string[] splitted = s.Split(delimeter);
                    lines.Add(splitted);
                    teller++;
                    if (teller == 10000)
                    {
                        Console.Write("*");
                        teller = 0;
                    }
                }
            }
            Console.WriteLine("\nFile read");
            return lines;
        }
        private List<Team> GetTeamsFromFile(string path) 
        {
            List<string[]> file = FileReader(path, ',');
            string spelerNaam; int rugNummer; int waarde; //spelerinfo
            string teamNaam; int stamNummer; string trainer; string teamBijnaam;  //teaminfo
            List<Team> teams = new List<Team>();
            foreach (string[] fileLine in file.Skip(1))
            {
                spelerNaam = fileLine[0];
                rugNummer = int.Parse(fileLine[1]);
                teamNaam = fileLine[2];
                string temp = fileLine[3].Replace(" ", String.Empty);
                waarde = int.Parse(temp);
                stamNummer = int.Parse(fileLine[4]);
                trainer = fileLine[5];
                teamBijnaam = fileLine[6];
                Speler speler = new Speler(spelerNaam, rugNummer, waarde);
                Team team = new Team(stamNummer, teamNaam, teamBijnaam, trainer);
                int teamIndex = teams.IndexOf(team);
                if (teamIndex != -1)
                {
                    teams[teamIndex].spelers.Add(speler);
                }
                else
                {
                    team.spelers.Add(speler);
                    teams.Add(team);
                }
            }
            return teams;
        }
    }
}
