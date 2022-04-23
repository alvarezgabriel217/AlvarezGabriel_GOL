using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AlvarezGabriel_GOL
{
    public partial class Form1 : Form
    {
        // Width
        static int width = 10;

        // Height
        static int height = 10;

        // Time in milliseconds
        static int time = 100;

        // Universe boundary behaviour
        bool boundary = false;

        // The universe array
        bool[,] universe = new bool[width, height];

        // The Scratch pad array
        bool[,] scratchPad = new bool[width, height];

        bool grid = true;

        bool neighborCount = true;

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        public Form1()
        {
            InitializeComponent();

            // Setup the timer
            timer.Interval = time; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
        }

        // Generates a random universe from time
        public void RandomUniverseTime()
        {
            Random rng = new Random();
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (rng.Next(0, 3) == 0)
                    {

                        universe[x, y] = true;
                    }
                }
            }
        }

        // Generates a random universe from a seed
        public void RandomUniverseSeed()
        {
            Random rng = new Random();
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (rng.Next(0, 3) == 0)
                    {

                        universe[x, y] = true;
                    }
                }
            }
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (boundary)
                    {
                        if (CountNeighborsFinite(x, y) < 2 && universe[x, y] == true)
                        {
                            scratchPad[x, y] = false;
                        }
                        if (CountNeighborsFinite(x, y) > 3 && universe[x, y] == true)
                        {
                            scratchPad[x, y] = false;
                        }
                        if (CountNeighborsFinite(x, y) == 3 && universe[x, y] == false)
                        {
                            scratchPad[x, y] = true;
                        }
                        else
                        {
                            scratchPad[x, y] = false;
                        }
                        if ((CountNeighborsFinite(x, y) == 3 || CountNeighborsFinite(x, y) == 2) && universe[x, y] == true)
                        {
                            scratchPad[x, y] = true;
                        }
                    }
                    else
                    {
                        if (CountNeighborsToroidal(x, y) < 2 && universe[x, y] == true)
                        {
                            scratchPad[x, y] = false;
                        }
                        if (CountNeighborsToroidal(x, y) > 3 && universe[x, y] == true)
                        {
                            scratchPad[x, y] = false;
                        }
                        if (CountNeighborsToroidal(x, y) == 3 && universe[x, y] == false)
                        {
                            scratchPad[x, y] = true;
                        }
                        else
                        {
                            scratchPad[x, y] = false;
                        }
                        if ((CountNeighborsToroidal(x, y) == 3 || CountNeighborsToroidal(x, y) == 2) && universe[x, y] == true)
                        {
                            scratchPad[x, y] = true;
                        }
                    }
                }
            }

            // Increment generation count
            generations++;

            // Swap the universe with the scratch Pad
            bool[,] temp = universe;
            universe = scratchPad;
            scratchPad = temp;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            graphicsPanel1.Invalidate();
        }

        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then continue
                    if (xCheck < 0)
                    {
                        continue;
                    }
                    // if yCheck is less than 0 then continue
                    if (yCheck < 0)
                    {
                        continue;
                    }
                    // if xCheck is greater than or equal too xLen then continue
                    if (xCheck >= xLen)
                    {
                        continue;
                    }
                    // if yCheck is greater than or equal too yLen then continue
                    if (yCheck >= yLen)
                    {
                        continue;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }

        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then set to xLen - 1
                    if (xCheck < 0)
                    {
                        xCheck = xLen - 1;
                    }
                    // if yCheck is less than 0 then set to yLen - 1
                    if (yCheck < 0)
                    {
                        yCheck = yLen - 1;
                    }
                    // if xCheck is greater than or equal too xLen then set to 0
                    if (xCheck >= xLen)
                    {
                        xCheck = 0;
                    }
                    // if yCheck is greater than or equal too yLen then set to 0
                    if (yCheck >= yLen)
                    {
                        yCheck = 0;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X

            //int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / universe.GetLength(0);

            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y

            //int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / universe.GetLength(1);


            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // A brush for filling number of neighbors of each cell (color)
            Brush neighborBrush = new SolidBrush(Color.Red);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels

                    //Rectangle cellRect = Rectangle.Empty;
                    RectangleF cellRect = Rectangle.Empty;


                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }


                    if (grid)
                    {
                        // Outline the cell with a pen
                        e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    }


                    // Font for the number of neighbors of each cell
                    Font font = new Font("Arial", 10f);
                    // Format for the number of neighbors of each cell
                    StringFormat stringFormat = new StringFormat();


                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    if (neighborCount)
                    {
                        // Change color of brush based on the rules
                        if (boundary)
                        {
                            if (CountNeighborsFinite(x, y) < 2 && universe[x, y] == true)
                            {
                                neighborBrush = new SolidBrush(Color.Red);
                            }
                            if (CountNeighborsFinite(x, y) > 3 && universe[x, y] == true)
                            {
                                neighborBrush = new SolidBrush(Color.Red);
                            }
                            if (CountNeighborsFinite(x, y) == 3 && universe[x, y] == false)
                            {
                                neighborBrush = new SolidBrush(Color.Green);
                            }
                            else
                            {
                                neighborBrush = new SolidBrush(Color.Red);
                            }
                            if ((CountNeighborsFinite(x, y) == 3 || CountNeighborsFinite(x, y) == 2) && universe[x, y] == true)
                            {
                                neighborBrush = new SolidBrush(Color.Green);
                            }

                            // Draw the number of neighbors of each cell
                            if (CountNeighborsFinite(x, y) > 0)
                            {
                                e.Graphics.DrawString(CountNeighborsFinite(x, y).ToString(), font, neighborBrush, cellRect, stringFormat);
                            }
                        }
                        else
                        {
                            if (CountNeighborsToroidal(x, y) < 2 && universe[x, y] == true)
                            {
                                neighborBrush = new SolidBrush(Color.Red);
                            }
                            if (CountNeighborsToroidal(x, y) > 3 && universe[x, y] == true)
                            {
                                neighborBrush = new SolidBrush(Color.Red);
                            }
                            if (CountNeighborsToroidal(x, y) == 3 && universe[x, y] == false)
                            {
                                neighborBrush = new SolidBrush(Color.Green);
                            }
                            else
                            {
                                neighborBrush = new SolidBrush(Color.Red);
                            }
                            if ((CountNeighborsToroidal(x, y) == 3 || CountNeighborsToroidal(x, y) == 2) && universe[x, y] == true)
                            {
                                neighborBrush = new SolidBrush(Color.Green);
                            }

                            // Draw the number of neighbors of each cell
                            if (CountNeighborsToroidal(x, y) > 0)
                            {
                                e.Graphics.DrawString(CountNeighborsToroidal(x, y).ToString(), font, neighborBrush, cellRect, stringFormat);
                            }
                        }
                    }
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
            neighborBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = e.Y / cellHeight;

                // Toggle the cell's state
                universe[x, y] = !universe[x, y];

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Start_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                    scratchPad[x, y] = false;
                }
            }
            generations = 0;
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            graphicsPanel1.Invalidate();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";


            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file. 
                // It appends a CRLF for you.
                //writer.WriteLine("!This is my comment.");

                // Iterate through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;

                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        // If the universe[x,y] is alive then append 'O' (capital O)
                        // to the row string.
                        // Else if the universe[x,y] is dead then append '.' (period)
                        // to the row string.
                        if (universe[x, y] == true)
                        {
                            currentRow += "O";
                        }
                        else
                        {
                            currentRow += ".";
                        }

                    }

                    // Once the current row has been read through and the 
                    // string constructed then write it to the file using WriteLine.
                    writer.WriteLine(currentRow);
                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then it is a comment
                    // and should be ignored.

                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.


                    if (String.IsNullOrEmpty(row) == false && row[0] != '!')
                    {
                        maxHeight++;
                    }

                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                    if (row.Length > maxWidth)
                    {
                        maxWidth = row.Length;
                    }
                }

                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.
                width = maxWidth;
                height = maxHeight;
                universe = new bool[width, height];
                scratchPad = new bool[width, height];

                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                int yPos = 0;
                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then
                    // it is a comment and should be ignored.

                    // If the row is not a comment then 
                    // it is a row of cells and needs to be iterated through.
                    if (String.IsNullOrEmpty(row) == false && row[0] != '!')
                    {
                        for (int xPos = 0; xPos < row.Length; xPos++)
                        {
                            // If row[xPos] is a 'O' (capital O) then
                            // set the corresponding cell in the universe to alive.
                            if (row[xPos] == 'O')
                            {
                                universe[xPos, yPos] = true;
                            }

                            // If row[xPos] is a '.' (period) then
                            // set the corresponding cell in the universe to dead.
                            if (row[xPos] == '.')
                            {
                                universe[xPos, yPos] = false;
                            }
                        }
                        yPos++;
                    }
                }

                // Close the file.
                reader.Close();
                graphicsPanel1.Invalidate();
            }
        }

        // Randomizes the cells based on a seed entered by the user or randomly generated.
        private void fromSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Randomize random = new Randomize();
            random.ShowDialog();
            if (random.DialogResult == DialogResult.OK)
            {
                Random rng = new Random(random.value);
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        universe[x, y] = false;
                        if (rng.Next(0, 3) == 0)
                        {

                            universe[x, y] = true;
                        }
                    }
                }
                graphicsPanel1.Invalidate();
            }
        }

        // Randomizes the cells based on time.
        private void fromTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rng = new Random();
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                    if (rng.Next(0, 3) == 0)
                    {

                        universe[x, y] = true;
                    }
                }
            }
            graphicsPanel1.Invalidate();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOptions options = new FormOptions();
            options.ShowDialog();
            graphicsPanel1.Invalidate();
            if (options.DialogResult == DialogResult.OK)
            {
                width = options.GridWidth;
                height = options.GridHeight;
                time = options.Time;
                if (width != universe.GetLength(0) || height != universe.GetLength(1))
                {
                    universe = new bool[width, height];
                    scratchPad = new bool[width, height];
                    graphicsPanel1.Invalidate();
                }
                timer.Interval = time;
            }
        }

        private void backColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
             = colorDialog1.Color;
            graphicsPanel1.Invalidate();
        }

        private void cellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            cellColor = colorDialog1.Color;
            graphicsPanel1.Invalidate();
        }

        private void grtidColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            gridColor = colorDialog1.Color;
            graphicsPanel1.Invalidate();
        }

        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid)
            {
                grid = false;
                gridToolStripMenuItem.Checked = false;
            }
            else
            {
                grid = true;
                gridToolStripMenuItem.Checked = true;

            }
            graphicsPanel1.Invalidate();
        }

        private void neighborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(neighborCountToolStripMenuItem.Checked ==  true)
            {
                neighborCountToolStripMenuItem.Checked = false;
                neighborCount = false;
            }
            else
            {
                neighborCountToolStripMenuItem.Checked = true;
                neighborCount = true;
            }
            graphicsPanel1.Invalidate();
        }

        private void toroidalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            boundary = false;
            toroidalToolStripMenuItem.Checked = true;
            finiteToolStripMenuItem.Checked = false;
            graphicsPanel1.Invalidate();
        }

        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            boundary = true;
            toroidalToolStripMenuItem.Checked = false;
            finiteToolStripMenuItem.Checked = true;
            graphicsPanel1.Invalidate();
        }
    }
}
