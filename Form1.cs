using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        List<Tile> allTiles;

        private void createAndAssignTiles()
        {
            allTiles = new List<Tile>();

            int regionNumber = 0;

            for(int offY = 0; offY <= 6; offY += 3)
            {
                for(int offX = 0; offX <= 6; offX += 3)
                {        
                    for(int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            // Create tile
                            NumericUpDown NUD = new NumericUpDown();
                            NUD.Value = 0;
                            Tile t = new Tile(i + offX, j + offY, regionNumber, NUD);
                            
                            allTiles.Add(t);

                            // Draw the tile to the grid
                            grid.Controls.Add(t.NUD, t.mX, t.mY);
                        }
                    }

                    regionNumber++;
                }
            }
        }

        // Returns true if any tiles with 'num' does not exist in 't' row, col, and region
        private bool canNumFitInUTile(Tile t, int num)
        {
            bool isNumberInRange = (!allTiles.Any(tile => (tile.NUD.Value == num) && 
                                        (tile.mX == t.mX || tile.mY == t.mY || tile.regionNumber == t.regionNumber)));

            return isNumberInRange;
        }

        // If a tile is equal to 0, it will return its column and row
        private bool isBoardComplete(out Tile t)
        {
            t = (allTiles.FirstOrDefault(tile => tile.NUD.Value == 0));

            return (t == null);
        }

        public bool solveGrid()
        {
            Tile uTile;             // Unassigned Tile

            if (!isBoardComplete(out uTile))
            {
                for(int num = 1; num < 10; num++)
                {
                    if(canNumFitInUTile(uTile, num))
                    {
                        uTile.NUD.Value = num;

                        if (solveGrid())
                            return true;

                        uTile.NUD.Value = 0;
                    }
                }

                // If no number between 1-9 fits in the unassigned tile, backtrack.
                return false;
            }

            return true;
        }

        private void solve_btn_Click(object sender, EventArgs e)
        {
            solveGrid();
        }

        public Form1()
        {
            InitializeComponent();
            createAndAssignTiles();
        }
    }

    class Tile
    {
        public int mX;
        public int mY;
        public int regionNumber;
        public NumericUpDown NUD;

        public Tile(int x, int y, int region, NumericUpDown nud)
        {
            regionNumber = region;
            mX = x;
            mY = y;
            NUD = nud;
        }
    }
}
