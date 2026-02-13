# Velocity Fixer (Unofficial + little outdated)

**Disclaimer:** This is NOT the official Velocity Fixer. The official version is in the Velocity Discord server. This is an improved version by **meridian_dev** with additional fixes.

Contact: **meridian_dev** on Discord for help if needed

## About

I made this as a fun project and to help people dealing with Velocity issues that the official fixer doesn't cover. If you think this is malware, cool don't use it. The source code is right here for you to read. I don't give a shit if you're paranoid.

## Requirements

- Windows 10/11
- Admin privileges (for some options)
- Internet connection
- .NET 10.0 installed

## Features

### [1] Install Velocity to Desktop

Downloads and installs the latest version of Velocity directly to your desktop.

**Note:** This link may become outdated at any time since it gets Velocity from not their official link.

### [2] Install Dependencies

Installs everything Velocity needs to run properly:

- **DirectX Runtime**
- **.NET 8.0 Runtime**
- **.NET 10.0 Runtime**
- **.NET Framework 4.8.1**
- **.NET Framework 4.8**
- **Visual C++ Redistributables**

### [3] Fix Tabs

Downloads Monaco Editor to fix broken tabs in Velocity.

**Fixes:**
- Missing script editor tabs
- Tab rendering issues
- Display problems

**Install Location:** `%LocalAppData%\Velocity Ui`

### [4] Setup Fishstrap

Configures Fishstrap (Roblox bootstrapper) with needed settings.

**Changes:**
- Sets the correct Roblox channel
- Disables analytics and telemetry
- Clears old version cache for fresh install

**Note:** Fishstrap must be installed before using this option also this will delete ur old settings.

### [5] Setup Voidstrap

Configures Voidstrap (Roblox bootstrapper) with needed settings.

**Changes:**
- Sets the correct Roblox channel
- Disables analytics and telemetry
- Clears old version cache for fresh install

**Note:** Voidstrap must be installed before using this option also this will delete ur old settings.

### [6] Change DNS Server (Cloudflare/Google)

Automatically switches all active network adapters to one of these DNS provider:

- **Cloudflare DNS:** 1.1.1.1 / 1.0.0.1
- **Google DNS:** 8.8.8.8 / 8.8.4.4

Requires admin privileges.

### [7] Fix Roblox Crashing

Clears Roblox cache and problematic files that can cause crashes across all bootstrappers (Roblox, Bloxstrap, Fishstrap, Voidstrap).

**Deletes:**
- Cache folders
- Storage files
- Old version data

**Warning:** This will log you out of Roblox. You'll need to log back in.

**ALSO:** Offers option to clear Windows temp files after cleanup (can help with another crash issues).

### [8] Fix Velocity Crashes (fixes on inject and on startup crashes)

Downloads and applies crash fix directly to your Velocity installation.

**Fixes:**
- Injection crashes
- Startup crashes
- Runtime instability

**Process:**
1. Asks for your Velocity.exe location
2. Downloads fix package
3. Extracts directly to Velocity folder
4. Overwrites broken files

## Usage Tips

1. **Run as Administrator** for best results
2. **Run with Anti-Viruses turned off** so they cannot fuck things up
3. Use option 2 first on fresh installs
4. For Velocity issues, try options 3 and 8 together
5. Option 7 is safe to spam if Roblox keeps crashing
6. Option 1 downloads from Pixeldrain - link may break at any time

## Build From Source

```bash
git clone https://github.com/meridian0dev/VelocityFixer.git
cd VelocityFixer
dotnet publish -c Release
```

Or use the included `build.bat` for quick builds.

Output: `bin\Release\net10.0\win-x64\publish\VelocityFixer.exe`

**Requires:** .NET 10.0 SDK
**Version:** 1.0.2  
**Author:** meridian_dev  
**Repository:** https://github.com/meridian0dev/VelocityFixer
