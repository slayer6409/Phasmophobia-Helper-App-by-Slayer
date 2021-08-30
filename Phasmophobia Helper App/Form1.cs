using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;

namespace Phasmophobia_Helper_App
{
    public enum evidence
    {
        Orbs, EMF, Fingerprints, SpiritBox, Temps, Writing, DOTS
    }
    public enum antiEvidence
    {
        AntiOrbs, AntiEMF, AntiFingerprints, AntiSpiritBox, AntiTemps, AntiWriting, AntiDOTS
    }

    public partial class Form1 : Form
    {
        static int counter = 0;
        static TcpClient client = new TcpClient();
        static bool isRecieving = false;
        static bool needsSend = false;
        static bool hasRan = false;
        static NetworkStream stream;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        public bool sync = false;
        Dictionary<evidence, CheckBox> evidenceToCheck = new Dictionary<evidence, CheckBox>();
        Dictionary<antiEvidence, CheckBox> antiToCheck = new Dictionary<antiEvidence, CheckBox>();
        public bool Orbs = false, EMF = false, Fingerprints = false, SpiritBox = false, Temps = false, Writing = false, DOTS=false;
        public bool AntiOrbs = false, AntiEMF = false, AntiFingerprints = false, AntiSpiritBox = false, AntiTemps = false, AntiWriting = false, AntiDOTS=false;
        public string Ghost = "Unknown";
        public string Evidence1 = "";
        public string Evidence2 = "";
        public string Evidence3 = "";
        public int p1 = 100;
        public int p2 = 100;
        public int p3 = 100;
        public int p4 = 100;
        public int totalPlayers = 0;
        public int difficulty = 1;
        public int roundTimer = 0;
        public bool SoundSetting = false;
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        public int textR = 255, textG = 255, textB = 255, bgR = 64, bgG = 64, bgB = 64;
        public Label[] Ghosts = new Label[16];
        Maps mp;
        Settings set;
        int huntTimer = 0;
        System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"c:\Windows\Media\chimes.wav");
        public bool hunt = false;
        public GhostTypes[] ghostTypes =
        {
            new GhostTypes("Spirit",evidence.EMF,evidence.SpiritBox,evidence.Writing,"Smudge stick affects ghost longer."),
            new GhostTypes("Wraith", evidence.EMF, evidence.SpiritBox, evidence.DOTS,"Almost never touches the ground.  Use salt.  It can see through doors."),
            new GhostTypes("Phantom", evidence.Fingerprints, evidence.SpiritBox, evidence.DOTS, "Drops sanity if looked at, take a photo to make it go away."),
            new GhostTypes("Poltergeist", evidence.Fingerprints, evidence.SpiritBox, evidence.Writing,"Can throw multiple objects.  Stay in empty rooms."),
            new GhostTypes("Banshee",evidence.Fingerprints,evidence.Orbs,evidence.DOTS,"Will target one player until they die.  Crucifix range increased from 3m to 5m."),
            new GhostTypes("Jinn",evidence.EMF,evidence.Fingerprints,evidence.Temps,"Power hungry, kill the breaker.  \n With power on, it moves faster, until it gets close to you."),
            new GhostTypes("Mare",evidence.SpiritBox,evidence.Orbs,evidence.Writing,"If the light is on in it's room the hunt sanity is 40, but if it is off, it is 60."),
            new GhostTypes("Revenant",evidence.Orbs,evidence.Temps,evidence.Writing,"Travels fast when hunting, hiding will slow it down"),
            new GhostTypes("Shade",evidence.EMF,evidence.Temps,evidence.Writing,"Shy guy, will be harder to find if in groups.\nWill not hunt if multiple people are around."),
            new GhostTypes("Demon",evidence.Fingerprints,evidence.Temps,evidence.Writing,"Will hunt at 75 sanity instead of 50, be careful. Ask Ouija board if demon (won't lower sanity)"),
            new GhostTypes("Yurei",evidence.Orbs,evidence.Temps,evidence.DOTS,"Strong effect on sanity.  Smudge to prevent from wandering."),
            new GhostTypes("Oni",evidence.EMF,evidence.Temps,evidence.DOTS,"More active when players are around / doing things nearby."),
            new GhostTypes("Hantu",evidence.Fingerprints,evidence.Orbs,evidence.Temps,"It hates cold temperatures, and will move faster in cold temps.\nIt moves slower in warmer areas."),
            new GhostTypes("Yokai",evidence.Orbs,evidence.SpiritBox,evidence.DOTS,"It can only hear when you when it is close to you.\nTalking makes it angry."),
            new GhostTypes("Goryo",evidence.EMF,evidence.Fingerprints,evidence.DOTS,"It doesn't wander very far, and getting DOTS evidence can only be seen via camera."),
            new GhostTypes("Myling",evidence.EMF,evidence.Fingerprints,evidence.Writing,"It makes a lot of noise, except when it is hunting.")
        };

