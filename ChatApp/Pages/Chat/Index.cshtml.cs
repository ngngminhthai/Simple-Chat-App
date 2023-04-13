using ChatApp.Entities;
using DemoSignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Pages.Chat
{
    [Authorize]
    public class IndexModel : PageModel
    {
        ChatDbContext context;
        private readonly IHubContext<signalrServer> _signalrHub;

        public IndexModel(ChatDbContext context, IHubContext<signalrServer> signalrHub)
        {
            this.context = context;
            _signalrHub = signalrHub;
        }

        public List<Entities.Chat> Chats { get; set; }
        public List<GroupChat> GroupChats { get; set; }
        public List<IndividualChat> IndividualChats { get; set; }
        public User CurrUser { get; set; }

        public string GroupName { get; set; }
        public bool isGroupChat { get; set; }
        public int GroupId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }


        public IActionResult OnPostCreateMessage(string groupId, string message)
        {
            var username = User.Identity.Name;
            var user = context.Users.FirstOrDefault(user => user.UserName == username);
            var id = Int32.Parse(groupId.Split("|")[0]);
            var isGr = groupId.Split("|")[1];
            if (Boolean.Parse(isGr))
                context.Chats.Add(new Entities.Chat { Message = message, GroupChatId = id, Sender = user });
            else
            {

                var conversation = context.IndividualChats.FirstOrDefault(c => (c.UserOneId == user.Id && c.UserTwoId == id) || (c.UserTwoId == user.Id && c.UserOneId == id));
                if (conversation == null)
                {
                    var newChat = new IndividualChat { UserOneId = user.Id, UserTwoId = id };
                    context.IndividualChats.Add(newChat);
                    context.SaveChanges();
                    context.Chats.Add(new Entities.Chat { IndividualChatId = newChat.Id, Sender = user, Message = message });
                }
                else context.Chats.Add(new Entities.Chat { IndividualChatId = conversation.Id, Sender = user, Message = message });
            }
            context.SaveChanges();

            return Redirect("/Chat");
        }

        public async Task OnGet()
        {
            var username = User.Identity.Name;
            var user = context.Users.FirstOrDefault(user => user.UserName == username);
            CurrUser = user;

            if (string.IsNullOrEmpty(Search))
                Search = "";
            GroupChats = context.GroupChats.Include(c => c.Chats).Where(c => c.Name.ToLower().Contains(Search.ToLower())).ToList();

            IndividualChats = context.IndividualChats
                .Include(c => c.Chats)
                .Include(c => c.UserOne)
                .Include(c => c.UserTwo)
                .Where(u => u.UserOneId == user.Id || u.UserTwoId == user.Id)
                .Select(gr => new IndividualChat
                {
                    Id = gr.Id,
                    DisplayName = user.UserName == gr.UserOne.UserName ? gr.UserTwo.UserName : gr.UserOne.UserName,
                    UserOne = gr.UserOne,
                    UserTwo = gr.UserTwo
                })
                .Where(c => c.DisplayName.ToLower().Contains(Search.ToLower()))
                .ToList();

            ViewData["IndividualChats"] = context.Users.Where(u => u.Id != user.Id).ToList();

            ViewData["CurrentChatIndi"] = 0;
            ViewData["CurrentChat"] = 0;

            if (GroupChats.Count > 0)
            {
                Chats = context.Chats.Include(c => c.Sender).Where(c => c.GroupChatId == GroupChats[0].Id).ToList();
                ViewData["CurrentChat"] = GroupChats[0].Id;
                GroupName = GroupChats[0].Name;
                isGroupChat = true;
                GroupId = GroupChats[0].Id;
            }
            else if (IndividualChats.Count > 0)
            {
                Chats = context.Chats.Include(c => c.Sender).Where(c => c.GroupChatId == IndividualChats[0].Id).ToList();
                ViewData["CurrentChatIndi"] = IndividualChats[0].Id;
                GroupName = IndividualChats[0].DisplayName;
                isGroupChat = false;
                GroupId = IndividualChats[0].Id;
            }


        }

        public void OnGetGetMessage(int id, bool isGr)
        {
            if (isGr)
                Chats = context.Chats.Include(c => c.Sender).Where(c => c.GroupChatId == id).ToList();
            else Chats = context.Chats.Include(c => c.Sender).Where(c => c.IndividualChatId == id).ToList();

            var username = User.Identity.Name;
            var user = context.Users.FirstOrDefault(user => user.UserName == username);
            CurrUser = user;

            if (string.IsNullOrEmpty(Search))
                Search = "";

            GroupChats = context.GroupChats.Include(c => c.Chats).Where(c => c.Name.ToLower().Contains(Search.ToLower())).ToList();

            IndividualChats = context.IndividualChats
                .Include(c => c.Chats)
                .Include(c => c.UserOne)
                .Include(c => c.UserTwo)
                .Where(u => u.UserOneId == user.Id || u.UserTwoId == user.Id)
                .Select(gr => new IndividualChat
                {
                    Id = gr.Id,
                    DisplayName = user.UserName == gr.UserOne.UserName ? gr.UserTwo.UserName : gr.UserOne.UserName,
                    UserOne = gr.UserOne,
                    UserTwo = gr.UserTwo
                })
                .Where(c => c.DisplayName.ToLower().Contains(Search.ToLower()))
                .ToList();

            ViewData["IndividualChats"] = context.Users.Where(u => u.Id != user.Id).ToList();

            ViewData["CurrentChatIndi"] = 0;
            ViewData["CurrentChat"] = 0;

            if (isGr)
            {
                ViewData["CurrentChat"] = id;
                GroupName = GroupChats.Find(g => g.Id == id).Name;
                isGroupChat = true;
                GroupId = id;
            }
            else
            {
                ViewData["CurrentChatIndi"] = id;
                GroupName = IndividualChats.Find(g => g.Id == id).UserOne.UserName + IndividualChats.Find(g => g.Id == id).UserTwo.UserName;
                isGroupChat = false;
                GroupId = id;
            }


        }

    }
}
