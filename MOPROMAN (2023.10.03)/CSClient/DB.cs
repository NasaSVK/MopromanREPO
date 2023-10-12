﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace nsAspur
{
    class DB
    {
        public static mopromanDBEntities Context;
        
        public static void TestujDB()
        {

            try
            {
                if (!pripojDB())
                {
                    Environment.Exit(0);
                    return;
                }
            }
            catch (ArgumentException e)
            {

                MessageBox.Show("Nepodarilo sa pripojiť k DB z nasledujúceho dôvodu!" + e.Message + "\n\nAplikácia bude skončená!", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
                return;
            }

        }

        public static bool pripojDB()
        {
            ///String IP = Properties.Settings.Default.DBServerIP;
            Context = new mopromanDBEntities();
            //String IP = Context.Database.Connection.DataSource.Substring(0, Context.Database.Connection.DataSource.IndexOf("\\"));
            String IP = Context.Database.Connection.DataSource.Substring(0, Context.Database.Connection.DataSource.IndexOf('\\'));
            //https://msdn.microsoft.com/en-us/library/7hzczzed.aspx
            if (new Ping().Send(IP).Status != IPStatus.Success)
            {
                MessageBox.Show("Zariadenie hostujúce DB server (" + IP + ") nie je dostupné!\nKontaktujte správcu!", "Chyba databázy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //https://stackoverflow.com/questions/19211082/testing-an-entity-framework-database-connection#
            if (!Context.Database.Exists())
            {
                MessageBox.Show("Databázový server na zariadení \"" + IP + "\" nie je dostupný.\nKontaktujte správcu2!", "Chyba databázy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //MessageBox.Show("Pripojenie OK"); 
            return true;
        }


        public static bool ZmazDB()
        {

            //Context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Svetla]");
            foreach (record rep in Context.records.ToList())
                Context.records.Remove(rep);
            return UlozDB();

        }

        public static void ZmazDBOpakovane()
        {
            while (true)
            {
                if (ZmazDB()) break;
                Thread.Sleep(2222);
            }
        }
       
        public static void ulozDoDB(record pReport)
        {
            Context.records.Add(pReport);
        }


        public static bool UlozDB()
        {
            try
            {
                Context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Zmeny sa nepodarilo uložiť do databázy z nasledujúceho dôvodu:\n\n" + ex.Message + "\n\n Vykonajte patričné kroky na odstránenie tejto chyby!!!"), "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        /*
        public static bool JeKodvDB(string pKOD)
        {

            return Context.svetla.Any(svl => svl.KOD == pKOD);

        }*/

        /*
        public static void generujDB(int pPocet)
        {

            for (int i = 0; i < pPocet; i++)

                Context.svetla.Add(new svetla() { KOD = Helpers.GenerujKod(), DATUM_CAS = DateTime.Now });

            UlozDB();

        }*/


    }
}
