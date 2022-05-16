﻿using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.Misc;

namespace PixelAimbot
{
    partial class GatheringBot
    {
        private bool _start;

        private bool _stop;
        private bool _repair;
        private bool _canrepair;
        private bool _logout;
        private bool _fishing = false;
        private bool _buff;
        private bool _checkEnergy = true;
        private bool _canFish = true;
        private bool _restart;
        private int _swap;
        private bool _minigameFound = false;
        private int _x, _y, _width, _height;
        private int _rodCounter;
        private int _gatheringCounter;
        private bool _telegramBotRunning;
        Random humanizer = new Random();
        PrintScreen _screenPrinter = new PrintScreen();
        public Config conf = new Config();
        private Image _rawScreen;
        private Bitmap _bitmapImage;
        Image<Bgr, byte> _screenCapture;

        public frmMinimized FormMinimized = new frmMinimized();
        public static GatheringBot _GatheringBot;

        private Config _conf = new Config();


        static int _screenWidth = Screen.PrimaryScreen.Bounds.Width;
        static int _screenHeight = Screen.PrimaryScreen.Bounds.Height;
        public Task TelegramTask;
        public Task DiscordTask;
        private CancellationTokenSource _telegramToken = new CancellationTokenSource();
        private CancellationTokenSource _discordToken = new CancellationTokenSource();
        private CancellationTokenSource _cts = new CancellationTokenSource();

        private bool _botIsRun = true;
        private bool _discordBotIsRun = true;


        private static readonly Random random = new Random();
        
        public Image<Bgr, Byte> Image_attention2 = ChaosBot.byteArrayToImage(Images.attention2);
        public Image<Bgr, Byte> Image_energy_fish = ChaosBot.byteArrayToImage(Images.energy_fish);
        public Image<Bgr, Byte> Image_fishing_minigame = ChaosBot.byteArrayToImage(Images.fishing_minigame);
        public Image<Bgr, Byte> Image_gathering = ChaosBot.byteArrayToImage(Images.gathering);
        public Image<Bgr, Byte> Image_gatheringRepair = ChaosBot.byteArrayToImage(Images.gatheringRepair);
        public Image<Bgr, Byte> Image_minigame = ChaosBot.byteArrayToImage(Images.minigame);
        public Image<Bgr, Byte> Image_minigame_mask = ChaosBot.byteArrayToImage(Images.minigame_mask);
    }
}