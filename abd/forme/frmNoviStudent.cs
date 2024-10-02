﻿using apk.baza;
using apk.klase;
using Student.Klase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Forms;
using Student.AO.Helpers;


namespace Student.AO.forme
{
    public partial class frmNoviStudent : Form
    {
        Baza db = new Baza();
        private Studenti studenti;
        public frmNoviStudent(Studenti studenti)
        {
            this.studenti = studenti;
            InitializeComponent();
        }

        private void btnSpasi_Click(object sender, EventArgs e)
        {
            if (ValidniPodaci())
            {
                if (studenti == null)
                {
                    var noviStudent = new Studenti
                    {
                        Ime = tbIme.Text,
                        Prezime = tbPrezime.Text,
                        BrojObroka = 44,
                        BrojKartice = (db.Studenti.Any() ? db.Studenti.Max(s => s.BrojKartice) + 1 : 001),
                        Datum = DateTime.Now,
                        Slika = Ekstenzije.ToByteArray(pbSlika.Image),
                    };
                    db.Studenti.Add(noviStudent);
                    db.SaveChanges();
                    Close();
                }
                else
                {
                    studenti.Ime = tbIme.Text;
                    studenti.Prezime = tbPrezime.Text;
                    studenti.Datum = dateTimePicker1.Value;
                    db.Studenti.Update(studenti);
                    db.SaveChanges();
                    Close();
                }
            }
        }

        private bool ValidniPodaci()
        {
            return Helpers.Validator.ProvjeriUnos(tbIme, err, "Obavezna vrijednosti") &&
                Helpers.Validator.ProvjeriUnos(tbPrezime, err, "Obavezna vrijednosti") &&
                Helpers.Validator.ProvjeriUnos(pbSlika, err, "Obavezna vrijednosti");
        }

        private void frmNoviStudent_Load(object sender, EventArgs e)
        {
            if (studenti != null)
            {
                tbIme.Text = studenti.Ime;
                tbPrezime.Text = studenti.Prezime;
                pbSlika.Image = Ekstenzije.ToImage(studenti.Slika);
                dateTimePicker1.Value = studenti.Datum;
            }
        }

        private void pbSlika_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbSlika.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void btnIzbrisi_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Da li ste sigurni da želite izbrisati studenta", "Potvrda", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                db.Studenti.Remove(studenti);
                db.SaveChanges();
                Close();
            }
            else
            {
                Close();
            }

        }
    }
}
