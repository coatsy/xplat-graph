using System.Collections.Generic;

namespace PropertyManager.Models
{
    public class NewPostModel
    {
        public BodyModel Body { get; set; }

        public List<ParticipantModel> NewParticipants { get; set; }
    }
}
