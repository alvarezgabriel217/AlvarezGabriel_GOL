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
        static int width = Properties.Settings.Default.Width;

        // Height
        static int height = Properties.Settings.Default.Height;

        // Time in milliseconds
        static int time;

        // Universe boundary behaviour
        bool boundary = false;

        // The universe array
        bool[,] universe = new bool[width, height];

        // The Scratch pad array
        bool[,] scratchPad = new bool[width, height];

        // Determines whether the grid is shown or not
        bool grid = true;
        
        // Determines whether the neighbors of each cell are shown or not
        bool neighborCount = true;

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;
        Color backColor = Color.White;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        static int generations = 0;

        // Number of living cells
        int aliveCells = 0;

        // Current seed
        int seed = 0;

        public int Generations
        {
            get { return generations; }
            set { generations = value; }
        }

        public Form1()
        {
            InitializeComponent();
            // Sets the values of the member fields to the values of the property settings
            backColor = Properties.Settings.Default.BackColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;
            time = Properties.Settings.Default.Interval;
            seed = Properties.Settings.Default.Seed;

            // Setup the timer
            timer.Interval = time; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running

            IntervalLabel.Text = "Interval: " + time.ToString();

            SeedLabel.Text = "Seed: " + seed.ToString();
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // If boundary = true, the universe is finite
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
                    // Else, the universe is toroidal
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
            toolStripStatusLabelGenerations.Text = "Generations: " + generations.ToString();
            graphicsPanel1.Invalidate();
        }

        // Counts the neighbors of each cell on a finite universe
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

        // Counts the neighbors of each cell on a toroidal universe
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
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / universe.GetLength(0);

            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / universe.GetLength(1);


            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // A brush for filling dead cells interiors (color)
            Brush backBrush = new SolidBrush(backColor);

            // A brush for filling number of neighbors of each cell (color)
            Brush neighborBrush = new SolidBrush(Color.Red);

            // A brush for filling the color of the HUD
            Brush hudBrush = new SolidBrush(Color.Red);



            // Sets the font and format of the HUD
            Font hudFont = new Font("Arial", 12f);
            StringFormat hudFormat = new StringFormat();
            hudFormat.LineAlignment = StringAlignment.Far;

            aliveCells = 0;

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels

                    RectangleF cellRect = Rectangle.Empty;

                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                        aliveCells++;
                    }
                    // Fill the cell with another brush if dead
                    else
                    {
                        e.Graphics.FillRectangle(backBrush, cellRect);
                    }


                    if (grid)
                    {
                        // Outline the cell with a pen
                        e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                    }


                    // Font and format for the number of neighbors of each cell
                    Font font = new Font("Arial", 10f);
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    // If neighborCount == true, it means that the neighbor count is enabled
                    if (neighborCount)
                    {
                        // Change color of brush based on the rules and the universe boundary type
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

            // Creates a second rectangle for the HUD
            RectangleF cellRect2 = Rectangle.Empty;
            cellRect2.Width = graphicsPanel1.ClientSize.Width;
            cellRect2.Height = graphicsPanel1.ClientSize.Height;

            // If the HUD option is enabled, draws the HUD
            if (hUDToolStripMenuItem.Checked)
            {
                if (boundary)
                {
                    e.Graphics.DrawString("Generations: " + generations + "\n" + "Cell Count: " + aliveCells + '\n' + "Boundary: Finite" + "\n" + "Universe Size: {Width: " + width + ", height: " + height + "}", hudFont, hudBrush, cellRect2, hudFormat);
                }
                else
                {
                    e.Graphics.DrawString("Generations: " + generations + "\n" + "Cell Count: " + aliveCells + '\n' + "Boundary: Toroidal" + "\n" + "Universe Size: {Width: " + width + ", height: " + height + "}", hudFont, hudBrush, cellRect2, hudFormat);
                }
            }
            AliveLabel.Text = "Alive: " + aliveCells.ToString();
            SeedLabel.Text = "Seed: " + seed.ToString();

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
            neighborBrush.Dispose();
            backBrush.Dispose();
            hudBrush.Dispose();
        }

        // Controls what happens when the user clicks a cell whether its alive or dead
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

        // Starts the timer, enables the pause button and disables the start button
        private void Start_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            Start.Enabled = false;
            startToolStripMenuItem.Enabled = false;
            Pause.Enabled = true;
            pauseToolStripMenuItem.Enabled = true;
        }

        // Pauses the timer, enables the start button and disables the pause button 
        private void Pause_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            Pause.Enabled = false;
            pauseToolStripMenuItem.Enabled = false;
            Start.Enabled = true;
            startToolStripMenuItem.Enabled = true;
        }

        // Increases the generation count by 1
        private void Next_Click(object sender, EventArgs e)
        {
            NextGeneration();
        }

        // Clears the universe
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
            toolStripStatusLabelGenerations.Text = "Generations: " + generations.ToString();
            graphicsPanel1.Invalidate();
        }

        //Saves the current universe to a text file
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

        // Opens a saved universe
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

        // Randomizes the cells based on a seed entered by the user or randomly generated seed
        private void fromSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Randomize random = new Randomize();
            if (DialogResult.OK == random.ShowDialog())
            {
                seed = random.Seed;
                Random rng = new Random(seed);
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
                SeedLabel.Text = "Seed: " + seed.ToString();
                graphicsPanel1.Invalidate();
            }
        }

        // Randomizes the cells based on time
        private void fromTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            seed = DateTime.Now.Millisecond;
            Random rng = new Random(seed);
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
            SeedLabel.Text = "Seed: " + seed.ToString();
            graphicsPanel1.Invalidate();
        }

        // Randomizes the cells based on the current seed
        private void currentSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rng = new Random(seed);
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

        // Opens the options menu which allows the user to change the height and width of the universe and the interval of the timer 
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
                IntervalLabel.Text = "Interval: " + time.ToString();
            }
        }

        // Allows the user to change the background color of the universe (Color of dead cells)
        private void backColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            backColor = colorDialog1.Color;
            graphicsPanel1.Invalidate();
        }

        // Allows the user to change the color of living cells
        private void cellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            cellColor = colorDialog1.Color;
            graphicsPanel1.Invalidate();
        }

        // Allows the user to change the color of the grid
        private void grtidColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            gridColor = colorDialog1.Color;
            graphicsPanel1.Invalidate();
        }

        // Enables or disables the HUD
        private void hUDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hUDToolStripMenuItem.Checked)
            {
                hUDToolStripMenuItem.Checked = false;
                hUDToolStripMenuItem1.Checked = false;
            }
            else
            {
                hUDToolStripMenuItem.Checked = true;
                hUDToolStripMenuItem1.Checked = true;
            }
            graphicsPanel1.Invalidate();
        }

        // Enables or disables the grid
        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid)
            {
                grid = false;
                gridToolStripMenuItem.Checked = false;
                gridToolStripMenuItem1.Checked = false;
            }
            else
            {
                grid = true;
                gridToolStripMenuItem.Checked = true;
                gridToolStripMenuItem1.Checked = true;

            }
            graphicsPanel1.Invalidate();
        }

        // Enables or disables the neighbor count
        private void neighborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (neighborCountToolStripMenuItem.Checked == true)
            {
                neighborCountToolStripMenuItem.Checked = false;
                neighborCountToolStripMenuItem1.Checked = false;
                neighborCount = false;
            }
            else
            {
                neighborCountToolStripMenuItem.Checked = true;
                neighborCountToolStripMenuItem1.Checked = true;
                neighborCount = true;
            }
            graphicsPanel1.Invalidate();
        }

        // Changes the boundary type of the universe to toroidal
        private void toroidalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            boundary = false;
            toroidalToolStripMenuItem.Checked = true;
            toroidalToolStripMenuItem1.Checked = true;
            finiteToolStripMenuItem.Checked = false;
            finiteToolStripMenuItem1.Checked = false;
            graphicsPanel1.Invalidate();
        }

        // Changes the boundary type of the universe to finite
        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            boundary = true;
            toroidalToolStripMenuItem.Checked = false;
            toroidalToolStripMenuItem1.Checked = false;
            finiteToolStripMenuItem.Checked = true;
            finiteToolStripMenuItem1.Checked = true;
            graphicsPanel1.Invalidate();
        }

        // Allows the user to go to a specific generation (must be a number greater than the current generation)
        private void toToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToGeneration to = new ToGeneration();
            if (DialogResult.OK == to.ShowDialog())
            {
                timer.Enabled = false;
                for(int i = generations; i < to.Generations; i++)
                {
                    NextGeneration();
                }
            }
        }

        // Saves the settings of the current universe whenever the application is closed
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.BackColor = backColor;
            Properties.Settings.Default.CellColor = cellColor;
            Properties.Settings.Default.GridColor = gridColor;
            Properties.Settings.Default.Seed = seed;
            Properties.Settings.Default.Width = width;
            Properties.Settings.Default.Height = height;

            Properties.Settings.Default.Save();
        }

        // Closes the application whenever the exit button is clicked
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Resets the universe's settings to the default settings
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            width = Properties.Settings.Default.Width;
            height = Properties.Settings.Default.Height;
            backColor = Properties.Settings.Default.BackColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;
            width = Properties.Settings.Default.Width;
            seed = Properties.Settings.Default.Seed;
            if (width != universe.GetLength(0) || height != universe.GetLength(1))
            {
                universe = new bool[width, height];
                scratchPad = new bool[width, height];
                graphicsPanel1.Invalidate();
            }
            graphicsPanel1.Invalidate();
        }

        // Reloads the latest settings entered by the user
        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            width = Properties.Settings.Default.Width;
            height = Properties.Settings.Default.Height;
            backColor = Properties.Settings.Default.BackColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;
            width = Properties.Settings.Default.Width;
            seed = Properties.Settings.Default.Seed;
            if (width != universe.GetLength(0) || height != universe.GetLength(1))
            {
                universe = new bool[width, height];
                scratchPad = new bool[width, height];
                graphicsPanel1.Invalidate();
            }
            graphicsPanel1.Invalidate();
        }
    }
}
