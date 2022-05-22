using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Discord.Rest;

namespace CD2_Bot
{
    public class Boss
    {
        public Boss(string type, string description, int hP, int level, int resistance, int minlevel, int damage, BossDrops drops, BossCost cost)
        {
            Type = type;
            Description = description;
            HP = hP;
            Level = level;
            Resistance = resistance;
            Minlevel = minlevel;
            Damage = damage;
            Drops = drops;
            Cost = cost;
        }

        public string Type { get; set; }
        public string Description { get; set; }
        public int HP { get; set; }
        public int Level { get; set; }
        public int Resistance { get; set; }
        public int Minlevel { get; set; }
        public int Damage { get; set; }
        public BossDrops Drops { get; set; }
        public BossCost Cost { get; set; }

        public Boss Clone()
        {
            return (Boss)this.MemberwiseClone();
        }
    }

    public class GearReward
    {
        public GearReward(string type, string name, int chance)
        {
            Type = type;
            Name = name;
            Chance = chance;
        }

        public string Type { get; set; }
        public string Name { get; set; }
        public int Chance { get; set; } //chance out of 10000
    }

    public class BossDrops
    {
        public BossDrops(int moneyReward, int xpReward, int weaponXpReward, int armorXpReward, int extraXpReward,
            EnemyDrops itemReward, GearReward gearReward)
        {
            MoneyReward = moneyReward;
            XpReward = xpReward;
            WeaponXpReward = weaponXpReward;
            ArmorXpReward = armorXpReward;
            ExtraXpReward = extraXpReward;
            ItemReward = itemReward;
            GearReward = gearReward;
        }

        public int MoneyReward { get; set; }
        public int XpReward { get; set; }
        public int WeaponXpReward { get; set; }
        public int ArmorXpReward { get; set; }
        public int ExtraXpReward { get; set; }
        public EnemyDrops ItemReward { get; set; }
        public GearReward GearReward { get; set; }

        public string PreviewDrops()
        {
            string tell = "";
            if (this.MoneyReward > 0)
            {
                tell += $"{this.MoneyReward} Coins\n";
            }

            if (this.XpReward > 0)
            {
                tell += $"{this.XpReward} XP\n";
            }

            if (this.WeaponXpReward > 0)
            {
                tell += $"{this.WeaponXpReward} Weapon XP\n";
            }

            if (this.ArmorXpReward > 0)
            {
                tell += $"{this.ArmorXpReward} Armor XP\n";
            }

            if (this.ExtraXpReward > 0)
            {
                tell += $"{this.ExtraXpReward} Extra XP\n";
            }

            if (this.ItemReward != null)
            {
                if (this.ItemReward.DropVariation < 1)
                {
                    tell += $"{this.ItemReward.DropAmount}x {this.ItemReward.Drop} ({this.ItemReward.DropChance}%)\n";
                }
                else
                {
                    tell +=
                        $"{this.ItemReward.DropAmount - this.ItemReward.DropVariation}-{this.ItemReward.DropAmount + this.ItemReward.DropVariation}x {this.ItemReward.Drop} ({this.ItemReward.DropChance}%)\n";
                }
            }

            if (this.GearReward != null)
            {
                tell += $"{this.GearReward.Name} ({Math.Round((double)this.GearReward.Chance / 100, 3)}%)\n";
            }

            if (tell == "")
            {
                tell = "No rewards.";
            }

            return tell;
        }

        public void GiveDrops(CharacterStructure stats, ISocketMessageChannel channel)
        {
            stats.Money += this.MoneyReward;
            stats.EXP += this.XpReward;
            stats.WeaponXP += this.WeaponXpReward;
            stats.ArmorXP += this.ArmorXpReward;
            stats.ExtraXP += this.ExtraXpReward;

            if (this.ItemReward != null)
            {
                Dictionary<string, int> inv = Utils.InvAsDict(stats);
                string drop = this.ItemReward.getDrops().Key;
                int am = this.ItemReward.getDrops().Value;
                if (inv.ContainsKey(drop))
                {
                    inv[drop] += am;
                }
                else
                {
                    inv.Add(drop, am);
                }

                Utils.SaveInv(stats, inv);
            }

            if (this.GearReward == null) return;
            if (Defaults.GRandom.Next(0, 10000) < this.GearReward.Chance)
            {
                Gear.RandomDrop(stats.PlayerID, channel, type: this.GearReward.Type, ovr: this.GearReward.Name, ovrtype: this.GearReward.Type);
            }
        }
    }

