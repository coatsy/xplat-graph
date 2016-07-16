using Android.OS;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Binding.Droid.BindingContext;
using PropertyManager.ViewModels;
using MvvmCross.Binding.Droid.Views;
using Android.Content;
using PropertyManager.Models;
using PropertyManager.Droid.Adapters;

namespace PropertyManager.Droid.Views
{
    public class ConversationsFragment : MvxFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, 
            Bundle savedInstanceState)
        {
            this.EnsureBindingContextIsSet(savedInstanceState);
            return this.BindingInflate(Resource.Layout.ConversationFragment, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var conversationsListView = (MvxListView)view.FindViewById(Resource.Id.conversation_list_view);
            conversationsListView.Adapter = new ConversationListViewAdapter(Context, 
                (IMvxAndroidBindingContext)BindingContext);

            // Get EditText and hook up the event listeners.
            var conversationEditText = (Android.Support.V7.Widget.AppCompatEditText)
                view.FindViewById(Resource.Id.conversation_edit_text);
            conversationEditText.EditorAction += OnConversationEditorAction;
        }

        private void OnConversationEditorAction(object sender, Android.Widget.TextView.EditorActionEventArgs e)
        {
            if (e.ActionId == Android.Views.InputMethods.ImeAction.Send)
            {
                (ViewModel as GroupViewModel)?.AddConversationCommand.Execute(null);
                e.Handled = true;
            }
        }
    }
}