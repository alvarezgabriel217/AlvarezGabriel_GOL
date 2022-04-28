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
    public partial class ToGeneration : Form
    {
        // Stores the generation
        int generations;
        public ToGeneration()
        {
            InitializeComponent();
            // Instantiates form1
            Form1 form = new Form1();
            // Sets the value of the member field to the current generation
            Generations = form.Generations;
            // sets the value of the numericupdown to the value of the member field
            numericUpDown1.Value = Generations;
        }

        // Property for the generations member field
        public int Generations 
        {
            get { return generations; }
            set { generations = value; } 
        }

        // Sets the newly entered value as the value of the member field,
        // generations now becomes the target generation
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Generations = (int)numericUpDown1.Value;
        }
    }
}
