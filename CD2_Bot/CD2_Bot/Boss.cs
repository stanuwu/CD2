using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public class Boss
    {
        public Boss(string type, string description, int hP, int level, int resistance, int minlevel, int damage, Biome biome, BossDrops drops, BossCost cost)
        {
            Type = type;
            Description = description;
            HP = hP;
            Level = level;
            Resistance = resistance;
            Minlevel = minlevel;
            Damage = damage;
            Biome = biome;
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
        public Biome Biome { get; set; }
        public BossDrops Drops { get; set; }
        public BossCost Cost { get; set; }

        public Boss Clone()
        {
            return (Boss)this.MemberwiseClone();
        }
    }

    public class GearReward
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int Chance { get; set; } //chance out of 10000
    }

    public class BossDrops
    {
        public BossDrops(int moneyReward, int xpReward, int weaponXpReward, int armorXpReward, int extraXpReward, Quest questReward, EnemyDrops itemReward, GearReward gearReward)
        {
            MoneyReward = moneyReward;
            XpReward = xpReward;
            WeaponXpReward = weaponXpReward;
            ArmorXpReward = armorXpReward;
            ExtraXpReward = extraXpReward;
            QuestReward = questReward;
            ItemReward = itemReward;
            GearReward = gearReward;
        }

        public int MoneyReward { get; set; }
        public int XpReward { get; set; }
        public int WeaponXpReward { get; set; }
        public int ArmorXpReward { get; set; }
        public int ExtraXpReward { get; set; }
        public Quest QuestReward { get; set; }
        public EnemyDrops ItemReward { get; set; }
        public GearReward GearReward { get; set; }
    }

    public class BossCost
    {
        public BossCost(int moneyCost, string itemType, int itemCost)
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
            return (player.Money >= this.MoneyCost && inv[this.ItemType] >= this.ItemCost);
        }

        public void buy(CharacterStructure player)
        {
            Dictionary<string, int> inv = Utils.InvAsDict(player);
            player.Money -= this.MoneyCost;
            inv[this.ItemType] -= this.ItemCost;
            Utils.SaveInv(player, inv);
        }
    }

    public static class BossFights
    {
        public static List<Boss> Bosses = new List<Boss>
        {
            new Boss("Hive Slayer", "A giant hivemind monster.", 3500, 0, 50, 25, 100, Biome.Any, new BossDrops(2500, 2500, 1000, 1000, 1000, null, null, null), new BossCost(25000, null, 0)),
        };

        public static Dictionary<ulong, BossRegisterEntry> BossRegistry = new Dictionary<ulong, BossRegisterEntry>() {};

        public static async Task SummonBossAsync(SocketSlashCommand cmd)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == cmd.User.Id
                                        select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("You don't have a character."));
                return;
            }

            string arg = (string)cmd.Data.Options.First().Value;
            Boss opponent = null;
            if ( arg != null)
            {
                opponent = BossFights.Bosses.Find(x => x.Type == arg);
            }

            if (arg == null || opponent == null)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("Please select a valid boss to fight."));
                return;
            }

            if (stats.Lvl < opponent.Level)
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("Your level is too low to summon this boss."));
                return;
            }

            if(opponent.Cost.canAfford(stats))
            {
                opponent.Cost.buy(stats);
            } else
            {
                await cmd.RespondAsync(embed: Utils.QuickEmbedError("You can not afford to summon this boss."));
                return;
            }

            await cmd.RespondAsync(embed: Utils.QuickEmbedNormal("Bossfight", $"Summoning {opponent.Type}!"));
            ulong gid = ((IGuildChannel)cmd.Channel).Guild.Id;
            BossRegistry.Add(gid, new BossRegisterEntry(gid, (ITextChannel) cmd.Channel, opponent.Clone(), false));
            BossRegistry[gid].Members.Add(stats.PlayerID);
        }
    }

    public class BossRegisterEntry
    {
        public BossRegisterEntry(ulong guildID, ITextChannel channel, Boss boss, bool started)
        {
            GuildID = guildID;
            Channel = channel;
            Boss = boss;
            Cooldowns = new Dictionary<ulong, DateTime>();
            Shields = new Dictionary<ulong, int>();
            Members = new List<ulong>();
            Phase = BossPhase.Attack;
            Started = started;
        }

        public ulong GuildID { get; set; }
        public ITextChannel Channel { get; set; }
        public Boss Boss { get; set; }
        public Dictionary<ulong, DateTime> Cooldowns { get; set; }
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
