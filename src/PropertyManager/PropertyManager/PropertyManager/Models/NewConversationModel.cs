using System.Collections.Generic;

namespace PropertyManager.Models
{
    public class NewConversationModel
    {
        public string Topic { get; set; }

        public List<NewPostModel> Posts { get; set; }
    }
}
