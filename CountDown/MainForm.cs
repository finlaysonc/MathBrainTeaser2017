// Written by David Hancock mailto:code@davidhancock.net
// This code is published under The Code Project Open License.
// For full license terms please see:
// http://www.codeproject.com/info/cpol10.aspx

using System;
using System.Media;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;


namespace CountDown
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// user entered parameters
        /// </summary>
        private int target = 0;
        private int[] tiles = new int[4];

        /// <summary>
        /// valid target range
        /// </summary>
        private const int min_Target = 1;  // a single tile solution is not possible
        private const int max_Target = 100;



        public MainForm()
        {
            InitializeComponent();
        }



        private void MainForm_Load(object sender, EventArgs e)
        {
            TileOptionsComboBox.SelectedIndex = 1;  // one large and 5 small tiles
            PickTilesAndTarget();
        }




        private void SolveButton_Click(object sender, EventArgs e)
        {
            if (SolveButton.Enabled)
            {
                SolveButton.Enabled = false;

                if (ValidateUserEnteredParameters())
                {
                    ResultsListBox.Items.Clear();

                    // Make a copy of the input params to avoid a possible race hazard
                    int[] localTiles = (int[])tiles.Clone();
           
                    SolveEquation(localTiles, target);
                }
                else
                    SolveButton.Enabled = true;
            }
        }



        /// <summary>
        /// Start the solving engine in a thread pool work item
        /// </summary>
        private void SolveEquation(int[] tiles, int target)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                PostfixSolvingEngine engine = new PostfixSolvingEngine(tiles, target);
                engine.Solve();

                stopWatch.Stop();
                UpdateUI(new PostfixSolvingEngine.Results(engine), stopWatch.Elapsed);
            });
        }





        /// <summary>
        /// Cross thread safe delegate to update the ui with the solver results
        /// Unfortunately no Async/Await to be found here (it's Net3.5)
        /// </summary>
        /// <param name="results"></param>
        /// <param name="duration"></param>
        public void UpdateUI(PostfixSolvingEngine.Results results, TimeSpan duration)
        {
            if (ResultsListBox.InvokeRequired)
                ResultsListBox.BeginInvoke((Action<PostfixSolvingEngine.Results, TimeSpan>)UpdateUI, results, duration);
            else
            {
                ResultsListBox.BeginUpdate();

                if (results.Matches.Count > 0)
                {
                    for (int row = 0; row < results.Matches.Count; row++)
                        ResultsListBox.Items.Add(results.Matches[row]);

                    ResultsListBox.Items.Add(String.Empty);
                    ResultsListBox.Items.Add(String.Format("There are {0} solutions.", results.Matches.Count));
                }
                else if (results.ClosestNonMatch.Length > 0)
                {
                    ResultsListBox.Items.Add("There are no solutions.");
                    ResultsListBox.Items.Add(String.Format("The closest match is {0} away. ", Math.Abs(results.Difference)));
                    ResultsListBox.Items.Add(String.Empty);
                    ResultsListBox.Items.Add(String.Format("{0} = {1}", results.ClosestNonMatch, results.Target - results.Difference));
                    ResultsListBox.Items.Add(String.Empty);
                }
                else
                    ResultsListBox.Items.Add("There are no solutions less than 10 from the target");

                ResultsListBox.Items.Add(String.Format("Evaluated in {0}.{1:D3} seconds.", duration.Seconds, duration.Milliseconds));
                //ResultsListBox.Items.Add(String.Format("Tiles are {0}, {1}, {2}, {3}, {4}, {5}", results.Tiles[0], results.Tiles[1], results.Tiles[2], results.Tiles[3], results.Tiles[4], results.Tiles[5])); 
                ResultsListBox.Items.Add(String.Format("Target is {0}", results.Target));              
                
                ResultsListBox.EndUpdate();
                SolveButton.Enabled = true;
            }
        }

        

        private void ChooseButton_Click(object sender, EventArgs e)
        {
            // VS 2010 express doesn't have a unit test framework
            Debug.Assert(TestCode.Run());

            PickTilesAndTarget();
        }
            

        /// <summary>
        /// Automatically chooses six tiles and a target. 
        /// Tiles can have 0 to 4 large tiles from the set (25, 50, 75, 100) without repetition,
        /// the remaining tiles are picked from the set (1, 2, 3, 4, 5, 6, 7, 8, 9, 10) allowing
        /// a maximum of two of each value. 
        /// The target can be between 100 and 999.
        /// </summary>
        private void PickTilesAndTarget() 
        {
            int[] largeTiles = { 25, 50, 75, 100 };
            int[] smallTiles = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            TextBox[] tileTextBoxes = { Tile1TextBox, Tile2TextBox, Tile3TextBox, Tile4TextBox, };

            Random rand = new Random(); // seed is the current tick count

            int largeCount = TileOptionsComboBox.SelectedIndex;
            int smallCount = 4 - largeCount ;
            
            // pick large tiles
            PickCards(rand, largeCount, largeTiles, tileTextBoxes, 0);

            // pick small tiles
            PickCards(rand, smallCount, smallTiles, tileTextBoxes, largeCount);

            // pick target
            target = rand.Next(min_Target, max_Target+1);
            TargetTextBox.Text = target.ToString();
        }


        /// <summary>
        /// Picks tiles randomly from the supplied array and loads the correspond ui 
        /// text boxes
        /// </summary>
        /// <param name="rand"></param>
        /// <param name="cardCount"></param>
        /// <param name="possibleCards"></param>
        /// <param name="tileTextBoxes"></param>
        /// <param name="textBoxIndex"></param>
        private void PickCards (Random rand, int cardCount, int[] possibleTiles, TextBox[] tileTextBoxes, int textBoxIndex)
        {
            while (cardCount > 0)
            {
                int rnd = rand.Next(possibleTiles.Length);

                if (possibleTiles[rnd] != 0) // check this tile hasn't already been used
                {
                    tiles[textBoxIndex] = possibleTiles[rnd];
                    tileTextBoxes[textBoxIndex].Text = possibleTiles[rnd].ToString();

                    possibleTiles[rnd] = 0;
                    ++textBoxIndex ;
                    --cardCount ;
                }
            }
        }



        /// <summary>
        /// Checks that the tiles and target contain valid values.
        /// </summary>
        /// <returns></returns>
        private bool ValidateUserEnteredParameters()
        {
            // check target value
            if (target < min_Target || target > max_Target)
            {
                ShowMessageBox(String.Format("The target value must be between {0} and {1}", min_Target, max_Target), "Invalid Target Value");
                TargetTextBox.SelectAll();
                TargetTextBox.Focus() ;
                return false;
            }

            // check tiles contain valid values
            int[] tileValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 25, 50, 75, 100 };
            int[] tileCount = new int[tileValues.Length];
            TextBox[] tileTextBoxes = { Tile1TextBox, Tile2TextBox, Tile3TextBox, Tile4TextBox, };

            //// check of invalid values
            //for(int index = 0 ; index < tiles.Length; index++)
            //{
            //    int searchIndex = Array.BinarySearch(tileValues, tiles[index]);
                
            //    if (searchIndex < 0)
            //    {
            //        ShowMessageBox("Tile values must be between 1 and 10, or 25, 50, 75 or 100", "Invalid Tile Value");
            //        tileTextBoxes[index].SelectAll();
            //        tileTextBoxes[index].Focus();
            //        return false;
            //    }
            //    else
            //        tileCount [searchIndex] += 1 ; // record how many of each value there is
            //}


            // check number of tiles are correct
            for (int index = 0; index < tileCount.Length; index++)
            {
                if ((tileValues[index] < 11 && tileCount[index] > 2) || (tileValues[index] > 10 && tileCount[index] > 1))
                {
                    string message;

                    if (tileValues[index] > 10)
                        message = String.Format("There is more than one {0} tile.{1}Only one 25, 50, 75 or 100 tile is allowed.", tileValues[index], Environment.NewLine);
                    else
                        message = String.Format("There are more than two {0} tile.{1}Only two tiles with the same value between 1 and 10 are allowed.", tileValues[index], Environment.NewLine);
                    
                    ShowMessageBox(message, "Invalid Tile Value");

                    // select the tile in the ui
                    for (int textIndex = 0; textIndex < tiles.Length; textIndex++)
                    {
                        if (tiles[textIndex] == tileValues[index])
                        {
                            tileTextBoxes[textIndex].SelectAll();
                            tileTextBoxes[textIndex].Focus();
                            break;
                        }
                    }
                    return false;
                }
            }

            // No checking if the number of large and small tiles match the selected combo box option
            // It will only be incorrect if the user enters tiles manually and as long as they are valid
            // tiles so be it...
            return true;
        }



        /// <summary>
        /// Ensure that common message box properties remain consistent
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        private void ShowMessageBox(string text, string caption)
        {
            MessageBox.Show(this, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }






        /// <summary>
        /// Start an approximately 30 second progress bar
        /// Never going to be too accurate because it relies on the os scheduler
        /// responding when we want, but it has other priorities...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerButton_Click(object sender, EventArgs e)
        {
            if (TimerButton.Enabled)
            {
                TimerButton.Enabled = false;

                progressBar.Value = 0;
                progressBar.Maximum = 200;

                long startTime = DateTime.Now.Ticks;

                ThreadPool.QueueUserWorkItem(o =>
                {
                    long interval = (30 * 1000 * 10000) / 200;  // 30 seconds divided by 200 steps
                    long next = startTime + interval;

                    for (long step = 0; step < 200; step++, next += interval)
                    {
                        long sleep = (next - DateTime.Now.Ticks) / 10000;

                        if (sleep > 0)
                            Thread.Sleep((int)sleep);

                        progressBar.BeginInvoke((Action)progressTimer_Tick);
                    }

                    Thread.Sleep(1500);
                    progressBar.BeginInvoke((Action)progressTimer_Tick);
                });
            }
        }


        /// <summary>
        /// Timer fired delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void progressTimer_Tick()
        {
            if (progressBar.Value < progressBar.Maximum)
            {
                progressBar.PerformStep();

                if (progressBar.Value == progressBar.Maximum)
                {
                    SystemSounds.Exclamation.Play();
                    progressBar.Update();
                }
            }
            else
            {
                progressBar.Value = 0;
                TimerButton.Enabled = true;
            }
        }




        /// <summary>
        /// Copy the selected items in the results list to the clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder data = new StringBuilder();

            foreach (string line in ResultsListBox.SelectedItems)
                data.AppendLine(line);

            Clipboard.SetText(data.ToString());
        }


        /// <summary>
        /// Selects all in the results list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectAllStripMenuItem_Click(object sender, EventArgs e)
        {
            ResultsListBox.BeginUpdate();

            for (int row = 0; row < ResultsListBox.Items.Count; row++)
                ResultsListBox.SetSelected(row, true);

            ResultsListBox.EndUpdate();
        }


        /// <summary>
        /// About to show the context menu for the results list. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ResultsListBox.Items.Count == 0) // don't show the menu
                e.Cancel = true;
            else
                copyToolStripMenuItem.Enabled = ResultsListBox.SelectedItems.Count > 0;
        }


        /// <summary>
        /// Stop non integer digits being typed into text boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true ;
                SystemSounds.Beep.Play();
            }
        }


        /// <summary>
        /// Stop the user pasting invalid text into the integer text boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox_TextChanged(TargetTextBox, ref target);
        }


        private void Card1TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox_TextChanged(Tile1TextBox, ref tiles[0]) ;
        }

        private void Card2TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox_TextChanged(Tile2TextBox, ref tiles[1]);
        }

        private void Card3TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox_TextChanged(Tile3TextBox, ref tiles[2]);
        }

        private void Card4TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox_TextChanged(Tile4TextBox, ref tiles[3]);
        }



        private void TextBox_TextChanged(TextBox textBox, ref int value)
        {
            try
            {
                if (textBox.TextLength > 0)
                    value = Convert.ToInt32(textBox.Text);
                else
                    value = -1;  // empty text is valid until the user has finished entering values
            }
            catch (FormatException)
            {
                if (value == -1)
                    textBox.Text = string.Empty;
                else
                {
                    textBox.Text = value.ToString();
                    TargetTextBox.Select(textBox.TextLength, 0);
                }

                SystemSounds.Beep.Play();
            }
        }
    }
}
