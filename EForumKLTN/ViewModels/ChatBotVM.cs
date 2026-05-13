namespace EForumKLTN.ViewModels.ChatBot
{
    public class ChatBotVM
    {
        public string? Message { get; set; }

        public bool HasOptions { get; set; }

        public List<ChatBotOptionVM>
        Options
        { get; set; }
        = new();
    }

    public class ChatBotOptionVM
    {
        public int Id { get; set; }

        public string Text { get; set; } = "";
    }
}