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
            Console.Title = "Velocity Fixer v1.0.3 | by meridian_dev";
            Console.ForegroundColor = ConsoleColor.Blue;
            
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                  VELOCITY FIXER v1.0.3                         ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("  1. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Install Velocity to Desktop ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("(may become outdated anytime)");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("  2. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Install Dependencies");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("  3. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Fix Tabs");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("  4. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Setup Fishstrap");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("  5. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Setup Voidstrap");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("  6. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Change DNS Server ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("(cloudflare/google)");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("  7. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Fix Roblox Crashing");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("  8. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Fix Velocity Crashes ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("(fixes on inject and on startup crashes)");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("  9. ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exit\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("  → Select option: ");
                Console.ForegroundColor = ConsoleColor.White;
                
                string choice = Console.ReadLine() ?? string.Empty;
                
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
                        Console.ForegroundColor = ConsoleColor.Red;                        Console.WriteLine("\n  [!] Invalid option.");
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

                LogWithTimestamp("Downloading...", ConsoleColor.DarkYellow);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  (Catbox can be slow, please be patient)\n");
                Console.ForegroundColor = ConsoleColor.White;

                var client = new HttpClient
                {
                    Timeout = TimeSpan.FromMinutes(10)
                };
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
                
                var response = await client.GetAsync("https://files.catbox.moe/u6xx7q.zip", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                
                long totalBytes = response.Content.Headers.ContentLength ?? 0;
                byte[] buffer = new byte[8192];
                long totalRead = 0;
                
                var contentStream = await response.Content.ReadAsStreamAsync();
                using var fileStream = new FileStream(velocityZipPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
                
                int bytesRead;
                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                    totalRead += bytesRead;
                    
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
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($"{percentage}% ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"({downloadedMB}MB/{totalMB}MB)");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                
                fileStream.Close();
                contentStream.Close();

                Console.WriteLine();
                LogWithTimestamp($"Download complete: {new FileInfo(velocityZipPath).Length / 1024 / 1024}MB", ConsoleColor.Green);
                LogWithTimestamp("Extracting to Desktop...", ConsoleColor.DarkYellow);
                
                if (Directory.Exists(velocityFolderPath))
                    Directory.Delete(velocityFolderPath, true);
                
                ZipFile.ExtractToDirectory(velocityZipPath, velocityFolderPath, true);
                File.Delete(velocityZipPath);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║          ✓ VELOCITY INSTALLED TO DESKTOP                      ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  Location: {velocityFolderPath}");
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

            Console.WriteLine("  Select package manager:\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("  1. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("WinGet ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("(Built into Windows 10+)");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("  2. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Chocolatey ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("(Requires choco installed)");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("  3. ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Go Back\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("  → Select option: ");
            Console.ForegroundColor = ConsoleColor.White;
            
            string installChoice = Console.ReadLine() ?? string.Empty;
            
            if (installChoice == "3")
            {
                return;
            }
            else if (installChoice != "1" && installChoice != "2")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  [!] Invalid option.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }

            bool useChoco = installChoice == "2";

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║               INSTALLING DEPENDENCIES                          ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                LogWithTimestamp("Downloading DirectX Runtime...", ConsoleColor.DarkYellow);
                string dxWebSetup = Path.Combine(Path.GetTempPath(), "dxwebsetup.exe");
                
                using (var client = new HttpClient())
                {
                    byte[] data = await client.GetByteArrayAsync("https://download.microsoft.com/download/1/7/1/1718CCC4-6315-4D8E-9543-8E28A4E18C4C/dxwebsetup.exe");
                    File.WriteAllBytes(dxWebSetup, data);
                }
                
                LogWithTimestamp("Installing DirectX...", ConsoleColor.DarkYellow);
                await RunInstallerWithProgress($"{dxWebSetup} /silent");
                
                if (useChoco)
                {
                    LogWithTimestamp("Installing .NET 8.0 Runtime...", ConsoleColor.DarkYellow);
                    await RunInstallerWithProgress("choco install dotnet-8.0-runtime -y");
                    
                    LogWithTimestamp("Installing .NET 8.0 Desktop Runtime...", ConsoleColor.DarkYellow);
                    await RunInstallerWithProgress("choco install dotnet-8.0-desktopruntime -y");
                    
                    LogWithTimestamp("Installing .NET 10.0 Preview...", ConsoleColor.DarkYellow);
                    await RunInstallerWithProgress("choco install dotnet-runtime-preview -y");
                    
                    LogWithTimestamp("Installing .NET Framework 4.8...", ConsoleColor.DarkYellow);
                    await RunInstallerWithProgress("choco install netfx-4.8 -y");
                    
                    LogWithTimestamp("Installing .NET Framework 4.8.1...", ConsoleColor.DarkYellow);
                    await RunInstallerWithProgress("choco install netfx-4.8.1 -y");
                    
                    LogWithTimestamp("Installing Visual C++ Redistributables...", ConsoleColor.DarkYellow);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("  (This may take a while, installing all versions)\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    
                    await RunInstallerWithProgress("choco install vcredist-all -y");
                }
                else
                {
                    LogWithTimestamp("Installing .NET 8.0 Runtime...", ConsoleColor.DarkYellow);
                    await RunInstallerWithProgress("winget install --id Microsoft.DotNet.Runtime.8 --silent --accept-source-agreements --accept-package-agreements");
                    
                    LogWithTimestamp("Installing .NET 8.0 Desktop Runtime...", ConsoleColor.DarkYellow);
                    await RunInstallerWithProgress("winget install --id Microsoft.DotNet.DesktopRuntime.8 --silent --accept-source-agreements --accept-package-agreements");
                    
                    LogWithTimestamp("Installing .NET 10.0 Preview...", ConsoleColor.DarkYellow);
                    await RunInstallerWithProgress("winget install --id Microsoft.DotNet.Runtime.Preview --silent --accept-source-agreements --accept-package-agreements");
                    
                    LogWithTimestamp("Installing .NET Framework 4.8...", ConsoleColor.DarkYellow);
                    await RunInstallerWithProgress("winget install --id Microsoft.DotNet.Framework.DeveloperPack_4.8 --silent --accept-source-agreements --accept-package-agreements");
                    
                    LogWithTimestamp("Installing .NET Framework 4.8.1...", ConsoleColor.DarkYellow);
                    await RunInstallerWithProgress("winget install --id Microsoft.DotNet.Framework.DeveloperPack_4 --silent --accept-source-agreements --accept-package-agreements");
                    
                    LogWithTimestamp("Installing Visual C++ Redistributables...", ConsoleColor.DarkYellow);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("  (This may take a while, installing all versions)\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    
                    await RunInstallerWithProgress("winget install --id Microsoft.VCRedist.2015+.x64 --silent --accept-source-agreements --accept-package-agreements");
                    await RunInstallerWithProgress("winget install --id Microsoft.VCRedist.2015+.x86 --silent --accept-source-agreements --accept-package-agreements");
                    await RunInstallerWithProgress("winget install --id Microsoft.VCRedist.2013.x64 --silent --accept-source-agreements --accept-package-agreements");
                    await RunInstallerWithProgress("winget install --id Microsoft.VCRedist.2013.x86 --silent --accept-source-agreements --accept-package-agreements");
                    await RunInstallerWithProgress("winget install --id Microsoft.VCRedist.2012.x64 --silent --accept-source-agreements --accept-package-agreements");
                    await RunInstallerWithProgress("winget install --id Microsoft.VCRedist.2012.x86 --silent --accept-source-agreements --accept-package-agreements");
                    await RunInstallerWithProgress("winget install --id Microsoft.VCRedist.2010.x64 --silent --accept-source-agreements --accept-package-agreements");
                    await RunInstallerWithProgress("winget install --id Microsoft.VCRedist.2010.x86 --silent --accept-source-agreements --accept-package-agreements");
                }

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
                string monacoUrl = "https://files.catbox.moe/odjdko.zip";
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string targetPath = Path.Combine(localAppData, "Velocity Ui");
                string zipPath = Path.Combine(Path.GetTempPath(), "MonacoEditor.zip");

                Directory.CreateDirectory(targetPath);

                LogWithTimestamp("Downloading...", ConsoleColor.DarkYellow);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  (Catbox can be slow, please be patient)\n");
                Console.ForegroundColor = ConsoleColor.White;
                
                var client = new HttpClient
                {
                    Timeout = TimeSpan.FromMinutes(10)
                };
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
                
                var response = await client.GetAsync(monacoUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                
                long totalBytes = response.Content.Headers.ContentLength ?? 0;
                byte[] buffer = new byte[8192];
                long totalRead = 0;
                
                var contentStream = await response.Content.ReadAsStreamAsync();
                using var fileStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
                
                int bytesRead;
                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                    totalRead += bytesRead;
                    
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
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($"{percentage}% ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"({downloadedMB}MB/{totalMB}MB)");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                
                fileStream.Close();
                contentStream.Close();

                Console.WriteLine();
                LogWithTimestamp($"Download complete: {new FileInfo(zipPath).Length / 1024 / 1024}MB", ConsoleColor.Green);
                LogWithTimestamp("Extracting...", ConsoleColor.DarkYellow);
                
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("║            SETTING UP FISHSTRAP (Unavailable)                  ║");
                Console.ForegroundColor = ConsoleColor.Cyan;
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

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("  ⚠️  WARNING: This will delete your original Fishstrap settings.");
                Console.Write("\n  Continue? (y/n): ");
                Console.ForegroundColor = ConsoleColor.White;
                
                string confirmation = Console.ReadLine() ?? string.Empty;
                confirmation = confirmation.ToLower();
                
                if (confirmation != "y" && confirmation != "yes")
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\n  Operation cancelled.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }

                string versionsPath = Path.Combine(fishstrapPath, "Versions");
                if (Directory.Exists(versionsPath))
                {
                    LogWithTimestamp("Removing old versions...", ConsoleColor.DarkYellow);
                    Directory.Delete(versionsPath, true);
                }

                LogWithTimestamp("Configuring...", ConsoleColor.DarkYellow);

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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("║            SETTING UP VOIDSTRAP (Unavailable)                  ║");
                Console.ForegroundColor = ConsoleColor.Cyan;
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

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("  ⚠️  WARNING: This will delete your original Voidstrap settings.");
                Console.Write("\n  Continue? (y/n): ");
                Console.ForegroundColor = ConsoleColor.White;                
                string confirmation = Console.ReadLine() ?? string.Empty;
                confirmation = confirmation.ToLower();
                
                if (confirmation != "y" && confirmation != "yes")
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
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

                LogWithTimestamp("Configuring...", ConsoleColor.DarkYellow);
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
  ""DisableSplashScreen"": true,  ""EnableAnalytics"": true,
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
  ""SkyBoxDataSending"": false,  ""FFlagRPCDisplayer"": true,
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
  ""CustomIntegrations"": [],  ""UseDisableAppPatch"": false,
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
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                   CHANGING DNS SERVER                          ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("  Select DNS provider:\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("  1. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Cloudflare ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("(1.1.1.1)");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("  2. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Google ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("(8.8.8.8)");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("  3. ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Cancel\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("  → Select option: ");
            Console.ForegroundColor = ConsoleColor.White;            
            string dnsChoice = Console.ReadLine() ?? string.Empty;
            
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
                Console.ForegroundColor = ConsoleColor.Blue;
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
                LogWithTimestamp("Getting network adapters...", ConsoleColor.DarkYellow);
                
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
                    LogWithTimestamp($"Setting {providerName} DNS on {adapter}...", ConsoleColor.DarkYellow);
                    
                    await RunInstaller($"netsh interface ip set dns \"{adapter}\" static {primaryDNS}");
                    await RunInstaller($"netsh interface ip add dns \"{adapter}\" {secondaryDNS} index=2");
                }

                Console.ForegroundColor = ConsoleColor.Green;                Console.WriteLine($"\n╔════════════════════════════════════════════════════════════════╗");
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

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("  ⚠️  WARNING: This will delete cache, logs, and settings.");
                Console.WriteLine("  ⚠️  This action will log you out of your account.");
                Console.Write("\n  Are you sure you want to continue? (y/n): ");
                Console.ForegroundColor = ConsoleColor.White;
                
                string confirmation = Console.ReadLine() ?? string.Empty;
                confirmation = confirmation.ToLower();
                
                if (confirmation != "y" && confirmation != "yes")
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
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

                    LogWithTimestamp($"Processing {name}...", ConsoleColor.DarkYellow);

                    foreach (string folder in foldersToDelete)
                    {
                        string folderPath = Path.Combine(path, folder);                        if (Directory.Exists(folderPath))
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

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("\n  Do you want to delete temp files? (y/n): ");
                Console.ForegroundColor = ConsoleColor.White;
                string response = Console.ReadLine() ?? string.Empty;
                response = response.ToLower();

                if (response == "y" || response == "yes")
                {
                    LogWithTimestamp("Cleaning temp files...", ConsoleColor.DarkYellow);
                    
                    string tempPath = Path.GetTempPath();
                    string[] tempFiles = Directory.GetFiles(tempPath, "*.*", SearchOption.TopDirectoryOnly);
                    int deletedCount = 0;
                    
                    foreach (string tempFile in tempFiles)
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
                string velocityFolder = Console.ReadLine() ?? string.Empty;
                velocityFolder = velocityFolder.Trim().Trim('"');

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
                    Console.ForegroundColor = ConsoleColor.Red;                    Console.WriteLine($"\n  [!] Velocity.exe not found in {velocityFolder}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }

                LogWithTimestamp("Downloading crash fix...", ConsoleColor.DarkYellow);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  (Catbox can be slow, please be patient)\n");
                Console.ForegroundColor = ConsoleColor.White;

                string fixUrl = "https://files.catbox.moe/w8oncl.zip";
                string zipPath = Path.Combine(Path.GetTempPath(), "VelocityCrashFix.zip");

                var client = new HttpClient
                {
                    Timeout = TimeSpan.FromMinutes(10)
                };
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
                
                var response = await client.GetAsync(fixUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                
                long totalBytes = response.Content.Headers.ContentLength ?? 0;
                byte[] buffer = new byte[8192];
                long totalRead = 0;
                
                var contentStream = await response.Content.ReadAsStreamAsync();
                using var fileStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
                
                int bytesRead;
                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                    totalRead += bytesRead;
                    
                    if (totalBytes > 0)
                    {                        int percentage = (int)((totalRead * 100) / totalBytes);
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
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($"{percentage}% ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"({downloadedMB}MB/{totalMB}MB)");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                
                fileStream.Close();
                contentStream.Close();

                Console.WriteLine();
                LogWithTimestamp($"Download complete: {new FileInfo(zipPath).Length / 1024 / 1024}MB", ConsoleColor.Green);
                LogWithTimestamp("Extracting...", ConsoleColor.DarkYellow);
                
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
                    CreateNoWindow = true
                }
            };

            process.Start();
            await process.WaitForExitAsync();
        }

        static async Task RunInstallerWithProgress(string command)
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
                    CreateNoWindow = true
                }
            };

            Console.Write("    ");
            process.Start();

            const int barWidth = 40;
            int currentProgress = 0;
            var random = new Random();
            int stuckCounter = 0;
            
            while (!process.HasExited)
            {
                if (currentProgress < 90)
                {
                    currentProgress += random.Next(2, 6);
                    stuckCounter = 0;
                }
                else if (currentProgress < 98)
                {
                    stuckCounter++;
                    if (stuckCounter > 3)
                    {
                        currentProgress += 1;
                        stuckCounter = 0;
                    }
                }
                
                if (currentProgress > 98) currentProgress = 98;
                
                int filled = (currentProgress * barWidth) / 100;
                
                Console.SetCursorPosition(4, Console.CursorTop);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("[");
                Console.Write(new string('█', filled));
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(new string('░', barWidth - filled));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"] ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{currentProgress}%");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("   ");
                
                await Task.Delay(250);
            }

            Console.SetCursorPosition(4, Console.CursorTop);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[");
            Console.Write(new string('█', barWidth));
            Console.Write($"] ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("100%");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            await process.WaitForExitAsync();
        }
    }
}
