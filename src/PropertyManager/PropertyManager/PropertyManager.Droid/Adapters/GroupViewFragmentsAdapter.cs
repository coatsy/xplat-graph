using Android.Support.V4.App;
using Java.Lang;
using MvvmCross.Droid.Support.V4;
using PropertyManager.Droid.Views;

namespace PropertyManager.Droid.Adapters
{
    public class GroupViewFragmentsAdapter : MvxCachingFragmentPagerAdapter
    {
        private readonly GroupView _groupView;
        private readonly FragmentInfo[] _fragmentInfos;

        public override int Count
        {
            get { return _fragmentInfos.Length; }
        }

        public GroupViewFragmentsAdapter(GroupView groupView)
            : base(groupView.SupportFragmentManager)
        {
            _groupView = groupView;
            _fragmentInfos = new FragmentInfo[]
            {
                new FragmentInfo("Details", new DetailsFragment()),
                new FragmentInfo("Conversations", new ConversationsFragment()),
                new FragmentInfo("Files", new FilesFragment()),
                new FragmentInfo("Tasks", new TasksFragment()),
            };
        }

        public override Fragment GetItem(int position, Fragment.SavedState fragmentSavedState = null)
        {
            return _fragmentInfos[position].Fragment;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(_fragmentInfos[position].Title);
        }
    }
}