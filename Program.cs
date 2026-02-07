using System;
using System.IO;
using System.Net.Http;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text;

namespace VelocityFixer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Velocity Fixer";
            Console.ForegroundColor = ConsoleColor.Cyan;
            
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                  VELOCITY FIXER v1.0.1                         ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("  1. Install Velocity to Desktop ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("(may become outdated anytime)");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("  2. Install Dependencies");
                Console.WriteLine("  3. Fix Tabs");
                Console.WriteLine("  4. Setup Fishstrap");
                Console.WriteLine("  5. Setup Voidstrap");
                Console.WriteLine("  6. Change DNS Server");
                Console.WriteLine("  7. Fix Roblox Crashing");
                Console.WriteLine("  8. Fix Velocity Crashes");
                Console.WriteLine("  9. Exit\n");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("  → Select option: ");
                
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        await InstallVelocity();
                        break;
                    case "2":
                        await InstallDependencies();
                        break;
                    case "3":
                        await FixTabs();
                        break;
                    case "4":
                        await SetupFishstrap();
                        break;
                    case "5":
                        await SetupVoidstrap();
                        break;
                    case "6":
                        await ChangeDNS();
                        break;
                    case "7":
                        await FixRobloxCrashing();
                        break;
                    case "8":
                        await FixVelocityCrashes();
                        break;
                    case "9":
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n  [!] Invalid option.");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadKey();
                        break;
                }
            }
        }

        static async Task InstallVelocity()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                INSTALLING VELOCITY TO DESKTOP                  ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string velocityZipPath = Path.Combine(Path.GetTempPath(), "Velocity.zip");
                string velocityFolderPath = Path.Combine(desktopPath, "Velocity");

                LogWithTimestamp("Downloading...", ConsoleColor.Yellow);

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(10);
                    
                    var response = await client.GetAsync("https://pixeldrain.com/api/file/ShyU39kZ", HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();
                    
                    var totalBytes = response.Content.Headers.ContentLength ?? 0;
                    var buffer = new byte[8192];
                    var totalRead = 0L;
                    
                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream(velocityZipPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        int bytesRead;
                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            totalRead += bytesRead;
                            
                            if (totalBytes > 0)
                            {
                                var percentage = (int)((totalRead * 100) / totalBytes);
                                Console.Write($"\r    Progress: {percentage}% ({totalRead / 1024 / 1024}MB / {totalBytes / 1024 / 1024}MB)");
                            }
                        }
                    }
                    Console.WriteLine();
                }

                LogWithTimestamp($"Complete: {new FileInfo(velocityZipPath).Length / 1024 / 1024}MB", ConsoleColor.Green);
                LogWithTimestamp("Extracting to Desktop...", ConsoleColor.Yellow);
                
                if (Directory.Exists(velocityFolderPath))
                {
                    Directory.Delete(velocityFolderPath, true);
                }
                
                ZipFile.ExtractToDirectory(velocityZipPath, velocityFolderPath, true);
                File.Delete(velocityZipPath);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║          ✓ VELOCITY INSTALLED TO DESKTOP                      ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  [!] Error: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        static async Task InstallDependencies()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║               INSTALLING DEPENDENCIES                          ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                LogWithTimestamp("Installing DirectX 11...", ConsoleColor.Yellow);
                await RunInstaller("winget install -e --id Microsoft.DirectX --accept-source-agreements --accept-package-agreements");
                
                LogWithTimestamp("Installing .NET 8.0...", ConsoleColor.Yellow);
                await RunInstaller("winget install -e --id Microsoft.DotNet.Runtime.8 --accept-source-agreements --accept-package-agreements");
                
                LogWithTimestamp("Installing .NET 10.0...", ConsoleColor.Yellow);
                await RunInstaller("winget install -e --id Microsoft.DotNet.Runtime.Preview --accept-source-agreements --accept-package-agreements");
                
                LogWithTimestamp("Installing .NET Framework 4.8.1...", ConsoleColor.Yellow);
                await RunInstaller("winget install -e --id Microsoft.DotNet.Framework.DeveloperPack_4 --accept-source-agreements --accept-package-agreements");
                
                LogWithTimestamp("Installing Visual C++ Redistributables (All)...", ConsoleColor.Yellow);
                await RunInstaller("winget install -e --id Microsoft.VCRedist.2015+.x64 --accept-source-agreements --accept-package-agreements");
                await RunInstaller("winget install -e --id Microsoft.VCRedist.2015+.x86 --accept-source-agreements --accept-package-agreements");
                await RunInstaller("winget install -e --id Microsoft.VCRedist.2013.x64 --accept-source-agreements --accept-package-agreements");
                await RunInstaller("winget install -e --id Microsoft.VCRedist.2013.x86 --accept-source-agreements --accept-package-agreements");
                await RunInstaller("winget install -e --id Microsoft.VCRedist.2012.x64 --accept-source-agreements --accept-package-agreements");
                await RunInstaller("winget install -e --id Microsoft.VCRedist.2012.x86 --accept-source-agreements --accept-package-agreements");
                await RunInstaller("winget install -e --id Microsoft.VCRedist.2010.x64 --accept-source-agreements --accept-package-agreements");
                await RunInstaller("winget install -e --id Microsoft.VCRedist.2010.x86 --accept-source-agreements --accept-package-agreements");
                
                LogWithTimestamp("Installing DirectX End-User Runtime...", ConsoleColor.Yellow);
                string dxWebSetup = Path.Combine(Path.GetTempPath(), "dxwebsetup.exe");
                using (var client = new HttpClient())
                {
                    var data = await client.GetByteArrayAsync("https://download.microsoft.com/download/1/7/1/1718CCC4-6315-4D8E-9543-8E28A4E18C4C/dxwebsetup.exe");
                    File.WriteAllBytes(dxWebSetup, data);
                }
                await RunInstaller($"{dxWebSetup} /silent");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║          ✓ ALL DEPENDENCIES INSTALLED SUCCESSFULLY            ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  [!] Error: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        static async Task FixTabs()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                      FIXING TABS                               ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                string monacoUrl = "https://cdn.discordapp.com/attachments/1469649181584130201/1469649329156522046/fIGRvh7.zip?ex=69886d18&is=69871b98&hm=fce7b1be665cbd8d77412068ca6c15215fe80624f02d5b94db30c101234dc56e&";
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string targetPath = Path.Combine(localAppData, "Velocity Ui");
                string zipPath = Path.Combine(Path.GetTempPath(), "MonacoEditor.zip");

                Directory.CreateDirectory(targetPath);

                LogWithTimestamp("Downloading...", ConsoleColor.Yellow);
                
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(10);
                    
                    var response = await client.GetAsync(monacoUrl, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();
                    
                    var totalBytes = response.Content.Headers.ContentLength ?? 0;
                    var buffer = new byte[8192];
                    var totalRead = 0L;
                    
                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        int bytesRead;
                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            totalRead += bytesRead;
                            
                            if (totalBytes > 0)
                            {
                                var percentage = (int)((totalRead * 100) / totalBytes);
                                Console.Write($"\r    Progress: {percentage}% ({totalRead / 1024 / 1024}MB / {totalBytes / 1024 / 1024}MB)");
                            }
                        }
                    }
                    Console.WriteLine();
                }

                LogWithTimestamp($"Complete: {new FileInfo(zipPath).Length / 1024 / 1024}MB", ConsoleColor.Green);
                LogWithTimestamp("Extracting...", ConsoleColor.Yellow);
                
                ZipFile.ExtractToDirectory(zipPath, targetPath, true);
                File.Delete(zipPath);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║               ✓ TABS FIXED SUCCESSFULLY                        ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  [!] Error: {ex.Message}");
                Console.WriteLine($"  [!] Stack Trace: {ex.StackTrace}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        static async Task SetupFishstrap()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            
            string fishstrapPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Fishstrap");
            bool fishstrapExists = Directory.Exists(fishstrapPath);
            
            if (fishstrapExists)
            {
                Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                   SETTING UP FISHSTRAP                         ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            }
            else
            {
                Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║            SETTING UP FISHSTRAP (Unavailable)                  ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            }
            
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                string settingsPath = Path.Combine(fishstrapPath, "Settings.json");

                if (!fishstrapExists)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  [!] Fishstrap not found. Please install Fishstrap first.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }

                string versionsPath = Path.Combine(fishstrapPath, "Versions");
                if (Directory.Exists(versionsPath))
                {
                    LogWithTimestamp("Removing old versions...", ConsoleColor.Yellow);
                    Directory.Delete(versionsPath, true);
                }

                LogWithTimestamp("Configuring...", ConsoleColor.Yellow);

                string jsonContent = @"{
  ""BootstrapperStyle"": 5,
  ""BootstrapperIcon"": 6,
  ""BootstrapperTitle"": ""True shit brotha"",
  ""BootstrapperIconCustomLocation"": """",
  ""Theme"": 0,
  ""ForceLocalData"": false,
  ""CheckForUpdates"": true,
  ""MultiInstanceLaunching"": false,
  ""ConfirmLaunches"": true,
  ""Locale"": ""en-US"",
  ""ForceRobloxLanguage"": false,
  ""UseFastFlagManager"": false,
  ""WPFSoftwareRender"": false,
  ""EnableAnalytics"": false,
  ""UpdateRoblox"": true,
  ""Channel"": ""production"",
  ""ChannelChangeMode"": 2,
  ""ChannelHash"": """",
  ""DownloadingStringFormat"": ""Downloading {0} - {1}MB / {2}MB"",
  ""SelectedCustomTheme"": null,
  ""BackgroundUpdatesEnabled"": false,
  ""DebugDisableVersionPackageCleanup"": false,
  ""WebEnvironment"": ""Production"",
  ""CleanerOptions"": 1,
  ""CleanerDirectories"": [
    ""RobloxCache"",
    ""RobloxLogs"",
    ""FishstrapLogs""
  ],
  ""EnableActivityTracking"": true,
  ""UseDiscordRichPresence"": true,
  ""HideRPCButtons"": true,
  ""ShowAccountOnRichPresence"": false,
  ""ShowServerDetails"": true,
  ""ShowServerUptime"": false,
  ""CustomIntegrations"": [],
  ""UseDisableAppPatch"": false
}";

                File.WriteAllText(settingsPath, jsonContent);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║            ✓ FISHSTRAP CONFIGURED SUCCESSFULLY                 ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  [!] Error: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        static async Task SetupVoidstrap()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            
            string voidstrapPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Voidstrap");
            bool voidstrapExists = Directory.Exists(voidstrapPath);
            
            if (voidstrapExists)
            {
                Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                   SETTING UP VOIDSTRAP                         ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            }
            else
            {
                Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║            SETTING UP VOIDSTRAP (Unavailable)                  ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            }
            
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                string settingsPath = Path.Combine(voidstrapPath, "AppSettings.json");

                if (!voidstrapExists)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  [!] Voidstrap not found. Please install Voidstrap first.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }

                string versionsPath = Path.Combine(voidstrapPath, "Versions");
                string rblxVersionsPath = Path.Combine(voidstrapPath, "RblxVersions");
                
                if (Directory.Exists(versionsPath))
                {
                    Directory.Delete(versionsPath, true);
                }
                
                if (Directory.Exists(rblxVersionsPath))
                {
                    Directory.Delete(rblxVersionsPath, true);
                }

                LogWithTimestamp("Configuring...", ConsoleColor.Yellow);

                string jsonContent = @"{
  ""BootstrapperStyle"": 7,
  ""BootstrapperIcon"": 0,
  ""CleanerOptions"": 1,
  ""CleanerDirectories"": [
    ""RobloxCache"",
    ""VoidstrapLogs"",
    ""RobloxLogs""
  ],
  ""BootstrapperTitle"": ""Voidstrap"",
  ""BootstrapperIconCustomLocation"": """",
  ""Theme2"": 4,
  ""SelectedCustomTheme"": null,
  ""CheckForUpdates"": true,
  ""SelectedCpuPriority"": ""Automatic"",
  ""MaxCpuCores"": 12,
  ""TotalLogicalCores"": 12,
  ""TotalPhysicalCores"": 6,
  ""IsChannelEnabled"": true,
  ""UpdateRoblox"": true,
  ""DisableCrash"": true,
  ""CpuCoreLimit"": 12,
  ""ShiftlockCursorSelectedPath"": """",
  ""UseCustomIcon"": """",
  ""CustomGameName"": """",
  ""PriorityLimit"": ""High"",
  ""SelectedStatus"": ""Gray"",
  ""ArrowCursorSelectedPath"": """",
  ""ArrowFarCursorSelectedPath"": """",
  ""IBeamCursorSelectedPath"": """",
  ""DisableSplashScreen"": true,
  ""EnableAnalytics"": true,
  ""ShouldExportConfig"": true,
  ""ShouldExportLogs"": true,
  ""UseFastFlagManager"": false,
  ""WPFSoftwareRender"": false,
  ""FixTeleports"": true,
  ""ConfirmLaunches"": true,
  ""HasLaunchedGame"": false,
  ""BackgroundWindow"": true,
  ""UsePlaceId"": false,
  ""PlaceId"": """",
  ""OptimizeRoblox"": false,
  ""BackgroundUpdatesEnabled"": true,
  ""VoidNotify"": true,
  ""MultiInstanceLaunching"": true,
  ""ServerPingCounter"": false,
  ""ShowServerDetailsUI"": false,
  ""EnableCustomStatusDisplay"": true,
  ""RenameClientToEuroTrucks2"": false,
  ""SnowWOWSOCOOLWpfSnowbtw"": false,
  ""MotionBlurOverlay"": false,
  ""MotionBlurOverlay2"": false,
  ""ClientPath"": ""Roblox\\Player"",
  ""Locale"": ""en-US"",
  ""BufferSizeKbte"": ""1024"",
  ""BufferSizeKbtes"": ""2048"",
  ""SkyboxName"": ""Pandora"",
  ""FontName"": ""Default"",
  ""LastServerSave"": ""112757576021097"",
  ""SkyBoxDataSending"": false,
  ""FFlagRPCDisplayer"": true,
  ""FPSCounter"": false,
  ""CurrentTimeDisplay"": false,
  ""ExclusiveFullscreen"": false,
  ""Crosshair"": false,
  ""LockDefault"": false,
  ""GameWIP"": false,
  ""ForceRobloxLanguage"": true,
  ""DarkTextures"": false,
  ""EnableActivityTracking"": true,
  ""OverClockCPU"": false,
  ""exitondissy"": false,
  ""ServerUptimeBetterBLOXcuzitsbetterXD"": true,
  ""DownloadingStringFormat"": ""Downloading {0} - {1}MB / {2}MB"",
  ""ConnectCloset"": false,
  ""Fullbright"": false,
  ""GameIconChecked"": true,
  ""ServerLocationGame"": false,
  ""GameNameChecked"": true,
  ""GameCreatorChecked"": true,
  ""GameStatusChecked"": true,
  ""DX12Like"": true,
  ""UseDiscordRichPresence"": true,
  ""HideRPCButtons"": true,
  ""ShowAccountOnRichPresence"": false,
  ""MultiAccount"": false,
  ""ShowServerDetails"": true,
  ""CustomFontLocation"": """",
  ""CursorType"": 0,
  ""CustomIntegrations"": [],
  ""UseDisableAppPatch"": false,
  ""Channel"": ""production"",
  ""ChannelHash"": """",
  ""LaunchGameID"": """",
  ""IsGameEnabled"": false,
  ""MatchUniverseId"": true,
  ""TargetUniverseId"": null,
  ""IsBetterServersEnabled"": false,
  ""OverClockGPU"": false,
  ""GRADmentFR"": false,
  ""VoidRPC"": false,
  ""AntiAFK"": false,
  ""InGameResolution"": null
}";

                File.WriteAllText(settingsPath, jsonContent);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║            ✓ VOIDSTRAP CONFIGURED SUCCESSFULLY                 ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  [!] Error: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        static void LogWithTimestamp(string message, ConsoleColor color)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"  [{timestamp}] ");
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        static async Task RunInstaller(string command)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = false
                }
            };

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    Console.WriteLine($"    {e.Data}");
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"    {e.Data}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            await process.WaitForExitAsync();
        }
        static async Task ChangeDNS()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                   CHANGING DNS SERVER                          ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                LogWithTimestamp("Getting network adapters...", ConsoleColor.Yellow);
                
                var getAdapters = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = "-Command \"Get-NetAdapter | Where-Object {$_.Status -eq 'Up'} | Select-Object -ExpandProperty Name\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };
                
                getAdapters.Start();
                string adapterOutput = await getAdapters.StandardOutput.ReadToEndAsync();
                await getAdapters.WaitForExitAsync();
                
                var adapters = adapterOutput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var adapter in adapters)
                {
                    LogWithTimestamp($"Setting Cloudflare DNS on {adapter}...", ConsoleColor.Yellow);
                    
                    await RunInstaller($"netsh interface ip set dns \"{adapter}\" static 1.1.1.1");
                    await RunInstaller($"netsh interface ip add dns \"{adapter}\" 1.0.0.1 index=2");
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║            ✓ DNS CHANGED TO CLOUDFLARE                        ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  [!] Error: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        static async Task FixRobloxCrashing()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                   FIXING ROBLOX CRASHING                       ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                
                string robloxPath = Path.Combine(localAppData, "Roblox");
                string bloxstrapPath = Path.Combine(localAppData, "Bloxstrap");
                string fishstrapPath = Path.Combine(localAppData, "Fishstrap");
                string voidstrapPath = Path.Combine(localAppData, "Voidstrap");

                bool anyExists = Directory.Exists(robloxPath) || Directory.Exists(bloxstrapPath) || 
                                Directory.Exists(fishstrapPath) || Directory.Exists(voidstrapPath);

                if (!anyExists)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  [!] No Roblox/Bloxstrap/Fishstrap/Voidstrap folders found.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  ⚠️  WARNING: This will delete cache, logs, and settings.");
                Console.WriteLine("  ⚠️  This action will log you out of your account.");
                Console.Write("\n  Are you sure you want to continue? (y/n): ");
                Console.ForegroundColor = ConsoleColor.White;
                
                string confirmation = Console.ReadLine()?.ToLower();
                
                if (confirmation != "y" && confirmation != "yes")
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n  Operation cancelled.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }

                var foldersToDelete = new[] { "UniversalApp", "rbx-storage", "Versions", "LocalStorage" };
                var filesToDelete = new[] 
                { 
                    "rbx-storage.db", 
                    "rbx-storage.db-shm", 
                    "rbx-storage.db-wal", 
                    "rbx-storage.id", 
                    "AnalysticsSettings.xml", 
                    "frm.cfg", 
                    "GlobalBasicSettings_13.xml" 
                };

                var bootstrappers = new[] 
                { 
                    ("Roblox", robloxPath),
                    ("Bloxstrap", bloxstrapPath),
                    ("Fishstrap", fishstrapPath),
                    ("Voidstrap", voidstrapPath)
                };

                foreach (var (name, path) in bootstrappers)
                {
                    if (!Directory.Exists(path))
                        continue;

                    LogWithTimestamp($"Processing {name}...", ConsoleColor.Yellow);

                    foreach (var folder in foldersToDelete)
                    {
                        string folderPath = Path.Combine(path, folder);
                        if (Directory.Exists(folderPath))
                        {
                            try
                            {
                                Directory.Delete(folderPath, true);
                            }
                            catch
                            {
                            }
                        }
                    }

                    foreach (var file in filesToDelete)
                    {
                        string filePath = Path.Combine(path, file);
                        if (File.Exists(filePath))
                        {
                            try
                            {
                                File.Delete(filePath);
                            }
                            catch
                            {
                            }
                        }
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║            ✓ ROBLOX CRASH FIX APPLIED                         ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
                Console.ForegroundColor = ConsoleColor.White;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n  Do you want to delete temp files? (y/n): ");
                Console.ForegroundColor = ConsoleColor.White;
                string response = Console.ReadLine()?.ToLower();

                if (response == "y" || response == "yes")
                {
                    LogWithTimestamp("Cleaning temp files...", ConsoleColor.Yellow);
                    
                    string tempPath = Path.GetTempPath();
                    var tempFiles = Directory.GetFiles(tempPath, "*.*", SearchOption.TopDirectoryOnly);
                    int deletedCount = 0;
                    
                    foreach (var tempFile in tempFiles)
                    {
                        try
                        {
                            File.Delete(tempFile);
                            deletedCount++;
                        }
                        catch
                        {
                        }
                    }
                    
                    LogWithTimestamp($"Deleted {deletedCount} temp files", ConsoleColor.Green);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  [!] Error: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        static async Task FixVelocityCrashes()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                  FIXING VELOCITY CRASHES                       ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                Console.Write("  Enter the folder path where Velocity.exe is located: ");
                string velocityFolder = Console.ReadLine()?.Trim().Trim('"');

                if (string.IsNullOrEmpty(velocityFolder))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n  [!] No path provided.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }

                string velocityExePath = Path.Combine(velocityFolder, "Velocity.exe");
                
                if (!File.Exists(velocityExePath))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n  [!] Velocity.exe not found in {velocityFolder}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }

                LogWithTimestamp("Downloading crash fix...", ConsoleColor.Yellow);

                string fixUrl = "https://pixeldrain.com/api/file/abwiH8SV";
                string zipPath = Path.Combine(Path.GetTempPath(), "VelocityCrashFix.zip");

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(10);
                    
                    var response = await client.GetAsync(fixUrl, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();
                    
                    var totalBytes = response.Content.Headers.ContentLength ?? 0;
                    var buffer = new byte[8192];
                    var totalRead = 0L;
                    
                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        int bytesRead;
                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            totalRead += bytesRead;
                            
                            if (totalBytes > 0)
                            {
                                var percentage = (int)((totalRead * 100) / totalBytes);
                                Console.Write($"\r    Progress: {percentage}% ({totalRead / 1024 / 1024}MB / {totalBytes / 1024 / 1024}MB)");
                            }
                        }
                    }
                    Console.WriteLine();
                }

                LogWithTimestamp($"Complete: {new FileInfo(zipPath).Length / 1024 / 1024}MB", ConsoleColor.Green);
                LogWithTimestamp("Extracting to Velocity folder...", ConsoleColor.Yellow);
                
                ZipFile.ExtractToDirectory(zipPath, velocityFolder, true);
                File.Delete(zipPath);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║           ✓ VELOCITY CRASH FIX APPLIED                        ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  [!] Error: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }
}
