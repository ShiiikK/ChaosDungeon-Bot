﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        private async Task Floortime(CancellationToken token)
        {
            try
            { 
                Process[] process1Name = Process.GetProcessesByName("LostArk");
                if (process1Name.Length == 0 && chBoxCrashDetection.Checked)
                {
                    ChaosGameCrashed++;

                    _stop = true;

                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_F10);
                    await Task.Delay(5000);
                    DiscordSendMessage("Game Crashed - Bot Stopped!");
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "GAME CRASHED - BOT STOPPED!"));
                    
                }
          
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                #region Floor1
                if (_floor1 && _stopp == false)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    _walktopUTurn = 0;
                    _stopp = false;
                    // START TASK BOOLS //
                    _floorFight = true;
                    _revive = true;
                    _ultimate = true;
                    _portaldetect = true;

                    _potions = true;
                    // CLASSES //
                    _gunlancer = true;
                    _shadowhunter = true;
                    _berserker = true;
                    _paladin = true;
                    _deathblade = true;
                    _Glavier = true;
                    _sharpshooter = true;
                    _sorcerer = true;
                    _soulfist = true;

                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    cts.Cancel();
                    cts.Dispose();
                    cts = new CancellationTokenSource();
                    token = cts.Token;
                    var t14 = Task.Run(() => UltimateAttack(token), token);
                    var t11 = Task.Run(() => SearchNearEnemys(token), token);
                    var t12 = Task.Run(() => Floorfight(token), token);
                    var t16 = Task.Run(() => Revive());
                    var t18 = Task.Run(() => Portaldetect(token), token);
                    var t20 = Task.Run(() => Potions(token), token);
                    await Task.WhenAny(t11, t12, t14, t16, t18, t20);

                }
                #endregion
                #region Floor2
                if (_floor2 && _stopp == false)
                {
                    Process[] process2Name = Process.GetProcessesByName("LostArk");
                    if (process2Name.Length == 0 && chBoxCrashDetection.Checked)
                    {
                        ChaosGameCrashed++;

                        _stop = true;

                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_F10);
                        await Task.Delay(5000);
                        DiscordSendMessage("Game Crashed - Bot Stopped!");
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "GAME CRASHED - BOT STOPPED!"));

                    }
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    _fightSequence++;
                    //_leavetimer++;
                    // START TASK BOOLS //
                    _floorFight = true;
                    _revive = true;
                    _ultimate = true;
                    _potions = true;


                    // CLASSES //
                    _gunlancer = true;
                    _shadowhunter = true;
                    _berserker = true;
                    _paladin = true;
                    _sharpshooter = true;
                    _bard = true;
                    _sorcerer = true;
                    _soulfist = true;

                   
                
                    Task t11 = Task.Run(() => SearchNearEnemys(token), token);
                    Task t12 = Task.Run(() => Floorfight(token), token);
                    Task t16 = Task.Run(() => Revive());
                    Task t20 = Task.Run(() => Potions(token), token);

                    await Task.Delay(humanizer.Next(10, 240) + (int.Parse(txtDungeon2.Text) * 1000));
                    token.ThrowIfCancellationRequested();
                    _floorFight = false;
                    _potions = false;
                    _revive = false;
                    _searchboss = true;

                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    
                    var t13 = Task.Run(() => SEARCHBOSS(tokenSearchBoss), tokenSearchBoss);
                    await Task.WhenAny(t13);

                    await Task.WhenAny(t11, t12, t16, t20);
                }
                
                #endregion
             
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

        private async Task SearchNearEnemys(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                var template = Image_red_hp;
                var detector = new ScreenDetector(template, null, 0.94f, ChaosBot.Recalc(460),
                    ChaosBot.Recalc(120, false), ChaosBot.Recalc(1000, true, true), ChaosBot.Recalc(780, false, true));
                detector.setMyPosition(new Point(ChaosBot.Recalc(500), ChaosBot.Recalc(390, false)));
                var screenPrinter = new PrintScreen();
 /*               if (currentMouseButton == KeyboardWrapper.VK_LBUTTON)
                {
                    VirtualMouse.LeftDown();
                }
                else
                {
                    VirtualMouse.RightDown();
                }*/
                while (_floorFight && _stopp == false )
                {
                    if (_canSearchEnemys)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            using (var screencap = screenPrinter.CaptureScreen())
                            {
                                using (var screenCapture = new Bitmap(screencap).ToImage<Bgr, byte>())
                                {
                                    var item = detector.GetClosest(screenCapture, false);
                                    if (item.HasValue)
                                    {
                                        if (item.Value.X > 0 && item.Value.Y > 0)
                                        {
                                            Point position = calculateFromCenter(item.Value.X, item.Value.Y);
                                            // correct mouse down
                                            int correction = 0;
                                            if (item.Value.Y > Recalc(383, false) && item.Value.Y < Recalc(435, false))
                                            {
                                                correction = Recalc(80, false);
                                            }

                                            VirtualMouse.MoveTo(position.X, position.Y + correction, 10);
                                        }
                                    }
                                    else
                                    {
                                        // Not found Swirl around with Mouse
                                        VirtualMouse.MoveTo(Between(Recalc(460), Recalc(1000)),
                                            Between(Recalc(120, false), Recalc(780, false)), 10);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionHandler.SendException(ex);
                            int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                            Debug.WriteLine("[" + line + "]" + ex.Message);
                        }
                    }
                }
            /*    if (currentMouseButton == KeyboardWrapper.VK_LBUTTON)
                {
                    VirtualMouse.LeftUp();
                }
                else
                {
                    VirtualMouse.RightUp();
                }*/
            }
            catch (Exception ex)
            {
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }

       

        private async Task Floorfight(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                while (_floorFight && _stopp == false )
                {
                    try
                    {
                        if (!_doUltimateAttack )
                        {
                            foreach (KeyValuePair<byte, int> skill in _skills.skillset.OrderBy(x => x.Value))
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                if (_floorFight && !_stopp )
                                {
                                    if (chBoxCooldownDetection.Checked)
                                    {
                                        if (!isKeyOnCooldownGray(skill.Key))
                                        {
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is fighting..."));
                                            KeyboardWrapper.AlternateHoldKey(skill.Key, CasttimeByKey(skill.Key));

                                            if (IsDoubleKey(skill.Key))
                                            {
                                                KeyboardWrapper.PressKey(skill.Key);
                                            }

                                            SetKeyCooldownGray(skill.Key); // Set Cooldown
                                                                          
                                            await Task.Delay(50, token);
                                            _walktopUTurn++;
                                        }
                                        else
                                        {
                                            if (int.Parse(textBoxAutoAttack.Text) >= 1 && _Q && _W && _E && _R && _A && _S && _D && _F)
                                            {
                                                lbStatus.Invoke(
                                                    (MethodInvoker)(() => lbStatus.Text = "Bot is autoattacking..."));
                                                KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_C,
                                                    int.Parse(textBoxAutoAttack.Text));
                                                _walktopUTurn++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!isKeyOnCooldown(skill.Key))
                                        {
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is fighting..."));
                                            KeyboardWrapper.AlternateHoldKey(skill.Key, CasttimeByKey(skill.Key));

                                            if (IsDoubleKey(skill.Key))
                                            {
                                                KeyboardWrapper.PressKey(skill.Key);
                                            }

                                            SetKeyCooldown(skill.Key); // Set Cooldown
                                            var td = Task.Run(() => SkillCooldown(token, skill.Key));
                                            await Task.Delay(50, token);
                                            _walktopUTurn++;
                                        }
                                        else
                                        {
                                            if (int.Parse(textBoxAutoAttack.Text) >= 1 && _Q && _W && _E && _R && _A && _S && _D && _F)
                                            {
                                                lbStatus.Invoke(
                                                    (MethodInvoker)(() => lbStatus.Text = "Bot is autoattacking..."));
                                                KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_C,
                                                    int.Parse(textBoxAutoAttack.Text));
                                                _walktopUTurn++;
                                            }
                                        }
                                    }
                                }
                                    


                                if (_walktopUTurn == 4 && chBoxAutoMovement.Checked && _floor1 && _stopp == false)
                                {
                                    _canSearchEnemys = false;
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    VirtualMouse.MoveTo(Recalc(960), Recalc(240, false), 10);
                                    KeyboardWrapper.AlternateHoldKey(currentMouseButton, 1500);
                                    VirtualMouse.MoveTo(Recalc(960), Recalc(566, false), 10);
                                    KeyboardWrapper.PressKey(currentMouseButton);
                                    _canSearchEnemys = true;
                                    _walktopUTurn++;
                                }

                                if (_walktopUTurn == 11 && chBoxAutoMovement.Checked && _floor1 && _stopp == false)
                                {
                                    _canSearchEnemys = false;
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    VirtualMouse.MoveTo(Recalc(523), Recalc(840, false), 10);
                                    KeyboardWrapper.AlternateHoldKey(currentMouseButton, 2200);
                                    VirtualMouse.MoveTo(Recalc(1007), Recalc(494, false), 10);
                                    KeyboardWrapper.PressKey(currentMouseButton);
                                    await Task.Delay(1, token);
                                    _canSearchEnemys = true;
                                    _walktopUTurn++;
                                }

                                if (_walktopUTurn == 18 && chBoxAutoMovement.Checked && _floor1 && _stopp == false)
                                {
                                    _canSearchEnemys = false;
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);

                                    VirtualMouse.MoveTo(Recalc(1578), Recalc(524, false), 10);
                                    KeyboardWrapper.AlternateHoldKey(currentMouseButton, 2000);
                                    VirtualMouse.MoveTo(Recalc(905), Recalc(531, false), 10);
                                    KeyboardWrapper.PressKey(currentMouseButton);
                                    _canSearchEnemys = true;
                                    _walktopUTurn++;
                                }

                                if (_walktopUTurn == 24 && chBoxAutoMovement.Checked && _floor1 && _stopp == false)
                                {
                                    _canSearchEnemys = false;
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);

                                    VirtualMouse.MoveTo(Recalc(523), Recalc(850, false), 10);
                                    KeyboardWrapper.AlternateHoldKey(currentMouseButton, 2200);
                                    VirtualMouse.MoveTo(Recalc(960), Recalc(500, false), 10);
                                    KeyboardWrapper.PressKey(currentMouseButton);
                                    await Task.Delay(1, token);
                                    _canSearchEnemys = true;
                                    _walktopUTurn++;
                                }

                                if (_walktopUTurn == 24 && chBoxAutoMovement.Checked && _floor1 && _stopp == false)
                                {
                                    _walktopUTurn = 1;
                                    await Task.Delay(1, token);
                                }

                       //         await Task.Delay(humanizer.Next(10, 40), token);
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




     

        private async Task SEARCHBOSS(CancellationToken tokenSearchBoss)
        {
            try
            {
                cts.Cancel();
                cts.Dispose();
                cts = new CancellationTokenSource();
                token = cts.Token;

                if (_searchboss == true && _floorFight == false && _stopp == false )
                {
                    tokenSearchBoss.ThrowIfCancellationRequested();
                    await Task.Delay(1, tokenSearchBoss);
                    if (_floorint2 == 1)
                    {
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 2: search enemy..."));
                    }

                    if (_floorint2 == 2)
                    {
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 3: search enemy..."));
                    }

                    if (_searchSequence == 1)
                    {
                        await Task.Delay(humanizer.Next(10, 240) + 1500, tokenSearchBoss);
                        VirtualMouse.MoveTo(Recalc(960), Recalc(529, false), 10);
                        KeyboardWrapper.PressKey(currentMouseButton);
                        if (chBoxCooldownDetection.Checked && chBoxY.Checked)
                        {
                            GetSkillQ();
                            GetSkillW();
                            GetSkillE();
                            GetSkillR();
                            GetSkillA();
                            GetSkillS();
                            GetSkillD();
                            GetSkillF();
                        }
                        _searchSequence = 2;
                    }

                    if (chBoxGunlancer.Checked == true && _gunlancer == false && _searchboss == true)
                    {
                        tokenSearchBoss.ThrowIfCancellationRequested();
                        await Task.Delay(1, tokenSearchBoss);

                        KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 1000);
                        _gunlancer = true;

                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Deactivate: Gunlancer Ultimate"));
                    }

                    for (int i = 0; i < int.Parse(txtDungeon2search.Text) ; i++)
                    {
                        tokenSearchBoss.ThrowIfCancellationRequested();
                        await Task.Delay(humanizer.Next(10, 240) + 100, tokenSearchBoss);
                        float threshold = 0.705f;

                        var enemyTemplate = Image_enemy;
                        var enemyMask = Image_mask;
                        var BossTemplate = Image_boss1;
                        var BossMask = Image_bossmask1;
                        var mobTemplate = Image_mob1;
                        var mobMask =  Image_mobmask1;
                        var portalTemplate = Image_portalenter1;
                        var portalMask = Image_portalentermask1;

                        Point myPosition = new Point(Recalc(148), Recalc(127, false));
                        Point screenResolution = new Point(screenWidth, screenHeight);
                        var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);
                        var BossDetector = new EnemyDetector(BossTemplate, BossMask, threshold);
                        var mobDetector = new EnemyDetector(mobTemplate, mobMask, threshold);
                        var portalDetector = new EnemyDetector(portalTemplate, portalMask, threshold);
                        var screenPrinter = new PrintScreen();

                        var rawScreen = screenPrinter.CaptureScreen();
                        Bitmap bitmapImage = new Bitmap(rawScreen);
                        using (var screenCapture = bitmapImage.ToImage<Bgr, byte>())
                        {

                            var enemy = enemyDetector.GetClosestEnemy(screenCapture, false);
                            var Boss = BossDetector.GetClosestEnemy(screenCapture, false);
                            var mob = mobDetector.GetClosestEnemy(screenCapture, false);
                            var portal = portalDetector.GetClosestEnemy(screenCapture, false);

                            if (Boss.HasValue && _searchboss == true)
                            {
                                tokenSearchBoss.ThrowIfCancellationRequested();
                                CvInvoke.Rectangle(screenCapture,
                                    new Rectangle(new Point(Boss.Value.X, Boss.Value.Y), BossTemplate.Size),
                                    new MCvScalar(255));
                                double distance_x = (screenWidth - Recalc(296)) / 2;
                                double distance_y = (screenHeight - Recalc(255, false)) / 2;

                                var boss_position = ((Boss.Value.X + distance_x), (Boss.Value.Y + distance_y));
                                double multiplier = 1;
                                var boss_position_on_minimap = ((Boss.Value.X), (Boss.Value.Y));
                                var my_position_on_minimap = ((Recalc(296) / 2), (Recalc(255, false) / 2));
                                var dist = Math.Sqrt(
                                    Math.Pow((my_position_on_minimap.Item1 - boss_position_on_minimap.Item1), 2) +
                                    Math.Pow((my_position_on_minimap.Item2 - boss_position_on_minimap.Item2), 2));

                                if (dist < 180 && _searchboss == true)
                                {
                                    multiplier = 1.2;
                                }

                                double posx;
                                double posy;
                                if (boss_position.Item1 < (screenWidth / 2) && _searchboss == true)
                                {
                                    posx = boss_position.Item1 * (2 - multiplier);
                                }
                                else
                                {
                                    posx = boss_position.Item1 * multiplier;
                                }

                                if (boss_position.Item2 < (screenHeight / 2) && _searchboss == true)
                                {
                                    posy = boss_position.Item2 * (2 - multiplier);
                                }
                                else
                                {
                                    posy = boss_position.Item2 * multiplier;
                                }

                                var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);

                                if (_floorint2 == 1 && _searchboss == true)
                                {
                                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 2: Big-Boss found!"));
                                }

                                if (_floorint2 == 2 && _searchboss == true)
                                {
                                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 3: Big-Boss found!"));
                                }
                                tokenSearchBoss.ThrowIfCancellationRequested();
                                VirtualMouse.MoveTo(absolutePositions.Item1, absolutePositions.Item2);

                                KeyboardWrapper.AlternateHoldKey(currentMouseButton, 1000);
                            }
                            else
                            {
                                if (enemy.HasValue && _searchboss == true)
                                {
                                    tokenSearchBoss.ThrowIfCancellationRequested();
                                    CvInvoke.Rectangle(screenCapture,
                                        new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                                        new MCvScalar(255));
                                    double distance_x = (screenWidth - Recalc(296)) / 2;
                                    double distance_y = (screenHeight - Recalc(255, false)) / 2;

                                    var enemy_position = ((enemy.Value.X + distance_x), (enemy.Value.Y + distance_y));
                                    double multiplier = 1;
                                    var enemy_position_on_minimap = ((enemy.Value.X), (enemy.Value.Y));
                                    var my_position_on_minimap = ((Recalc(296) / 2), (Recalc(255, false) / 2));
                                    var dist = Math.Sqrt(
                                        Math.Pow((my_position_on_minimap.Item1 - enemy_position_on_minimap.Item1), 2) +
                                        Math.Pow((my_position_on_minimap.Item2 - enemy_position_on_minimap.Item2), 2));

                                    if (dist < 180 && _searchboss == true)
                                    {
                                        multiplier = 1.2;
                                    }

                                    double posx;
                                    double posy;
                                    if (enemy_position.Item1 < (screenWidth / 2) && _searchboss == true)
                                    {
                                        posx = enemy_position.Item1 * (2 - multiplier);
                                    }
                                    else
                                    {
                                        posx = enemy_position.Item1 * multiplier;
                                    }

                                    if (enemy_position.Item2 < (screenHeight / 2) && _searchboss == true)
                                    {
                                        posy = enemy_position.Item2 * (2 - multiplier);
                                    }
                                    else
                                    {
                                        posy = enemy_position.Item2 * multiplier;
                                    }

                                    var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);
                                    if (_floorint2 == 1 && _searchboss == true)
                                    {
                                        lbStatus.Invoke((MethodInvoker) (() =>
                                            lbStatus.Text = "Floor 2: Mid-Boss found!"));
                                    }

                                    if (_floorint2 == 2 && _searchboss == true)
                                    {
                                        lbStatus.Invoke((MethodInvoker) (() =>
                                            lbStatus.Text = "Floor 3: Mid-Boss found!"));
                                    }
                                    tokenSearchBoss.ThrowIfCancellationRequested();
                                    VirtualMouse.MoveTo(absolutePositions.Item1, absolutePositions.Item2);

                                    KeyboardWrapper.AlternateHoldKey(currentMouseButton, 1000);
                                }
                                else
                                {
                                    if (mob.HasValue && _searchboss == true)
                                    {
                                        tokenSearchBoss.ThrowIfCancellationRequested();
                                        CvInvoke.Rectangle(screenCapture,
                                            new Rectangle(new Point(mob.Value.X, mob.Value.Y), mobTemplate.Size),
                                            new MCvScalar(255));
                                        double distance_x = (screenWidth - Recalc(296)) / 2;
                                        double distance_y = (screenHeight - Recalc(255, false)) / 2;

                                        var mob_position = ((mob.Value.X + distance_x), (mob.Value.Y + distance_y));
                                        double multiplier = 1;
                                        var mob_position_on_minimap = ((mob.Value.X), (mob.Value.Y));
                                        var my_position_on_minimap = ((Recalc(296) / 2), (Recalc(255, false) / 2));
                                        var dist = Math.Sqrt(
                                            Math.Pow((my_position_on_minimap.Item1 - mob_position_on_minimap.Item1),
                                                2) +
                                            Math.Pow((my_position_on_minimap.Item2 - mob_position_on_minimap.Item2),
                                                2));

                                        if (dist < 180 && _searchboss == true)
                                        {
                                            multiplier = 1.2;
                                        }

                                        double posx;
                                        double posy;
                                        if (mob_position.Item1 < (screenWidth / 2) && _searchboss == true)
                                        {
                                            posx = mob_position.Item1 * (2 - multiplier);
                                        }
                                        else
                                        {
                                            posx = mob_position.Item1 * multiplier;
                                        }

                                        if (mob_position.Item2 < (screenHeight / 2) && _searchboss == true)
                                        {
                                            posy = mob_position.Item2 * (2 - multiplier);
                                        }
                                        else
                                        {
                                            posy = mob_position.Item2 * multiplier;
                                        }

                                        var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);
                                        if (_floorint2 == 1 && _searchboss == true)
                                        {
                                            lbStatus.Invoke(
                                                (MethodInvoker) (() => lbStatus.Text = "Floor 2: Mob found!"));
                                        }

                                        if (_floorint2 == 2 && _searchboss == true)
                                        {
                                            lbStatus.Invoke(
                                                (MethodInvoker) (() => lbStatus.Text = "Floor 3: Mob found!"));
                                        }
                                        tokenSearchBoss.ThrowIfCancellationRequested();
                                        VirtualMouse.MoveTo(absolutePositions.Item1, absolutePositions.Item2);

                                        KeyboardWrapper.AlternateHoldKey(currentMouseButton, 1000);
                                    }
                                }
                            }
                        }
                        tokenSearchBoss.ThrowIfCancellationRequested();
                        Random random = new Random();
                        var sleepTime = random.Next(100, 150);
                        await Task.Delay(sleepTime, tokenSearchBoss);
                    }
                    tokenSearchBoss.ThrowIfCancellationRequested();

                    if (_floorint2 == 1)
                    {
                        _floor1 = false;
                        _floor2 = true;
                        starten = true;
                        _bossKillDetection = true;
                      
                        var t36 = Task.Run(() => Leavetimerfloor2());
                        var t18 = Task.Run(() => BossKillDetection());
                    }

                    if (_floorint3 == 2)
                    {
                        _floor1 = false;
                        _floor2 = false;
                        
                    }

                    _searchboss = false;
                    _gunlancer = true;
                    _shadowhunter = true;
                    _berserker = true;
                    _paladin = true;
                    _sharpshooter = true;
                    _bard = true;
                    _sorcerer = true;
                    _soulfist = true;
                    _ultimate = true;
                    _floorFight = true;


                    cts = new CancellationTokenSource();
                    token = cts.Token;
                    var t14 = Task.Run(() => UltimateAttack(token),token);
                    var t12 = Task.Run(() => Floortime(token),token);
                   
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
    }
}