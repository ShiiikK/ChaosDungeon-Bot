﻿using AutoItX3Lib;
using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes;
using PixelAimbot.Classes.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace PixelAimbot
{
    public partial class ChaosBot : Form
    {
        ///BOOLS START///////////BOOLS START///////////BOOLS START///////////BOOLS START///////////BOOLS START///////////BOOLS START///////////BOOLS START///
        ///                                                                                                                                               ///
        private bool _start = false;
        private bool _stop = false;

        private bool _Floor1 = false;
        private bool _Floor2 = false;
        private bool _Floor3 = false;
        private bool _Floor1Fight = false;
        private bool _Floor2Fight = false;
        private bool _Floor3Fight = false;

        private bool _REPAIR = false;
        private bool _Shadowhunter = false;
        private bool _Berserker = false;
        private bool _Paladin = false;
        private bool _Deathblade = false;
        private bool _Sharpshooter = false;
        private bool _Bard = false;
        private bool _Sorcerer = false;
        private bool _Soulfist = false;

        private bool _LOGOUT = false;

        private bool _FIGHT = false;
        private bool Search = false;

        //SKILL AND COOLDOWN//
        private bool _Q = true;
        private bool _W = true;
        private bool _E = true;
        private bool _R = true;
        private bool _A = true;
        private bool _S = true;
        private bool _D = true;
        private bool _F = true;

        private System.Timers.Timer timer;
        private int fightSequence = 0;
        private int fightSequence2 = 0;
        private int searchSequence = 0;
        private int searchSequence2 = 0;
        private int CompleteIteration = 1;
        private int fightOnSecondAbility = 1;
        private int walktopUTurn = 1;
        private int walktopUTurn2 = 1;
        private int Floor2 = 1;
        private int Floor3 = 1;

        ///                                                                                                                                                 ///
        ///BOOLS ENDE////////////BOOLS ENDE////////////////BOOLS ENDE//////////////////BOOLS ENDE///////////////BOOLS ENDE/////////////////////BOOLS ENDE/////

        /// OPENCV START  /// OPENCV START  /// OPENCV START  /// OPENCV START

        public string resourceFolder = "";
        Priorized_Skills SKILLS = new Priorized_Skills();
        private (int, int) PixelToAbsolute(double x, double y, Point screenResolution)
        {
            int newX = (int)(x / screenResolution.X * 65535);
            int newY = (int)(y / screenResolution.Y * 65535);
            return (newX, newY);
        }

        private static readonly Random random = new Random();
        public Rotations rotation = new Rotations();
        /////
        ///
        // 2. Import the RegisterHotKey Method
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        protected override void WndProc(ref Message m)
        {
            // 5. Catch when a HotKey is pressed !
            if (m.Msg == 0x0312)
            {
                int id = m.WParam.ToInt32();
                // MessageBox.Show(string.Format("Hotkey #{0} pressed", id));

                // 6. Handle what will happen once a respective hotkey is pressed
                switch (id)
                {
                    case 1:
                        btnStart_Click(null, null);

                        break;

                    case 2:
                        btnPause_Click(null, null);

                        break;
                }
            }

            base.WndProc(ref m);
        }

        private AutoItX3 au3 = new AutoItX3();

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                        int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        private static readonly IntPtr HWND_TOP = new IntPtr(0);

        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        private const UInt32 SWP_NOSIZE = 0x0001;

        private const UInt32 SWP_NOMOVE = 0x0002;

        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        public static InputSimulator inputSimulator = new InputSimulator();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public Layout_Keyboard currentLayout;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
        public static string ConfigPath { get; set; } = Directory.GetCurrentDirectory() + @"\" + HWID.GetAsMD5();
        public ChaosBot()
        {
            InitializeComponent();
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string applicationFolder = Path.Combine(folder, "cb_res");

            resourceFolder = applicationFolder;

            this.FormBorderStyle = FormBorderStyle.None;
            refreshRotationCombox();
            this.Text = RandomString(15);
            // 3. Register HotKeys
            label15.Text = Config.version;
            // Set an unique id to your Hotkey, it will be used to
            // identify which hotkey was pressed in your code to execute something
            int FirstHotkeyId = 1;
            // Set the Hotkey triggerer the F9 key
            // Expected an integer value for F9: 0x78, but you can convert the Keys.KEY to its int value
            // See: https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx
            int FirstHotKeyKey = (int)Keys.F9;
            // Register the "F9" hotkey
            Boolean F9Registered = RegisterHotKey(
                this.Handle, FirstHotkeyId, 0x0000, FirstHotKeyKey
            );

            // Repeat the same process but with F10
            int SecondHotkeyId = 2;
            int SecondHotKeyKey = (int)Keys.F10;
            Boolean F10Registered = RegisterHotKey(
                this.Handle, SecondHotkeyId, 0x0000, SecondHotKeyKey
            );

            // 4. Verify if both hotkeys were succesfully registered, if not, show message in the console
            if (!F9Registered)
            {
                btnStart_Click(null, null);
            }

            if (!F10Registered)
            {
                btnPause_Click(null, null);
                cts.Cancel();
            }
        }

        public void refreshRotationCombox()
        {

            string[] files = Directory.GetFiles(ConfigPath);
            comboBoxRotations.Items.Clear();
            foreach (string file in files)
            {
                if (Path.GetFileNameWithoutExtension(file) != "main")
                {
                    comboBoxRotations.Items.Add(Path.GetFileNameWithoutExtension(file));
                }
            }

        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private CancellationTokenSource cts = new CancellationTokenSource();

        private void btnPause_Click(object sender, EventArgs e)

        {
            if (_stop == true)
            {
                cts.Cancel();
                _start = false;
                _stop = false;
                _REPAIR = false;
                _Shadowhunter = false;
                _Berserker = false;
                _Paladin = false;
                _Bard = false;

                _Deathblade = false;
                _Sharpshooter = false;
                _Sorcerer = false;
                _Soulfist = false;
    

                _LOGOUT = false;

                _FIGHT = false;

                _Q = true;
                _W = true;
                _E = true;
                _R = true;
                _A = true;
                _S = true;
                _D = true;
                _F = true;
     





                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "STOPPED!"));
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
           
            if (_start == false)
                try
                {
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is starting..."));
                    _start = true;
                    _stop = true;
                    cts = new CancellationTokenSource();
                    var token = cts.Token;
                    var t1 = Task.Run(() => STARTKLICK(token));
                    await Task.WhenAny(new[] { t1 });
                    if (chBoxAutoRepair.Checked == true && _start == false)
                    {
                        var repair = Task.Run(() => REPAIRTIMER(token));

                    }
                    else
                    {
                        _REPAIR = false;
                    }
                    if (chBoxLOGOUT.Checked == true && _start == false)
                    {
                        var logout = Task.Run(() => LOGOUTTIMER(token));
                    }
                    else
                    {
                        _LOGOUT = false;
                    }
                }
                catch (OperationCanceledException)
                {
                    // Handle canceled
                }
                catch (Exception)
                {
                    // Handle other exceptions
                }
        }

        public async Task REPAIRTIMER(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                timer = new System.Timers.Timer((int.Parse(txtRepair.Text) * 1000) * 60);

                timer.Elapsed += OnTimedEvent;
                timer.AutoReset = false;
                timer.Enabled = true;
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        public async Task LOGOUTTIMER(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);
                timer = new System.Timers.Timer((int.Parse(txtLOGOUT.Text) * 1000) * 60);

                timer.Elapsed += OnTimedEvent2;
                timer.AutoReset = false;
                timer.Enabled = true;
                cts.Cancel();
        }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
               
                _REPAIR = true;
            }
            catch (AggregateException)
            {
                MessageBox.Show("Expected");
            }
            catch (ObjectDisposedException)
            {
                MessageBox.Show("Bug");
            }
            catch { }

        }

        private void OnTimedEvent2(object source, ElapsedEventArgs e)
        {
            try
            {
              
                _LOGOUT = true;
            }
            catch (AggregateException)
            {
                MessageBox.Show("Expected");
            }
            catch (ObjectDisposedException)
            {
                MessageBox.Show("Bug");
            }
            catch { }
        }

        private async Task STARTKLICK(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);


                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);

                    for (int i = 0; i < 2; i++)
                    {
                        try
                        {
                                au3.MouseClick("" + txtLEFT.Text + "", 960, 529, 1,10);

                        }
                        catch (AggregateException)
                        {
                            MessageBox.Show("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            MessageBox.Show("Bug");
                        }
                        catch { }
                    }
                }
                catch (AggregateException)
                {
                    MessageBox.Show("Expected");
                }
                catch (ObjectDisposedException)
                {
                    MessageBox.Show("Bug");
                }
                catch { }
                Thread.Sleep(2000);

                var t2 = Task.Run(() => START(token));
                await Task.WhenAny(new[] { t2 });
            }
            catch (AggregateException)
            {
                MessageBox.Show("Expected");
            }
            catch (ObjectDisposedException)
            {
                MessageBox.Show("Bug");
            }
            catch { }

        }

        private async Task START(CancellationToken token)
        {
            try
            {
                _Berserker = true;
                CompleteIteration = 1;
                Floor2 = 1;
                Floor3 = 1;
                searchSequence = 0;
                searchSequence2 = 0;
                fightSequence = 0;
                fightSequence2 = 0;
                walktopUTurn = 1;

                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);

                    for (int i = 0; i < 2; i++)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);                           
                                au3.Send("{G}");


                                Thread.Sleep(1000);
                            
                        }
                        catch (AggregateException)
                        {
                            MessageBox.Show("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            MessageBox.Show("Bug");
                        }
                        catch { }
                        ////////////////////////////////HIER FOLGT ENTER 2
                        try
                        {
                            au3.MouseClick("LEFT", 1467, 858, 1, 10);

                            Thread.Sleep(1000);
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                        /////////////// ACCEPT
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object walk = au3.PixelSearch(560, 260, 1382, 817, 0x21BD08, 10);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseClick("LEFT", 903, 605, 1, 5);
                            }


                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                    }
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Expected");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Bug");
                }
                catch { }
                Thread.Sleep(7000);

                var t3 = Task.Run(() => MOVE(token));
                await Task.WhenAny(new[] { t3 });
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        private async Task MOVE(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);

                    for (int i = 0; i < 2; i++)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot moves to start the Dungeon..."));

                                au3.MouseClick("" + txtLEFT.Text + "", 960, 529, 1);
                                Thread.Sleep(1000);
                            
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                                au3.MouseClick("" + txtLEFT.Text + "", 960, 529, 2);
  
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                            if (chBoxBerserker.Checked == true && _Berserker == true)
                            {
                                var sim = new InputSimulator();
                               
                                sim.Keyboard.KeyPress(VirtualKeyCode.VK_Y);
                                  
                               

                                _Berserker = false;
                            }
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                    }
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Expected");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Bug");
                }
                catch { }
                _Floor1 = true;
                var t12 = Task.Run(() => FLOORTIME(token));
                await Task.WhenAny(new[] { t12 });
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        private async Task SEARCHPORTAL(CancellationToken token)
        {
            try
            {

                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);


                    _Shadowhunter = true;
                    _Paladin = true;
                    _Berserker = true;
                    for (int i = 0; i <= 10; i++)
                    {
                        try
                        {
                            au3.Send("{G}");
                            au3.Send("{G}");

                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Search Portal..."));
                            // Tunable variables
                            float threshold = 0.7f; // set this higher for fewer false positives and lower for fewer false negatives
                            var enemyTemplate =
                                new Image<Bgr, byte>(resourceFolder + "/portalenter1.png"); // icon of the enemy
                            var enemyMask =
                                new Image<Bgr, byte>(resourceFolder + "/portalentermask1.png"); // make white what the important parts are, other parts should be black
                                                                                                //var screenCapture = new Image<Bgr, byte>("D:/Projects/bot-enemy-detection/EnemyDetection/screen.png");
                            Point myPosition = new Point(150, 128);
                            Point screenResolution = new Point(1920, 1080);

                            // Main program loop
                            var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);
                            var screenPrinter = new PrintScreen();

                            screenPrinter.CaptureScreenToFile("screen.png", ImageFormat.Png);
                            var screenCapture = new Image<Bgr, byte>("screen.png");
                            var enemy = enemyDetector.GetClosestEnemy(screenCapture);
                            if (enemy.HasValue)
                            {
                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 1: Portal found..."));
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(100, token);
                                CvInvoke.Rectangle(screenCapture,
                                    new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                                    new MCvScalar(255));

                                double x1 = 963f / myPosition.X;
                                double y1 = 551f / myPosition.Y;
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(100, token);
                                var x2 = x1 * enemy.Value.X;
                                var y2 = y1 * enemy.Value.Y;
                                if (x2 <= 963)
                                    x2 = x2 * 0.68f;
                                else
                                    x2 = x2 * 1.38f;
                                if (y2 <= 551)
                                    y2 = y2 * 0.68;
                                else
                                    y2 = y2 * 1.38;
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(100, token);
                                var absolutePositions = PixelToAbsolute(x2, y2, screenResolution);
                                inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 1: Enter Portal..."));

                                au3.Send("{G}");
                                if (txtLEFT.Text == "LEFT")
                                {
                                    inputSimulator.Mouse.LeftButtonClick();
                                }
                                else
                                {
                                    inputSimulator.Mouse.RightButtonClick();
                                }
                                au3.Send("{G}");



                                au3.Send("{G}");
                                if (txtLEFT.Text == "LEFT")
                                {
                                    inputSimulator.Mouse.LeftButtonClick();
                                }
                                else
                                {
                                    inputSimulator.Mouse.RightButtonClick();
                                }

                                au3.Send("{G}");

                                au3.Send("{G}");
                            }
                            else
                            {
                            }
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }

                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        Random random = new Random();
                        var sleepTime = random.Next(300, 500);
                        Thread.Sleep(sleepTime);
                        au3.Send("{G}");
                        au3.Send("{G}");
                    }
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Expected");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Bug");
                }
                catch { }
                searchSequence = 1;
                walktopUTurn = 0;
                _Floor2 = true;
                var t12 = Task.Run(() => SEARCHBOSS(token));
                await Task.WhenAny(new[] { t12 });
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }

        }

        private async Task SEARCHBOSS(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);
                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: search enemy..."));
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);

                    _Shadowhunter = true;
                    _Paladin = true;
                    _Berserker = true;
                    if (searchSequence == 1)
                    {
                        au3.MouseClick("" + txtLEFT.Text + "", 960, 529, 1);
                        au3.MouseClick("" + txtLEFT.Text + "", 960, 529, 2);
                        searchSequence++;
                    }

                    for (int i = 0; i < int.Parse(txtDungeon2search.Text); i++)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            float shardthreshold = 1f;
                            float threshold = 0.7f;
                            var shardTemplate =
                            new Image<Bgr, byte>(resourceFolder + "/shard.png");
                            var shardMask =
                            new Image<Bgr, byte>(resourceFolder + "/shardmask.png");
                            var enemyTemplate =
                            new Image<Bgr, byte>(resourceFolder + "/enemy.png");
                            var enemyMask =
                            new Image<Bgr, byte>(resourceFolder + "/mask.png");
                            var BossTemplate =
                            new Image<Bgr, byte>(resourceFolder + "/boss.png");
                            var BossMask =
                            new Image<Bgr, byte>(resourceFolder + "/bossmask.png");
                            var mobTemplate =
                            new Image<Bgr, byte>(resourceFolder + "/mob1.png");
                            var mobMask =
                            new Image<Bgr, byte>(resourceFolder + "/mobmask1.png");
                            var portalTemplate =
                            new Image<Bgr, byte>(resourceFolder + "/portalenter1.png");
                            var portalMask =
                            new Image<Bgr, byte>(resourceFolder + "/portalentermask1.png");

                            Point myPosition = new Point(150, 128);
                            Point screenResolution = new Point(1920, 1080);
                            var shardDetector = new EnemyDetector(shardTemplate, shardMask, shardthreshold);
                            var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);
                            var BossDetector = new EnemyDetector(BossTemplate, BossMask, threshold);
                            var mobDetector = new EnemyDetector(mobTemplate, mobMask, threshold);
                            var portalDetector = new EnemyDetector(portalTemplate, portalMask, threshold);
                            var screenPrinter = new PrintScreen();

                            screenPrinter.CaptureScreenToFile("screen.png", ImageFormat.Png);
                            var screenCapture = new Image<Bgr, byte>("screen.png");
                            var shard = shardDetector.GetClosestEnemy(screenCapture);
                            var enemy = enemyDetector.GetClosestEnemy(screenCapture);
                            var Boss = BossDetector.GetClosestEnemy(screenCapture);
                            var mob = mobDetector.GetClosestEnemy(screenCapture);
                            var portal = portalDetector.GetClosestEnemy(screenCapture);

                            if (shard.HasValue)
                            {
                                CvInvoke.Rectangle(screenCapture,
                                    new Rectangle(new Point(shard.Value.X, shard.Value.Y), shardTemplate.Size),
                                    new MCvScalar(255));
                                double x1 = 963f / myPosition.X;
                                double y1 = 551f / myPosition.Y;

                                var x2 = x1 * shard.Value.X;
                                var y2 = y1 * shard.Value.Y;
                                if (x2 <= 963)
                                    x2 = x2 * 0.9f;
                                else
                                    x2 = x2 * 1.1f;
                                if (y2 <= 551)
                                    y2 = y2 * 0.9;
                                else
                                    y2 = y2 * 1.1;
                                var absolutePositions = PixelToAbsolute(x2, y2, screenResolution);
                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 3: Shard found!"));
                                inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                if (txtLEFT.Text == "LEFT")
                                {
                                    inputSimulator.Mouse.LeftButtonClick();
                                }
                                else
                                {
                                    inputSimulator.Mouse.RightButtonClick();
                                }
                            }
                            else
                            {
                                if (Boss.HasValue)
                                {
                                    CvInvoke.Rectangle(screenCapture,
                                        new Rectangle(new Point(Boss.Value.X, Boss.Value.Y), BossTemplate.Size),
                                        new MCvScalar(255));
                                    double x1 = 963f / myPosition.X;
                                    double y1 = 551f / myPosition.Y;

                                    var x2 = x1 * Boss.Value.X;
                                    var y2 = y1 * Boss.Value.Y;
                                    if (x2 <= 963)
                                        x2 = x2 * 0.9f;
                                    else
                                        x2 = x2 * 1.1f;
                                    if (y2 <= 551)
                                        y2 = y2 * 0.9;
                                    else
                                        y2 = y2 * 1.1;
                                    var absolutePositions = PixelToAbsolute(x2, y2, screenResolution);
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: Big-Boss found!"));
                                    inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                    if (txtLEFT.Text == "LEFT")
                                    {
                                        inputSimulator.Mouse.LeftButtonClick();
                                    }
                                    else
                                    {
                                        inputSimulator.Mouse.RightButtonClick();
                                    }
                                }
                                else
                                {
                                    if (enemy.HasValue)
                                    {
                                        CvInvoke.Rectangle(screenCapture,
                                            new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                                            new MCvScalar(255));
                                        double x1 = 963f / myPosition.X;
                                        double y1 = 551f / myPosition.Y;

                                        var x2 = x1 * enemy.Value.X;
                                        var y2 = y1 * enemy.Value.Y;
                                        if (x2 <= 963)
                                            x2 = x2 * 0.9f;
                                        else
                                            x2 = x2 * 1.1f;
                                        if (y2 <= 551)
                                            y2 = y2 * 0.9;
                                        else
                                            y2 = y2 * 1.1;
                                        var absolutePositions = PixelToAbsolute(x2, y2, screenResolution);
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: Mid-Boss found!"));
                                        inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                        if (txtLEFT.Text == "LEFT")
                                        {
                                            inputSimulator.Mouse.LeftButtonClick();
                                        }
                                        else
                                        {
                                            inputSimulator.Mouse.RightButtonClick();
                                        }
                                    }
                                    else
                                    {
                                        if (mob.HasValue)
                                        {
                                            CvInvoke.Rectangle(screenCapture,
                                                new Rectangle(new Point(mob.Value.X, mob.Value.Y), mobTemplate.Size),
                                                new MCvScalar(255));
                                            double x1 = 963f / myPosition.X;
                                            double y1 = 551f / myPosition.Y;

                                            var x2 = x1 * mob.Value.X;
                                            var y2 = y1 * mob.Value.Y;
                                            if (x2 <= 963)
                                                x2 = x2 * 0.9f;
                                            else
                                                x2 = x2 * 1.1f;
                                            if (y2 <= 551)
                                                y2 = y2 * 0.9;
                                            else
                                                y2 = y2 * 1.1;
                                            var absolutePositions = PixelToAbsolute(x2, y2, screenResolution);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: Mob found!"));

                                            inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                            if (txtLEFT.Text == "LEFT")
                                            {
                                                inputSimulator.Mouse.LeftButtonClick();
                                            }
                                            else
                                            {
                                                inputSimulator.Mouse.RightButtonClick();
                                            }
                                        }
                                    }
                                }
                            }      
                                

                            Random random = new Random();
                            var sleepTime = random.Next(150, 255);
                            Thread.Sleep(sleepTime);
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                    }

                    if(Floor2 == 1)
                    { _Floor2 = true; }
                    
                    var t12 = Task.Run(() => FLOORTIME(token));
                    await Task.WhenAny(new[] { t12 });
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Expected");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Bug");
                }
                catch { }
                
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        private async Task FLOORTIME(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    if (_Floor1 == true)
                    {

                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        walktopUTurn = 0;
                        _Shadowhunter = true;
                        _Berserker = true;
                        _Paladin = true;
                        _Deathblade = true;
                        _Sharpshooter = true;
                        _Sorcerer = true;
                        _Soulfist = true;

                        _Floor1Fight = true;
                            var t12 = Task.Run(() => FLOORFIGHT(token));
                            await Task.Delay(int.Parse(txtDungeon.Text) * 1000);
                            

                            _Floor1Fight = false;

                        if (chBoxActivateF2.Checked)
                        {
                             
                            _Floor1 = false;
                            var t7 = Task.Run(() => SEARCHPORTAL(token));
                            await Task.WhenAny(new[] { t7 });
                        }
                        else
                        if (!chBoxActivateF2.Checked)
                        {
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "ChaosDungeon Floor 1 Complete!"));
                            _Floor1 = false;
                            var leave = Task.Run(() => LEAVEDUNGEON(token));
                            await Task.WhenAny(new[] { leave });
                        }
                            await Task.WhenAny(new[] { t12 });
                        }
                    catch (AggregateException)
                    {
                        Console.WriteLine("Expected");
                    }
                    catch (ObjectDisposedException)
                    {
                        Console.WriteLine("Bug");
                    }
                    catch { }

                }
                    if (_Floor2 == true)
                    {

                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            walktopUTurn2 = 0;
                            fightSequence++;
                            Search = false;
                            _Shadowhunter = true;
                            _Berserker = true;
                            _Paladin = true;
                            _Deathblade = true;
                            _Sharpshooter = true;
                            _Bard = true;
                            _Sorcerer = true;
                            _Soulfist = true;

                            _Floor2Fight = true;
                            var t14 = Task.Run(() => FLOORFIGHT(token));
                            await Task.Delay(int.Parse(txtDungeon2.Text) * 1000);

                            _Floor2Fight = false;

                            if (fightSequence == int.Parse(txtDungeon2Iteration.Text) && chBoxActivateF2.Checked == true && chBoxActivateF3.Checked == false)
                            {
                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "ChaosDungeon Floor 2 Complete!"));
                                Search = true;
                                var t12 = Task.Run(() => LEAVEDUNGEON(token));
                                await Task.WhenAny(new[] { t12 });
                            }
                            else

                            if (fightSequence >= int.Parse(txtDungeon2Iteration.Text) - 1 && chBoxActivateF3.Checked == true)
                            {
                                Search = true;
                                var t13 = Task.Run(() => FLOOR2PORTAL(token));
                                await Task.WhenAny(new[] { t13 });
                            }
                            else
                            if (fightSequence < int.Parse(txtDungeon2Iteration.Text))
                            {
                                 Search = true;
                                var t13 = Task.Run(() => SEARCHBOSS(token));
                                await Task.WhenAny(new[] { t13 });
                            }
                            await Task.WhenAny(new[] { t14 });
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }

                    }
                    if (_Floor3 == true)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        CompleteIteration = 1;
                        fightSequence2++;
                        _Shadowhunter = true;
                        _Berserker = true;
                        _Paladin = true;
                        _Deathblade = true;
                        _Sharpshooter = true;
                        _Bard = true;
                        _Sorcerer = true;
                        _Soulfist = true;

                        _Floor3Fight = true;
                            var t14 = Task.Run(() => FLOORFIGHT(token));
                            await Task.Delay(int.Parse(txtDungeon3.Text) * 1000);

                        _Floor3Fight = false;

                        if (fightSequence2 == int.Parse(txtDungeon3Iteration.Text))
                        {

                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Leaved ChaosDungeon - not completed!"));
                            var t12 = Task.Run(() => LEAVEDUNGEON(token));
                            await Task.WhenAny(new[] { t12 });
                        }
                        else
                        if (fightSequence2 < int.Parse(txtDungeon3Iteration.Text))
                        {
                            searchSequence2 = 1;
                            var t13 = Task.Run(() => SEARCHBOSS2(token));
                            await Task.WhenAny(new[] { t13 });
                        }
                            await Task.WhenAny(new[] { t14 });

                        }
                    catch (AggregateException)
                    {
                        Console.WriteLine("Expected");
                    }
                    catch (ObjectDisposedException)
                    {
                        Console.WriteLine("Bug");
                    }
                    catch { }
                }
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Expected");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Bug");
                }
                catch { }
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        private async Task FLOORFIGHT(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);
                try
                {
                    while (_Floor1Fight == true)
                    {

                        foreach (KeyValuePair<VirtualKeyCode, int> skill in SKILLS.skillset.OrderBy(x => x.Value))
                        {
                            try
                            {

                                token.ThrowIfCancellationRequested();
                                await Task.Delay(100, token);

                                object fight = au3.PixelSearch(600, 250, 1319, 843, 0xDD2C02, 10);
                                if (fight.ToString() != "1")
                                {
                                    object[] fightCoord = (object[])fight;
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is fighting..."));
                                    au3.MouseMove((int)fightCoord[0], (int)fightCoord[1] + 80);
                                    var sim = new InputSimulator();
                                    for (int t = 0; t < 10; t++) // TEXTBOX MUSS CUSTOM SEIN
                                    { 
                                        sim.Keyboard.KeyDown(skill.Key);
                                        await Task.Delay(10);
                                    }
                                    sim.Keyboard.KeyUp(skill.Key);
                                    sim.Keyboard.KeyPress(skill.Key);
                                    if (chBoxDoubleQ.Checked || chBoxDoubleW.Checked || chBoxDoubleE.Checked || chBoxDoubleR.Checked || chBoxDoubleA.Checked || chBoxDoubleS.Checked || chBoxDoubleD.Checked || chBoxDoubleF.Checked)
                                    {
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Key Pressed twice!"));
                                        sim.Keyboard.KeyPress(skill.Key);
                                        sim.Keyboard.KeyPress(skill.Key);
                                        sim.Keyboard.KeyPress(skill.Key);
                                    }
                                    setKeyCooldown(skill.Key); // Set Cooldown
                                    var td = Task.Run(() => SkillCooldown(token, skill.Key));
                                    au3.MouseMove((int)fightCoord[0], (int)fightCoord[1] + 80);
                                    fightOnSecondAbility++;
                                    if (isKeyOnCooldown(skill.Key) == false)
                                    {
                                        try
                                        {
                                            token.ThrowIfCancellationRequested();
                                            await Task.Delay(100, token);
                                            walktopUTurn++;
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            fightOnSecondAbility = 1;
                                        }
                                        catch (AggregateException)
                                        {
                                            Console.WriteLine("Expected");
                                        }
                                        catch (ObjectDisposedException)
                                        {
                                            Console.WriteLine("Bug");
                                        }
                                        catch { }

                                    }
                                }
                               
                                    if (walktopUTurn == 6 && chBoxAutoMovement.Checked)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);
                                        for (int t = 0; t < 40; t++)
                                        {
                                            au3.MouseClick("LEFT", 960, 240, 1, 10);
                                            await Task.Delay(10);
                                        }
                                        for (int t = 0; t < 0.5; t++)
                                        {
                                            au3.MouseClick("LEFT", 960, 566, 1, 10);
                                            await Task.Delay(10);

                                        }
                                        walktopUTurn++;
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                if (walktopUTurn == 10)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);
                                        for (int t = 0; t < 80; t++)
                                        {
                                            au3.MouseClick("LEFT", 523, 800, 1, 10);
                                            await Task.Delay(10);
                                        }
                                        for (int t = 0; t < 0.5; t++)
                                        {
                                            au3.MouseClick("LEFT", 1007, 494, 1, 10);
                                            await Task.Delay(10);
                                        }
                                        walktopUTurn++;
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                if (walktopUTurn == 14)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);
                                        for (int t = 0; t < 90; t++)
                                        {
                                            au3.MouseClick("LEFT", 1578, 524, 1, 10);
                                            await Task.Delay(10);
                                        }
                                        for (int t = 0; t < 0.5; t++)
                                        {
                                            au3.MouseClick("LEFT", 905, 531, 1, 10);
                                            await Task.Delay(10);
                                        }
                                        walktopUTurn++;
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                if (walktopUTurn == 18)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);
                                        for (int t = 0; t < 70; t++)
                                        {
                                            au3.MouseClick("LEFT", 523, 810, 1, 10);
                                            await Task.Delay(10);
                                        }
                                        for (int t = 0; t < 0.5; t++)
                                        {
                                            au3.MouseClick("LEFT", 960, 500, 1, 10);
                                            await Task.Delay(10);
                                        }
                                        walktopUTurn++;
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                if (walktopUTurn == 18)
                                {
                                    walktopUTurn = 0;
                                    await Task.Delay(10);
                                }

                                
                            }
                            catch (AggregateException)
                            {
                                Console.WriteLine("Expected");
                            }
                            catch (ObjectDisposedException)
                            {
                                Console.WriteLine("Bug");
                            }
                            catch { }
                            ///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE
                            try
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(100, token);


                                if (chBoxPaladin.Checked == true && _Paladin == true)
                                { 
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);
                                        object d = au3.PixelSearch(892, 1027, 934, 1060, 0x75D6FF, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            var sim = new InputSimulator();
                                            for (int t = 0; t < 50; t++)
                                            {
                                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                                await Task.Delay(1);
                                            }
                                            _Paladin = false;
                                            sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Paladin Ultimate"));
                                        }
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                else
                               if (chBoxDeathblade.Checked == true && _Deathblade == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);
                                        object d = au3.PixelSearch(986, 1029, 1017, 1035, 0xDAE7F3, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            var sim = new InputSimulator();
                                            for (int t = 0; t < 50; t++)
                                            {
                                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                                await Task.Delay(1);
                                            }
                                            _Deathblade = false;
                                            sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);
                                            sim.Keyboard.KeyPress(VirtualKeyCode.VK_Y);
                                            sim.Keyboard.KeyPress(VirtualKeyCode.VK_Y);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Deathblade Ultimate"));
                                        }
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                else
                               if (chBoxSharpshooter.Checked == true && _Sharpshooter == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);
                                        object d = au3.PixelSearch(1006, 1049, 1019, 1068, 0x09B4EB, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            var sim = new InputSimulator();
                                            for (int t = 0; t < 50; t++)
                                            {
                                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                                await Task.Delay(1);
                                            }
                                            _Sharpshooter = false;
                                            sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);


                                            var Sharpshooter = Task.Run(() => SharpshooterSecondPress(token));

                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Sharpshooter Ultimate"));
                                        }
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                else
                               if (chBoxSorcerer.Checked == true && _Sorcerer == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);
                                        object d = au3.PixelSearch(1006, 1038, 1010, 1042, 0x8993FF, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            var sim = new InputSimulator();
                                            for (int t = 0; t < 50; t++)
                                            {
                                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                                await Task.Delay(1);
                                            }
                                            _Sorcerer = false;
                                            sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Sorcerer Ultimate"));
                                        }
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                else
                               if (chBoxSoulfist.Checked == true && _Soulfist == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);

                                        var sim = new InputSimulator();
                                        for (int t = 0; t < 50; t++)
                                        {
                                            sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                            await Task.Delay(1);
                                        }
                                        _Soulfist = false;
                                        sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Soulfist Ultimate"));

                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                //////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(100, token);
                                    object health = au3.PixelSearch(633, 962, 651, 969, 0x050405, 15);
                                    if (health.ToString() != "1" && checkBoxHeal10.Checked)
                                    {
                                        object[] healthCoord = (object[])health;
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 10%"));
                                    }
                                }
                                catch (AggregateException)
                                {
                                    Console.WriteLine("Expected");
                                }
                                catch (ObjectDisposedException)
                                {
                                    Console.WriteLine("Bug");
                                }
                                catch { }
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(100, token);
                                    object health = au3.PixelSearch(633, 962, 820, 970, 0x050405, 15);

                                    if (health.ToString() != "1" && checkBoxHeal70.Checked)
                                    {


                                        object[] healthCoord = (object[])health;
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 70%"));
                                    }
                                }
                                catch (AggregateException)
                                {
                                    Console.WriteLine("Expected");
                                }
                                catch (ObjectDisposedException)
                                {
                                    Console.WriteLine("Bug");
                                }
                                catch { }
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(100, token);
                                    object healthi = au3.PixelSearch(633, 962, 686, 969, 0x050405, 15);

                                    if (healthi.ToString() != "1" && checkBoxHeal30.Checked)
                                    {


                                        object[] healthiCoord = (object[])healthi;
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 30%"));
                                    }
                                }

                                catch (AggregateException)
                                {
                                    Console.WriteLine("Expected");
                                }
                                catch (ObjectDisposedException)
                                {
                                    Console.WriteLine("Bug");
                                }
                                catch { }

                            }
                            catch (AggregateException)
                            {
                                Console.WriteLine("Expected");
                            }
                            catch (ObjectDisposedException)
                            {
                                Console.WriteLine("Bug");
                            }
                            catch { }
                        }
                    }
                    while (_Floor2Fight == true)
                    {

                        foreach (KeyValuePair<VirtualKeyCode, int> skill in SKILLS.skillset.OrderBy(x => x.Value))
                        {
                            try
                            {

                                token.ThrowIfCancellationRequested();
                                await Task.Delay(100, token);

                                object fight = au3.PixelSearch(600, 250, 1319, 843, 0xDD2C02, 10);
                                if (fight.ToString() != "1")
                                {
                                    object[] fightCoord = (object[])fight;
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is fighting..."));
                                    au3.MouseMove((int)fightCoord[0], (int)fightCoord[1] + 80);
                                    var sim = new InputSimulator();
                                    for (int t = 0; t < int.Parse(txD.Text) / 10; t++) // TEXTBOX MUSS CUSTOM SEIN
                                    {
                                        sim.Keyboard.KeyDown(skill.Key);
                                        await Task.Delay(10);
                                    }
                                    sim.Keyboard.KeyUp(skill.Key);
                                    sim.Keyboard.KeyPress(skill.Key);
                                    if (chBoxDoubleQ.Checked || chBoxDoubleW.Checked || chBoxDoubleE.Checked || chBoxDoubleR.Checked || chBoxDoubleA.Checked || chBoxDoubleS.Checked || chBoxDoubleD.Checked || chBoxDoubleF.Checked)
                                    {
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Key Pressed twice!"));
                                        sim.Keyboard.KeyPress(skill.Key);
                                        sim.Keyboard.KeyPress(skill.Key);
                                        sim.Keyboard.KeyPress(skill.Key);
                                    }
                                    setKeyCooldown(skill.Key); // Set Cooldown
                                    var td = Task.Run(() => SkillCooldown(token, skill.Key));
                                    au3.MouseMove((int)fightCoord[0], (int)fightCoord[1] + 80);
                                    fightOnSecondAbility++;
                                    if (isKeyOnCooldown(skill.Key) == false && Search == false)
                                    {
                                        try
                                        {
                                            token.ThrowIfCancellationRequested();
                                            await Task.Delay(100, token);
                                            fightOnSecondAbility++;
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                           
                                        }
                                        catch (AggregateException)
                                        {
                                            Console.WriteLine("Expected");
                                        }
                                        catch (ObjectDisposedException)
                                        {
                                            Console.WriteLine("Bug");
                                        }
                                        catch { }
                                    }
                                    if (fightOnSecondAbility == 8 && Search == false && chBoxAutoMovement.Checked)
                                    { 
                                        try
                                        {
                                            token.ThrowIfCancellationRequested();
                                            await Task.Delay(100, token);
                                            for (int t = 0; t < 40; t++)
                                            {

                                                au3.MouseClick("LEFT", 960, 240, 1, 10);
                                                await Task.Delay(10);

                                            }
                                            for (int t = 0; t < 80; t++)
                                            {

                                                au3.MouseClick("LEFT", 960, 566, 1, 10);
                                                await Task.Delay(10);

                                            }
                                            for (int t = 0; t < 1; t++)
                                            {

                                                au3.MouseClick("LEFT", 960, 500, 1, 10);
                                                await Task.Delay(10);

                                            }
                                            
                                        }
                                        catch (AggregateException)
                                        {
                                            Console.WriteLine("Expected");
                                        }
                                        catch (ObjectDisposedException)
                                        {
                                            Console.WriteLine("Bug");
                                        }
                                        catch { }
                                        fightOnSecondAbility = 1;
                                    }
                                }
                               
                                
                               
                                
                            }
                            catch (AggregateException)
                            {
                                Console.WriteLine("Expected");
                            }
                            catch (ObjectDisposedException)
                            {
                                Console.WriteLine("Bug");
                            }
                            catch { }
                            ///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE
                            try
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(100, token);

                                if (chBoxBard.Checked == true && _Bard == true)
                                {
                                    try
                                    {

                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);

                                        var sim = new InputSimulator();
                                        sim.Keyboard.KeyPress(VirtualKeyCode.VK_Y);
                                        await Task.Delay(1);

                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Bard try to heal..."));


                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                else
                                if (chBoxY.Checked == true && _Shadowhunter == true)
                                {
                                    try
                                    {

                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);

                                        object d = au3.PixelSearch(948, 969, 968, 979, 0xBC08F0, 5);

                                        if (d.ToString() != "1")
                                        {

                                            object[] dCoord = (object[])d;
                                            var sim = new InputSimulator();
                                            for (int t = 0; t < 50; t++)
                                            {
                                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                                await Task.Delay(1);
                                            }
                                            _Shadowhunter = false;
                                            sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Shadowhunter Ultimate"));

                                        }
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                else
                                 if (chBoxPaladin.Checked == true && _Paladin == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);
                                        object d = au3.PixelSearch(892, 1027, 934, 1060, 0x75D6FF, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            var sim = new InputSimulator();
                                            for (int t = 0; t < 50; t++)
                                            {
                                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                                await Task.Delay(1);
                                            }
                                            _Paladin = false;
                                            sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Paladin Ultimate"));
                                        }
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                else
                                if (chBoxDeathblade.Checked == true && _Deathblade == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);
                                        object d = au3.PixelSearch(986, 1029, 1017, 1035, 0xDAE7F3, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            var sim = new InputSimulator();
                                            for (int t = 0; t < 50; t++)
                                            {
                                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                                await Task.Delay(1);
                                            }
                                            _Deathblade = false;
                                            sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);
                                            sim.Keyboard.KeyPress(VirtualKeyCode.VK_Y);
                                            sim.Keyboard.KeyPress(VirtualKeyCode.VK_Y);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Deathblade Ultimate"));
                                        }
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                else
                                if (chBoxSharpshooter.Checked == true && _Sharpshooter == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);
                                        object d = au3.PixelSearch(1006, 1049, 1019, 1068, 0x09B4EB, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            var sim = new InputSimulator();
                                            for (int t = 0; t < 50; t++)
                                            {
                                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                                await Task.Delay(1);
                                            }
                                            _Sharpshooter = false;
                                            sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);


                                            var Sharpshooter = Task.Run(() => SharpshooterSecondPress(token));

                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Sharpshooter Ultimate"));
                                        }
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                else
                                if (chBoxSorcerer.Checked == true && _Sorcerer == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);
                                        object d = au3.PixelSearch(1006, 1038, 1010, 1042, 0x8993FF, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            var sim = new InputSimulator();
                                            for (int t = 0; t < 50; t++)
                                            {
                                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                                await Task.Delay(1);
                                            }
                                            _Sorcerer = false;
                                            sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Sorcerer Ultimate"));
                                        }
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                else
                                if (chBoxSoulfist.Checked == true && _Soulfist == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);

                                        var sim = new InputSimulator();
                                        for (int t = 0; t < 50; t++)
                                        {
                                            sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                            await Task.Delay(1);
                                        }
                                        _Soulfist = false;
                                        sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Soulfist Ultimate"));

                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                //////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(100, token);
                                    object health = au3.PixelSearch(633, 962, 651, 969, 0x050405, 15);
                                    if (health.ToString() != "1" && checkBoxHeal10.Checked)
                                    {
                                        object[] healthCoord = (object[])health;
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 10%"));
                                    }
                                }
                                catch (AggregateException)
                                {
                                    Console.WriteLine("Expected");
                                }
                                catch (ObjectDisposedException)
                                {
                                    Console.WriteLine("Bug");
                                }
                                catch { }
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(100, token);
                                    object health = au3.PixelSearch(633, 962, 820, 970, 0x050405, 15);

                                    if (health.ToString() != "1" && checkBoxHeal70.Checked)
                                    {


                                        object[] healthCoord = (object[])health;
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 70%"));
                                    }
                                }
                                catch (AggregateException)
                                {
                                    Console.WriteLine("Expected");
                                }
                                catch (ObjectDisposedException)
                                {
                                    Console.WriteLine("Bug");
                                }
                                catch { }
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(100, token);
                                    object healthi = au3.PixelSearch(633, 962, 686, 969, 0x050405, 15);

                                    if (healthi.ToString() != "1" && checkBoxHeal30.Checked)
                                    {


                                        object[] healthiCoord = (object[])healthi;
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 30%"));
                                    }
                                }

                                catch (AggregateException)
                                {
                                    Console.WriteLine("Expected");
                                }
                                catch (ObjectDisposedException)
                                {
                                    Console.WriteLine("Bug");
                                }
                                catch { }

                            }
                            catch (AggregateException)
                            {
                                Console.WriteLine("Expected");
                            }
                            catch (ObjectDisposedException)
                            {
                                Console.WriteLine("Bug");
                            }
                            catch { }
                        }
                    }
                    while (_Floor3Fight == true)
                    {
                        foreach (KeyValuePair<VirtualKeyCode, int> skill in SKILLS.skillset.OrderBy(x => x.Value))
                        {
                            try
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(100, token);
                                object shardHit = au3.PixelSearch(600, 250, 1319, 843, 0x630E17, 10);
                                object fight = au3.PixelSearch(600, 250, 1319, 843, 0xDD2C02, 10);
                                if (fight.ToString() != "1" && shardHit.ToString() != "1" && isKeyOnCooldown(skill.Key) == true)
                                {
                                    object[] shardHitCoord = (object[])shardHit;
                                    object[] fightCoord = (object[])fight;
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is fighting..."));
                                    au3.MouseMove((int)shardHitCoord[0], (int)shardHitCoord[1] + 80);
                                    au3.MouseMove((int)fightCoord[0], (int)fightCoord[1] + 80);
                                    var sim = new InputSimulator();
                                    for (int t = 0; t < int.Parse(txD.Text) / 10; t++) // TEXTBOX MUSS CUSTOM SEIN
                                    {
                                        sim.Keyboard.KeyDown(skill.Key);
                                        await Task.Delay(10);
                                    }
                                    sim.Keyboard.KeyUp(skill.Key);
                                    sim.Keyboard.KeyPress(skill.Key);
                                    if (chBoxDoubleQ.Checked || chBoxDoubleW.Checked || chBoxDoubleE.Checked || chBoxDoubleR.Checked || chBoxDoubleA.Checked || chBoxDoubleS.Checked || chBoxDoubleD.Checked || chBoxDoubleF.Checked)
                                    {
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Key Pressed twice!"));
                                        sim.Keyboard.KeyPress(skill.Key);
                                        sim.Keyboard.KeyPress(skill.Key);
                                        sim.Keyboard.KeyPress(skill.Key);
                                    }
                                    setKeyCooldown(skill.Key); // Set Cooldown
                                    var td = Task.Run(() => SkillCooldown(token, skill.Key));
                                    au3.MouseMove((int)shardHitCoord[0], (int)shardHitCoord[1] + 80);
                                    au3.MouseMove((int)fightCoord[0], (int)fightCoord[1] + 80);
                                    fightOnSecondAbility++;
                                    if (fightOnSecondAbility == 4)
                                    {
                                        try
                                        {
                                            token.ThrowIfCancellationRequested();
                                            await Task.Delay(100, token);
                                            walktopUTurn++;
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");
                                            au3.Send("{C}");

                                            fightOnSecondAbility = 1;
                                        }
                                        catch (AggregateException)
                                        {
                                            Console.WriteLine("Expected");
                                        }
                                        catch (ObjectDisposedException)
                                        {
                                            Console.WriteLine("Bug");
                                        }
                                        catch { }
                                    }
                                }
                            }
                            catch (AggregateException)
                            {
                                Console.WriteLine("Expected");
                            }
                            catch (ObjectDisposedException)
                            {
                                Console.WriteLine("Bug");
                            }
                            catch { }
                            ///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE///////////ULTIMATE
                            try
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(100, token);

                                if (chBoxBard.Checked == true && _Bard == true)
                                {
                                    try
                                    {

                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);

                                        var sim = new InputSimulator();
                                        sim.Keyboard.KeyPress(VirtualKeyCode.VK_Y);
                                        await Task.Delay(1);

                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Bard try to heal..."));


                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                else
                                if (chBoxY.Checked == true && _Shadowhunter == true)
                                {
                                    try
                                    {

                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);

                                        object d = au3.PixelSearch(948, 969, 968, 979, 0xBC08F0, 5);

                                        if (d.ToString() != "1")
                                        {

                                            object[] dCoord = (object[])d;
                                            var sim = new InputSimulator();
                                            for (int t = 0; t < 50; t++)
                                            {
                                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                                await Task.Delay(1);
                                            }
                                            _Shadowhunter = false;
                                            sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Shadowhunter Ultimate"));

                                        }
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                else
                                 if (chBoxPaladin.Checked == true && _Paladin == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);
                                        object d = au3.PixelSearch(892, 1027, 934, 1060, 0x75D6FF, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            var sim = new InputSimulator();
                                            for (int t = 0; t < 50; t++)
                                            {
                                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                                await Task.Delay(1);
                                            }
                                            _Paladin = false;
                                            sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Paladin Ultimate"));
                                        }
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                else
                                if (chBoxDeathblade.Checked == true && _Deathblade == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);
                                        object d = au3.PixelSearch(986, 1029, 1017, 1035, 0xDAE7F3, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            var sim = new InputSimulator();
                                            for (int t = 0; t < 50; t++)
                                            {
                                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                                await Task.Delay(1);
                                            }
                                            _Deathblade = false;
                                            sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);
                                            sim.Keyboard.KeyPress(VirtualKeyCode.VK_Y);
                                            sim.Keyboard.KeyPress(VirtualKeyCode.VK_Y);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Deathblade Ultimate"));
                                        }
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                else
                                if (chBoxSharpshooter.Checked == true && _Sharpshooter == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);
                                        object d = au3.PixelSearch(1006, 1049, 1019, 1068, 0x09B4EB, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            var sim = new InputSimulator();
                                            for (int t = 0; t < 50; t++)
                                            {
                                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                                await Task.Delay(1);
                                            }
                                            _Sharpshooter = false;
                                            sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);


                                            var Sharpshooter = Task.Run(() => SharpshooterSecondPress(token));

                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Sharpshooter Ultimate"));
                                        }
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                else
                                if (chBoxSorcerer.Checked == true && _Sorcerer == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);
                                        object d = au3.PixelSearch(1006, 1038, 1010, 1042, 0x8993FF, 10);
                                        if (d.ToString() != "1")
                                        {
                                            object[] dCoord = (object[])d;
                                            var sim = new InputSimulator();
                                            for (int t = 0; t < 50; t++)
                                            {
                                                sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                                await Task.Delay(1);
                                            }
                                            _Sorcerer = false;
                                            sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Sorcerer Ultimate"));
                                        }
                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                else
                                if (chBoxSoulfist.Checked == true && _Soulfist == true)
                                {
                                    try
                                    {
                                        token.ThrowIfCancellationRequested();
                                        await Task.Delay(100, token);

                                        var sim = new InputSimulator();
                                        for (int t = 0; t < 50; t++)
                                        {
                                            sim.Keyboard.KeyDown(VirtualKeyCode.VK_Y);
                                            await Task.Delay(1);
                                        }
                                        _Soulfist = false;
                                        sim.Keyboard.KeyUp(VirtualKeyCode.VK_Y);
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Soulfist Ultimate"));

                                    }
                                    catch (AggregateException)
                                    {
                                        Console.WriteLine("Expected");
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        Console.WriteLine("Bug");
                                    }
                                    catch { }
                                }
                                //////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION//////////POTION
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(100, token);
                                    object health = au3.PixelSearch(633, 962, 651, 969, 0x050405, 15);
                                    if (health.ToString() != "1" && checkBoxHeal10.Checked)
                                    {
                                        object[] healthCoord = (object[])health;
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        au3.Send("{" + txtHeal10.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 10%"));
                                    }
                                }
                                catch (AggregateException)
                                {
                                    Console.WriteLine("Expected");
                                }
                                catch (ObjectDisposedException)
                                {
                                    Console.WriteLine("Bug");
                                }
                                catch { }
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(100, token);
                                    object health = au3.PixelSearch(633, 962, 820, 970, 0x050405, 15);

                                    if (health.ToString() != "1" && checkBoxHeal70.Checked)
                                    {
                                        object[] healthCoord = (object[])health;
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        au3.Send("{" + txtHeal70.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 70%"));
                                    }
                                }
                                catch (AggregateException)
                                {
                                    Console.WriteLine("Expected");
                                }
                                catch (ObjectDisposedException)
                                {
                                    Console.WriteLine("Bug");
                                }
                                catch { }
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(100, token);
                                    object healthi = au3.PixelSearch(633, 962, 686, 969, 0x050405, 15);

                                    if (healthi.ToString() != "1" && checkBoxHeal30.Checked)
                                    {
                                        object[] healthiCoord = (object[])healthi;
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        au3.Send("{" + txtHeal30.Text + "}");
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Heal-Potion at 30%"));
                                    }
                                }
                                catch (AggregateException)
                                {
                                    Console.WriteLine("Expected");
                                }
                                catch (ObjectDisposedException)
                                {
                                    Console.WriteLine("Bug");
                                }
                                catch { }
                            }
                            catch (AggregateException)
                            {
                                Console.WriteLine("Expected");
                            }
                            catch (ObjectDisposedException)
                            {
                                Console.WriteLine("Bug");
                            }
                            catch { }
                        }
                    }
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Expected");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Bug");
                }
                catch { }

            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        private void setKeyCooldown(VirtualKeyCode key)
        {
            switch (key)
            {
                case VirtualKeyCode.VK_A:
                    _A = false;
                    break;
                case VirtualKeyCode.VK_S:
                    _S = false;
                    break;
                case VirtualKeyCode.VK_D:
                    _D = false;
                    break;
                case VirtualKeyCode.VK_F:
                    _F = false;
                    break;
                case VirtualKeyCode.VK_Q:
                    _Q = false;
                    break;
                case VirtualKeyCode.VK_W:
                    _W = false;
                    break;
                case VirtualKeyCode.VK_E:
                    _E = false;
                    break;
                case VirtualKeyCode.VK_R:
                    _R = false;
                    break;
            }
        }
        private bool isKeyOnCooldown(VirtualKeyCode key)
        {
            bool returnBoolean = false;
            switch (key)
            {
                case VirtualKeyCode.VK_A:
                    returnBoolean = _A;
                    break;
                case VirtualKeyCode.VK_S:
                    returnBoolean = _S;
                    break;
                case VirtualKeyCode.VK_D:
                    returnBoolean = _D;
                    break;
                case VirtualKeyCode.VK_F:
                    returnBoolean = _F;
                    break;
                case VirtualKeyCode.VK_Q:
                    returnBoolean = _Q;
                    break;
                case VirtualKeyCode.VK_W:
                    returnBoolean = _W;
                    break;
                case VirtualKeyCode.VK_E:
                    returnBoolean = _E;
                    break;
                case VirtualKeyCode.VK_R:
                    returnBoolean = _R;
                    break;
            }
            return returnBoolean;
        }

        private async Task SEARCHBOSS2(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);
                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 3: search enemy..."));
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);

                    _Shadowhunter = true;
                    _Paladin = true;
                    _Berserker = true;
                    if (searchSequence2 == 1)
                    {
                        au3.MouseClick("" + txtLEFT.Text + "", 960, 529, 1);
                        au3.MouseClick("" + txtLEFT.Text + "", 960, 529, 2);
                        searchSequence2++;
                    }

                    for (int i = 0; i < int.Parse(txtDungeon3search.Text); i++)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                       
                            float threshold = 0.7f;
                            var shardTemplate =
                            new Image<Bgr, byte>(resourceFolder + "/shard.png");
                            var shardMask =
                            new Image<Bgr, byte>(resourceFolder + "/shardmask.png");
                            var enemyTemplate =
                            new Image<Bgr, byte>(resourceFolder + "/enemy.png");
                            var enemyMask =
                            new Image<Bgr, byte>(resourceFolder + "/mask.png");
                            var BossTemplate =
                            new Image<Bgr, byte>(resourceFolder + "/boss.png");
                            var BossMask =
                            new Image<Bgr, byte>(resourceFolder + "/bossmask.png");
                            var mobTemplate =
                            new Image<Bgr, byte>(resourceFolder + "/mob1.png");
                            var mobMask =
                            new Image<Bgr, byte>(resourceFolder + "/mobmask1.png");

                            Point myPosition = new Point(150, 128);
                            Point screenResolution = new Point(1920, 1080);
                            var shardDetector = new EnemyDetector(shardTemplate, shardMask, threshold);
                            var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);
                            var BossDetector = new EnemyDetector(BossTemplate, BossMask, threshold);
                            var mobDetector = new EnemyDetector(mobTemplate, mobMask, threshold);

                            var screenPrinter = new PrintScreen();

                            screenPrinter.CaptureScreenToFile("screen.png", ImageFormat.Png);
                            var screenCapture = new Image<Bgr, byte>("screen.png");
                            var shard = shardDetector.GetClosestEnemy(screenCapture);
                            var enemy = enemyDetector.GetClosestEnemy(screenCapture);
                            var Boss = BossDetector.GetClosestEnemy(screenCapture);
                            var mob = mobDetector.GetClosestEnemy(screenCapture);

                            if (CompleteIteration == 1)
                            {
                                
                                au3.MouseClick("LEFT", 963, 961, 3, 10);
                                CompleteIteration++;
                            }
                            else
                            {
                                if (shard.HasValue)
                                {
                                    CvInvoke.Rectangle(screenCapture,
                                        new Rectangle(new Point(shard.Value.X, shard.Value.Y), shardTemplate.Size),
                                        new MCvScalar(255));
                                    double x1 = 963f / myPosition.X;
                                    double y1 = 551f / myPosition.Y;

                                    var x2 = x1 * shard.Value.X;
                                    var y2 = y1 * shard.Value.Y;
                                    if (x2 <= 963)
                                        x2 = x2 * 0.8f;
                                    else
                                        x2 = x2 * 1.2f;
                                    if (y2 <= 551)
                                        y2 = y2 * 0.8;
                                    else
                                        y2 = y2 * 1.2;
                                    var absolutePositions = PixelToAbsolute(x2, y2, screenResolution);
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 3: Shard found!"));
                                    inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                    if (txtLEFT.Text == "LEFT")
                                    {
                                        inputSimulator.Mouse.LeftButtonClick();
                                    }
                                    else
                                    {
                                        inputSimulator.Mouse.RightButtonClick();
                                    }
                                }
                                else
                                {
                                    if (enemy.HasValue)
                                    {
                                        CvInvoke.Rectangle(screenCapture,
                                            new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                                            new MCvScalar(255));
                                        double x1 = 963f / myPosition.X;
                                        double y1 = 551f / myPosition.Y;

                                        var x2 = x1 * enemy.Value.X;
                                        var y2 = y1 * enemy.Value.Y;
                                        if (x2 <= 963)
                                            x2 = x2 * 0.9f;
                                        else
                                            x2 = x2 * 1.1f;
                                        if (y2 <= 551)
                                            y2 = y2 * 0.9;
                                        else
                                            y2 = y2 * 1.1;
                                        var absolutePositions = PixelToAbsolute(x2, y2, screenResolution);
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 3: Mid-Boss found!"));
                                        inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                        if (txtLEFT.Text == "LEFT")
                                        {
                                            inputSimulator.Mouse.LeftButtonClick();
                                        }
                                        else
                                        {
                                            inputSimulator.Mouse.RightButtonClick();
                                        }
                                    }
                                    else
                                    {
                                        if (mob.HasValue)
                                        {
                                            CvInvoke.Rectangle(screenCapture,
                                                new Rectangle(new Point(mob.Value.X, mob.Value.Y), mobTemplate.Size),
                                                new MCvScalar(255));
                                            double x1 = 963f / myPosition.X;
                                            double y1 = 551f / myPosition.Y;

                                            var x2 = x1 * mob.Value.X;
                                            var y2 = y1 * mob.Value.Y;
                                            if (x2 <= 963)
                                                x2 = x2 * 0.9f;
                                            else
                                                x2 = x2 * 1.1f;
                                            if (y2 <= 551)
                                                y2 = y2 * 0.9;
                                            else
                                                y2 = y2 * 1.1;
                                            var absolutePositions = PixelToAbsolute(x2, y2, screenResolution);
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 3: Mob found!"));

                                            inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                            if (txtLEFT.Text == "LEFT")
                                            {
                                                inputSimulator.Mouse.LeftButtonClick();
                                            }
                                            else
                                            {
                                                inputSimulator.Mouse.RightButtonClick();
                                            }
                                        }

                                    }

                                }
                            }
                            

                            Random random = new Random();
                            var sleepTime = random.Next(150, 255);
                            Thread.Sleep(sleepTime);
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                        
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object walk = au3.PixelSearch(560, 260, 1382, 817, 0x21BD08, 10);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseClick("LEFT", 903, 605, 1, 5);

                                try
                            {


                                token.ThrowIfCancellationRequested();
                                await Task.Delay(100, token);
                                object complete = au3.PixelSearch(31, 97, 81, 108, 0x8A412C, 5);
                                if (complete.ToString() != "1")
                                {
                                    object[] completeCoord = (object[])complete;
                                    au3.MouseClick("LEFT", 191, 285, 1, 5);
                                
                                    await Task.Delay(1000);


                                
                                }
                                Thread.Sleep(2000);
                                if (_REPAIR == true)
                                {
                                    Thread.Sleep(2000);
                                    var t7 = Task.Run(() => REPAIR(token));
                                    await Task.WhenAny(new[] { t7 });
                                }
                                else
                                if (_LOGOUT == true)
                                {
                                    var t11 = Task.Run(() => LOGOUT(token));
                                    await Task.WhenAny(new[] { t11 });
                                }
                                else
                                if (_REPAIR == false && _LOGOUT == false)
                                {
                                    await Task.Delay(2000);
                                    var t9 = Task.Run(() => RESTART(token));
                                    await Task.WhenAny(new[] { t9 });
                                }
                            }
                            catch { }
                            }
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                        
                    }
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Expected");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Bug");
                }
                catch { }
                if(Floor3 == 1)
                { _Floor3 = true; }

                var t12 = Task.Run(() => FLOORTIME(token));
                await Task.WhenAny(new[] { t12 }); 
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        private async Task FLOOR2PORTAL(CancellationToken token)
        {
            try
            {

                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);


                    _Shadowhunter = true;
                    _Paladin = true;
                    _Berserker = true;
                    for (int i = 0; i <= 20; i++)
                    {
                        try
                        {
                            au3.Send("{G}");
                            au3.Send("{G}");

                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: Search Portal..."));
                            // Tunable variables
                            float threshold = 0.7f; // set this higher for fewer false positives and lower for fewer false negatives
                            var enemyTemplate =
                                new Image<Bgr, byte>(resourceFolder + "/portalenter1.png"); // icon of the enemy
                            var enemyMask =
                                new Image<Bgr, byte>(resourceFolder + "/portalentermask1.png"); // make white what the important parts are, other parts should be black
                                                                                                //var screenCapture = new Image<Bgr, byte>("D:/Projects/bot-enemy-detection/EnemyDetection/screen.png");
                            Point myPosition = new Point(150, 128);
                            Point screenResolution = new Point(1920, 1080);

                            // Main program loop
                            var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);
                            var screenPrinter = new PrintScreen();

                            screenPrinter.CaptureScreenToFile("screen.png", ImageFormat.Png);
                            var screenCapture = new Image<Bgr, byte>("screen.png");
                            var enemy = enemyDetector.GetClosestEnemy(screenCapture);
                            if (enemy.HasValue)
                            {
                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: Portal found..."));
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(100, token);
                                CvInvoke.Rectangle(screenCapture,
                                    new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                                    new MCvScalar(255));

                                double x1 = 963f / myPosition.X;
                                double y1 = 551f / myPosition.Y;
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(100, token);
                                var x2 = x1 * enemy.Value.X;
                                var y2 = y1 * enemy.Value.Y;
                                if (x2 <= 963)
                                    x2 = x2 * 0.68f;
                                else
                                    x2 = x2 * 1.38f;
                                if (y2 <= 551)
                                    y2 = y2 * 0.68;
                                else
                                    y2 = y2 * 1.38;
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(100, token);
                                var absolutePositions = PixelToAbsolute(x2, y2, screenResolution);
                                inputSimulator.Mouse.MoveMouseTo(absolutePositions.Item1, absolutePositions.Item2);
                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: Enter Portal..."));

                                au3.Send("{G}");
                                if (txtLEFT.Text == "LEFT")
                                {
                                    
                                    inputSimulator.Mouse.LeftButtonClick();
                                }
                                else
                                {
                                    inputSimulator.Mouse.RightButtonClick();
                                }
                                au3.Send("{G}");



                                au3.Send("{G}");
                                if (txtLEFT.Text == "LEFT")
                                {
                                    inputSimulator.Mouse.LeftButtonClick();
                                }
                                else
                                {
                                    inputSimulator.Mouse.RightButtonClick();
                                }
                                
                                au3.Send("{G}");

                                au3.Send("{G}");
                            }
                            else
                            {
                            }
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }

                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        Random random = new Random();
                        var sleepTime = random.Next(300, 500);
                        Thread.Sleep(sleepTime);
                        au3.Send("{G}");
                        au3.Send("{G}");
                    }
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Expected");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Bug");
                }
                catch { }
                searchSequence2 = 1;
                var t12 = Task.Run(() => SEARCHBOSS2(token));
                await Task.WhenAny(new[] { t12 });
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }

        }

        private async Task LEAVEDUNGEON(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    _Bard = false;
                    _Shadowhunter = false;
                    _Berserker = false;
                    _Paladin = false;
                    _Deathblade = false;
                    _Sharpshooter = false;
                    _Bard = false;
                    _Sorcerer = false;
                    _Soulfist = false;



                    for (int i = 0; i < 1; i++)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object walk = au3.PixelSearch(77, 270, 190, 298, 0x29343F, 5);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 5);
                            }
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object walk = au3.PixelSearch(77, 270, 190, 298, 0x29343F, 5);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 5);
                            }
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                    }
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Expected");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Bug");
                }
                catch { }
                var t6 = Task.Run(() => LEAVEACCEPT(token));
                await Task.WhenAny(new[] { t6 });
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        private async Task LEAVEDUNGEONCOMPLETE(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);
                    _Shadowhunter = true;
                    _Paladin = true;
                    _Berserker = true;
                    for (int i = 0; i < 1; i++)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object walk = au3.PixelSearch(141, 274, 245, 294, 0x29343F, 10);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 5);
                            }
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object walk = au3.PixelSearch(141, 274, 245, 294, 0x29343F, 10);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseClick("LEFT", (int)walkCoord[0], (int)walkCoord[1], 1, 5);
                            }
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                    }
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Expected");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Bug");
                }
                catch { }
                var t6 = Task.Run(() => LEAVEACCEPT(token));
                await Task.WhenAny(new[] { t6 });
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        private async Task LEAVEACCEPT(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);

                    for (int i = 0; i < 1; i++)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object walk = au3.PixelSearch(560, 260, 1382, 817, 0x21BD08, 10);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseClick("LEFT", 903, 605, 1, 5);
                            }
                        }
                        catch { }
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object walk = au3.PixelSearch(560, 260, 1382, 817, 0x21BD08, 10);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseClick("LEFT", 903, 605, 1, 5);
                            }
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            object walk = au3.PixelSearch(560, 260, 1382, 817, 0x21BD08, 10);

                            if (walk.ToString() != "1")
                            {
                                object[] walkCoord = (object[])walk;
                                au3.MouseClick("LEFT", 903, 605, 1, 5);
                            }
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                    }
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Expected");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Bug");
                }
                catch { }

                Thread.Sleep(2000);
                if (_REPAIR == true)
                {
                    Thread.Sleep(2000);
                    var t7 = Task.Run(() => REPAIR(token));
                    await Task.WhenAny(new[] { t7 });
                }
                else
                if (_LOGOUT == true)
                {
                    var t11 = Task.Run(() => LOGOUT(token));
                    await Task.WhenAny(new[] { t11 });
                }
                else
                if (_REPAIR == false && _LOGOUT == false)
                {
                    await Task.Delay(2000);
                    var t9 = Task.Run(() => RESTART(token));
                    await Task.WhenAny(new[] { t9 });
                }
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        private async Task LOGOUT(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);

                for (int i = 0; i < 1; i++)
                {
                    try
                    {
                        Thread.Sleep(20000);
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(100, token);
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "LOGOUT Process starts..."));
                        au3.Send("{ESCAPE}");
                        Thread.Sleep(2000);
                        au3.MouseClick("LEFT", 1238, 728, 1, 5);
                        Thread.Sleep(2000);
                        au3.MouseClick("LEFT", 906, 575, 1, 5);
                        Thread.Sleep(1000);
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "You are loged out!"));
                        _start = false;
                        cts.Cancel();
                    }
                    catch (AggregateException)
                    {
                        Console.WriteLine("Expected");
                    }
                    catch (ObjectDisposedException)
                    {
                        Console.WriteLine("Bug");
                    }
                    catch { }
                }
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        private async Task REPAIR(CancellationToken token)

        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);

                    for (int i = 0; i < 1; i++)
                    {
                        try
                        {
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Auto-Repair starts in 20 seconds..."));
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            await Task.Delay(25000);
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }

                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                            au3.MouseClick("LEFT", 1741, 1040, 1, 5);
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                        await Task.Delay(2000);
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                            await Task.Delay(1500);
                            au3.MouseClick("LEFT", 1684, 823, 1, 5);
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }

                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);

                            await Task.Delay(1500);
                            au3.MouseClick("LEFT", 1256, 693, 1, 5);
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }

                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            await Task.Delay(1500);
                            au3.MouseClick("LEFT", 1085, 429, 1, 5);
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            await Task.Delay(1500);
                            au3.Send("{ESCAPE}");
                            await Task.Delay(1000);
                            au3.Send("{ESCAPE}");

                            _REPAIR = false;
                            _REPAIR = false;
                            var repair = Task.Run(() => REPAIRTIMER(token));
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                    }
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Expected");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Bug");
                }
                catch { }
                await Task.Delay(2000);
                var t10 = Task.Run(() => RESTART_AFTERREPAIR(token));
                await Task.WhenAny(new[] { t10 });
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        private async Task RESTART(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);

                    for (int i = 0; i < 1; i++)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot Paused: Resume in " + int.Parse(txtRestartTimer.Text) + " seconds."));
                            Thread.Sleep(int.Parse(txtRestartTimer.Text) * 1000);
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                    }
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Expected");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Bug");
                }
                catch { }


                Thread.Sleep(2000);
                var t1 = Task.Run(() => START(token));
                await Task.WhenAny(new[] { t1 });
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        private async Task RESTART_AFTERREPAIR(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100, token);

                    for (int i = 0; i < 1; i++)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(100, token);
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Auto-Repair done!"));
                            Thread.Sleep(4000);
                        }
                        catch (AggregateException)
                        {
                            Console.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Console.WriteLine("Bug");
                        }
                        catch { }
                    }
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Expected");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Bug");
                }
                catch { }
                var t1 = Task.Run(() => START(token));
                await Task.WhenAny(new[] { t1 });
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        private void lbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        public void ChaosBot_Load(object sender, EventArgs e)
        {
            List<Layout_Keyboard> LAYOUT = new List<Layout_Keyboard>();
            Layout_Keyboard QWERTZ = new Layout_Keyboard
            {
                LAYOUTS = "QWERTZ",
                Q = VirtualKeyCode.VK_Q,
                W = VirtualKeyCode.VK_W,
                E = VirtualKeyCode.VK_E,
                R = VirtualKeyCode.VK_R,
                A = VirtualKeyCode.VK_A,
                S = VirtualKeyCode.VK_S,
                D = VirtualKeyCode.VK_D,
                F = VirtualKeyCode.VK_F,
            };
            LAYOUT.Add(QWERTZ);

            Layout_Keyboard QWERTY = new Layout_Keyboard
            {
                LAYOUTS = "QWERTY",
                Q = VirtualKeyCode.VK_Q,
                W = VirtualKeyCode.VK_W,
                E = VirtualKeyCode.VK_E,
                R = VirtualKeyCode.VK_R,
                A = VirtualKeyCode.VK_A,
                S = VirtualKeyCode.VK_S,
                D = VirtualKeyCode.VK_D,
                F = VirtualKeyCode.VK_F,
            };
            LAYOUT.Add(QWERTY);

            Layout_Keyboard AZERTY = new Layout_Keyboard
            {
                LAYOUTS = "AZERTY",
                Q = VirtualKeyCode.VK_A,
                W = VirtualKeyCode.VK_Z,
                E = VirtualKeyCode.VK_E,
                R = VirtualKeyCode.VK_R,
                A = VirtualKeyCode.VK_Q,
                S = VirtualKeyCode.VK_S,
                D = VirtualKeyCode.VK_D,
                F = VirtualKeyCode.VK_F
            };
            LAYOUT.Add(AZERTY);

            comboBox1.DataSource = LAYOUT;
            comboBox1.DisplayMember = "LAYOUTS";
            currentLayout = comboBox1.SelectedItem as Layout_Keyboard;
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);

            txtDungeon.Text = Properties.Settings.Default.dungeontimer;
            txtLEFT.Text = Properties.Settings.Default.left;
           
            txQ.Text = Properties.Settings.Default.q;
            txW.Text = Properties.Settings.Default.w;
            txE.Text = Properties.Settings.Default.e;
            txR.Text = Properties.Settings.Default.r;
            txA.Text = Properties.Settings.Default.a;
            txS.Text = Properties.Settings.Default.s;
            txD.Text = Properties.Settings.Default.d;
            txF.Text = Properties.Settings.Default.f;
            txCoolQ.Text = Properties.Settings.Default.cQ;
            txCoolW.Text = Properties.Settings.Default.cW;
            txCoolE.Text = Properties.Settings.Default.cE;
            txCoolR.Text = Properties.Settings.Default.cR;
            txCoolA.Text = Properties.Settings.Default.cA;
            txCoolS.Text = Properties.Settings.Default.cS;
            txCoolD.Text = Properties.Settings.Default.cD;
            txCoolF.Text = Properties.Settings.Default.cF;
            txtHeal30.Text = Properties.Settings.Default.instant;
            txtHeal70.Text = Properties.Settings.Default.potion;
            checkBoxHeal30.Checked = Properties.Settings.Default.chboxinstant;
            checkBoxHeal70.Checked = Properties.Settings.Default.chboxheal;
            chBoxAutoRepair.Checked = Properties.Settings.Default.chBoxAutoRepair;
            txtRepair.Text = Properties.Settings.Default.autorepair;
            chBoxY.Checked = Properties.Settings.Default.chBoxShadowhunter;
            chBoxPaladin.Checked = Properties.Settings.Default.chboxPaladin;
            chBoxBerserker.Checked = Properties.Settings.Default.chBoxBerserker;
            txtRestartTimer.Text = Properties.Settings.Default.RestartTimer;
            chBoxAutoMovement.Checked = Properties.Settings.Default.chBoxSaveAll;
            chBoxActivateF2.Checked = Properties.Settings.Default.chBoxActivateF2;
            txtDungeon2search.Text = Properties.Settings.Default.txtDungeon2search;
            txtDungeon2.Text = Properties.Settings.Default.txtDungeon2;
            txtDungeon3search.Text = Properties.Settings.Default.txtDungeon3search;
            txtDungeon3.Text = Properties.Settings.Default.txtDungeon3;
            chBoxActivateF3.Checked = Properties.Settings.Default.chBoxActivateF3;
            txtDungeon3Iteration.Text = Properties.Settings.Default.txtDungeon3Iteration;
            txtDungeon2Iteration.Text = Properties.Settings.Default.txtDungeon2Iteration;

        }

        private void ChaosBot_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void checkBoxInstant_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHeal30.Checked)
            {
                txtHeal30.ReadOnly = false;
            }
            else
            if (!checkBoxHeal30.Checked)
            {
                txtHeal30.ReadOnly = true;
                txtHeal30.Text = "";
            }
        }

        private void checkBoxHeal_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHeal70.Checked)
            {
                txtHeal70.ReadOnly = false;
            }
            else
            if (!checkBoxHeal70.Checked)
            {
                txtHeal70.ReadOnly = true;
                txtHeal70.Text = "";
            }
        }

        private void checkIsDigit(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void checkIsLetter(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void chBoxAutoRepair_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxAutoRepair.Checked)
            {
                txtRepair.ReadOnly = false;
            }
            else
                if (!chBoxAutoRepair.Checked)
            {
                txtRepair.ReadOnly = true;
                _REPAIR = false;
            }
        }

        private void chBoxLOGOUT_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxLOGOUT.Checked)
            {
                txtLOGOUT.ReadOnly = false;
            }
            else
               if (!chBoxLOGOUT.Checked)
            {
                txtLOGOUT.ReadOnly = true;
                _LOGOUT = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.txtDungeon3Iteration = "12";
                Properties.Settings.Default.txtDungeon2Iteration = "9";

                Properties.Settings.Default.dungeontimer = "65";
                Properties.Settings.Default.instant = "";
                Properties.Settings.Default.potion = "";
                Properties.Settings.Default.heal10 = "";
                Properties.Settings.Default.chboxinstant = false;
                Properties.Settings.Default.chboxheal = false;
                Properties.Settings.Default.chBoxAutoRepair = false;
                Properties.Settings.Default.chBoxLOGOUT = false;
                Properties.Settings.Default.txtLOGOUT = "";
                Properties.Settings.Default.autorepair = "10";
                Properties.Settings.Default.chBoxShadowhunter = false;
                Properties.Settings.Default.chBoxSoulfist = false;
                Properties.Settings.Default.chBoxBerserker = false;
                Properties.Settings.Default.chBoxBard = false;
                Properties.Settings.Default.chboxPaladin = false;
                Properties.Settings.Default.RestartTimer = "25";
                Properties.Settings.Default.chBoxSaveAll = false;
                Properties.Settings.Default.chBoxActivateF2 = false;
                Properties.Settings.Default.txtDungeon2 = "18";
                Properties.Settings.Default.txtDungeon2search = "7";
                Properties.Settings.Default.txtDungeon3 = "20";
                Properties.Settings.Default.txtDungeon3search = "10";
                Properties.Settings.Default.chBoxActivateF3 = false;
                Properties.Settings.Default.chBoxAutoMovement = false;


                Properties.Settings.Default.chBoxSharpshooter = false;
                Properties.Settings.Default.chBoxSorcerer = false;
                Properties.Settings.Default.chBoxDeathblade = false;

                Properties.Settings.Default.RQ = "1";
                Properties.Settings.Default.RW = "2";
                Properties.Settings.Default.RE = "3";
                Properties.Settings.Default.RR = "4";
                Properties.Settings.Default.RA = "5";
                Properties.Settings.Default.RS = "6";
                Properties.Settings.Default.RD = "7";
                Properties.Settings.Default.RF = "8";
                
                Properties.Settings.Default.cQ = "500";
                Properties.Settings.Default.cW = "500";
                Properties.Settings.Default.cE = "500";
                Properties.Settings.Default.cR = "500";
                Properties.Settings.Default.cA = "500";
                Properties.Settings.Default.cS = "500";
                Properties.Settings.Default.cD = "500";
                Properties.Settings.Default.cF = "500";
                Properties.Settings.Default.q = "500";
                Properties.Settings.Default.w = "500";
                Properties.Settings.Default.e = "500";
                Properties.Settings.Default.r = "500";
                Properties.Settings.Default.a = "500";
                Properties.Settings.Default.s = "500";
                Properties.Settings.Default.d = "500";
                Properties.Settings.Default.f = "500";

                Properties.Settings.Default.chBoxDoubleQ = false;
                Properties.Settings.Default.chBoxDoubleW = false;
                Properties.Settings.Default.chBoxDoubleE = false;
                Properties.Settings.Default.chBoxDoubleR = false;
                Properties.Settings.Default.chBoxDoubleA = false;
                Properties.Settings.Default.chBoxDoubleS = false;
                Properties.Settings.Default.chBoxDoubleD = false;
                Properties.Settings.Default.chBoxDoubleF = false;

                Properties.Settings.Default.Save();

                chBoxAutoMovement.Checked = Properties.Settings.Default.chBoxAutoMovement;
                txtDungeon3Iteration.Text = Properties.Settings.Default.txtDungeon3Iteration;
                txtDungeon2Iteration.Text = Properties.Settings.Default.txtDungeon2Iteration;
                txtDungeon.Text = Properties.Settings.Default.dungeontimer;
                txtHeal10.Text = Properties.Settings.Default.instant;
                chBoxLOGOUT.Checked = Properties.Settings.Default.chBoxLOGOUT;
                txtHeal30.Text = Properties.Settings.Default.instant;
                txtHeal70.Text = Properties.Settings.Default.potion;
                checkBoxHeal30.Checked = Properties.Settings.Default.chboxinstant;
                checkBoxHeal70.Checked = Properties.Settings.Default.chboxheal;
                checkBoxHeal10.Checked = Properties.Settings.Default.checkBoxHeal10;
                chBoxAutoRepair.Checked = Properties.Settings.Default.chBoxAutoRepair;
                txtRepair.Text = Properties.Settings.Default.autorepair;
                chBoxY.Checked = Properties.Settings.Default.chBoxShadowhunter;
                chBoxPaladin.Checked = Properties.Settings.Default.chboxPaladin;
                chBoxBerserker.Checked = Properties.Settings.Default.chBoxBerserker;
                chBoxDeathblade.Checked = Properties.Settings.Default.chBoxDeathblade;
                chBoxSorcerer.Checked = Properties.Settings.Default.chBoxSorcerer;
                chBoxSharpshooter.Checked = Properties.Settings.Default.chBoxSharpshooter;
                chBoxSoulfist.Checked = Properties.Settings.Default.chBoxSoulfist;
                txtRestartTimer.Text = Properties.Settings.Default.RestartTimer;
                chBoxAutoMovement.Checked = Properties.Settings.Default.chBoxSaveAll;
                chBoxActivateF2.Checked = Properties.Settings.Default.chBoxActivateF2;
                txtDungeon2search.Text = Properties.Settings.Default.txtDungeon2search;
                txtDungeon2.Text = Properties.Settings.Default.txtDungeon2;
                txCoolQ.Text = Properties.Settings.Default.cQ;
                txCoolW.Text = Properties.Settings.Default.cW;
                txCoolE.Text = Properties.Settings.Default.cE;
                txCoolR.Text = Properties.Settings.Default.cR;
                txCoolA.Text = Properties.Settings.Default.cA;
                txCoolS.Text = Properties.Settings.Default.cS;
                txCoolD.Text = Properties.Settings.Default.cD;
                txCoolF.Text = Properties.Settings.Default.cF;
                txtLOGOUT.Text = Properties.Settings.Default.txtLOGOUT;
                txQ.Text = Properties.Settings.Default.q;
                txW.Text = Properties.Settings.Default.w;
                txE.Text = Properties.Settings.Default.e;
                txR.Text = Properties.Settings.Default.r;
                txA.Text = Properties.Settings.Default.a;
                txS.Text = Properties.Settings.Default.s;
                txD.Text = Properties.Settings.Default.d;
                txF.Text = Properties.Settings.Default.f;
           

                txPQ.Text = Properties.Settings.Default.RQ;
                txPW.Text = Properties.Settings.Default.RW;
                txPE.Text = Properties.Settings.Default.RE;
                txPR.Text = Properties.Settings.Default.RR;
                txPA.Text = Properties.Settings.Default.RA;
                txPS.Text = Properties.Settings.Default.RS;
                txPD.Text = Properties.Settings.Default.RD;
                txPF.Text = Properties.Settings.Default.RF;
                chBoxDoubleQ.Checked = Properties.Settings.Default.chBoxDoubleQ;
                chBoxDoubleW.Checked = Properties.Settings.Default.chBoxDoubleW;
                chBoxDoubleE.Checked = Properties.Settings.Default.chBoxDoubleE;
                chBoxDoubleR.Checked = Properties.Settings.Default.chBoxDoubleR;
                chBoxDoubleA.Checked = Properties.Settings.Default.chBoxDoubleA;
                chBoxDoubleS.Checked = Properties.Settings.Default.chBoxDoubleS;
                chBoxDoubleD.Checked = Properties.Settings.Default.chBoxDoubleD;
                chBoxDoubleF.Checked = Properties.Settings.Default.chBoxDoubleF;
                txtDungeon3search.Text = Properties.Settings.Default.txtDungeon3search;
                txtDungeon3.Text = Properties.Settings.Default.txtDungeon3;
                chBoxActivateF3.Checked = Properties.Settings.Default.chBoxActivateF3;
                chBoxBard.Checked = Properties.Settings.Default.chBoxBard;
                    }
            catch { }
        }

        private void btnInstructions_Click(object sender, EventArgs e)
        {
            frmGuide Form = new frmGuide();
            Form.Show();
        }

        public void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Layout_Keyboard currentLayout = comboBox1.SelectedItem as Layout_Keyboard;
            lbQ.Text = currentLayout.Q.ToString().Replace("VK_", "");
            lbW.Text = currentLayout.W.ToString().Replace("VK_", "");
            lbE.Text = currentLayout.E.ToString().Replace("VK_", "");
            lbR.Text = currentLayout.R.ToString().Replace("VK_", "");
            lbA.Text = currentLayout.A.ToString().Replace("VK_", "");
            lbS.Text = currentLayout.S.ToString().Replace("VK_", "");
            lbD.Text = currentLayout.D.ToString().Replace("VK_", "");
            lbF.Text = currentLayout.F.ToString().Replace("VK_", "");

            lb2Q.Text = currentLayout.Q.ToString().Replace("VK_", "");
            lb2W.Text = currentLayout.W.ToString().Replace("VK_", "");
            lb2E.Text = currentLayout.E.ToString().Replace("VK_", "");
            lb2R.Text = currentLayout.R.ToString().Replace("VK_", "");
            lb2A.Text = currentLayout.A.ToString().Replace("VK_", "");
            lb2S.Text = currentLayout.S.ToString().Replace("VK_", "");
            lb2D.Text = currentLayout.D.ToString().Replace("VK_", "");
            lb2F.Text = currentLayout.F.ToString().Replace("VK_", "");

            lbPQ.Text = currentLayout.Q.ToString().Replace("VK_", "");
            lbPW.Text = currentLayout.W.ToString().Replace("VK_", "");
            lbPE.Text = currentLayout.E.ToString().Replace("VK_", "");
            lbPR.Text = currentLayout.R.ToString().Replace("VK_", "");
            lbPA.Text = currentLayout.A.ToString().Replace("VK_", "");
            lbPS.Text = currentLayout.S.ToString().Replace("VK_", "");
            lbPD.Text = currentLayout.D.ToString().Replace("VK_", "");
            lbPF.Text = currentLayout.F.ToString().Replace("VK_", "");
        }
      
        public async void SharpshooterSecondPress(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(3000, token);
                var sim = new InputSimulator();
                sim.Keyboard.KeyPress(VirtualKeyCode.VK_Y);
               
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        public async void SkillCooldown(CancellationToken token, VirtualKeyCode key)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                for (int i = 0; i <= 1; i++)
                {
                    token.ThrowIfCancellationRequested();
                    int cooldownDuration = 0;
                    await Task.Delay(100, token);
                    switch (key)
                    {
                        case VirtualKeyCode.VK_A:
                            cooldownDuration = int.Parse(txCoolA.Text);
                            break;
                        case VirtualKeyCode.VK_S:
                            cooldownDuration = int.Parse(txCoolS.Text);

                            break;
                        case VirtualKeyCode.VK_D:
                            cooldownDuration = int.Parse(txCoolD.Text);

                            break;
                        case VirtualKeyCode.VK_F:
                            cooldownDuration = int.Parse(txCoolF.Text);

                            break;
                        case VirtualKeyCode.VK_Q:
                            cooldownDuration = int.Parse(txCoolQ.Text);

                            break;
                        case VirtualKeyCode.VK_W:
                            cooldownDuration = int.Parse(txCoolW.Text);

                            break;
                        case VirtualKeyCode.VK_E:
                            cooldownDuration = int.Parse(txCoolE.Text);

                            break;
                        case VirtualKeyCode.VK_R:
                            cooldownDuration = int.Parse(txCoolR.Text);
                            break;
                    }
                    timer = new System.Timers.Timer(cooldownDuration);
                    switch (key)
                    {
                        case VirtualKeyCode.VK_A:
                            timer.Elapsed += A_CooldownEvent;
                            break;
                        case VirtualKeyCode.VK_S:
                            timer.Elapsed += S_CooldownEvent;

                            break;
                        case VirtualKeyCode.VK_D:
                            timer.Elapsed += D_CooldownEvent;

                            break;
                        case VirtualKeyCode.VK_F:
                            timer.Elapsed += F_CooldownEvent;

                            break;
                        case VirtualKeyCode.VK_Q:
                            timer.Elapsed += Q_CooldownEvent;

                            break;
                        case VirtualKeyCode.VK_W:
                            timer.Elapsed += W_CooldownEvent;

                            break;
                        case VirtualKeyCode.VK_E:
                            timer.Elapsed += E_CooldownEvent;

                            break;
                        case VirtualKeyCode.VK_R:
                            timer.Elapsed += R_CooldownEvent;
                            break;
                    }
                    timer.AutoReset = false;
                    timer.Enabled = true;
                }
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch { }
        }

        private void Q_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _Q = true;
        }

        private void W_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _W = true;
        }

        private void E_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _E = true;
        }

        private void R_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _R = true;
        }

        private void A_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _A = true;
        }

        private void S_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _S = true;
        }

        private void D_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _D = true;
        }

        private void F_CooldownEvent(object source, ElapsedEventArgs e)
        {
            _F = true;
        }

        private void buttonSaveRotation_Click(object sender, EventArgs e)
        {


            if (comboBoxRotations.Text != "")
            {
                if (comboBoxRotations.Text != "main")
                {
                    rotation.txtDungeon2Iteration = txtDungeon2Iteration.Text;
                    rotation.txtDungeon3Iteration = txtDungeon3Iteration.Text;

                    rotation.dungeontimer = txtDungeon.Text;
                    rotation.instant = txtHeal30.Text;
                    rotation.potion = txtHeal70.Text;
                    rotation.txtHeal10 = txtHeal10.Text;
                    rotation.chboxinstant = (bool)checkBoxHeal30.Checked;
                    rotation.chboxheal = (bool)checkBoxHeal70.Checked;
                    rotation.chboxheal10 = (bool)checkBoxHeal10.Checked;
                    rotation.chBoxAutoRepair = (bool)chBoxAutoRepair.Checked;
                    rotation.autorepair = txtRepair.Text;

                    rotation.autologout = txtLOGOUT.Text;
                    rotation.chBoxautologout = chBoxLOGOUT.Checked;
                    rotation.chBoxAutoMovement = chBoxAutoMovement.Checked;
                    rotation.autorepair = txtRepair.Text;
                    rotation.chBoxShadowhunter = (bool)chBoxY.Checked;
                    rotation.chboxPaladin = (bool)chBoxPaladin.Checked;
                    rotation.chBoxBerserker = (bool)chBoxBerserker.Checked;
                    rotation.chBoxDeathblade = (bool)chBoxDeathblade.Checked;
                    rotation.chBoxSharpshooter = (bool)chBoxSharpshooter.Checked;
                    rotation.chBoxSoulfist = (bool)chBoxSoulfist.Checked;
                    rotation.chBoxSorcerer = (bool)chBoxSorcerer.Checked;
                    rotation.chBoxBard = (bool)chBoxBard.Checked;
                    rotation.RestartTimer = txtRestartTimer.Text;
                    rotation.chBoxSaveAll = chBoxAutoMovement.Checked;
                    rotation.chBoxActivateF2 = chBoxActivateF2.Checked;
                    rotation.chBoxActivateF3 = chBoxActivateF3.Checked;
                    rotation.txtDungeon3search = txtDungeon3search.Text;
                    rotation.txtDungeon3 = txtDungeon3.Text;
                    rotation.txtLEFT = txtLEFT.Text;

                    rotation.txtDungeon2search = txtDungeon2search.Text;
                    rotation.txtDungeon2 = txtDungeon2.Text;
                    rotation.cQ = txCoolQ.Text;
                    rotation.cW = txCoolW.Text;
                    rotation.cE = txCoolE.Text;
                    rotation.cR = txCoolR.Text;
                    rotation.cA = txCoolA.Text;
                    rotation.cS = txCoolS.Text;
                    rotation.cD = txCoolD.Text;
                    rotation.cF = txCoolF.Text;
                    rotation.q = txQ.Text;
                    rotation.w = txW.Text;
                    rotation.e = txE.Text;
                    rotation.r = txR.Text;
                    rotation.a = txA.Text;
                    rotation.s = txS.Text;
                    rotation.d = txD.Text;
                    rotation.f = txF.Text;
                    rotation.pQ = txPQ.Text;
                    rotation.pW = txPW.Text;
                    rotation.pE = txPE.Text;
                    rotation.pR = txPR.Text;
                    rotation.pA = txPA.Text;
                    rotation.pS = txPS.Text;
                    rotation.pD = txPD.Text;
                    rotation.pF = txPF.Text;
                    rotation.chBoxDoubleQ = chBoxDoubleQ.Checked;
                    rotation.chBoxDoubleW = chBoxDoubleW.Checked;
                    rotation.chBoxDoubleE = chBoxDoubleE.Checked;
                    rotation.chBoxDoubleR = chBoxDoubleR.Checked;
                    rotation.chBoxDoubleA = chBoxDoubleA.Checked;
                    rotation.chBoxDoubleS = chBoxDoubleS.Checked;
                    rotation.chBoxDoubleD = chBoxDoubleD.Checked;
                    rotation.chBoxDoubleF = chBoxDoubleF.Checked;


                    rotation.Save(comboBoxRotations.Text);
                    MessageBox.Show("Rotation \"" + comboBoxRotations.Text + "\" saved");
                }
                else
                {
                    MessageBox.Show("Rotation can not be named \"main\"");
                }
            }
            else
            {
                MessageBox.Show("Please enter a name for your Rotation Config!");
            }
        }

        private void buttonLoadRotation_Click(object sender, EventArgs e)
        {

            rotation = Rotations.Load(comboBoxRotations.Text + ".ini");
            if (rotation != null)
            {
                txtLEFT.Text = rotation.left;
                txtDungeon.Text = rotation.dungeontimer;
                txtHeal30.Text = rotation.instant;
                txtHeal70.Text = rotation.potion;
                checkBoxHeal30.Checked = rotation.chboxinstant;
                checkBoxHeal70.Checked = rotation.chboxheal;
                checkBoxHeal10.Checked = rotation.chboxheal10;
                chBoxAutoRepair.Checked = rotation.chBoxAutoRepair;
                txtRepair.Text = rotation.autorepair;
                chBoxY.Checked = rotation.chBoxShadowhunter;
                chBoxPaladin.Checked = rotation.chboxPaladin;
                chBoxBerserker.Checked = rotation.chBoxBerserker;
                chBoxBard.Checked ^= rotation.chBoxBard;
                chBoxDeathblade.Checked = rotation.chBoxDeathblade;
                chBoxSharpshooter.Checked = rotation.chBoxSharpshooter;
                chBoxSoulfist.Checked = rotation.chBoxSoulfist;
                txtLOGOUT.Text = rotation.autologout;
                chBoxLOGOUT.Checked = rotation.chBoxautologout;
                txtHeal10.Text = rotation.txtHeal10;
                txtDungeon2Iteration.Text = rotation.txtDungeon2Iteration;
                txtDungeon3Iteration.Text = rotation.txtDungeon3Iteration;
                chBoxAutoMovement.Checked = rotation.chBoxAutoMovement; 
                chBoxActivateF3.Checked = rotation.chBoxActivateF3;
                txtDungeon3search.Text = rotation.txtDungeon3search;
                txtDungeon3.Text = rotation.txtDungeon3;

                chBoxSorcerer.Checked = rotation.chBoxSorcerer;
                txtRestartTimer.Text = rotation.RestartTimer;
                chBoxAutoMovement.Checked = rotation.chBoxSaveAll;
                chBoxActivateF2.Checked = rotation.chBoxActivateF2;
                txtDungeon2search.Text = rotation.txtDungeon2search;
                txtDungeon2.Text = rotation.txtDungeon2;
                txCoolQ.Text = rotation.cQ;
                txCoolW.Text = rotation.cW;
                txCoolE.Text = rotation.cE;
                txCoolR.Text = rotation.cR;
                txCoolA.Text = rotation.cA;
                txCoolS.Text = rotation.cS;
                txCoolD.Text = rotation.cD;
                txCoolF.Text = rotation.cF;
                txQ.Text = rotation.q;
                txW.Text = rotation.w;
                txE.Text = rotation.e;
                txR.Text = rotation.r;
                txA.Text = rotation.a;
                txS.Text = rotation.s;
                txD.Text = rotation.d;
                txF.Text = rotation.f;
                txPQ.Text = rotation.pQ;
                txPW.Text = rotation.pW;
                txPE.Text = rotation.pE;
                txPR.Text = rotation.pR;
                txPA.Text = rotation.pA;
                txPS.Text = rotation.pS;
                txPD.Text = rotation.pD;
                txPF.Text = rotation.pF;
                chBoxDoubleQ.Checked = rotation.chBoxDoubleQ;
                chBoxDoubleW.Checked = rotation.chBoxDoubleW;
                chBoxDoubleE.Checked = rotation.chBoxDoubleE;
                chBoxDoubleR.Checked = rotation.chBoxDoubleR;
                chBoxDoubleA.Checked = rotation.chBoxDoubleA;
                chBoxDoubleS.Checked = rotation.chBoxDoubleS;
                chBoxDoubleD.Checked = rotation.chBoxDoubleD;
                chBoxDoubleF.Checked = rotation.chBoxDoubleF;



                MessageBox.Show("Rotation \"" + comboBoxRotations.Text + "\" loaded");
            }
        }

        private void comboBoxRotations_MouseClick(object sender, MouseEventArgs e)
        {
            refreshRotationCombox();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void changeSkillSet(object sender, EventArgs e)
        {
            if (txPA.Text != "" && txPS.Text != "" && txPD.Text != "" && txPF.Text != "" && txPQ.Text != "" && txPW.Text != "" && txPE.Text != "" && txPR.Text != "")
                SKILLS.skillset = new Dictionary<VirtualKeyCode, int>()
            {
                { VirtualKeyCode.VK_A, int.Parse(txPA.Text)},
                { VirtualKeyCode.VK_S, int.Parse(txPS.Text)},
                { VirtualKeyCode.VK_D, int.Parse(txPD.Text)},
                { VirtualKeyCode.VK_F, int.Parse(txPF.Text)},
                { VirtualKeyCode.VK_Q, int.Parse(txPQ.Text)},
                { VirtualKeyCode.VK_W, int.Parse(txPW.Text)},
                { VirtualKeyCode.VK_E, int.Parse(txPE.Text)},
                { VirtualKeyCode.VK_R, int.Parse(txPR.Text)},
            }.ToList();
        }

        private void chBoxActivateF2_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxActivateF2.Checked)
            {
                txtDungeon2search.ReadOnly = false;
                txtDungeon2.ReadOnly = false;
                txtDungeon2Iteration.ReadOnly = false;

            }
            else
               if (!chBoxActivateF2.Checked)
            {
                txtDungeon2search.ReadOnly = true;
                txtDungeon2.ReadOnly = true;
                txtDungeon2Iteration.ReadOnly = true;

            }
        }

        private void chBoxActivateF3_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxActivateF3.Checked)
            {
                txtDungeon3search.ReadOnly = false;
                txtDungeon3.ReadOnly = false;
                txtDungeon3Iteration.ReadOnly = false;
            }
            else
              if (!chBoxActivateF3.Checked)
            {
                txtDungeon3search.ReadOnly = true;
                txtDungeon3.ReadOnly = true;
                txtDungeon3Iteration.ReadOnly = true;

            }
        }

        
        
    }
}