        public evidence inverse(antiEvidence anti)
        {
            if (anti == antiEvidence.AntiDOTS) return evidence.DOTS;
            if (anti == antiEvidence.AntiEMF) return evidence.EMF;
            if (anti == antiEvidence.AntiFingerprints) return evidence.Fingerprints;
            if (anti == antiEvidence.AntiOrbs) return evidence.Orbs;
            if (anti == antiEvidence.AntiSpiritBox) return evidence.SpiritBox;
            if (anti == antiEvidence.AntiTemps) return evidence.Temps;
            if (anti == antiEvidence.AntiWriting) return evidence.Writing;
            return 0;
        }
        public antiEvidence inverse(evidence ev)
        {
            if (ev == evidence.DOTS) return antiEvidence.AntiDOTS;
            if (ev == evidence.EMF) return antiEvidence.AntiEMF;
            if (ev == evidence.Fingerprints) return antiEvidence.AntiFingerprints;
            if (ev == evidence.Orbs) return antiEvidence.AntiOrbs;
            if (ev == evidence.SpiritBox) return antiEvidence.AntiSpiritBox;
            if (ev == evidence.Temps) return antiEvidence.AntiTemps;
            if (ev == evidence.Writing) return antiEvidence.AntiWriting;
            return 0;
        }

        public Form1()
        {
            InitializeComponent();
            setColors();
            mp = new Maps();
            set = new Settings(this);
            Ghosts[0] = label12;
            Ghosts[1] = label13;
            Ghosts[2] = label14;
            Ghosts[3] = label15;
            Ghosts[4] = label16;
            Ghosts[5] = label17;
            Ghosts[6] = label18;
            Ghosts[7] = label19;
            Ghosts[8] = label20;
            Ghosts[9] = label21;
            Ghosts[10] = label22;
            Ghosts[11] = label23;
            Ghosts[12] = label2;
            Ghosts[13] = label6;
            Ghosts[14] = label8;
            Ghosts[15] = label9;
            evidenceToCheck.Add(evidence.DOTS, checkBox3);
            evidenceToCheck.Add(evidence.EMF, checkBox10);
            evidenceToCheck.Add(evidence.Fingerprints, checkBox6);
            evidenceToCheck.Add(evidence.Orbs, checkBox1);
            evidenceToCheck.Add(evidence.SpiritBox, checkBox4);
            evidenceToCheck.Add(evidence.Temps, checkBox13);
            evidenceToCheck.Add(evidence.Writing, checkBox14);
            antiToCheck.Add(antiEvidence.AntiDOTS, checkBox2);
            antiToCheck.Add(antiEvidence.AntiEMF, checkBox11);
            antiToCheck.Add(antiEvidence.AntiFingerprints, checkBox9);
            antiToCheck.Add(antiEvidence.AntiOrbs, checkBox7);
            antiToCheck.Add(antiEvidence.AntiSpiritBox, checkBox8);
            antiToCheck.Add(antiEvidence.AntiTemps, checkBox12);
            antiToCheck.Add(antiEvidence.AntiWriting, checkBox15);
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }


        #region PlayerStuff

