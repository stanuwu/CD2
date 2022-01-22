using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CD2_Bot
{
    public class PlayerCommands : ModuleBase<SocketCommandContext>
    {
        [Command("pvp")]
        [Summary("Play against another user to test your strength.")]
        public async Task PlayerFightRequestAsync(SocketUser opponent = null, int wager = 0, [Remainder] string xargs = null)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == Context.User.Id
                                        select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("You do not have a character."));
                return;
            }

            if (wager < 0)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("Enter a valid wager."));
                return;
            }

            if (opponent == null)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("Please tag an opponent."));
                return;
            }
            else
            {
                EmbedBuilder embed = new EmbedBuilder
                {
                    Title = "Player Fight",
                    Description = $"{stats.CharacterName} has invited <@{opponent.Id}> to a fight for {wager} coins!\nThis expires after 15 minutes."
                };
                embed.WithFooter(Defaults.FOOTER);
                embed.WithColor(Color.Magenta);

                ComponentBuilder btnb = new ComponentBuilder()
                        .WithButton("Accept", "playerfight;ask;" + Context.User.Id.ToString() + ";" + opponent.Id.ToString() + ";" + wager.ToString(), ButtonStyle.Success)
                        .WithButton("Cancel", "playerfight;cancel;" + Context.User.Id.ToString() + ";" + opponent.Id.ToString(), ButtonStyle.Danger);

                MessageComponent btn = btnb.Build();

                await ReplyAsync(embed: embed.Build(), components: btn);
            }
        }

        [Command("quest")]
        [Summary("View your current quest.")]
        public async Task PlayerViewQuestAsync([Remainder] string xargs = null)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == Context.User.Id
                                        select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("You do not have a character."));
                return;
            }

            if (stats.QuestData == "none")
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("You do not have an active quest."));
                return;
            } else
            {
                Quest q = Quests.WhatQuest(stats.QuestData);
                EmbedBuilder embed = new EmbedBuilder
                {
                    Title = q.Name,
                    Description = q.ShowProgress(stats)
                };

                if (q.isQuestExpired(stats))
                {
                    Embed embedf = Utils.QuickEmbedNormal("Quest Failed", q.QuestFailedDialogue);
                    stats.QuestData = "none";
                    await ReplyAsync(embed: embedf);
                }
                else if (q.isQuestCompleted(stats))
                {
                    string qd = stats.QuestData;
                    Embed embedf = Utils.QuickEmbedNormal("Quest Complete!", q.CompletionDialogue);
                    EmbedBuilder b = embedf.ToEmbedBuilder();
                    b.Description += "\n" + q.GenerateRewards(stats);
                    tempstorage.guilds.Find(g => g.GuildID == Context.Guild.Id).QuestsFinished += 1;
                    embedf = b.Build();
                    if (stats.QuestData == qd)
                    {
                        stats.QuestData = "none";
                    }
                    await ReplyAsync(embed: embedf);
                }
                else
                {
                    embed.AddField("Rewards", q.ViewRewards());
                    TimeSpan timeleft = q.timeLeft(stats);
                    if (timeleft.TotalMilliseconds < 0)
                    {
                        embed.AddField("Time Remaining", "Expired");
                    }
                    else
                    {
                        embed.AddField("Time Remaining", $"{timeleft.Days}d {timeleft.Hours}:{timeleft.Minutes}:{timeleft.Seconds}");
                    }
                    embed.WithFooter(Defaults.FOOTER);
                    embed.WithColor(Color.Magenta);

                    ComponentBuilder btnb = new ComponentBuilder()
                            .WithButton("Cancel", "questview;cancel;" + Context.User.Id.ToString(), ButtonStyle.Danger);

                    MessageComponent btn = btnb.Build();

                    await ReplyAsync(embed: embed.Build(), components: btn);
                }
            }
        }

        [Command("vote")]
        [Summary("Claim free rewards.")]
        public async Task VoteAsync([Remainder] string xargs = null)
        {
            CharacterStructure stats = (from user in tempstorage.characters
                                        where user.PlayerID == Context.User.Id
                                        select user).SingleOrDefault();

            if (stats == null || stats.Deleted == true)
            {
                await ReplyAsync(embed: Utils.QuickEmbedError("You do not have a character."));
                return;
            }
            bool hasvoted = await DBLRestClient.UserVotedStatus(stats.PlayerID);
            if (hasvoted == false)
            {
                MessageComponent btn = new ComponentBuilder()
                        .WithButton("Vote", "voteurl", ButtonStyle.Link, url: "https://top.gg/bot/717757487482273813").Build();
                await ReplyAsync(embed: Utils.QuickEmbedNormal("Voting Rewards", "You have not voted yet. Vote every 12 hours on top.gg (DiscordBotList) below for some free rewards. Note that votes might take a few minutes to register."), components: btn);
            }
            else
            {
                if ((DateTime.Now - stats.LastVote).TotalMinutes < 720)
                {
                    await ReplyAsync(embed: Utils.QuickEmbedNormal("Voting Rewards", $"You already claimed your vote rewards for today.\nYou may vote again in {(DateTime.Now - stats.LastVote).TotalHours} hours."));
                } else
                {
                    stats.LastVote = DateTime.Now;
                    await ReplyAsync(embed: Utils.QuickEmbedNormal("Voting Rewards", "Claimed your voting rewards!\n+500 coins\n+50 exp"));
                    stats.EXP += 50;
                    stats.Money += 500;
                }
            }
        }
    }
}
