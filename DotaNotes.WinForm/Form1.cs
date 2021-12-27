using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dota2GSI;
using Dota2GSI.Nodes;
using DotaNotes.Core.Common;

namespace DotaNotes.WinForm
{
    public partial class Form1 : Form
    {
        private readonly IGSIConfigService _gsiConfigService;
        private readonly IGameStateListener _gameStateListener;
        public Form1()
        {
            _gsiConfigService = new GSIConfigService();
            
            Process[] pname = Process.GetProcessesByName("Dota2");
            if (pname.Length == 0)
               MessageBox.Show("Dota 2 is not running. Please start Dota 2.", "Status");
            
            
            _gameStateListener = new GameStateListener(4000);
            _gameStateListener.NewGameState += OnNewGameState;
           
            if (!_gameStateListener.Start())
                MessageBox.Show("GameStateListener could not start. Try running this program as Administrator. Exiting.");
            
            InitializeComponent();
        }
        
        private void OnNewGameState(GameState gs)
        {
            textBox1.Text += gs.Player.Activity.ToString();
            //Console.WriteLine(gs.Player.Activity);
            // Console.WriteLine("Press ESC to quit");
            // Console.WriteLine("Current Dota version: " + gs.Provider.Version);
            // Console.WriteLine("Current time as displayed by the clock (in seconds): " + gs.Map.ClockTime);
            // Console.WriteLine("Your steam name: " + gs.Player.Name);
            // Console.WriteLine("hero ID: " + gs.Hero.ID);
            // Console.WriteLine("Health: " + gs.Hero.Health);
            for (int i = 0; i < gs.Abilities.Count; i++)
            {
                // Console.WriteLine("Ability {0} = {1}", i, gs.Abilities[i].Name);
            }
            // Console.WriteLine("First slot inventory: " + gs.Items.GetInventoryAt(0).Name);
            // Console.WriteLine("Second slot inventory: " + gs.Items.GetInventoryAt(1).Name);
            // Console.WriteLine("Third slot inventory: " + gs.Items.GetInventoryAt(2).Name);
            // Console.WriteLine("Fourth slot inventory: " + gs.Items.GetInventoryAt(3).Name);
            // Console.WriteLine("Fifth slot inventory: " + gs.Items.GetInventoryAt(4).Name);
            // Console.WriteLine("Sixth slot inventory: " + gs.Items.GetInventoryAt(5).Name);
            //
            // if (gs.Items.InventoryContains("item_blink"))
            //     Console.WriteLine("You have a blink dagger");
            // else
            //     Console.WriteLine("You DO NOT have a blink dagger");
        }

      
    }
}