using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using static CounterStrikeSharp.API.Core.Listeners;
using System.Text.Json.Serialization;

namespace TeamIdentityManager;

public class TeamLogoConfig : BasePluginConfig
{
    [JsonPropertyName("RandomTeamLogos")]
    public bool RandomTeamLogos { get; set; } = true;

    [JsonPropertyName("RandomTeamNames")]
    public bool RandomTeamNames { get; set; } = true;

    [JsonPropertyName("CtTeamName")]
    public string CtTeamName { get; set; } = "";

    [JsonPropertyName("CtTeamLogo")]
    public string CtTeamLogo { get; set; } = "";

    [JsonPropertyName("TTeamName")]
    public string TTeamName { get; set; } = "";

    [JsonPropertyName("TTeamLogo")]
    public string TTeamLogo { get; set; } = "";
}

public class TeamIdentityManager : BasePlugin, IPluginConfig<TeamLogoConfig>
{
    public override string ModuleAuthor => "ZeSpama";
    public override string ModuleName => "Team Identity Manager";
    public override string ModuleVersion => "1.0.1";

    private static readonly Dictionary<string, string> teamNames = new()
    {
            { "zzn", "00 Nation" }, { "thv", "100 Thieves" }, { "3dm", "3DMAX" }, { "nein", "9INE" },
            { "pand", "9Pandas" }, { "nine", "9z Team" }, { "amka", "AMKAL ESPORTS" }, { "apex", "Apeks" },
            { "ad", "Astana Dragons" }, { "astr", "Astralis" }, { "avg", "Avangar" }, { "bne", "Bad News Eagles" },
            { "big", "BIG" }, { "bravg", "Bravado Gaming" }, { "cm", "Clan-Mystik" }, { "c9", "Cloud9" },
            { "col", "compLexity Gaming" }, { "cope", "Copenhagen Flames" }, { "cw", "Copenhagen Wolves" },
            { "clg", "Counter Logic Gaming" }, { "cr4z", "CR4ZY" }, { "dat", "dAT Team" }, { "dig", "Team Dignitas" },
            { "drea", "DreamEaters" }, { "ebet", "Team eBettle" }, { "ecst", "ECSTATIC" }, { "ence", "ENCE eSports" },
            { "ent", "Entropiq" }, { "nv", "Team EnVyUs" }, { "eps", "Epsilon eSports" }, { "ecs", "ESC Gaming" },
            { "eter", "Eternal Fire" }, { "evl", "Evil Geniuses" }, { "faze", "FaZe Clan" }, { "flg", "Flash Gaming" },
            { "flip", "Flipsid3 Tactics" }, { "flux", "Fluxo" }, { "fq", "FlyQuest" }, { "fntc", "Fnatic" }, { "forz", "FORZE Esports" },
            { "furi", "FURIA Esports" }, { "g2", "G2 Esports" }, { "gamb", "Gambit Esports" }, { "gl", "GamerLegion" }, { "god", "GODSENT" },
            { "gray", "Grayhound Gaming" }, { "hlr", "HellRaisers" }, { "hero", "Heroic" }, { "ibp", "iBUYPOWER" }, { "ihc", "IHC Esports" },
            { "imt", "Immortals" }, { "im", "Team Immunity" }, { "imp", "Imperial Esports" }, { "itb", "Into The Breach" },
            { "intz", "INTZ eSports" }, { "keyd", "Keyd Stars" }, { "king", "Team Кinguin" }, { "koi", "KOI" }, { "ldlc", "Team LDLC" },
            { "lgcy", "Legacy" }, { "lgb", "LGB eSports" }, { "liq", "Team Liquid" }, { "lc", "London Conspiracy" }, { "lumi", "Luminosity Gaming" },
            { "lumik", "Luminosity Gaming" }, { "lynn", "Lynn Vision Gaming" }, { "mibr", "MIBR" }, { "mfg", "Misfits" }, { "mngz", "The MongolZ" },
            { "mont", "Monte" }, { "mss", "mousesports" }, { "mouz", "MOUZ" }, { "ride", "Movistar Riders" }, { "myxmg", "myXMG" },
            { "nf", "n!faculty" }, { "navi", "Natus Vincere" }, { "nip", "Ninjas in Pyjamas" }, { "nor", "North" }, { "nrg", "NRG Esports" },
            { "og", "OG" }, { "optc", "OpTic Gaming" }, { "orbit", "Orbit Esport" }, { "out", "Outsiders" }, { "pain", "paiN Gaming" },
            { "psnu", "Passion UA" }, { "penta", "PENTA Sports" }, { "pkd", "Planetkey Dynamics" }, { "qb", "Quantum Bellator Fire" },
            { "ratm", "Rare Atom" }, { "rgg", "Reason Gaming" }, { "wgg", "Recursive eSports" }, { "ren", "Renegades" }, { "rog", "Rogue" },
            { "saw", "SAW" }, { "shrk", "Sharks Esports" }, { "sk", "SK Gaming" }, { "spc", "Space Soldiers" }, { "spir", "Team Spirit" },
            { "splc", "Splyce" }, { "spr", "Sprout" }, { "syma", "Syman Gamingf" }, { "tsm", "Team SoloMid" }, { "tit", "Titan" },
            { "tyl", "Tyloo" }, { "us", "Universal Soldiers" }, { "vega", "Vega Squadron" }, { "vg", "VeryGames" }, { "vex", "Vexed Gaming" },
            { "vici", "ViCi Gaming" }, { "vp", "Virtus.Pro" }, { "vita", "Team Vitality" }, { "ve", "Vox Eminor" }, { "wins", "Winstrike Team" },
            { "wcrd", "Wildcard" }, { "indw", "Team Wolf" }, { "e6ten", "x6tence" }, { "xapso", "Xapso" }
    };

