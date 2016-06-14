using System.Collections.ObjectModel;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;
using PropertyManager.Models;
using PropertyManager.Services;

namespace PropertyManager.ViewModels
{
    public class GroupsViewModel
        : MvxViewModel
    {
        private readonly IGraphService _graphService;

        public ObservableCollection<GroupModel> Groups { get; set; } 

        public GroupsViewModel(IGraphService graphService)
        {
            _graphService = graphService;
            Groups = new ObservableCollection<GroupModel>();
        }

        public void Init(string data)
        {
            var groups = JsonConvert.DeserializeObject<GroupModel[]>(data);
            foreach (var group in groups)
            {
                Groups.Add(group);
            }
        }
    }
}
