# Velocity Fixer (Unofficial)

**Disclaimer:** This is NOT the official Velocity Fixer. The official version is in the Velocity Discord server. This is an improved/modded version by **meridian_dev** with additional fixes.

Contact: **meridian_dev** on Discord

---

## About

I made this as a fun project and to help people dealing with Velocity issues that the official fixer doesn't cover. If you think this is malware, cool don't use it. The source code is right here for you to read. I don't give a shit if you're paranoid, audit the code yourself.

---

## Requirements

- Windows 10/11
- Admin privileges (for some options)
- Internet connection
- .NET 10.0 installed

---

## Features

### [1] Install Dependencies

Installs everything Velocity needs to run properly:

- **DirectX 11**
- **.NET 8.0 Runtime**
- **.NET 10.0 Runtime**
- **.NET Framework 4.8.1**
- **Visual C++ Redistributables** (2010, 2012, 2013, 2015+)
- **DirectX End-User Runtime**

Uses winget package manager. Run this first if you're setting up fresh.

---

### [2] Fix Tabs

Downloads Monaco Editor components from Discord and installs them to fix broken tabs in Velocity.

**Fixes:**
- Missing script editor tabs
- Tab rendering issues
- Editor display problems

**Install Location:** `%LocalAppData%\Velocity Ui`

---

### [3] Setup Fishstrap

Configures Fishstrap (Roblox bootstrapper) with optimized settings.

**Changes:**
- Sets custom Roblox channel/product settings
- Disables analytics and telemetry
- Clears old version cache for fresh install

**Note:** Fishstrap must be installed before using this option.

---

### [4] Change DNS Server

Automatically switches all active network adapters to **Cloudflare DNS** (1.1.1.1 / 1.0.0.1).

May require admin privileges.

---

### [5] Fix Roblox Crashing

Clears Roblox cache and problematic files that can cause crashes.

**AND** Offers option to clear Windows temp files after cleanup (can help with additional crash issues).

---

### [6] Fix Velocity Crashes

Downloads and applies crash fix directly to your Velocity installation.

**Process:**
1. Asks for your Velocity.exe location
2. Downloads fix package

---

## Usage Tips

1. **Run as Administrator** for best results
2. **Run with Anti-Viruses turned off** so they cannot fuck things up
3. Use option 1 first on fresh installs
4. For Velocity issues, try options 2 and 6 together
5. Option 5 is safe to spam if Roblox keeps crashing

---

## Build From Source

```bash
git clone https://github.com/meridian0dev/VelocityFixer.git
cd VelocityFixer
dotnet publish -c Release
```

Output: `bin\Release\net10.0\win-x64\publish\VelocityFixer.exe`

**Requires:** .NET 10.0 SDK

---

## License

Do whatever you want with this. I don't care. Open source, modify it, redistribute it, claim you made it I really don't give a fuck. Just don't come crying to me if something breaks.

---

**Version:** 1.0  
**Author:** meridian_dev  
**Repository:** https://github.com/meridian0dev/VelocityFixer