    public class BossCost
    {
        public BossCost(int moneyCost, string itemType, int itemCost = 0)
        {
            MoneyCost = moneyCost;
            ItemType = itemType;
            ItemCost = itemCost;
        }

        public int MoneyCost { get; set; }
        public string ItemType { get; set; }
        public int ItemCost { get; set; }

        public bool canAfford(CharacterStructure player)
        {
            Dictionary<string, int> inv = Utils.InvAsDict(player);
            return (player.Money >= this.MoneyCost && (this.ItemType == null
                ? true
                : inv.ContainsKey(this.ItemType) && inv[this.ItemType] >= this.ItemCost));
        }

        public void buy(CharacterStructure player)
        {
            player.Money -= this.MoneyCost;
            if (this.ItemType != null)
            {
                Dictionary<string, int> inv = Utils.InvAsDict(player);
                inv[this.ItemType] -= this.ItemCost;
                Utils.SaveInv(player, inv);
            }
        }
    }

    public static class BossFights
    {
        public static List<Boss> Bosses = new List<Boss>

        {
            new Boss("Hive Slayer", "A giant hivemind monster.", 2500, 0, 80, 20, 80,
                new BossDrops(5000, 2500, 2500, 2500, 2500, new EnemyDrops("Hive Shards", 10, 5, 100), null),
                new BossCost(25000, null)),

            new Boss("World Eater", "A universal terror.", 7000, 0, 50, 35, 60,
                new BossDrops(7500, 5000, 3000, 3000, 3000, new EnemyDrops("Life Shard", 10, 2, 85), new GearReward("armor", "World Armor", 2500)),
                new BossCost(50000, null)),

            new Boss("Life Collector", "He has come to claim what is his.", 10000, 0, 70, 50, 100,
                new BossDrops(10000, 7000, 7000, 7000, 7000, null, new GearReward("weapon", "Life Extractor", 1000)),
                new BossCost(25000, "Life Shard", 50)),
        };

        public static Dictionary<ulong, BossRegisterEntry>
            BossRegistry = new Dictionary<ulong, BossRegisterEntry>() { };

