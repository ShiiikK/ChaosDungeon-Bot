﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PixelAimbot.Classes.Misc;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        public async void DeathbladeSecondPress(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 10000, token);
                KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                await Task.Delay(humanizer.Next(10, 240) + 10000, token);
                KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                _deathblade = true;
            }
            catch (AggregateException)
            {
                Console.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Bug");
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
                ExceptionHandler.SendException(ex);
            }
        }


        

    }
}