    private static readonly string[] teamLogos = teamNames.Keys.ToArray();
    private readonly Random random = new();
    public TeamLogoConfig Config { get; set; } = new();

    public override void Load(bool hotReload)
    {
        RegisterListener<OnMapStart>(OnMapStart);
    }

    public override void Unload(bool hotReload)
    {
        RemoveListener<OnMapStart>(OnMapStart);
    }

    public void OnConfigParsed(TeamLogoConfig config)
    {
        Config = config;
    }

    public void OnMapStart(string mapName)
    {
        string logoCT = Config.CtTeamLogo;
        string logoT = Config.TTeamLogo;
        string nameCT = Config.CtTeamName;
        string nameT = Config.TTeamName;

        if (Config.RandomTeamLogos || Config.RandomTeamNames)
        {
            if (Config.RandomTeamLogos && string.IsNullOrEmpty(Config.CtTeamLogo))
            {
                logoCT = teamLogos[random.Next(teamLogos.Length)];
            }
            if (Config.RandomTeamLogos && string.IsNullOrEmpty(Config.TTeamLogo))
            {
                logoT = teamLogos[random.Next(teamLogos.Length)];
            }

            if (Config.RandomTeamNames)
            {
                nameCT = teamNames.TryGetValue(logoCT, out string? valueCT) ? valueCT : "Unknown";
                nameT = teamNames.TryGetValue(logoT, out string? valueT) ? valueT : "Unknown";
            }
        }

        if (!string.IsNullOrEmpty(nameCT))
        {
            Server.ExecuteCommand($"mp_teamname_1 \"{nameCT}\"");
        }
        if (!string.IsNullOrEmpty(nameT))
        {
            Server.ExecuteCommand($"mp_teamname_2 \"{nameT}\"");
        }

        AddTimer(5, () =>
        {
            Server.ExecuteCommand($"mp_teamlogo_1 \"{logoCT}\"");
            Server.ExecuteCommand($"mp_teamlogo_2 \"{logoT}\"");
        });
    }
}
