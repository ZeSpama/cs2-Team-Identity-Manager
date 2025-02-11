# [CS2] Team Identity Manager

### This plugin was created based on a plugin request by crashzk on the [CounterStrikeSharp](https://discord.com/channels/1160907911501991946/1332488087300866131/1332488087300866131) server and allows you to modify the team name and logo manually and randomly

## Screenshots

![1](https://prnt.sc/V5xf-J4Fdmn5)

## Features

- [x] Allows you to modify the team name
- [x] Allows you to modify the team logo

## Installation

1. Download the [latest verison](hhttps://github.com/ZeSpama/cs2-Team-Identity-Manager/releases)
2. Unzip into your server `csgo/addons/counterstrikesharp/`
3. Restart your server
4. Configure the config file

## Config File

```json
{
  "RandomTeamLogos": true,
  "RandomTeamNames": true,
  "CtTeamName": "",
  "CtTeamLogo": "",
  "TTeamName": "",
  "TTeamLogo": "",
  "ConfigVersion": 1
}
```

> [!NOTE]
> To use your own custom logos, you can use [MultiAddonManager](https://github.com/Source2ZE/MultiAddonManager)  
> SVG logos have to be added into `panorama/images/tournaments/teams` directory  
> Then in the TeamLogo field, just enter the corresponding file name and restart the server
