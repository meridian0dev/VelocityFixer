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
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("║                  VELOCITY FIXER v1.0.2                         ║");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  1. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Install Velocity to Desktop ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("(may become outdated anytime)");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  2. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Install Dependencies");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  3. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Fix Tabs");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  4. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Setup Fishstrap");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  5. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Setup Voidstrap");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  6. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Change DNS Server ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("(cloudflare/google)");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  7. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Fix Roblox Crashing");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  8. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Fix Velocity Crashes ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("(fixes on inject and on startup crashes)");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  9. ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exit\n");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("  → Select option: ");
                Console.ForegroundColor = ConsoleColor.White;
                
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
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("║                INSTALLING VELOCITY TO DESKTOP                  ║");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string velocityZipPath = Path.Combine(Path.GetTempPath(), "Velocity.zip");
                string velocityFolderPath = Path.Combine(desktopPath, "Velocity");

                LogWithTimestamp("Downloading...", ConsoleColor.Yellow);

                var client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(10);
                
                var response = await client.GetAsync("https://pixeldrain.com/api/file/rTjRFR8w", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                
                long totalBytes = response.Content.Headers.ContentLength ?? 0;
                byte[] buffer = new byte[8192];
                long totalRead = 0;
                
                var contentStream = await response.Content.ReadAsStreamAsync();
                var fileStream = new FileStream(velocityZipPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
                
                int bytesRead;
                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                    totalRead = totalRead + bytesRead;
                    
                    if (totalBytes > 0)
                    {
                        int percentage = (int)((totalRead * 100) / totalBytes);
                        int downloadedMB = (int)(totalRead / 1024 / 1024);
                        int totalMB = (int)(totalBytes / 1024 / 1024);
                        
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write("    ");
                        
                        int barWidth = 40;
                        int filled = (percentage * barWidth) / 100;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("[");
                        Console.Write(new string('█', filled));
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(new string('░', barWidth - filled));
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"] ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write($"{percentage}% ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"({downloadedMB}MB/{totalMB}MB)");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                
                Console.WriteLine();
                fileStream.Close();
                contentStream.Close();
                client.Dispose();

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
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("║               INSTALLING DEPENDENCIES                          ║");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                LogWithTimestamp("Installing DirectX Runtime...", ConsoleColor.Yellow);
                string dxWebSetup = Path.Combine(Path.GetTempPath(), "dxwebsetup.exe");
                var client = new HttpClient();
                byte[] data = await client.GetByteArrayAsync("https://download.microsoft.com/download/1/7/1/1718CCC4-6315-4D8E-9543-8E28A4E18C4C/dxwebsetup.exe");
                File.WriteAllBytes(dxWebSetup, data);
                client.Dispose();
                await RunInstaller($"{dxWebSetup} /silent");
                
                LogWithTimestamp("Installing .NET 8.0...", ConsoleColor.Yellow);
                await RunInstaller("winget install --id Microsoft.DotNet.Runtime.8 --silent --accept-source-agreements --accept-package-agreements");
                
                LogWithTimestamp("Installing .NET Desktop Runtime 8.0...", ConsoleColor.Yellow);
                await RunInstaller("winget install --id Microsoft.DotNet.DesktopRuntime.8 --silent --accept-source-agreements --accept-package-agreements");
                
                LogWithTimestamp("Installing .NET 10.0...", ConsoleColor.Yellow);
                await RunInstaller("winget install --id Microsoft.DotNet.Runtime.Preview --silent --accept-source-agreements --accept-package-agreements");
                
                LogWithTimestamp("Installing .NET Framework 4.8...", ConsoleColor.Yellow);
                await RunInstaller("winget install --id Microsoft.DotNet.Framework.DeveloperPack_4.8 --silent --accept-source-agreements --accept-package-agreements");
                
                LogWithTimestamp("Installing .NET Framework 4.8.1...", ConsoleColor.Yellow);
                await RunInstaller("winget install --id Microsoft.DotNet.Framework.DeveloperPack_4 --silent --accept-source-agreements --accept-package-agreements");
                
                LogWithTimestamp("Installing Visual C++ Redistributables (All)...", ConsoleColor.Yellow);
                await RunInstaller("winget install --id Microsoft.VCRedist.2015+.x64 --silent --accept-source-agreements --accept-package-agreements");
                await RunInstaller("winget install --id Microsoft.VCRedist.2015+.x86 --silent --accept-source-agreements --accept-package-agreements");
                await RunInstaller("winget install --id Microsoft.VCRedist.2013.x64 --silent --accept-source-agreements --accept-package-agreements");
                await RunInstaller("winget install --id Microsoft.VCRedist.2013.x86 --silent --accept-source-agreements --accept-package-agreements");
                await RunInstaller("winget install --id Microsoft.VCRedist.2012.x64 --silent --accept-source-agreements --accept-package-agreements");
                await RunInstaller("winget install --id Microsoft.VCRedist.2012.x86 --silent --accept-source-agreements --accept-package-agreements");
                await RunInstaller("winget install --id Microsoft.VCRedist.2010.x64 --silent --accept-source-agreements --accept-package-agreements");
                await RunInstaller("winget install --id Microsoft.VCRedist.2010.x86 --silent --accept-source-agreements --accept-package-agreements");

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
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("║                      FIXING TABS                               ║");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                string monacoUrl = "https://pixeldrain.com/api/file/x4SyWnYG";
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string targetPath = Path.Combine(localAppData, "Velocity Ui");
                string zipPath = Path.Combine(Path.GetTempPath(), "MonacoEditor.zip");

                Directory.CreateDirectory(targetPath);

                LogWithTimestamp("Downloading...", ConsoleColor.Yellow);
                
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(10);
                
                var response = await client.GetAsync(monacoUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                
                long totalBytes = response.Content.Headers.ContentLength ?? 0;
                byte[] buffer = new byte[8192];
                long totalRead = 0;
                
                var contentStream = await response.Content.ReadAsStreamAsync();
                var fileStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
                
                int bytesRead;
                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                    totalRead = totalRead + bytesRead;
                    
                    if (totalBytes > 0)
                    {
                        int percentage = (int)((totalRead * 100) / totalBytes);
                        int downloadedMB = (int)(totalRead / 1024 / 1024);
                        int totalMB = (int)(totalBytes / 1024 / 1024);
                        
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write("    ");
                        
                        int barWidth = 40;
                        int filled = (percentage * barWidth) / 100;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("[");
                        Console.Write(new string('█', filled));
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(new string('░', barWidth - filled));
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"] ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write($"{percentage}% ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"({downloadedMB}MB/{totalMB}MB)");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                
                Console.WriteLine();
                fileStream.Close();
                contentStream.Close();
                client.Dispose();

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
            Console.ForegroundColor = ConsoleColor.Magenta;
            
            string fishstrapPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Fishstrap");
            bool fishstrapExists = Directory.Exists(fishstrapPath);
            
            if (fishstrapExists)
            {
                Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("║                   SETTING UP FISHSTRAP                         ║");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            }
            else
            {
                Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("║            SETTING UP FISHSTRAP (Unavailable)                  ║");
                Console.ForegroundColor = ConsoleColor.Magenta;
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

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  ⚠️  WARNING: This will delete your original Fishstrap settings.");
                Console.Write("\n  Continue? (y/n): ");
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
            Console.ForegroundColor = ConsoleColor.Magenta;
            
            string voidstrapPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Voidstrap");
            bool voidstrapExists = Directory.Exists(voidstrapPath);
            
            if (voidstrapExists)
            {
                Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("║                   SETTING UP VOIDSTRAP                         ║");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            }
            else
            {
                Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("║            SETTING UP VOIDSTRAP (Unavailable)                  ║");
                Console.ForegroundColor = ConsoleColor.Magenta;
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

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  ⚠️  WARNING: This will delete your original Voidstrap settings.");
                Console.Write("\n  Continue? (y/n): ");
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

        static async Task ChangeDNS()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("║                   CHANGING DNS SERVER                          ║");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("  Select DNS provider:\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  1. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Cloudflare ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("(1.1.1.1)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  2. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Google ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("(8.8.8.8)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  3. ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Cancel\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("  → Select option: ");
            Console.ForegroundColor = ConsoleColor.White;
            
            string dnsChoice = Console.ReadLine();
            
            string primaryDNS = "";
            string secondaryDNS = "";
            string providerName = "";
            
            if (dnsChoice == "1")
            {
                primaryDNS = "1.1.1.1";
                secondaryDNS = "1.0.0.1";
                providerName = "CLOUDFLARE";
            }
            else if (dnsChoice == "2")
            {
                primaryDNS = "8.8.8.8";
                secondaryDNS = "8.8.4.4";
                providerName = "GOOGLE";
            }
            else if (dnsChoice == "3")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n  Operation cancelled.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  [!] Invalid option.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }

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
                
                string[] adapters = adapterOutput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                
                foreach (string adapter in adapters)
                {
                    LogWithTimestamp($"Setting {providerName} DNS on {adapter}...", ConsoleColor.Yellow);
                    
                    await RunInstaller($"netsh interface ip set dns \"{adapter}\" static {primaryDNS}");
                    await RunInstaller($"netsh interface ip add dns \"{adapter}\" {secondaryDNS} index=2");
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n╔════════════════════════════════════════════════════════════════╗");
                Console.WriteLine($"║            ✓ DNS CHANGED TO {providerName}                        ║");
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
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("║                   FIXING ROBLOX CRASHING                       ║");
            Console.ForegroundColor = ConsoleColor.Magenta;
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

                string[] foldersToDelete = new[] { "UniversalApp", "rbx-storage", "Versions", "LocalStorage" };
                string[] filesToDelete = new[] 
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

                foreach (var item in bootstrappers)
                {
                    string name = item.Item1;
                    string path = item.Item2;
                    
                    if (!Directory.Exists(path))
                        continue;

                    LogWithTimestamp($"Processing {name}...", ConsoleColor.Yellow);

                    foreach (string folder in foldersToDelete)
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

                    foreach (string file in filesToDelete)
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
                    string[] tempFiles = Directory.GetFiles(tempPath, "*.*", SearchOption.TopDirectoryOnly);
                    int deletedCount = 0;
                    
                    foreach (string tempFile in tempFiles)
                    {
                        try
                        {
                            File.Delete(tempFile);
                            deletedCount = deletedCount + 1;
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
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("║                  FIXING VELOCITY CRASHES                       ║");
            Console.ForegroundColor = ConsoleColor.Magenta;
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

                var client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(10);
                
                var response = await client.GetAsync(fixUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                
                long totalBytes = response.Content.Headers.ContentLength ?? 0;
                byte[] buffer = new byte[8192];
                long totalRead = 0;
                
                var contentStream = await response.Content.ReadAsStreamAsync();
                var fileStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
                
                int bytesRead;
                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                    totalRead = totalRead + bytesRead;
                    
                    if (totalBytes > 0)
                    {
                        int percentage = (int)((totalRead * 100) / totalBytes);
                        int downloadedMB = (int)(totalRead / 1024 / 1024);
                        int totalMB = (int)(totalBytes / 1024 / 1024);
                        
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write("    ");
                        
                        int barWidth = 40;
                        int filled = (percentage * barWidth) / 100;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("[");
                        Console.Write(new string('█', filled));
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(new string('░', barWidth - filled));
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"] ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write($"{percentage}% ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"({downloadedMB}MB/{totalMB}MB)");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                
                Console.WriteLine();
                fileStream.Close();
                contentStream.Close();
                client.Dispose();

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

        static void LogWithTimestamp(string message, ConsoleColor color)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
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
    }
}
