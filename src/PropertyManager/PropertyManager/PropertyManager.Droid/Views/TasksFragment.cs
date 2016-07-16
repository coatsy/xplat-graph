using Android.OS;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Binding.Droid.BindingContext;
using PropertyManager.ViewModels;

namespace PropertyManager.Droid.Views
{
    public class TasksFragment : MvxFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, 
            Bundle savedInstanceState)
        {
            this.EnsureBindingContextIsSet(savedInstanceState);
            return this.BindingInflate(Resource.Layout.TasksFragment, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            // Get EditText and hook up the event listeners.
            var taskEditText = (Android.Support.V7.Widget.AppCompatEditText)
                view.FindViewById(Resource.Id.task_edit_text);
            taskEditText.EditorAction += OnTaskEditorAction;
        }

        private void OnTaskEditorAction(object sender, Android.Widget.TextView.EditorActionEventArgs e)
        {
            if (e.ActionId == Android.Views.InputMethods.ImeAction.Send)
            {
                (ViewModel as GroupViewModel)?.AddTaskCommand.Execute(null);
                e.Handled = true;
            }
        }
    }
}