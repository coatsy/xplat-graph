using ExcelFormsTest.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExcelFormsTest.ViewModels
{
    public class GroupViewModel : ViewModelBase
    {
        public GroupViewModel(Group group)
        {
            Id = group.id;
            Name = group.displayName;
            Description = group.description;
            GetGroupThumbnailBase64(group.id);
        }

        private async void GetGroupThumbnailBase64(string id)
        {
            GroupThumbnailBase64 = await DataService.GetGroupThumbnailAsBase64(Id);
        }

        private string id;
        public string Id
        {
            get { return id; }
            set { if (id == value) return; id = value; NotifyPropertyChanged(); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                name = value;
                Title = value;
                NotifyPropertyChanged();
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { if (description == value) return; description = value; NotifyPropertyChanged(); }
        }

        private string groupThumbnailBase64;
        public string GroupThumbnailBase64
        {
            get { return groupThumbnailBase64; }
            set
            {
                if (groupThumbnailBase64 == value) return;
                groupThumbnailBase64 = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("GroupThumbnail");
            }
        }

        public ImageSource GroupThumbnail
        {
            get
            {
                return string.IsNullOrEmpty(GroupThumbnailBase64) ?
                    null :
                    ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(GroupThumbnailBase64)));
            }
        }


    }
}
