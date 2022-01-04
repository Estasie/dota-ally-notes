using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dota2GSI;
using Dota2GSI.Nodes;
using DotaNotes.Core;
using DotaNotes.Core.Common;
using DotaNotes.Core.Common.SteamIdConverter;

namespace DotaNotes.WinForm
{
    public partial class Form1 : Form
    {
        private IGSIConfigService _gsiConfigService;
        private IGameStateListener _gameStateListener;
        private ServerLogParserService _serverLogParserService;
        private PlayerInfoService _playerInfoService;
        private readonly Point _pictureBoxPos = new Point(15, 20);
        private readonly Point _labelPos = new Point(50,15);
        private readonly Point _linkLabelPos = new Point(170,15);
        private readonly Point _noteInfoLabelPos = new Point(310, 15);
        private readonly Point _noteButtonPos = new Point(430, 15);
        private readonly Size _posOffset = new Size(0, 40);
        
        private bool isWorking = false;
        
        
        private event Func<Task> DoWork;

        public Form1()
        {
            InitializeComponent();
            
            DoWork += Initialize;
            timer1.Enabled = true;
        }

        private async Task Initialize()
        {
            if (isWorking == false)
            {
                isWorking = true;
                
                _gsiConfigService = new GSIConfigService();
                if (!_gsiConfigService.Create())
                {
                    MessageBox.Show("Registry key for steam not found, cannot create Gamestate Integration file", "Dota Notes");
                    Environment.Exit(0);
                }

                _serverLogParserService = new ServerLogParserService();
                if (!_serverLogParserService.Initialize())
                {
                    MessageBox.Show("Server_log.txt not found in Dota2 folder", "Dota Notes");
                    Environment.Exit(0);
                }

                _playerInfoService = new PlayerInfoService();
            
                _gameStateListener = new GameStateListener(Convert.ToInt32(ConfigurationManager.AppSettings["GSIPort"]));
                if (!_gameStateListener.Start())
                {
                    MessageBox.Show("GameStateListener could not start. Try running this program as Administrator", "Dota Notes");
                    Environment.Exit(0);
                }
            
                _gameStateListener.NewGameState += OnNewGameState;
            }
        }

        private void Dispose()
        {
            isWorking = false;
            _gameStateListener.NewGameState -= OnNewGameState;
        }
        
        private void OnNewGameState(GameState gs)
        {
            var playerSteamIds = _serverLogParserService.GetPlayerSteamIds();
            
            if (playerSteamIds == null) 
            {
                MessageBox.Show("Players Steam Ids couldn't parse", "Dota Notes");
                Environment.Exit(0);
            }
            
            var users = _playerInfoService.GetInfo(playerSteamIds).ToList();
            
            if (users.Count > 0)
            {
                var pictureBoxPos = _pictureBoxPos;
                var labelPos = _labelPos;
                var linkLabelPos = _linkLabelPos;
                var noteInfoLabelPos = _noteInfoLabelPos;
                var noteButtonPos = _noteButtonPos;
                
                foreach (var user in users)
                {
                    var pictureBox = new PictureBox
                    {
                        Name = user.SteamId64, 
                        Size = new Size(30, 30),
                        Location = pictureBoxPos,
                        SizeMode =  PictureBoxSizeMode.StretchImage,
                        ImageLocation = user.AvatarFullURL,
                        Visible =  true
                    };
                    
                    var nicknameLabel = new Label()
                    {
                        Name = user.SteamId64,
                        Location = labelPos,
                        Text = user.Name,
                        AutoSize = true,
                        Font = new Font("Calibri", 12),
                        ForeColor = Color.Black,
                        Padding = new Padding(4),
                    };

                    var linkLabel = new LinkLabel()
                    {
                        Name = user.SteamId64,
                        Location = linkLabelPos,
                        Text = user.SteamId64,
                        Font = new Font("Calibri", 10),
                        ForeColor = Color.Blue,
                        Padding = new Padding(4),
                        Anchor = AnchorStyles.Left,
                        AutoSize = true
                    };
                    
                    linkLabel.LinkClicked += (sender, args) =>
                    {
                        Clipboard.SetText(user.ProfileUrl);
                    };
                    
                    var noteInfoLabel = new Label()
                    {
                        Name = user.SteamId64,
                        Location = noteInfoLabelPos,
                        Text = user.Note == null ? "Нет заметок" : "Найдены заметки",
                        Font = new Font("Calibri", 10),
                        ForeColor = user.Note == null ? Color.Green : Color.Red,
                        Padding = new Padding(4),
                        Anchor = AnchorStyles.Left,
                        AutoSize = true
                    };
                    
                    var createNoteButton = new Button()
                    {
                        Name = user.SteamId64,
                        Location = noteButtonPos,
                        Text = user.Note == null ? "Добавить заметку" : "Изменить заметку",
                        Font = new Font("Calibri", 9),
                        ForeColor = Color.Black,
                        Padding = new Padding(4),
                        Anchor = AnchorStyles.Left,
                        AutoSize = true
                    };
                    
                    pictureBoxPos = Point.Add(pictureBoxPos, _posOffset);
                    labelPos = Point.Add(labelPos, _posOffset);
                    linkLabelPos = Point.Add(linkLabelPos, _posOffset);
                    noteInfoLabelPos = Point.Add(noteInfoLabelPos, _posOffset);
                    noteButtonPos = Point.Add(noteButtonPos, _posOffset);
                    
                    groupBox1.Controls.Add(pictureBox);
                    groupBox1.Controls.Add(nicknameLabel);
                    groupBox1.Controls.Add(linkLabel);
                    groupBox1.Controls.Add(noteInfoLabel);
                    groupBox1.Controls.Add(createNoteButton);
                }
            }
            else
            {
                MessageBox.Show("Cannot load user profiles", "Dota notes");
            }
            
            
            //Console.WriteLine(gs.Player.Activity);
            // Console.WriteLine("Press ESC to quit");
            // Console.WriteLine("Current Dota version: " + gs.Provider.Version);
            // Console.WriteLine("Current time as displayed by the clock (in seconds): " + gs.Map.ClockTime);
            // Console.WriteLine("Your steam name: " + gs.Player.Name);
            // Console.WriteLine("hero ID: " + gs.Hero.ID);
            // Console.WriteLine("Health: " + gs.Hero.Health);
            // for (int i = 0; i < gs.Abilities.Count; i++)
            // {
            //     // Console.WriteLine("Ability {0} = {1}", i, gs.Abilities[i].Name);
            // }
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
        

        private void timer1_Tick(object sender, EventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("Dota2");

            if (pname.Length > 0)
            {
                dotaStatusLabel.Text = "Dota status: Running";
                dotaStatusLabel.ForeColor = Color.Green;
                DoWork?.Invoke();
                //timer1.Stop();
            }
            else
            {
                dotaStatusLabel.Text = "Dota status: Not Running";
                dotaStatusLabel.ForeColor = Color.Red;
                if(isWorking) Dispose();
            }
        }
    }
}