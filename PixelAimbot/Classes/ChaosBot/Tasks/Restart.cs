﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelAimbot.Classes.Misc;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        private async Task Restart(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Restart in " + int.Parse(txtRestart.Text) + " seconds."));
                await Task.Delay(humanizer.Next(10, 240) + (int.Parse(txtRestart.Text) * 1000),token);
                starten = false;
                gefunden = false;
                _restart = true;
                _stopp = true;
                _portalIsDetected = false;
                _portalIsNotDetected = false;
                _floorFight = false;
                _searchboss = false;
                _revive = false;
                _ultimate = false;
                _portaldetect = false;
                _potions = false;
                _floor1 = false;
                _floor2 = false;
                _Leavetimerfloor1 = 0;
                _Leavetimerfloor2 = 0;
                _GlobalLeavetimerfloor2 = 0;
                _Floor1Detectiontimer = 0;

                _bard = false;
                _gunlancer = false;
                _shadowhunter = false;
                _paladin = false;
                _deathblade = false;
                _Glavier = false;
                _sharpshooter = false;
                _sorcerer = false;
                _soulfist = false;
                _sharpshooter = false;
                _berserker = false;
                _doUltimateAttack = true;
                _Q = true;
                _W = true;
                _E = true;
                _R = true;
                _A = true;
                _S = true;
                _D = true;
                _F = true;
                if (chBoxChannelSwap.Checked)
                {
                    if (_swap == 4)
                    {


                        token.ThrowIfCancellationRequested();
                        Random random = new Random();
                        await Task.Delay(1, token);
                        VirtualMouse.MoveTo(Recalc(1875), Recalc(16, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(humanizer.Next(10, 240) + 1000, token);
                        VirtualMouse.MoveTo(Recalc(1844), Recalc(44, false), 10);

                        for (int i = 0; i < random.Next(2, 10); i++)
                        {
                            VirtualMouse.Scroll(-120);
                            await Task.Delay(100);
                        }
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        _swap++;
                        _restart = false;
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Channel-Swap activated..."));
                        await Task.Delay(humanizer.Next(10, 240) + 2000, token);
                        token.ThrowIfCancellationRequested();
                        cts.Cancel();
                        cts.Dispose();
                        cts = new CancellationTokenSource();
                        token = cts.Token;
                        var t9 = Task.Run(() => Restart(token),token);
                    }
                    else if (_swap == 7)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        Random random = new Random();
                        VirtualMouse.MoveTo(Recalc(1875), Recalc(16, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(humanizer.Next(10, 240) + 1000, token);
                        VirtualMouse.MoveTo(Recalc(1844), Recalc(64, false), 10);

                        for (int i = 0; i < random.Next(2, 10); i++)
                        {
                            VirtualMouse.Scroll(-120);
                            await Task.Delay(100);
                        }
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        _swap = 0;
                        _restart = false;
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Channel-Swap activated..."));
                        await Task.Delay(humanizer.Next(10, 240) + 2000, token);
                        token.ThrowIfCancellationRequested();
                        cts.Cancel();
                        cts.Dispose();
                        cts = new CancellationTokenSource();
                        token = cts.Token;
                        var t9 = Task.Run(() => Restart(token),token);
                    }
                }
              
                if (_restart )
                {
                    try
                    {

                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        cts.Cancel();
                        cts.Dispose();
                        cts = new CancellationTokenSource();
                        token = cts.Token;
                        _start = false;
                        var t9 = Task.Run(() => EndlessStart(token), token);

                       


                        //_stopp = true;
                        //_restart = false;
                        //_logout = false;
                        //_Leavetimerfloor1 = 0;
                        //_Leavetimerfloor2 = 0;
                        //_GlobalLeavetimerfloor2 = 0;
                        //_Floor1Detectiontimer = 0;


                        //_gunlancer = false;
                        //_shadowhunter = false;
                        //_berserker = false;
                        //_paladin = false;
                        //_Glavier = false;
                        //_deathblade = false;
                        //_sharpshooter = false;
                        //_bard = false;
                        //_sorcerer = false;
                        //_soulfist = false;

                        //_floor1 = false;
                        //_floor2 = false;

                        //_floorFight = false;
                        //_searchboss = false;

                        //_revive = false;
                        //_portaldetect = false;
                        //_ultimate = false;
                        //_potions = false;

                        //_Q = false;
                        //_W = false;
                        //_E = false;
                        //_R = false;
                        //_A = false;
                        //_S = false;
                        //_D = false;
                        //_F = false;

                        //await Task.Delay(humanizer.Next(10, 240) + 2000);
                        //_start = false;
                        //KeyboardWrapper.PressKey(KeyboardWrapper.VK_F9);
                        //await Task.Delay(1000);
                        //token.ThrowIfCancellationRequested(); // TEST
                    }
                    catch (AggregateException)
                    {
                        Console.WriteLine("Expected");
                    }
                    catch (ObjectDisposedException)
                    {
                        Console.WriteLine("Bug");
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
            catch (Exception ex)
            {
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }

        private async void EndlessStart(CancellationToken token)
        {
            if (_start == false)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(humanizer.Next(10, 240) + 2000);
                    Process[] processName = Process.GetProcessesByName("LostArk");
                    if (processName.Length == 0 && chBoxCrashDetection.Checked)
                    {
                        ChaosGameCrashed++;
                        _stop = true;
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_F10);
                        await Task.Delay(5000);
                        DiscordSendMessage("Game Crashed - Bot Stopped!");
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "GAME CRASHED - BOT STOPPED!"));

                    }


                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "READY!"));
                    _start = true;
                    _stop = true;
                    _Leavetimerfloor1 = 0;
                    _Leavetimerfloor2 = 0;
                    _GlobalLeavetimerfloor2 = 0;
                    _Floor1Detectiontimer = 0;

                    if (chBoxAutoRepair.Checked && _RepairReset == true)
                    {

                        _RepairReset = false;
                        _repairTimer = DateTime.Now.AddMinutes(Convert.ToDouble(txtRepair.Text));
                    }

                    if (chBoxLOGOUT.Checked)
                    {

                        var dateNow = DateTime.Now;
                        if (cmbHOUR.SelectedIndex < dateNow.Hour)
                        {
                            _Logout = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day + 1, cmbHOUR.SelectedIndex, cmbMINUTE.SelectedIndex, 00);

                        }
                        else
                        {
                            _Logout = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, cmbHOUR.SelectedIndex, cmbMINUTE.SelectedIndex, 00);

                        }

                    }
                    token.ThrowIfCancellationRequested();
                    cts.Cancel();
                    cts.Dispose();
                    cts = new CancellationTokenSource();
                    token = cts.Token;

                    var t1 = Task.Run(() => Start(token), token);

                }
                catch (OperationCanceledException)
                {
                    // Handle canceled
                }
                catch (Exception ex)
                {
                    ExceptionHandler.SendException(ex);
                    // Handle other exceptions
                }

            }
        }
    }
}