        //p1
        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            //comboBox1.Enabled = true;
            //if (checkBox19.Checked)
            //{
            //    totalPlayers = 1;
            //    comboBox2.Enabled = false;
            //    comboBox3.Enabled = false;
            //    comboBox4.Enabled = false;
            //    comboBox2.Visible = false;
            //    comboBox3.Visible = false;
            //    comboBox4.Visible = false;
            //    checkBox20.Visible = false;
            //    checkBox21.Visible = false;
            //    checkBox22.Visible = false;
            //}
            //else
            //{
            //    totalPlayers = 0;
            //    comboBox2.Enabled = false;
            //    comboBox3.Enabled = false;
            //    comboBox4.Enabled = false;
            //    comboBox2.Visible = true;
            //    comboBox3.Visible = true;
            //    comboBox4.Visible = true;
            //    checkBox20.Visible = true;
            //    checkBox21.Visible = true;
            //    checkBox22.Visible = true;
            //}
        }

        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (comboBox1.ToString() != "Player 1 Sanity")
        //        {
        //            p1 = Int32.Parse(comboBox1.SelectedItem.ToString());
        //        }
        //        checkSanity();
        //    }
        //    catch (Exception error)
        //    {
        //        Console.WriteLine("Error:", error);
        //    }

        //}

        //p2
        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            //comboBox1.Enabled = true;
            //if (checkBox20.Checked)
            //{
            //    totalPlayers = 2;
            //    comboBox2.Enabled = true;
            //    comboBox3.Visible = false;
            //    comboBox4.Visible = false;
            //    checkBox19.Visible = false;
            //    checkBox21.Visible = false;
            //    checkBox22.Visible = false;
            //}
            //else
            //{
            //    totalPlayers = 0;
            //    comboBox2.Enabled = false;
            //    comboBox3.Enabled = false;
            //    comboBox4.Enabled = false;
            //    comboBox2.Visible = true;
            //    comboBox3.Visible = true;
            //    comboBox4.Visible = true;
            //    checkBox19.Visible = true;
            //    checkBox21.Visible = true;
            //    checkBox22.Visible = true;
            //}
        }

        //private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (comboBox2.ToString() != "Player 2 Sanity")
        //        {
        //            p2 = Int32.Parse(comboBox2.SelectedItem.ToString());
        //        }
        //        checkSanity();
        //    }
        //    catch
        //    {

        //    }
        //}

        //p3
        //private void checkBox21_CheckedChanged(object sender, EventArgs e)
        //{
        //    comboBox1.Enabled = true;
        //    if (checkBox21.Checked)
        //    {
        //        totalPlayers = 3;
        //        comboBox2.Enabled = true;
        //        comboBox3.Enabled = true;
        //        comboBox4.Visible = false;
        //        checkBox19.Visible = false;
        //        checkBox20.Visible = false;
        //        checkBox22.Visible = false;
        //    }
        //    else
        //    {
        //        totalPlayers = 0;
        //        comboBox2.Enabled = false;
        //        comboBox3.Enabled = false;
        //        comboBox4.Enabled = false;
        //        comboBox2.Visible = true;
        //        comboBox3.Visible = true;
        //        comboBox4.Visible = true;
        //        checkBox19.Visible = true;
        //        checkBox20.Visible = true;
        //        checkBox22.Visible = true;
        //    }
        //}

        //private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (comboBox3.ToString() != "Player 3 Sanity")
        //        {
        //            p3 = Int32.Parse(comboBox3.SelectedItem.ToString());
        //        }
        //        checkSanity();
        //    }
        //    catch
        //    {

        //    }
        //}

        ////p4
        //private void checkBox22_CheckedChanged(object sender, EventArgs e)
        //{
        //    comboBox1.Enabled = true;
        //    if (checkBox22.Checked)
        //    {
        //        totalPlayers = 4;
        //        comboBox2.Enabled = true;
        //        comboBox3.Enabled = true;
        //        comboBox4.Enabled = true;
        //        comboBox2.Visible = true;
        //        comboBox3.Visible = true;
        //        comboBox4.Visible = true;
        //        checkBox19.Visible = false;
        //        checkBox21.Visible = false;
        //        checkBox20.Visible = false;
        //    }
        //    else
        //    {
        //        totalPlayers = 0;
        //        comboBox2.Enabled = false;
        //        comboBox3.Enabled = false;
        //        comboBox4.Enabled = false;
        //        comboBox2.Visible = true;
        //        comboBox3.Visible = true;
        //        comboBox4.Visible = true;
        //        checkBox19.Visible = true;
        //        checkBox21.Visible = true;
        //        checkBox20.Visible = true;
        //    }

        //}

        //private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (comboBox4.ToString() != "Player 4 Sanity")
        //        {
        //            p4 = Int32.Parse(comboBox4.SelectedItem.ToString());
        //        }
        //        checkSanity();
        //    }
        //    catch
        //    {

        //    }
        //}

        #endregion PlayerStuff

        #region Evidence

        //Ghost Orb
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState != CheckState.Indeterminate)
            {
                if (checkBox1.Checked)
                {
                    checkBox7.CheckState = CheckState.Indeterminate;
                    checkBox7.Enabled = false;
                    Orbs = true;
                }
                else
                {

                    checkBox7.CheckState = CheckState.Unchecked;
                    checkBox7.Enabled = true;
                    Orbs = false;
                }
            }

            checkGhost();

        }
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.CheckState != CheckState.Indeterminate)
            {
                if (checkBox7.Checked)
                {
                    checkBox1.CheckState = CheckState.Indeterminate;
                    checkBox1.Enabled = false;
                    AntiOrbs = true;
                }
                else
                {

                    checkBox1.CheckState = CheckState.Unchecked;
                    checkBox1.Enabled = true;
                    AntiOrbs = false;
                }
            }

            checkGhost();
        }
        
        //Spirit Box
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.CheckState != CheckState.Indeterminate)
            {
                if (checkBox4.Checked)
                {


                    checkBox8.CheckState = CheckState.Indeterminate;
                    checkBox8.Enabled = false;
                    SpiritBox = true;
                }
                else
                {

                    checkBox8.CheckState = CheckState.Unchecked;
                    checkBox8.Enabled = true;
                    SpiritBox = false;
                }
            }
            
            checkGhost();
        }
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.CheckState != CheckState.Indeterminate)
            {
                if (checkBox8.Checked)
                {
                    checkBox4.CheckState = CheckState.Indeterminate;
                    checkBox4.Enabled = false;
                    AntiSpiritBox = true;
                }
                else
                {

                    checkBox4.CheckState = CheckState.Unchecked;
                    checkBox4.Enabled = true;
                    AntiSpiritBox = false;
                }
            }

            checkGhost();
        }

        //Fingerprints
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.CheckState != CheckState.Indeterminate)
            {
                if (checkBox6.Checked)
                {

                    checkBox9.CheckState = CheckState.Indeterminate;
                    checkBox9.Enabled = false;
                    Fingerprints = true;
                }
                else
                {

                    checkBox9.CheckState = CheckState.Unchecked;
                    checkBox9.Enabled = true;
                    Fingerprints = false;

                }
            }
            checkGhost();
        }
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.CheckState != CheckState.Indeterminate)
            {
                if (checkBox9.Checked)
                {
                    checkBox6.CheckState = CheckState.Indeterminate;
                    checkBox6.Enabled = false;
                    AntiFingerprints = true;
                }
                else
                {

                    checkBox6.CheckState = CheckState.Unchecked;
                    checkBox6.Enabled = true;
                    AntiFingerprints = false;
                }
            }

            checkGhost();
        }

        //EMF
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.CheckState != CheckState.Indeterminate)
            {
                if (checkBox10.Checked)
                {

                    checkBox11.CheckState = CheckState.Indeterminate;
                    checkBox11.Enabled = false;
                    EMF = true;
                }
                else
                {

                    checkBox11.CheckState = CheckState.Unchecked;
                    checkBox11.Enabled = true;
                    EMF = false;

                }
            }
            checkGhost();
        }
        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox11.CheckState != CheckState.Indeterminate)
            {
                if (checkBox11.Checked)
                {
                    checkBox10.CheckState = CheckState.Indeterminate;
                    checkBox10.Enabled = false;
                    AntiEMF = true;
                }
                else
                {

                    checkBox10.CheckState = CheckState.Unchecked;
                    checkBox10.Enabled = true;
                    AntiEMF = false;
                }
            }

            checkGhost();
        }

        //Freezing Temperatures
        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox13.CheckState != CheckState.Indeterminate)
            {
                if (checkBox13.Checked)
                {

                    checkBox12.CheckState = CheckState.Indeterminate;
                    checkBox12.Enabled = false;
                    Temps = true;
                }
                else
                {

                    checkBox12.CheckState = CheckState.Unchecked;
                    checkBox12.Enabled = true;
                    Temps = false;

                }
            }
            checkGhost();
        }
        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox12.CheckState != CheckState.Indeterminate)
            {
                if (checkBox12.Checked)
                {
                    checkBox13.CheckState = CheckState.Indeterminate;
                    checkBox13.Enabled = false;
                    AntiTemps = true;
                }
                else
                {

                    checkBox13.CheckState = CheckState.Unchecked;
                    checkBox13.Enabled = true;
                    AntiTemps = false;
                }
            }

            checkGhost();
        }

        //Ghost Writing
        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox14.CheckState != CheckState.Indeterminate)
            {
                if (checkBox14.Checked)
                {
                    checkBox15.CheckState = CheckState.Indeterminate;
                    checkBox15.Enabled = false;
                    Writing = true;
                }
                else
                {
                    checkBox15.CheckState = CheckState.Unchecked;
                    checkBox15.Enabled = true;
                    Writing = false;

                }
            }
            checkGhost();
        }
        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox15.CheckState != CheckState.Indeterminate)
            {
                if (checkBox15.Checked)
                {
                    checkBox14.CheckState = CheckState.Indeterminate;
                    checkBox14.Enabled = false;
                    AntiWriting = true;
                }
                else
                {

                    checkBox14.CheckState = CheckState.Unchecked;
                    checkBox14.Enabled = true;
                    AntiWriting = false;
                }
            }

            checkGhost();
        }

        //DOTS
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.CheckState != CheckState.Indeterminate)
            {
                if (checkBox2.Checked)
                {
                    checkBox3.CheckState = CheckState.Indeterminate;
                    checkBox3.Enabled = false;
                    AntiDOTS = true;
                }
                else
                {
                    checkBox3.CheckState = CheckState.Unchecked;
                    checkBox3.Enabled = true;
                    AntiDOTS = false;

                }
            }
            checkGhost();
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox3.CheckState != CheckState.Indeterminate)
            {
                if (checkBox3.Checked)
                {
                    checkBox2.CheckState = CheckState.Indeterminate;
                    checkBox2.Enabled = false;
                    DOTS = true;
                }
                else
                {

                    checkBox2.CheckState = CheckState.Unchecked;
                    checkBox2.Enabled = true;
                    DOTS = false;
                }
            }

            checkGhost();
        }

        #endregion Evidence

        #region Maps


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                mp.Show();
                mp.currentMap = 1;
            }
            catch (System.ObjectDisposedException exc)
            {
                mp = new Maps();
                mp.Show();
                mp.currentMap = 1;
            }
            mp.updateMap();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                mp.Show();
                mp.currentMap = 2;
            }
            catch (System.ObjectDisposedException exc)
            {
                mp = new Maps();
                mp.Show();
                mp.currentMap = 2;
            }
            mp.updateMap();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                mp.Show();
                mp.currentMap = 3;
            }
            catch (System.ObjectDisposedException exc)
            {
                mp = new Maps();
                mp.Show();
                mp.currentMap = 3;
            }
            mp.updateMap();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                mp.Show();
                mp.currentMap = 4;
            }
            catch (System.ObjectDisposedException exc)
            {
                mp = new Maps();
                mp.Show();
                mp.currentMap = 4;
            }
            mp.updateMap();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                mp.Show();
                mp.currentMap = 5;
            }
            catch (System.ObjectDisposedException exc)
            {
                mp = new Maps();
                mp.Show();
                mp.currentMap = 5;
            }
            mp.updateMap();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                mp.Show();
                mp.currentMap = 6;
            }
            catch (System.ObjectDisposedException exc)
            {
                mp = new Maps();
                mp.Show();
                mp.currentMap = 6;
            }
            mp.updateMap();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                mp.Show();
                mp.currentMap = 7;
            }
            catch (System.ObjectDisposedException exc)
            {
                mp = new Maps();
                mp.Show();
                mp.currentMap = 7;
            }
            mp.updateMap();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                mp.Show();
                mp.currentMap = 8;
            }
            catch (System.ObjectDisposedException exc)
            {
                mp = new Maps();
                mp.Show();
                mp.currentMap = 8;
            }
            mp.updateMap();
        }

        #endregion Maps

        public void checkGhost()
        {
            List<evidence> currentEvidence = new List<evidence>();
            List<antiEvidence> currentAnti = new List<antiEvidence>();
            if (currentEvidence.Count != 0) currentEvidence.Clear();
            if (currentAnti.Count != 0) currentAnti.Clear();
            //public bool AntiOrbs = false, AntiEMF = false, AntiFingerprints = false, AntiSpiritBox = false, AntiTemps = false, AntiWriting = false, AntiDOTS = false;
            if (Orbs) currentEvidence.Add(evidence.Orbs);
            if (EMF) currentEvidence.Add(evidence.EMF);
            if (Fingerprints) currentEvidence.Add(evidence.Fingerprints);
            if (SpiritBox) currentEvidence.Add(evidence.SpiritBox);
            if (Temps) currentEvidence.Add(evidence.Temps);
            if (Writing) currentEvidence.Add(evidence.Writing);
            if (DOTS) currentEvidence.Add(evidence.DOTS);
            if (AntiOrbs) currentAnti.Add(antiEvidence.AntiOrbs);
            if (AntiEMF) currentAnti.Add(antiEvidence.AntiEMF);
            if (AntiFingerprints) currentAnti.Add(antiEvidence.AntiFingerprints);
            if (AntiSpiritBox) currentAnti.Add(antiEvidence.AntiSpiritBox);
            if (AntiTemps) currentAnti.Add(antiEvidence.AntiTemps);
            if (AntiWriting) currentAnti.Add(antiEvidence.AntiWriting);
            if (AntiDOTS) currentAnti.Add(antiEvidence.AntiDOTS);
            List<Label> toKeep = new List<Label>();
            List<GhostTypes> ghostKeep = new List<GhostTypes>();
            List<antiEvidence> evdToKeep = new List<antiEvidence>();
            foreach (Label g in Ghosts) { g.Visible = true; } 
            foreach (antiEvidence anti in antiToCheck.Keys) { antiToCheck[anti].Visible = true; }
            foreach (evidence evidence in evidenceToCheck.Keys) { evidenceToCheck[evidence].Visible = true; }
            foreach (GhostTypes ghost in ghostTypes)
            {
                if (ghost.checkEvidence(currentAnti, currentEvidence))
                {
                    foreach(Label g in Ghosts)
                    {
                        if (g.Text == ghost.Name) { toKeep.Add(g); ghostKeep.Add(ghost); }
                    }
                }
            }
            foreach (Label g in Ghosts)
            {
                if (!toKeep.Contains(g)) g.Visible = false;
                else g.Visible = true;
                if (toKeep.Count == 1) Ghost = toKeep[0].Text; else Ghost = "Unknown";
            }
            foreach (GhostTypes g in ghostKeep)
            {
                var allEvidence = g.GetEvidence();
                foreach(evidence e in allEvidence)
                {
                    if(!evdToKeep.Contains(inverse(e)))evdToKeep.Add(inverse(e));
                }
            }
            foreach (antiEvidence anti in antiToCheck.Keys)
            {
                if (!evdToKeep.Contains(anti)) if(antiToCheck[anti].Checked!=true)antiToCheck[anti].Visible = false; else antiToCheck[anti].Visible = true;
            }
            foreach (evidence evidence in evidenceToCheck.Keys)
            {
                if (antiToCheck[inverse(evidence)].Visible) evidenceToCheck[evidence].Visible = true; else if (!evidenceToCheck[evidence].Checked) evidenceToCheck[evidence].Visible = false;
            }
            
            label3.Text = "Current Ghost: " + Ghost;
            checkSanity();
            UpdateText();
        }

        private void UpdateText()
        {
            if (Ghost == "Unknown")
            {
                label7.Visible = false;
            }
            else
            {
                label7.Visible = true;
                foreach(GhostTypes ghost in ghostTypes)
                {
                    if (ghost.Name == Ghost) label7.Text = ghost.Description;
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //comboBox1.SelectedIndex = 10;
            //comboBox2.SelectedIndex = 10;
            //comboBox3.SelectedIndex = 10;
            //comboBox4.SelectedIndex = 10;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            //checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox10.Checked = false;
            checkBox14.Checked = false;
            checkBox13.Checked = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                mp.Show();
                mp.currentMap = 9;
            }
            catch (System.ObjectDisposedException exc)
            {
                mp = new Maps();
                mp.Show();
                mp.currentMap = 9;
            }
            mp.updateMap();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            
            //if (hunt)
            //{
            //    if (huntTimer < 10)
            //    {
            //        label2.Text = "0:0"+huntTimer.ToString();
            //    }
            //    else
            //    {
            //        label2.Text = "0:"+huntTimer.ToString();
            //    }
                
            //    if (huntTimer == 0)
            //    {
            //        if (SoundSetting) player.Play();
            //        button10.Text = "Hunt";
            //        button11.Text = "Round Start";
            //        hunt = false;
            //        timer1.Enabled = false;
            //    }
            //    huntTimer--;

            //}
            //else
            //{
            //    double temp = 0;
            //    double temp2 = 0;
            //    roundTimer--;
            //    temp = roundTimer/60;
            //    temp2 = (Math.Floor(temp) * 60) - roundTimer;
            //    temp2 = Math.Abs(temp2);
            //    if (temp2 < 10)
            //    {
            //        label2.Text = temp + ":0" + temp2;
            //    }
            //    else
            //    {
            //        label2.Text = temp + ":" + temp2;
            //    }
            //    if (roundTimer == 0)
            //    {
            //        if (SoundSetting) player.Play();
            //        button10.Text = "Hunt";
            //        button11.Text = "Round Start";
            //        timer1.Enabled = false;
            //    }
            //}

        }
        
        //private void checkBox2_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBox2.Checked == true)
        //    {
        //        difficulty = 1;
        //        checkBox5.Visible = false;
        //        checkBox3.Visible = false;
        //    }
        //    else
        //    {
        //        checkBox5.Visible = true;
        //        checkBox3.Visible = true;
        //    }
        //    if (timer1.Enabled == false)
        //    {
        //        label2.Text="5:00";
        //    }
        //}
        
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox5.Checked == true)
            //{
            //    difficulty = 3;
            //    checkBox2.Visible = false;
            //    checkBox3.Visible = false;
            //    if (timer1.Enabled == false)
            //    {
            //        label2.Text = "0:00";
            //    }
            //}
            //else
            //{
            //    if (timer1.Enabled == false)
            //    {
            //        label2.Text = "5:00";
            //    }
            //    difficulty = 1;
            //    checkBox2.Visible = true;
            //    checkBox3.Visible = true;
            //}
            
        }

        //private void checkBox3_CheckedChanged(object sender, EventArgs e)
        //{
        //    //if (checkBox3.Checked == true)
        //    //{
        //    //    difficulty = 2;
        //    //    checkBox2.Visible = false;
        //    //    checkBox5.Visible = false;
        //    //    if (timer1.Enabled == false)
        //    //    {
        //    //        label2.Text = "2:00";
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    difficulty = 1;
        //    //    checkBox2.Visible = true;
        //    //    checkBox5.Visible = true;
        //    //    if (timer1.Enabled == false)
        //    //    {
        //    //        label2.Text = "5:00";
        //    //    }
        //    //}
           
        //}

        //private void button10_Click(object sender, EventArgs e)
        //{
        //    if (timer1.Enabled == true)
        //    {
        //        timer1.Enabled = false;
        //        hunt = false;
        //        button10.Text = "Hunt";
        //        button11.Text = "Round Start";

        //    }
        //    else
        //    {
        //        hunt = true;
        //        button10.Text = "Stop";
        //        button11.Text = "Stop";
        //        switch (difficulty)
        //        {
        //            case 1:
        //                huntTimer = 24;
        //                label2.Text = "0:25";
        //                break;
        //            case 2:
        //                huntTimer = 34;
        //                label2.Text = "0:40";
        //                break;
        //            case 3:
        //                huntTimer = 49;
        //                label2.Text = "0:55";
        //                break;
        //            default:
        //                huntTimer = 24;
        //                label2.Text = "0:25";
        //                break;
        //        }
        //        timer1.Enabled = true;
        //    }
        //}

        //private void button11_Click(object sender, EventArgs e)
        //{
        //    if (timer1.Enabled != true)
        //    {
        //        button10.Text = "Stop";
        //        button11.Text = "Stop";
        //        switch (difficulty)
        //        {
        //            case 1:
        //                roundTimer = 300;
        //                hunt = false;
        //                timer1.Enabled = true;
        //                label2.Text = "5:00";
        //                break;
        //            case 2:
        //                roundTimer = 120;
        //                hunt = false;
        //                timer1.Enabled = true;
        //                label2.Text = "2:00";
        //                break;
        //            case 3:
        //                roundTimer = 0;
        //                button10.Text = "Hunt";
        //                button11.Text = "Round Start";
        //                break;
        //            default:
        //                roundTimer = 300;
        //                hunt = false;
        //                timer1.Enabled = true;
        //                label2.Text = "5:00";
        //                break;
        //        }
        //    }
        //    else
        //    {

        //        timer1.Enabled = false;
        //        hunt = false;
        //        button10.Text = "Hunt";
        //        button11.Text = "Round Start";
                
        //    }
            
        //}


        public void checkSanity()
        {
            int modifier=0;
            int pAvg = 100;
            int huntCalc = 0;
            double HuntChance = 0;
            if (Ghost == "Demon") modifier = 15;
            if (Ghost == "Banshee") modifier = 77;
            if (Ghost == "Mare") modifier = 10;
            if (totalPlayers == 0 || totalPlayers == 1)
            {
                pAvg = p1;
            }
            else if(totalPlayers==2){
                pAvg = (p1 + p2) / 2;
            }
            else if (totalPlayers == 3)
            {
                pAvg = (p1 + p2 + p3) / 3;
            }else if (totalPlayers == 4)
            {
                pAvg = (p1 + p2 + p3 + p4) / 4;
            }
            else
            {
                pAvg = 100;
            }

            huntCalc = 100 - pAvg + modifier;

            if (huntCalc < 50) HuntChance = 0;
            else if (huntCalc >= 50 && huntCalc < 75) HuntChance = 10;
            else if (huntCalc >= 75) HuntChance = 16.50;

            if (modifier == 77)
            {
                //label10.Text = "Hunt Chance: ~Unknown";
            }
            else
            {
                //label10.Text = "Hunt Chance: ~"+HuntChance.ToString();
                if (label22.Visible == true)
                {
                    //demon
                    double HuntChance2=0;
                    var huntCalc2 = 100 - pAvg + 15;
                    if (huntCalc2 < 50) HuntChance2 = 0;
                    else if (huntCalc2 >= 50 && huntCalc2 < 75) HuntChance2 = 10;
                    else if (huntCalc2 >= 75) HuntChance2 = 16.50;
                    //label9.Text = "Possible Hunt: ~" + HuntChance2.ToString();
                }
                else if (label15.Visible == true)
                {
                    //mare
                    double HuntChance2 = 0;
                    var huntCalc2 = 100 - pAvg + 10;
                    if (huntCalc2 < 50) HuntChance2 = 0;
                    else if (huntCalc2 >= 50 && huntCalc2 < 75) HuntChance2 = 10;
                    else if (huntCalc2 >= 75) HuntChance2 = 16.50;
                    //label9.Text = "Possible Hunt: ~" + HuntChance2.ToString();
                }
                else
                {
                    //label9.Text = "Possible Hunt: ~0";
                }
            }
            if (!isRecieving) needsSend=true ; else isRecieving = false;

        }

        public void setColors()
        {
            Color bgColor = Color.FromArgb(bgR, bgG, bgB);
            Color textColor = Color.FromArgb(textR, textG, textB);
            BackColor = bgColor;
            foreach(Control x in this.Controls)
            {
                if(!(x is Button)&&!(x is ComboBox)&&!(x is TextBox))
                {
                    x.ForeColor = textColor;
                }
            }
        }

        public void showHideTimer(bool show)
        {
            //if (show)
            //{
            //    label2.Visible = true;
            //    button10.Visible = true;
            //    button11.Visible = true;
            //}
            //else
            //{
            //    label2.Visible = false;
            //    button10.Visible = false;
            //    button11.Visible = false;
            //}
        }

        public void KeepOnTop(bool keep)
        {
            if (!keep)
            {
                SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            }
            else
            {
                SetWindowPos(this.Handle, HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                set.Show();
            }
            catch
            {
                set = new Settings(this);
                set.Show();
            }
            
        }

        #region useless


        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }


        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }


        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }


        #endregion useless


    }

    public class GhostTypes
    {
        public string Name;
        public string Description;
        public evidence evidence1;
        public evidence evidence2;
        public evidence evidence3;
        public antiEvidence anti1;
        public antiEvidence anti2;
        public antiEvidence anti3;
        public antiEvidence anti4;

        public GhostTypes(string name,evidence evidence1,evidence evidence2,evidence evidence3,string desc)
        {
            Name = name;
            Description = desc;
            this.evidence1 = evidence1;
            this.evidence2 = evidence2;
            this.evidence3 = evidence3;
            var allEvidence = GetEvidence();
            List<antiEvidence> anti = new List<antiEvidence>();
            anti.Add(antiEvidence.AntiDOTS);
            anti.Add(antiEvidence.AntiEMF);
            anti.Add(antiEvidence.AntiFingerprints);
            anti.Add(antiEvidence.AntiOrbs);
            anti.Add(antiEvidence.AntiSpiritBox);
            anti.Add(antiEvidence.AntiTemps);
            anti.Add(antiEvidence.AntiWriting);
            foreach(evidence e in allEvidence)
            {
                if(e == evidence.DOTS)
                {
                    anti.Remove(antiEvidence.AntiDOTS);
                }
                else if(e == evidence.EMF)
                {
                    anti.Remove(antiEvidence.AntiEMF);
                }
                else if(e == evidence.Fingerprints)
                {
                    anti.Remove(antiEvidence.AntiFingerprints);
                }
                else if(e == evidence.SpiritBox)
                {
                    anti.Remove(antiEvidence.AntiSpiritBox);
                }
                else if(e == evidence.Temps)
                {
                    anti.Remove(antiEvidence.AntiTemps);
                }
                else if(e == evidence.Writing)
                {
                    anti.Remove(antiEvidence.AntiWriting);
                }
                else if(e == evidence.Orbs)
                {
                    anti.Remove(antiEvidence.AntiOrbs);
                }
            }
            anti1 = anti[0];
            anti2 = anti[1];
            anti3 = anti[2];
            anti4 = anti[3];
        }
        

        public Array GetEvidence()
        {
            evidence[] e = { evidence1, evidence2, evidence3};
            return e;
        }
        public Array GetAnti()
        {
            antiEvidence[] e = {anti1, anti2, anti3, anti4};
            return e;
        }
        public bool checkEvidence(evidence ev)
        {
            var allEvidence = GetEvidence();
            foreach(evidence e in allEvidence)
            {
                if (e == ev) return true;
            }
            return false;
        }
        public bool checkEvidence(antiEvidence anti)
        {
            var allEvidence = GetAnti();
            foreach(antiEvidence e in allEvidence)
            {
                if (e == anti) return true;
            }
            return false;
        }
        public bool checkEvidence(List<antiEvidence> antiList, List<evidence> evidences)
        {
            
            var allEvidence = GetEvidence().Cast<evidence>().ToList();
            var allAnti = GetAnti().Cast<antiEvidence>().ToList();
            foreach (antiEvidence anti in antiList)
            {
                if (!allAnti.Contains(anti)) return false;
            }
            foreach(evidence ev in evidences)
            {
                if (!allEvidence.Contains(ev)) return false;
            }
            return true;
        }
    }
}