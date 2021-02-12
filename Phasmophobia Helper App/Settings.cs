using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phasmophobia_Helper_App
{
    public partial class Settings : Form
    {
        public Form1 form;
        static Microsoft.Win32.RegistryKey SlayersPhasmophobiaHelper;
        public Settings(Form1 pForm1)
        {
            form = pForm1;
            InitializeComponent();
            SlayersPhasmophobiaHelper = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\SlayersPhasmophobiaHelper\\");
            try
            {
                if (SlayersPhasmophobiaHelper.GetValue("Timer") == null) SlayersPhasmophobiaHelper.SetValue("Timer", true);
                if (SlayersPhasmophobiaHelper.GetValue("Sound") == null) SlayersPhasmophobiaHelper.SetValue("Sound", true);
                if (SlayersPhasmophobiaHelper.GetValue("KeepOnTop") == null) SlayersPhasmophobiaHelper.SetValue("KeepOnTop", false);
                if (SlayersPhasmophobiaHelper.GetValue("Sync") == null) SlayersPhasmophobiaHelper.SetValue("Sync", true);

               
                if (SlayersPhasmophobiaHelper.GetValue("Sound").ToString() == "True")
                {
                    form.SoundSetting = true;
                    //checkBox1.Checked = true;
                }
                else
                {

                    form.SoundSetting = false;
                    //checkBox1.Checked = false;
                }
                if (SlayersPhasmophobiaHelper.GetValue("Timer").ToString() == "True")
                {
                    form.showHideTimer(true);
                    //checkBox2.Checked = false;
                }
                else
                {
                    form.showHideTimer(false);
                    //checkBox2.Checked = true;
                }
                if (SlayersPhasmophobiaHelper.GetValue("KeepOnTop").ToString() == "True")
                {
                    form.KeepOnTop(false);
                    checkBox3.Checked = true;
                }
                else
                {
                    form.KeepOnTop(true);
                    checkBox3.Checked = false;

                }
                
                if (SlayersPhasmophobiaHelper.GetValue("BGR") == null) SlayersPhasmophobiaHelper.SetValue("BGR", "64");
                if (SlayersPhasmophobiaHelper.GetValue("BGG") == null) SlayersPhasmophobiaHelper.SetValue("BGG", "64");
                if (SlayersPhasmophobiaHelper.GetValue("BGB") == null) SlayersPhasmophobiaHelper.SetValue("BGB", "64");
                if (SlayersPhasmophobiaHelper.GetValue("TEXTR") == null) SlayersPhasmophobiaHelper.SetValue("TEXTR", "255");
                if (SlayersPhasmophobiaHelper.GetValue("TEXTG") == null) SlayersPhasmophobiaHelper.SetValue("TEXTG", "255");
                if (SlayersPhasmophobiaHelper.GetValue("TEXTB") == null) SlayersPhasmophobiaHelper.SetValue("TEXTB", "255");
                textBox1.Text = SlayersPhasmophobiaHelper.GetValue("BGR").ToString();
                form.bgR = Int32.Parse(SlayersPhasmophobiaHelper.GetValue("BGR").ToString());
                textBox2.Text = SlayersPhasmophobiaHelper.GetValue("BGG").ToString();
                form.bgG = Int32.Parse(SlayersPhasmophobiaHelper.GetValue("BGG").ToString());
                textBox3.Text = SlayersPhasmophobiaHelper.GetValue("BGB").ToString();
                form.bgB = Int32.Parse(SlayersPhasmophobiaHelper.GetValue("BGB").ToString());
                textBox4.Text = SlayersPhasmophobiaHelper.GetValue("TEXTR").ToString();
                form.textR = Int32.Parse(SlayersPhasmophobiaHelper.GetValue("TEXTR").ToString());
                textBox5.Text = SlayersPhasmophobiaHelper.GetValue("TEXTG").ToString();
                form.textG = Int32.Parse(SlayersPhasmophobiaHelper.GetValue("TEXTG").ToString());
                textBox6.Text = SlayersPhasmophobiaHelper.GetValue("TEXTB").ToString();
                form.textB = Int32.Parse(SlayersPhasmophobiaHelper.GetValue("TEXTB").ToString());
                form.setColors();
            }
            catch(Exception exc)
            {

            }
            SlayersPhasmophobiaHelper.Flush();
        }

        private void colorThings()
        {
            form.bgR = Int32.Parse(SlayersPhasmophobiaHelper.GetValue("BGR").ToString());
            form.bgG = Int32.Parse(SlayersPhasmophobiaHelper.GetValue("BGG").ToString());
            form.bgB = Int32.Parse(SlayersPhasmophobiaHelper.GetValue("BGB").ToString());
            form.textR = Int32.Parse(SlayersPhasmophobiaHelper.GetValue("TEXTR").ToString());
            form.textG = Int32.Parse(SlayersPhasmophobiaHelper.GetValue("TEXTG").ToString());
            form.textB = Int32.Parse(SlayersPhasmophobiaHelper.GetValue("TEXTB").ToString());
            form.setColors();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox1.Checked)
            //{
            //    SlayersPhasmophobiaHelper.SetValue("Sound", true);
            //}
            //else
            //{
            //    SlayersPhasmophobiaHelper.SetValue("Sound", false);
            //}
            //SlayersPhasmophobiaHelper.Flush();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int txtInt=0;
            if(Int32.TryParse(textBox1.Text,out txtInt)){
                SlayersPhasmophobiaHelper.SetValue("BGR", txtInt);
                SlayersPhasmophobiaHelper.Flush();
                colorThings();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int txtInt = 0;
            if (Int32.TryParse(textBox2.Text, out txtInt))
            {
                SlayersPhasmophobiaHelper.SetValue("BGG", txtInt);
                SlayersPhasmophobiaHelper.Flush();
                colorThings();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            int txtInt = 0;
            if (Int32.TryParse(textBox3.Text, out txtInt))
            {
                SlayersPhasmophobiaHelper.SetValue("BGB", txtInt);
                SlayersPhasmophobiaHelper.Flush();
                colorThings();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            int txtInt = 0;
            if (Int32.TryParse(textBox4.Text, out txtInt))
            {
                SlayersPhasmophobiaHelper.SetValue("TEXTR", txtInt);
                SlayersPhasmophobiaHelper.Flush();
                colorThings();
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            int txtInt = 0;
            if (Int32.TryParse(textBox5.Text, out txtInt))
            {
                SlayersPhasmophobiaHelper.SetValue("TEXTG", txtInt);
                SlayersPhasmophobiaHelper.Flush();
                colorThings();
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            int txtInt = 0;
            if (Int32.TryParse(textBox6.Text, out txtInt))
            {
                SlayersPhasmophobiaHelper.SetValue("TEXTB", txtInt);
                SlayersPhasmophobiaHelper.Flush();
                colorThings();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //if (!checkBox2.Checked)
            //{
            //    form.showHideTimer(true);
            //    SlayersPhasmophobiaHelper.SetValue("Timer", true);
            //}
            //else
            //{
            //    form.showHideTimer(false);
            //    SlayersPhasmophobiaHelper.SetValue("Timer", false);
            //}
            //SlayersPhasmophobiaHelper.Flush();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox3.Checked)
            {
                form.KeepOnTop(true);
                SlayersPhasmophobiaHelper.SetValue("KeepOnTop", true);

            }
            else
            {
                form.KeepOnTop(false);
                SlayersPhasmophobiaHelper.SetValue("KeepOnTop", false);
            }
        }

       
    }
}
