```
████████╗ ██████╗ ██╗  ██╗███████╗████████╗███████╗ █████╗ ██╗     
╚══██╔══╝██╔═══██╗╚██╗██╔╝██╔════╝╚══██╔══╝██╔════╝██╔══██╗██║     
   ██║   ██║   ██║ ╚███╔╝ ███████╗   ██║   █████╗  ███████║██║     
   ██║   ██║   ██║ ██╔██╗ ╚════██║   ██║   ██╔══╝  ██╔══██║██║     
   ██║   ╚██████╔╝██╔╝ ██╗███████║   ██║   ███████╗██║  ██║███████╗
   ╚═╝    ╚═════╝ ╚═╝  ╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═╝╚══════╝
```

**Advanced Multi Target Data Extraction Framework**  
*Developer: @tcixt*

(old version, for in usermode and uptodate version contact me on telegram)
---

## Core 

**browser**
- Chromium (Chrome, Edge, Brave, Opera)
- Firefox & Gecko variants
- Passwords, cookies, autofill, history
- Crypto wallet extensions

**applications**
- FTP clients (FileZilla, WinSCP, CoreFTP)
- Remote access (TeamViewer, AnyDesk, RDP, PuTTY)
- Dev tools (JetBrains, Git, VS Code)
- Database managers (Navicat, HeidiSQL)

**games**
- Steam, Epic Games, Battle.net
- Riot Games, Roblox, Minecraft
- Uplay, Xbox Live

**communication**
- Discord, Telegram, Skype
- Signal, Element, WhatsApp
- Email clients & messengers

**VPN**
- NordVPN, ExpressVPN, ProtonVPN
- Mullvad, CyberGhost, Surfshark
- OpenVPN, WireGuard configs

**cryptoc**
- Desktop wallets (Exodus, Electrum, Atomic)
- Browser extensions (MetaMask, Phantom)
- Private key extraction

**System**
- Hardware fingerprinting (HWID, CPU, GPU)
- Installed software enumeration
- Product keys & licenses
- WiFi credentials
- Live screenshots

---

## Quick Deploy

**One-Click Build:**
```batch
build_payload.bat
```

**Manual Build Options:**
- .NET SDK: `dotnet build -c Release`
- MSBuild: `msbuild /p:Configuration=Release`
- Visual Studio: Build → Release

**Output:** `Builds/toxsteal_[timestamp].exe`

---

## 🛠️ Builder Features

**🎛️ Interactive Builder**
- Multiple build environments (.NET SDK, MSBuild, VS)
- Discord webhook integration
- Telegram bot delivery
- Connection testing
- "FUD" optimizations

**Advanced Options**
- Debug symbol removal
- Metadata stripping
- Timestamp modification
- Code optimization
- Anti-analysis features

**Delivery Methods**
- Discord webhooks with rich embeds
- Telegram bot with file upload
- Automatic retry mechanisms
- Connection validation


```

**Target Modules:**
- `Browsers/` - Web browser data extraction
- `Applications/` - Software credential harvesting  
- `Games/` - Gaming platform integration
- `Messengers/` - Communication app targeting
- `Vpn/` - VPN configuration extraction
- `Crypto/` - Cryptocurrency wallet handling
- `Device/` - System information gathering

---

**Requirements:**
- Windows 7+ (x86/x64)
- .NET Framework 4.6.2+
- Admin privileges (recommended)

**Stealth Features:**
- Anti-VM detection
- Sandbox evasion
- Process hollowing
- String obfuscation
- Metadata removal

**Delivery Integration:**
- Real-time data transmission
- Encrypted communication
- Automatic cleanup
- Error handling & logging

---

*Built for educational and authorized testing purposes only.*
