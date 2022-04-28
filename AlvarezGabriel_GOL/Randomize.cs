using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlvarezGabriel_GOL
{
    public partial class Randomize : Form
    {
        // Stores the seed generated randomly or entered by the user
        int seed;

        public Randomize()
        {
            InitializeComponent();
            // Adjusts the min and max of the numericupdown to the min and max of ints
            numericUpDown1.Maximum = int.MaxValue;
            numericUpDown1.Minimum = int.MinValue;
            // Sets the values of the member fields to the values of the property settings
            Seed = Properties.Settings.Default.Seed;
            numericUpDown1.Value = Seed;
        }

        // Property for the seed member field
        public int Seed
        {
            get { return seed; }
            set { seed = value; }
        }

        // Generates a random seed
        private void button1_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            Seed = random.Next(int.MinValue, int.MaxValue);
            numericUpDown1.Value = seed;
        }

        // Sets the newly entered value as the value of the member field
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            seed = (int)numericUpDown1.Value;
        }
    }
}