        public static async Task SummonBossAsync(SocketSlashCommand cmd)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                where user.PlayerID == cmd.User.Id
                select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("You don't have a character."), ephemeral: true);
                return;
            }

            string arg = (string)cmd.Data.Options.First().Value;
            Boss opponent = null;
            if (arg != null)
            {
                opponent = BossFights.Bosses.Find(x => x.Type == arg);
            }

            if (arg == null || opponent == null)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("Please select a valid boss to fight."),
                    ephemeral: true);
                return;
            }

            if (stats.Lvl < opponent.Minlevel)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("Your level is too low to summon this boss."),
                    ephemeral: true);
                return;
            }

            if (opponent.Cost.canAfford(stats))
            {
                opponent.Cost.buy(stats);
            }
            else
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("You can not afford to summon this boss."),
                    ephemeral: true);
                return;
            }

            ulong gid = ((IGuildChannel)cmd.Channel).Guild.Id;

            if (BossRegistry.ContainsKey(gid))
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("There is already a boss in this guild."),
                    ephemeral: true);
                return;
            }

            Embed embed = Utils.QuickEmbedNormal("Bossfight", $"Summoning {opponent.Type}...");

            ComponentBuilder btnb = new ComponentBuilder()
                .WithButton("Join", "bossfight;join;" + gid, ButtonStyle.Primary)
                .WithButton("Leave", "bossfight;leave;" + gid, ButtonStyle.Danger);

            await cmd.RespondAsync(embed: embed, components: btnb.Build());
            BossRegistry.Add(gid, new BossRegisterEntry(gid, (ITextChannel)cmd.Channel, opponent.Clone(), false));
            BossRegistry[gid].Members.Add(stats.PlayerID);
            BossFights.BossFightTask(BossRegistry[gid], cmd);
        }

        static public async Task HandleButtons(SocketMessageComponent btn)
        {
            string[] btndata = btn.Data.CustomId.Split(';');
            switch (btndata[1])
            {
                case "join":
                    await JoinFightButton(btn, btndata);
                    break;
                case "leave":
                    await LeaveFightButton(btn, btndata);
                    break;
                default:
                    await CombatButtons(btn, btndata);
                    break;
            }
        }

        static public async Task JoinFightButton(SocketMessageComponent btn, string[] btndata)
        {
            ulong gid = Convert.ToUInt64(btndata[2]);
            BossRegisterEntry fight = BossRegistry[gid];

            CharacterStructure stats = (from user in tempstorage.characters
                where user.PlayerID == btn.User.Id
                select user).SingleOrDefault();

            if (!fight.Members.Contains(btn.User.Id) && stats != null && stats.Lvl > fight.Boss.Minlevel &&
                !fight.Started)
            {
                fight.Members.Add(btn.User.Id);
                await btn.RespondAsync(embed: Utils.QuickEmbedNormal("Bossfight", "You joined the fight."),
                    ephemeral: true);
            }
            else
            {
                await btn.RespondAsync(
                    embed: Utils.QuickEmbedError(
                        "You can not join this fight.\nMake sure you have a character and its level is high enough."),
                    ephemeral: true);
            }
        }

        static public async Task LeaveFightButton(SocketMessageComponent btn, string[] btndata)
        {
            ulong gid = Convert.ToUInt64(btndata[2]);
            BossRegisterEntry fight = BossRegistry[gid];
            if (fight.Members.Contains(btn.User.Id) && fight.Members[0] != btn.User.Id && !fight.Started)
            {
                fight.Members.Remove(btn.User.Id);
                await btn.RespondAsync(embed: Utils.QuickEmbedNormal("Bossfight", "You left the fight."),
                    ephemeral: true);
            }
            else
            {
                await btn.RespondAsync(
                    embed: Utils.QuickEmbedError(
                        "You can not leave this fight.\nEither you are not in the fight or you summoned the boss."),
                    ephemeral: true);
            }
        }

        static public async Task CombatButtons(SocketMessageComponent btn, string[] btndata)
        {
            ulong gid = Convert.ToUInt64(btndata[2]);
            BossRegisterEntry fight = BossRegistry[gid];

            CharacterStructure stats = (from user in tempstorage.characters
                where user.PlayerID == btn.User.Id
                select user).SingleOrDefault();

            if (!fight.Members.Contains(btn.User.Id))
            {
                await btn.RespondAsync(
                    embed: Utils.QuickEmbedNormal("Bossfight", "You are not part of this fight.\nJoin before the fight starts to participate."),
                    ephemeral: true);
                return;
            }

            if (fight.Cooldowns.ContainsKey(btn.User.Id) && fight.Cooldowns[btn.User.Id] > 0)
            {
                await btn.RespondAsync(
                    embed: Utils.QuickEmbedNormal("Bossfight", "You are on cooldown!\nPlease slow down!"), ephemeral: true);
                return;
            }

            if (stats.HP < stats.MaxHP / 25)
            {
                await btn.RespondAsync(
                    embed: Utils.QuickEmbedNormal("Bossfight", "You do not have enough health to fight."),
                    ephemeral: true);
                return;
            }

            double playerDamage =
                Convert.ToDouble((stats.Weapon.Damage + stats.Extra.Damage) *
                                 (decimal)stats.CharacterClass.DamageMultiplier) * stats.StatMultiplier *
                ((double)fight.Boss.Resistance / 100) + 0.01;

            StringBuilder action = new StringBuilder();
            double damagemul = fight.Phase == BossPhase.Defense ? 0.3 : 1;
            switch (btndata[1])
            {
                case "atk":
                    fight.Boss.HP -= (int)(playerDamage * damagemul);
                    action.Append($"You dealt {(int)(playerDamage * damagemul)} damage!");
                    break;
                case "dfl":
                    fight.Boss.HP -= (int)(playerDamage / 2 * damagemul);
                    if (fight.Shields.ContainsKey(btn.User.Id))
                    {
                        fight.Shields[btn.User.Id] += (int)(playerDamage / 2 * damagemul);
                    }
                    else
                    {
                        fight.Shields.Add(btn.User.Id, (int)(playerDamage / 2 * damagemul));
                    }

                    action.Append($"You parry and deal {(int)(playerDamage / 2 * damagemul)} damage!");
                    break;
                case "blk":
                    if (fight.Shields.ContainsKey(btn.User.Id))
                    {
                        fight.Shields[btn.User.Id] += (int)playerDamage;
                    }
                    else
                    {
                        fight.Shields.Add(btn.User.Id, (int)playerDamage);
                    }

                    action.Append("You block dealing no damage.");
                    break;
            }

            int shield = fight.Shields.ContainsKey(btn.User.Id) ? fight.Shields[btn.User.Id] : 0;
            action.Append($"\nShield: {shield}");
            if (fight.Cooldowns.ContainsKey(btn.User.Id))
            {
                fight.Cooldowns[btn.User.Id] = 2;
            }
            else
            {
                fight.Cooldowns.Add(btn.User.Id, 2);
            }

            await btn.RespondAsync(embed: Utils.QuickEmbedNormal("Bossfight", action.ToString()), ephemeral: true);
        }

        static public BossPhase randomPhase(BossRegisterEntry fight)
        {
            List<BossPhase> phases = ((BossPhase[])(Enum.GetValues(typeof(BossPhase)))).ToList();
            phases.Remove(fight.Phase);
            return phases[Defaults.GRandom.Next(phases.Count)];
        }

        static public async void BossFightTask(BossRegisterEntry fight, SocketSlashCommand bossCommand)
        {
            string status = "Press the buttons below to fight!";
            int phaseCountdown = 5;
            DateTime started = DateTime.Now;
            ulong[] membersArray = { 0ul };
            List<CharacterStructure> membersList = new List<CharacterStructure>
                { Utils.getCharacter(fight.Members[0]) };
            ISocketMessageChannel channel = bossCommand.Channel;
            fight.Boss.Level = Utils.getCharacter(fight.Members[0]).Lvl;
            MessageComponent btnb = new ComponentBuilder()
                .WithButton("Attack", "bossfight;atk;" + fight.GuildID, ButtonStyle.Primary, emote: new Emoji("🗡"))
                .WithButton("Parry", "bossfight;dfl;" + fight.GuildID, ButtonStyle.Primary, emote: new Emoji("⚔"))
                .WithButton("Block", "bossfight;blk;" + fight.GuildID, ButtonStyle.Primary, emote: new Emoji("🛡️"))
                .Build();
            RestUserMessage msg = null;
            BossPhase nextPhase = randomPhase(fight);

            while (true)
            {
                if (fight.Started == false)
                {
                    if (!fight.Members.All(x => membersArray.Contains(x)) ||
                        !membersArray.All(x => fight.Members.Contains(x)))
                    {
                        membersList.Clear();
                        membersList.AddRange(fight.Members.Select(Utils.getCharacter));
                        int[] levels = membersList.Select(x => x.Lvl).ToArray();
                        fight.Boss.Level = (int)levels.Average();
                        await bossCommand.ModifyOriginalResponseAsync(x =>
                            x.Embed = getBossEmbed(fight, started, nextPhase, status));
                    }

                    if ((DateTime.Now - started).TotalMinutes > Defaults.BOSSPREPTIME)
                    {
                        fight.Started = true;
                        await bossCommand.DeleteOriginalResponseAsync();
                        fight.Boss.HP = ((fight.Boss.HP * fight.Boss.Level) / 10) *
                                        (int)Math.Ceiling((double)fight.Members.Count / 5);
                        fight.Boss.Damage = fight.Boss.Damage * fight.Boss.Level / 10;
                    }
                }

                if (fight.Started)
                {
                    if (fight.Boss.HP < 0)
                    {
                        await msg.DeleteAsync();

                        EmbedBuilder embedBuilder = new EmbedBuilder()
                            .WithTitle($"DEFEATED - {fight.Boss.Type} [Lvl. {fight.Boss.Level}]");
                        TimestampTag startingIn =
                            Discord.TimestampTag.FromDateTime(started + TimeSpan.FromMinutes(Defaults.BOSSPREPTIME));
                        startingIn.Style = TimestampTagStyles.Relative;
                        embedBuilder.Description = "Everyone has received their reward!";
                        embedBuilder.AddField("Players", fight.Members.Count, inline: true);
                        embedBuilder.AddField("Started", startingIn, inline: true);
                        embedBuilder.AddField("Rewards", fight.Boss.Drops.PreviewDrops());
                        embedBuilder.Color = Color.Orange;
                        await channel.SendMessageAsync(embed: embedBuilder.Build());

                        foreach (CharacterStructure player in membersList)
                        {
                            fight.Boss.Drops.GiveDrops(player, channel);
                        }

                        tempstorage.guilds.Find(x => x.GuildID == fight.GuildID).BossesSlain++;

                        BossRegistry.Remove(fight.GuildID);

                        break;
                    }
                    else
                    {
                        foreach (ulong e in fight.Cooldowns.Keys.ToArray())
                        {
                            fight.Cooldowns[e]--;
                        }

                        phaseCountdown--;
                        if (phaseCountdown < 0)
                        {
                            fight.Phase = nextPhase;
                            nextPhase = randomPhase(fight);
                            switch (fight.Phase)
                            {
                                case BossPhase.Attack:
                                    status = $"The Boss attacks you dealing {fight.Boss.Damage} damage!";
                                    foreach (CharacterStructure player in membersList)
                                    {
                                        double idamage = fight.Boss.Damage * (player.Armor.Resistance / 100);
                                        if (!fight.Shields.ContainsKey(player.PlayerID))
                                        {
                                            fight.Shields.Add(player.PlayerID, 0);
                                        }

                                        if (player.HP + fight.Shields[player.PlayerID] - idamage < 0)
                                        {
                                            player.HP = 0;
                                            fight.Shields[player.PlayerID] = 0;
                                        }
                                        else
                                        {
                                            if (fight.Shields[player.PlayerID] < idamage)
                                            {
                                                int thru = (int)idamage - fight.Shields[player.PlayerID];
                                                fight.Shields[player.PlayerID] = 0;
                                                player.HP -= Math.Min(player.HP, thru);
                                            }
                                            else
                                            {
                                                fight.Shields[player.PlayerID] -= (int)idamage;
                                            }
                                        }
                                    }

                                    break;
                                case BossPhase.Charge:
                                    status = "The Boss charges up to increase his attack by 10%!";
                                    fight.Boss.Damage = (int)(fight.Boss.Damage * 1.1);
                                    break;
                                case BossPhase.Defense:
                                    status = "The boss is defending itself and only takes 30% damage!";
                                    break;
                                case BossPhase.Regen:
                                    status = "The boss is regenerates some health!";
                                    fight.Boss.HP += fight.Boss.Damage * 3;
                                    break;
                            }

                            phaseCountdown = 5;
                        }

                        msg = await renewBossMessage(fight, channel, started, msg, btnb, nextPhase, status);
                    }
                }

                membersArray = fight.Members.ToArray();
                await Task.Delay(TimeSpan.FromMilliseconds(5000));
            }
        }

        public static async Task<RestUserMessage> renewBossMessage(BossRegisterEntry fight,
            ISocketMessageChannel channel, DateTime started, RestUserMessage message, MessageComponent btnb,
            BossPhase nextPhase, string status)
        {
            RestUserMessage msg = await channel.SendMessageAsync(embed: getBossEmbed(fight, started, nextPhase, status),
                components: btnb);
            if (message != null) await message.DeleteAsync();
            return msg;
        }

        static public Embed getBossEmbed(BossRegisterEntry fight, DateTime started, BossPhase nextPhase, string status)
        {
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithTitle($"{fight.Boss.Type} [Lvl. {fight.Boss.Level}]");
            TimestampTag startingIn =
                Discord.TimestampTag.FromDateTime(started + TimeSpan.FromMinutes(Defaults.BOSSPREPTIME));
            startingIn.Style = TimestampTagStyles.Relative;
            if (fight.Started == false)
            {
                embedBuilder.AddField("Players", fight.Members.Count, inline: true);
                embedBuilder.AddField("Starting", startingIn, inline: true);
                embedBuilder.AddField("Rewards", fight.Boss.Drops.PreviewDrops());
                embedBuilder.Color = Color.Orange;
            }
            else
            {
                embedBuilder.Color = Color.Green;
                embedBuilder.Description = status;
                embedBuilder.AddField("Started", startingIn);
                embedBuilder.AddField("HP", fight.Boss.HP);
                embedBuilder.AddField("Damage", fight.Boss.Damage);
                embedBuilder.AddField("Phase", fight.Phase.ToString());
                embedBuilder.AddField("Next Phase", nextPhase.ToString());
            }

            return embedBuilder.Build();
        }
    }

    public class BossRegisterEntry
    {
        public BossRegisterEntry(ulong guildID, ITextChannel channel, Boss boss, bool started)
        {
            GuildID = guildID;
            Channel = channel;
            Boss = boss;
            Cooldowns = new Dictionary<ulong, int>();
            Shields = new Dictionary<ulong, int>();
            Members = new List<ulong>();
            Phase = BossPhase.Defense;
            Started = started;
        }

        public ulong GuildID { get; set; }
        public ITextChannel Channel { get; set; }
        public Boss Boss { get; set; }
        public Dictionary<ulong, int> Cooldowns { get; set; }
        public Dictionary<ulong, int> Shields { get; set; }
        public List<ulong> Members { get; set; }
        public BossPhase Phase { get; set; }
        public bool Started { get; set; }
    }

    public enum BossPhase
    {
        Attack,
        Defense,
        Charge,
        Regen,
    }
}