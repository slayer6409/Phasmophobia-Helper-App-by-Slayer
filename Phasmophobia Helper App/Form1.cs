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
using System.Text;

namespace Phasmophobia_Helper_App
{

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
        public bool Orbs = false, EMF = false, Fingerprints = false, SpiritBox = false, Temps = false, Writing = false;
        public bool AntiOrbs = false, AntiEMF = false, AntiFingerprints = false, AntiSpiritBox = false, AntiTemps = false, AntiWriting = false;
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
        public Label[] Ghosts = new Label[12];
        Maps mp;
        Settings set;
        int huntTimer = 0;
        System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"c:\Windows\Media\chimes.wav");
        public bool hunt = false;


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

        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }


        #region PlayerStuff

        //p1
        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
            if (checkBox19.Checked)
            {
                totalPlayers = 1;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox2.Visible = false;
                comboBox3.Visible = false;
                comboBox4.Visible = false;
                checkBox20.Visible = false;
                checkBox21.Visible = false;
                checkBox22.Visible = false;
            }
            else
            {
                totalPlayers = 0;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox2.Visible = true;
                comboBox3.Visible = true;
                comboBox4.Visible = true;
                checkBox20.Visible = true;
                checkBox21.Visible = true;
                checkBox22.Visible = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.ToString() != "Player 1 Sanity")
                {
                    p1 = Int32.Parse(comboBox1.SelectedItem.ToString());
                }
                checkSanity();
            }
            catch (Exception error)
            {
                Console.WriteLine("Error:", error);
            }

        }

        //p2
        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
            if (checkBox20.Checked)
            {
                totalPlayers = 2;
                comboBox2.Enabled = true;
                comboBox3.Visible = false;
                comboBox4.Visible = false;
                checkBox19.Visible = false;
                checkBox21.Visible = false;
                checkBox22.Visible = false;
            }
            else
            {
                totalPlayers = 0;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox2.Visible = true;
                comboBox3.Visible = true;
                comboBox4.Visible = true;
                checkBox19.Visible = true;
                checkBox21.Visible = true;
                checkBox22.Visible = true;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox2.ToString() != "Player 2 Sanity")
                {
                    p2 = Int32.Parse(comboBox2.SelectedItem.ToString());
                }
                checkSanity();
            }
            catch
            {

            }
        }

        //p3
        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
            if (checkBox21.Checked)
            {
                totalPlayers = 3;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Visible = false;
                checkBox19.Visible = false;
                checkBox20.Visible = false;
                checkBox22.Visible = false;
            }
            else
            {
                totalPlayers = 0;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox2.Visible = true;
                comboBox3.Visible = true;
                comboBox4.Visible = true;
                checkBox19.Visible = true;
                checkBox20.Visible = true;
                checkBox22.Visible = true;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox3.ToString() != "Player 3 Sanity")
                {
                    p3 = Int32.Parse(comboBox3.SelectedItem.ToString());
                }
                checkSanity();
            }
            catch
            {

            }
        }

        //p4
        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
            if (checkBox22.Checked)
            {
                totalPlayers = 4;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comboBox2.Visible = true;
                comboBox3.Visible = true;
                comboBox4.Visible = true;
                checkBox19.Visible = false;
                checkBox21.Visible = false;
                checkBox20.Visible = false;
            }
            else
            {
                totalPlayers = 0;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox2.Visible = true;
                comboBox3.Visible = true;
                comboBox4.Visible = true;
                checkBox19.Visible = true;
                checkBox21.Visible = true;
                checkBox20.Visible = true;
            }

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox4.ToString() != "Player 4 Sanity")
                {
                    p4 = Int32.Parse(comboBox4.SelectedItem.ToString());
                }
                checkSanity();
            }
            catch
            {

            }
        }

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
            //demon
            if (EMF||Fingerprints||Orbs||AntiTemps||AntiSpiritBox||AntiWriting)
                label22.Visible = false;
            else label22.Visible = true;

            //spirit
            if (EMF || Temps || Orbs||AntiFingerprints||AntiSpiritBox|| AntiWriting)
                label12.Visible = false;
            else label12.Visible = true;

            //banshee
            if (Writing||SpiritBox||Orbs||AntiEMF||AntiFingerprints||AntiTemps)
                label14.Visible = false;
            else label14.Visible = true;

            //jinn
            if (Writing || Temps || Fingerprints||AntiSpiritBox||AntiOrbs||AntiEMF)
                label20.Visible = false;
            else label20.Visible = true;
            
            //mare
            if (EMF||Writing||Fingerprints||AntiOrbs||AntiSpiritBox||AntiTemps)
                label15.Visible = false;
            else label15.Visible = true;

            //oni
            if (Fingerprints || Temps || Orbs||AntiWriting||AntiSpiritBox||AntiEMF)
                label23.Visible = false;
            else label23.Visible = true;

            //phantom
            if (Writing||Fingerprints||SpiritBox||AntiEMF||AntiOrbs||AntiTemps)
                label13.Visible = false;
            else label13.Visible = true;

            //poltergeist
            if (EMF || Temps || Writing||AntiFingerprints||AntiOrbs||AntiSpiritBox)
                label19.Visible = false;
            else label19.Visible = true;

            //revenant
            if (SpiritBox||Temps||Orbs||AntiEMF||AntiFingerprints||AntiWriting)
                label21.Visible = false;
            else label21.Visible = true;

            //shade
            if (Fingerprints || SpiritBox || Temps||AntiWriting||AntiOrbs||AntiEMF)
                label16.Visible = false;
            else label16.Visible = true;

            //wraith
            if (EMF||Writing||Orbs||AntiFingerprints||AntiSpiritBox||AntiTemps)
                label18.Visible = false;
            else label18.Visible = true;

            //yurei
            if (EMF || Fingerprints || SpiritBox||AntiTemps||AntiWriting||AntiOrbs)
                label17.Visible = false;
            else label17.Visible = true;



            //1,7 orb   4,8 SpiritBox    6,9 Fingies      10,11 EMF     13,12 Temps     14,15 Writing
            if (EMF && Writing)
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = false; checkBox12.Visible = false;
                checkBox14.Visible = true; checkBox15.Visible = true;
            }
            else if (EMF && SpiritBox)
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = false; checkBox9.Visible = false;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = false; checkBox12.Visible = false;
                checkBox14.Visible = true; checkBox15.Visible = true;
            }
            else if (EMF && Orbs)
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = false; checkBox9.Visible = false;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = true; checkBox15.Visible = true;
            }
            else if (EMF && Temps)
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = false; checkBox8.Visible = false;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = false; checkBox15.Visible = false;
            }
            else if (EMF && Fingerprints)
            {
                checkBox1.Visible = false; checkBox7.Visible = false;
                checkBox4.Visible = false; checkBox8.Visible = false;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = true; checkBox15.Visible = true;
            }
            else if (Writing && SpiritBox)
            {
                checkBox1.Visible = false; checkBox7.Visible = false;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = true; checkBox15.Visible = true;
            }
            else if (Writing && Orbs)
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = false; checkBox8.Visible = false;
                checkBox6.Visible = false; checkBox9.Visible = false;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = true; checkBox15.Visible = true;
            }
            else if (Writing && Temps)
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = false; checkBox9.Visible = false;
                checkBox10.Visible = false; checkBox11.Visible = false;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = true; checkBox15.Visible = true;
            }
            else if (Writing && Fingerprints)
            {
                checkBox1.Visible = false; checkBox7.Visible = false;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = false; checkBox12.Visible = false;
                checkBox14.Visible = true; checkBox15.Visible = true;
            }
            else if (SpiritBox && Fingerprints)
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox10.Visible = false; checkBox11.Visible = false;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = true; checkBox15.Visible = true;
            }
            else if (SpiritBox && Orbs)
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = false; checkBox15.Visible = false;
            }
            else if (SpiritBox && Temps)
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox10.Visible = false; checkBox11.Visible = false;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = true; checkBox15.Visible = true;
            }
            else if (Orbs && Temps)
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = false; checkBox9.Visible = false;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = true; checkBox15.Visible = true;
            }
            else if (Orbs && Fingerprints)
            {
                
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox10.Visible = false; checkBox11.Visible = false;
                checkBox13.Visible = false; checkBox12.Visible = false;
                checkBox14.Visible = false; checkBox15.Visible = false;
            }
            else if (Temps && Fingerprints)
            {
                checkBox1.Visible = false; checkBox7.Visible = false;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = false; checkBox15.Visible = false;
            }
            else
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = true; checkBox15.Visible = true;
            }
            

            if (EMF && Fingerprints && Temps)
            {
                checkBox1.Visible = false; checkBox7.Visible = false;
                checkBox4.Visible = false; checkBox8.Visible = false;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = false; checkBox15.Visible = false;
                Ghost = "Banshee";
            } 
            else if (Writing && SpiritBox && Temps)
            {

                checkBox1.Visible = false; checkBox7.Visible = false;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = false; checkBox9.Visible = false;
                checkBox10.Visible = false; checkBox11.Visible = false;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = true; checkBox15.Visible = true;
                Ghost = "Demon";
            }
            else if (EMF && SpiritBox && Orbs)
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = false; checkBox9.Visible = false;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = false; checkBox12.Visible = false;
                checkBox14.Visible = false; checkBox15.Visible = false;
                Ghost = "Jinn";
            }
            else if (Temps && SpiritBox && Orbs)
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = false; checkBox9.Visible = false;
                checkBox10.Visible = false; checkBox11.Visible = false;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = false; checkBox15.Visible = false;
                Ghost = "Mare";
            }
            else if (EMF && SpiritBox && Writing)
            {
                checkBox1.Visible = false; checkBox7.Visible = false;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = false; checkBox9.Visible = false;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = false; checkBox12.Visible = false;
                checkBox14.Visible = true; checkBox15.Visible = true;
                Ghost = "Oni";
            }
            else if (EMF && Orbs && Temps)
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = false; checkBox8.Visible = false;
                checkBox6.Visible = false; checkBox9.Visible = false;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = false; checkBox15.Visible = false;
                Ghost = "Phantom";
            }
            else if (Orbs && Fingerprints && SpiritBox)
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox10.Visible = false; checkBox11.Visible = false;
                checkBox13.Visible = false; checkBox12.Visible = false;
                checkBox14.Visible = false; checkBox15.Visible = false;
                Ghost = "Poltergeist";
            }
            else if (EMF && Writing && Fingerprints)
            {
                checkBox1.Visible = false; checkBox7.Visible = false;
                checkBox4.Visible = false; checkBox8.Visible = false;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = false; checkBox12.Visible = false;
                checkBox14.Visible = true; checkBox15.Visible = true;
                Ghost = "Revenant";
            }
            else if (EMF && Writing && Orbs)
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = false; checkBox8.Visible = false;
                checkBox6.Visible = false; checkBox9.Visible = false;
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = false; checkBox12.Visible = false;
                checkBox14.Visible = true; checkBox15.Visible = true;
                Ghost = "Shade";
            }
            else if (Writing && Fingerprints && SpiritBox)
            {
                checkBox1.Visible = false; checkBox7.Visible = false;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox10.Visible = false; checkBox11.Visible = false;
                checkBox13.Visible = false; checkBox12.Visible = false;
                checkBox14.Visible = true; checkBox15.Visible = true;
                Ghost = "Spirit";
            }
            else if (Temps && SpiritBox && Fingerprints)
            {
                checkBox1.Visible = false; checkBox7.Visible = false;
                checkBox4.Visible = true; checkBox8.Visible = true;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox10.Visible = false; checkBox11.Visible = false;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = false; checkBox15.Visible = false;
                Ghost = "Wraith";
            }
            else if (Temps && Writing && Orbs)
            {
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = false; checkBox8.Visible = false;
                checkBox6.Visible = false; checkBox9.Visible = false;
                checkBox10.Visible = false; checkBox11.Visible = false;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox14.Visible = true; checkBox15.Visible = true;
                Ghost = "Yurei";
            }
            else if (Orbs && Fingerprints)
            {
                Ghost = "Poltergeist";
            }
            else Ghost = "Unknown";


            //Ghost Orb - checkBox1.Visible=true;checkBox7.Visible=true;
            //Spirit Box - checkBox4.Visible=true;checkBox8.Visible=true;
            //Fingerprints - checkBox6.Visible=true;checkBox9.Visible=true;
            //EMF - checkBox10.Visible=true;checkBox11.Visible=true;
            //Temps - checkBox13.Visible=true;checkBox12.Visible=true;
            //GhostWriting - checkBox14.Visible=true;checkBox15.Visible=true;

            if (!checkBox7.Checked)
            {
                checkBox1.Visible = false;
                checkBox7.Visible = false;
            }
            if (!checkBox8.Checked)
            {
                checkBox4.Visible = false;
                checkBox8.Visible = false;
            }
            if (!checkBox9.Checked)
            {
                checkBox6.Visible = false;
                checkBox9.Visible = false;
            }
            if (!checkBox11.Checked)
            {
                checkBox10.Visible = false;
                checkBox11.Visible = false;
            }
            if (!checkBox12.Checked)
            {
                checkBox13.Visible = false;
                checkBox12.Visible = false;
            }
            if (!checkBox15.Checked)
            {
                checkBox14.Visible = false;
                checkBox15.Visible = false;
            }
            if (label12.Visible == true)
            {
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox14.Visible = true; checkBox15.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
            }
            if (label13.Visible == true)
            {
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox1.Visible = true; checkBox7.Visible = true;
            }
            if (label14.Visible == true)
            {
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox13.Visible = true; checkBox12.Visible = true;
            }
            if (label15.Visible == true)
            {
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
            }
            if (label16.Visible == true)
            {
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox14.Visible = true; checkBox15.Visible = true;
            }
            if (label17.Visible == true)
            {
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox14.Visible = true; checkBox15.Visible = true;
            }
            if (label18.Visible == true)
            {
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
            }
            if (label19.Visible == true)
            {
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
            }
            if (label20.Visible == true)
            {
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox1.Visible = true; checkBox7.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
            }
            if (label21.Visible == true)
            {
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox6.Visible = true; checkBox9.Visible = true;
                checkBox14.Visible = true; checkBox15.Visible = true;
            }
            if (label22.Visible == true)
            {
                checkBox14.Visible = true; checkBox15.Visible = true;
                checkBox13.Visible = true; checkBox12.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
            }
            if (label23.Visible == true)
            {
                checkBox10.Visible = true; checkBox11.Visible = true;
                checkBox14.Visible = true; checkBox15.Visible = true;
                checkBox4.Visible = true; checkBox8.Visible = true;
            }

            int gCount = 0;
            string possibleGhost = "";
            foreach(var cb in Ghosts)
            {
                if (cb.Visible == true)
                {
                    gCount++;
                    possibleGhost = cb.Text;
                }
            }
            if (gCount == 1)
            {
                Ghost = possibleGhost;
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
                switch (Ghost)
                {
                    case "Spirit":
                        label7.Text = "Smudge stick affects ghost longer.";
                        break;
                    case "Shade":
                        label7.Text = "Shy guy, will be harder to find if in groups.\nWill not hunt if multiple people are around.";
                        break;
                    case "Phantom":
                        label7.Text = "Drops sanity if looked at, take a photo to make it go away.";
                        break;
                    case "Jinn":
                        label7.Text = "Power hungry, kill the breaker.  Territorial, will attack when threatened.\n With power on, it moves faster";
                        break;
                    case "Yurei":
                        label7.Text = "Strong effect on sanity.  Smudge to prevent from wandering.";
                        break;
                    case "Mare":
                        label7.Text = "Lights on! If any light is on in the room with it, it decreases it's chance to hunt";
                        break;
                    case "Demon":
                        label7.Text = "Very aggressive, be careful. Ask Ouija board if demon (won't lower sanity)";
                        break;
                    case "Banshee":
                        label7.Text = "Will target one player badly.  Less aggressive when near a crucifix.";
                        break;
                    case "Revenant":
                        label7.Text = "Travels fast when hunting, hiding will slow it down";
                        break;
                    case "Oni":
                        label7.Text = "More active when players are around / doing things nearby.";
                        break;
                    case "Poltergeist":
                        label7.Text = "Can throw multiple objects.  Stay in empty rooms.";
                        break;
                    case "Wraith":
                        label7.Text = "Almost never touches the ground.  Use salt.  It can see through doors.";
                        break;
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 10;
            comboBox2.SelectedIndex = 10;
            comboBox3.SelectedIndex = 10;
            comboBox4.SelectedIndex = 10;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox10.Checked = false;
            checkBox14.Checked = false;
            checkBox13.Checked = false;
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            
            if (hunt)
            {
                if (huntTimer < 10)
                {
                    label2.Text = "0:0"+huntTimer.ToString();
                }
                else
                {
                    label2.Text = "0:"+huntTimer.ToString();
                }
                
                if (huntTimer == 0)
                {
                    if (SoundSetting) player.Play();
                    button10.Text = "Hunt";
                    button11.Text = "Round Start";
                    hunt = false;
                    timer1.Enabled = false;
                }
                huntTimer--;

            }
            else
            {
                double temp = 0;
                double temp2 = 0;
                roundTimer--;
                temp = roundTimer/60;
                temp2 = (Math.Floor(temp) * 60) - roundTimer;
                temp2 = Math.Abs(temp2);
                if (temp2 < 10)
                {
                    label2.Text = temp + ":0" + temp2;
                }
                else
                {
                    label2.Text = temp + ":" + temp2;
                }
                if (roundTimer == 0)
                {
                    if (SoundSetting) player.Play();
                    button10.Text = "Hunt";
                    button11.Text = "Round Start";
                    timer1.Enabled = false;
                }
            }

        }
        
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                difficulty = 1;
                checkBox5.Visible = false;
                checkBox3.Visible = false;
            }
            else
            {
                checkBox5.Visible = true;
                checkBox3.Visible = true;
            }
            if (timer1.Enabled == false)
            {
                label2.Text="5:00";
            }
        }
        
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                difficulty = 3;
                checkBox2.Visible = false;
                checkBox3.Visible = false;
                if (timer1.Enabled == false)
                {
                    label2.Text = "0:00";
                }
            }
            else
            {
                if (timer1.Enabled == false)
                {
                    label2.Text = "5:00";
                }
                difficulty = 1;
                checkBox2.Visible = true;
                checkBox3.Visible = true;
            }
            
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                difficulty = 2;
                checkBox2.Visible = false;
                checkBox5.Visible = false;
                if (timer1.Enabled == false)
                {
                    label2.Text = "2:00";
                }
            }
            else
            {
                difficulty = 1;
                checkBox2.Visible = true;
                checkBox5.Visible = true;
                if (timer1.Enabled == false)
                {
                    label2.Text = "5:00";
                }
            }
           
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
            {
                timer1.Enabled = false;
                hunt = false;
                button10.Text = "Hunt";
                button11.Text = "Round Start";

            }
            else
            {
                hunt = true;
                button10.Text = "Stop";
                button11.Text = "Stop";
                switch (difficulty)
                {
                    case 1:
                        huntTimer = 24;
                        label2.Text = "0:25";
                        break;
                    case 2:
                        huntTimer = 34;
                        label2.Text = "0:40";
                        break;
                    case 3:
                        huntTimer = 49;
                        label2.Text = "0:55";
                        break;
                    default:
                        huntTimer = 24;
                        label2.Text = "0:25";
                        break;
                }
                timer1.Enabled = true;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled != true)
            {
                button10.Text = "Stop";
                button11.Text = "Stop";
                switch (difficulty)
                {
                    case 1:
                        roundTimer = 300;
                        hunt = false;
                        timer1.Enabled = true;
                        label2.Text = "5:00";
                        break;
                    case 2:
                        roundTimer = 120;
                        hunt = false;
                        timer1.Enabled = true;
                        label2.Text = "2:00";
                        break;
                    case 3:
                        roundTimer = 0;
                        button10.Text = "Hunt";
                        button11.Text = "Round Start";
                        break;
                    default:
                        roundTimer = 300;
                        hunt = false;
                        timer1.Enabled = true;
                        label2.Text = "5:00";
                        break;
                }
            }
            else
            {

                timer1.Enabled = false;
                hunt = false;
                button10.Text = "Hunt";
                button11.Text = "Round Start";
                
            }
            
        }


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
                label10.Text = "Hunt Chance: ~Unknown";
            }
            else
            {
                label10.Text = "Hunt Chance: ~"+HuntChance.ToString();
                if (label22.Visible == true)
                {
                    //demon
                    double HuntChance2=0;
                    var huntCalc2 = 100 - pAvg + 15;
                    if (huntCalc2 < 50) HuntChance2 = 0;
                    else if (huntCalc2 >= 50 && huntCalc2 < 75) HuntChance2 = 10;
                    else if (huntCalc2 >= 75) HuntChance2 = 16.50;
                    label9.Text = "Possible Hunt: ~" + HuntChance2.ToString();
                }
                else if (label15.Visible == true)
                {
                    //mare
                    double HuntChance2 = 0;
                    var huntCalc2 = 100 - pAvg + 10;
                    if (huntCalc2 < 50) HuntChance2 = 0;
                    else if (huntCalc2 >= 50 && huntCalc2 < 75) HuntChance2 = 10;
                    else if (huntCalc2 >= 75) HuntChance2 = 16.50;
                    label9.Text = "Possible Hunt: ~" + HuntChance2.ToString();
                }
                else
                {
                    label9.Text = "Possible Hunt: ~0";
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
            if (show)
            {
                label2.Visible = true;
                button10.Visible = true;
                button11.Visible = true;
            }
            else
            {
                label2.Visible = false;
                button10.Visible = false;
                button11.Visible = false;
            }
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
}
